using System;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileClaims.Core.ViewModels
{
    public class DocumentSelectionHelper
    {
        private readonly IUserDialogs _userDialogService;
        private readonly ILoadFileService _loadFileService;
        private readonly Func<DocumentInfo, Task> _addFileToAttachments;
        private readonly string _actionSheetTitle;

        public DocumentSelectionHelper(
            IUserDialogs userDialogService,
            ILoadFileService loadFileService,
            string actionSheetTitle,
            Func<DocumentInfo, Task> addFileToAttachments
        )
        {
            _userDialogService = userDialogService;
            _loadFileService = loadFileService;
            _actionSheetTitle = actionSheetTitle;
            _addFileToAttachments = addFileToAttachments;
        }

        public void ExecuteShowDocumentSelectionPopoverCommand()
        {
            var actionSheetConfig = new ActionSheetConfig
            {
                Title = _actionSheetTitle,
                Cancel = new ActionSheetOption(Resource.Cancel)
            };
            actionSheetConfig.Add(Resource.SelectFiles, () => LoadFile(false));
            actionSheetConfig.Add(Resource.TakePhoto, TakePhoto);
            actionSheetConfig.Add(Resource.SelectPhotos, () => LoadFile(true));
            _userDialogService.ActionSheet(actionSheetConfig);
        }

        public async void TakePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    _userDialogService.Alert(Resource.CameraNotAvailable, Resource.CameraError);
                    return;
                }

                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                    { CompressionQuality = 80 });
                if (photo == null)
                {
                    return;
                }

                var photoName = Path.GetFileName(photo.Path);
                var photoBytes = photo.GetBytes();

                if (photoBytes.Length <= 0)
                {
                    return;
                }

                var file = new DocumentInfo
                {
                    ByteContent = photoBytes,
                    Name = photoName,
                };

                await _addFileToAttachments(file);
            }
            catch (MediaPermissionException e)
            {
                _userDialogService.Alert(Resource.NeedCameraPermission);
            }
            catch (Exception e)
            {
                _userDialogService.Alert(Resource.GenericErrorDialogMessage);
            }
        }

        public async void LoadFile(bool isMediaFileOnly)
        {
            try
            {
                var status = await GetPermission(isMediaFileOnly ? Permission.Photos : Permission.Storage);

                if (status == PermissionStatus.Granted)
                {
                    var files = await _loadFileService.OpenFilePickerAsync(isMediaFileOnly);
                    foreach (var item in files)
                    {
                        await _addFileToAttachments(item);
                    }
                }
                else
                {
                    _userDialogService.Alert(isMediaFileOnly
                        ? Resource.NeedPhotoLibraryPermission
                        : Resource.NeedStoragePermission);
                }
            }
            catch (Exception)
            {
                _userDialogService.Alert(Resource.GenericErrorDialogMessage);
            }
        }

        private static async Task<PermissionStatus> GetPermission(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            if (status != PermissionStatus.Granted)
            {
                var result = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                if (result.ContainsKey(permission))
                {
                    status = result[permission];
                }
            }

            return status;
        }

    }
}