using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileCoreServices;
using Photos;
using UIKit;

namespace MobileClaims.iOS.Services
{
    public class LoadFileService : ILoadFileService
    {
        private EventHandler<UIImagePickerMediaPickedEventArgs> _mediaEventHandler;
        private EventHandler<UIImagePickerImagePickedEventArgs> _imageEventHandler;
        private EventHandler _canceledEventHandler;

        private TaskCompletionSource<IEnumerable<DocumentInfo>> _taskCompletionSource;
        private UIDocumentPickerViewController _pickerViewController;
        private IEnumerable<DocumentInfo> _files;

        public async Task<IEnumerable<DocumentInfo>> OpenFilePickerAsync(bool isMediaFileOnly)
        {
            _taskCompletionSource = new TaskCompletionSource<IEnumerable<DocumentInfo>>();
            if (isMediaFileOnly)
            {
                await PickImage();
            }
            else
            {
                await PickDocuments();
            }

            return await _taskCompletionSource.Task;
        }

        private async Task PickImage()
        {
            var imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = new string[] { UTType.Image }
            };
            _mediaEventHandler = (s, e) => Picker_FinishedPickingMedia(s, e, imagePicker);
            _imageEventHandler = (s, e) => Picker_FinishedPickingImage(s, e, imagePicker);
            _canceledEventHandler = (s, e) => Picker_Canceled(s, e, imagePicker);

            imagePicker.FinishedPickingMedia += _mediaEventHandler;
            imagePicker.FinishedPickingImage += _imageEventHandler;
            imagePicker.Canceled += _canceledEventHandler;

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.ModalInPopover = true;
            await viewController.PresentViewControllerAsync(imagePicker, true);

            _files = await _taskCompletionSource.Task;
        }

        private async Task PickDocuments()
        {
            _pickerViewController = new UIDocumentPickerViewController(new string[]
            {
                    UTType.PDF,  UTType.PlainText,
                    UTType.JPEG, UTType.PNG,
                    UTType.TIFF, UTType.BMP,
                    UTType.GIF
            }, UIDocumentPickerMode.Import);

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                _pickerViewController.AllowsMultipleSelection = true;
            }

            _pickerViewController.WasCancelled += OnPickerViewControllerCancelled;
            _pickerViewController.DidPickDocumentAtUrls += OnPickerViewControllerPickedDocuments;

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.ModalInPopover = true;
            await viewController.PresentViewControllerAsync(_pickerViewController, true);

            _files = await _taskCompletionSource.Task;
        }

        private void OnPickerViewControllerPickedDocuments(object sender, UIDocumentPickedAtUrlsEventArgs args)
        {
            var documentInfo = new List<DocumentInfo>();
            foreach (var url in args.Urls)
            {
                var data = NSData.FromUrl(url);
                var dataBytes = new byte[data.Length];

                Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
                var filename = Path.GetFileName(WebUtility.UrlDecode(url.ToString()));

                documentInfo.Add(new DocumentInfo { ByteContent = dataBytes, Name = filename });
            }

            _taskCompletionSource.SetResult(documentInfo);
            _pickerViewController.DismissViewController(true, null);
        }

        private void OnPickerViewControllerCancelled(object sender, EventArgs e)
        {
            _pickerViewController.DismissViewController(true, null);
        }

        private async void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e, UIImagePickerController imagePicker)
        {
            imagePicker.FinishedPickingMedia -= _mediaEventHandler;
            await (sender as UIImagePickerController).DismissViewControllerAsync(true);
            var image = GetSelectedImage(e.Info);
            var fileName = GetFileName(e.Info);
            List<DocumentInfo> documentInfo = CreateDocumentInfoList(image, fileName);

            _taskCompletionSource.SetResult(documentInfo);
        }

        private async void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e, UIImagePickerController imagePicker)
        {
            imagePicker.FinishedPickingImage -= _imageEventHandler;
            await (sender as UIImagePickerController).DismissViewControllerAsync(true);
            var image = GetSelectedImage(e.EditingInfo);
            var fileName = GetFileName(e.EditingInfo);
            List<DocumentInfo> documentInfo = CreateDocumentInfoList(image, fileName);

            _taskCompletionSource.SetResult(documentInfo);
        }

        private void Picker_Canceled(object sender, EventArgs e, UIImagePickerController imagePicker)
        {
            imagePicker.Canceled -= _canceledEventHandler;
            if (sender is UIImagePickerController)
            {
                ((UIImagePickerController)sender).DismissViewController(true, null);
            }
        }

        int fileNameIncrementor = 100;
        private string GetFileName(NSDictionary info)
        {
            string fileName = string.Empty;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                var phAsset = info[UIImagePickerController.PHAsset] as PHAsset;

                if (phAsset == null)
                {
                    fileName = "file" + fileNameIncrementor++.ToString() + ".jpeg";
                }                    
                else
                {
                    var assetResources = PHAssetResource.GetAssetResources(phAsset);
                    var selectedResource = assetResources.FirstOrDefault();
                    fileName = selectedResource?.OriginalFilename;
                }
            }
            else
            {
                var referenceUrl = info[UIImagePickerController.ReferenceUrl] as NSUrl;
                var results = PHAsset.FetchAssets(new[] { referenceUrl }, null);
                var asset = results.firstObject as PHAsset;
                var resource = PHAssetResource.GetAssetResources(asset).FirstOrDefault();
                fileName = resource?.OriginalFilename;
            }

            var newFileName = fileName;
            if (fileName.Contains("HEIC", StringComparison.OrdinalIgnoreCase))
            {
                newFileName = fileName.Replace("HEIC", "JPEG", StringComparison.OrdinalIgnoreCase);
            }

            return newFileName;
        }

        private UIImage GetSelectedImage(NSDictionary info)
        {
            var image = info[UIImagePickerController.EditedImage] as UIImage;
            if (image == null)
            {
                image = info[UIImagePickerController.OriginalImage] as UIImage;
            }

            return image;
        }

        private static List<DocumentInfo> CreateDocumentInfoList(UIImage image, string fileName)
        {
            var documentInfo = new DocumentInfo
            {
                Name = fileName
            };

            using (var jpegImage = image.AsJPEG((nfloat)0.8))
            {
                var byteArray = new Byte[jpegImage.Length];
                System.Runtime.InteropServices.Marshal.Copy(jpegImage.Bytes, byteArray, 0, Convert.ToInt32(jpegImage.Length));
                documentInfo.ByteContent = byteArray;
            }

            return new List<DocumentInfo>
            {
                documentInfo
            };
        }
    }
}