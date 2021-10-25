using MobileClaims.Core.Entities;
using MvvmCross.Logging;
using MvvmCross.Plugin.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MobileClaims.Core.Services
{
    public class ImageContent
    {
        public string ImageFor { get; set; }

        public string ImagePath { get; set; }

        public byte[] ImageByteArray { get; set; }
    }

    public class CardService : ApiClientHelper, ICardService
    {
        private readonly IMvxFileStore _mvxFileStore;
        private readonly IDataService _dataService;
        private readonly IMvxLog _log;
        private IDCard _idCard;

        public TaskCompletionSource<IDCard> TaskCompletionSource { get; private set; }

        public CardService(IDataService dataService, IMvxFileStore mvxFileStore, IMvxLog log)
        {
            _mvxFileStore = mvxFileStore;
            _dataService = dataService;
            _log = log;
            TaskCompletionSource = new TaskCompletionSource<IDCard>();
        }

        public async Task<IDCard> GetIdCardAsync(string planMemberId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            try
            {
                TaskCompletionSource = new TaskCompletionSource<IDCard>();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                _log.Trace("Making GetIdCard request.");

                var apiClient = new ApiClient<IDCard>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                    HttpMethod.Get,
                    $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemberId}/idcard");

                var idCard = await ExecuteRequestWithRetry(apiClient);
                idCard.PlanMemberID = $"{idCard.PlanMemberID}-{idCard.ParticipantNumber}";
                sw.Stop();
                _log.Trace($"Making GetIdCard request complete in {sw.ElapsedMilliseconds}.");

                sw.Start();
                Dictionary<string, string> idCardImageUrls = new Dictionary<string, string>();
                idCardImageUrls.Add("FrontImageUri", idCard.FrontImageUri);
                idCardImageUrls.Add("BackImageUri", idCard.BackImageUri);

                Dictionary<string, string> idCardLogoImageUrls = new Dictionary<string, string>();
                idCardLogoImageUrls.Add("FrontLeftLogoImageUri", idCard.FrontLeftLogoImageUri);
                idCardLogoImageUrls.Add("FrontRightLogoImageUri", idCard.FrontRightLogoImageUri);
                idCardLogoImageUrls.Add("BackTopLogoImageUri", idCard.BackTopLogoImageUri);

                var getImageArrayBlock = new TransformBlock<KeyValuePair<string, string>, ImageContent>(
                    async uri =>
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            _log.Error($"Downloading cancelled for {uri.Key}");
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        var imageArray = await GetIdCardImageByteArray(uri.Value, cancellationToken);
                        var imagePath = WriteImageToDevice(imageArray, uri.Value);
                        return new ImageContent()
                        {
                            ImageFor = uri.Key,
                            ImagePath = imagePath,
                            ImageByteArray = imageArray
                        };
                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                    });

                var getlogoArrayBlock = new TransformBlock<KeyValuePair<string, string>, ImageContent>(
                  async uri =>
                  {
                      var imageArray = await GetIdCardLogoByteArray(uri.Value, cancellationToken);
                      var imagePath = WriteImageToDevice(imageArray, uri.Value);
                      return new ImageContent()
                      {
                          ImageFor = uri.Key,
                          ImagePath = imagePath,
                      };
                  },
                  new ExecutionDataflowBlockOptions
                  {
                      MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded
                  });

                var assignImageContentBlock = new ActionBlock<ImageContent>(c =>
                {
                    _log.Trace($"File save under path {c.ImagePath}");
                    if (c.ImageFor.Equals("BackImageUri"))
                    {
                        idCard.BackImageByteArray = c.ImageByteArray;
                        idCard.BackImageFilePath = c.ImagePath;
                    }
                    else if (c.ImageFor.Equals("FrontImageUri"))
                    {
                        idCard.FrontImageByteArray = c.ImageByteArray;
                        idCard.FrontImageFilePath = c.ImagePath;
                    }
                });

                var assignLogoContentBlock = new ActionBlock<ImageContent>(c =>
                {
                    _log.Trace($"File save under path {c.ImagePath}");
                    if (c.ImageFor.Equals("FrontLeftLogoImageUri"))
                    {
                        idCard.FrontLeftLogoFilePath = c.ImagePath;
                    }
                    else if (c.ImageFor.Equals("FrontRightLogoImageUri"))
                    {
                        idCard.FrontRightLogoFilePath = c.ImagePath;
                    }
                    else if (c.ImageFor.Equals("BackTopLogoImageUri"))
                    {
                        idCard.BackTopLogoFilePath = c.ImagePath;
                    }
                });

                getImageArrayBlock.LinkTo(
                    assignImageContentBlock, new DataflowLinkOptions
                    {
                        PropagateCompletion = true
                    });

                getlogoArrayBlock.LinkTo(
                    assignLogoContentBlock, new DataflowLinkOptions
                    {
                        PropagateCompletion = true
                    });

                try
                {
                    foreach (var item in idCardImageUrls.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                    {
                        getImageArrayBlock.Post(item);
                    }

                    foreach (var item in idCardLogoImageUrls.Where(x => !string.IsNullOrWhiteSpace(x.Value)))
                    {
                        getlogoArrayBlock.Post(item);
                    }

                    getImageArrayBlock.Complete();
                    getlogoArrayBlock.Complete();
                    assignImageContentBlock.Completion.Wait(cancellationToken);
                    assignLogoContentBlock.Completion.Wait(cancellationToken);

                    _dataService.PersistIDCard(idCard);
                    TaskCompletionSource.TrySetResult(idCard);
                }
                catch (AggregateException ae)
                {
                    ae.Handle(e =>
                    {
                        _log.Error($"Error occurred: {ae.GetType().Name} with {ae.Message}");
                        return true;
                    });
                    TaskCompletionSource.TrySetException(ae);
                }
                sw.Stop();
                _log.Trace($"Time taken to download all images: {sw.ElapsedMilliseconds}");

                return await TaskCompletionSource.Task;
            }
            catch (Exception ex)
            {
                _log.Trace($"Error occurred while downloading and saving id card data: {ex.Message}");
                return null;
            }
        }

        public IDCard UpdateIdCardImage(IDCard idCard)
        {
            try
            {
                var frontImageFilePath = WriteImageToDevice(idCard.FrontImageByteArray, idCard.FrontImageUri);
                var backImageFilePath = WriteImageToDevice(idCard.BackImageByteArray, idCard.BackImageUri);

                idCard.FrontImageFilePath = frontImageFilePath;
                idCard.BackImageFilePath = backImageFilePath;
                _dataService.PersistIDCard(idCard);
            }
            catch (Exception ex)
            {
                _log.Trace($"Error occurred while re writing id card images: {ex.Message}");
            }
            return idCard;
        }

        private async Task<byte[]> GetIdCardImageByteArray(string imageFileName, CancellationToken cancellationToken)
        {
            return await GetImageByteArray(imageFileName, "idcardimage", cancellationToken);
        }

        private async Task<byte[]> GetIdCardLogoByteArray(string frontLeftLogoImageUri, CancellationToken cancellationToken)
        {
            return await GetImageByteArray(frontLeftLogoImageUri, "idcardlogo", cancellationToken);
        }

        private async Task<byte[]> GetImageByteArray(string imageFileName, string restApiPathPart, CancellationToken cancellationToken)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Debug.Assert(!string.IsNullOrWhiteSpace(imageFileName), "imageFileName is empty or null");
            Debug.Assert(!string.IsNullOrWhiteSpace(restApiPathPart), "restApiPathPart is empty or null");

            if (cancellationToken.IsCancellationRequested)
            {
                _log.Error($"Downloading cancelled for image: {imageFileName}");
                cancellationToken.ThrowIfCancellationRequested();
            }

            _log.Trace($"Downloading {imageFileName} on thread id: {TaskScheduler.Current.Id}");

            var apiClient = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/{restApiPathPart}/{imageFileName}");

            var httpResponseMessage = await ExecuteRequestWithRetry(apiClient, cancellationToken);
            var imageBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

            sw.Stop();
            _log.Trace($"Downloading finished for {imageFileName} and took {sw.ElapsedMilliseconds} mS");

            return imageBytes;
        }

        private string WriteImageToDevice(byte[] imageBytes, string imageFileName)
        {
            if (_mvxFileStore.Exists(imageFileName))
            {
                _mvxFileStore.DeleteFile(imageFileName);
            }

            _mvxFileStore.WriteFile(imageFileName, imageBytes);
            return _mvxFileStore.NativePath(imageFileName);
        }
    }
}
