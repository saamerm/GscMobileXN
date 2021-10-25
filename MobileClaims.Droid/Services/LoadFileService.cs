using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;
using Android.Widget;


using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Droid.Views;
using MvvmCross;
using MvvmCross.Platforms.Android;
using Exception = System.Exception;
using FileNotFoundException = System.IO.FileNotFoundException;
using Path = System.IO.Path;
using Uri = Android.Net.Uri;

namespace MobileClaims.Droid.Services
{
    public class LoadFileService : ILoadFileService
    {
        private bool _isMediaFilesOnly;
        public const string IsMediaFile = "IsMediaFile";
        private List<string> _filePaths = new List<string>();
        private TaskCompletionSource<IEnumerable<DocumentInfo>> _taskCompletionSource;

        public IEnumerable<DocumentInfo> GetFiles()
        {
            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var documentInfos = new List<DocumentInfo>();

            foreach (var filePath in _filePaths)
            {
                var uri = Uri.Parse(filePath);

                try
                {
                    var cursor = currentActivity.ContentResolver.Query(uri, null, null, null, null);
                    var nameCursor = cursor.GetColumnIndex(OpenableColumns.DisplayName);
                    cursor.MoveToFirst();
                    var fileName = cursor.GetString(nameCursor);

                    if (!StringConstants.FileExtensions.Contains(Path.GetExtension(fileName),
                        StringComparer.OrdinalIgnoreCase))
                    {
                        throw new FileTypeNotSupportedException();
                    }

                    var stream = currentActivity.ContentResolver.OpenInputStream(uri);
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    documentInfos.Add(new DocumentInfo
                    {
                        ByteContent = fileBytes,
                        Name = fileName,
                    });
                }
                catch (FileTypeNotSupportedException e)
                {
                    Toast.MakeText(currentActivity, currentActivity.GetString(Resource.String.fileTypeNotSupported), ToastLength.Short).Show();
                }
                catch (FileNotFoundException e)
                {
                    Toast.MakeText(currentActivity, currentActivity.GetString(Resource.String.fileNotFound), ToastLength.Short).Show();
                }
                catch (Exception e)
                {
                    Toast.MakeText(currentActivity, currentActivity.GetString(Resource.String.error), ToastLength.Short).Show();
                }
            }

            return documentInfos;
        }

        public async Task<IEnumerable<DocumentInfo>> OpenFilePickerAsync(bool isMediaFilesOnly)
        {
            _taskCompletionSource = new TaskCompletionSource<IEnumerable<DocumentInfo>>();
            _isMediaFilesOnly = isMediaFilesOnly;

            OpenFilePickerActivity();

            return await _taskCompletionSource.Task;
        }

        private void OpenFilePickerActivity()
        {
            var currentActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var intent = new Intent(currentActivity, typeof(ActivityForCallbackService));
            intent.PutExtra(LoadFileService.IsMediaFile, _isMediaFilesOnly);
            currentActivity.StartActivity(intent);
        }

        public void SetUris(List<string> uris)
        {
            _filePaths = uris;
        }

        public void FinishFilePicking()
        {
            var files = GetFiles();

            _taskCompletionSource.SetResult(files);
        }
    }
}