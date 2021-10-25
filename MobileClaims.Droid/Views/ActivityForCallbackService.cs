using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using MobileClaims.Core.Services;
using MobileClaims.Droid.Helpers;
using MobileClaims.Droid.Services;
using MvvmCross;
using Plugin.Permissions;
using System.Collections.Generic;
using MvvmCross.Platforms.Android.Views;
using Android.Content.PM;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "ActivityForCallbackService")]
    public class ActivityForCallbackService : MvxActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var isMediaFileOnly = Intent.Extras.GetBoolean(LoadFileService.IsMediaFile);

            Intent intent;
            string[] mimeTypes;
            if (isMediaFileOnly)
            {
                intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                mimeTypes = new[]
                {
                    "image/x-portable-bitmap", "image/bmp", "image/x-windows-bmp",
                    "image/x-ms-bmp", "image/gif", "image/jpeg", "image/png", "image/tiff"
                };

                intent.PutExtra(Intent.ExtraMimeTypes, mimeTypes);
            }
            else
            {
                intent = new Intent(Intent.ActionGetContent);
                mimeTypes = new[]
                {
                    "application/pdf", "text/plain",
                    "image/x-portable-bitmap", "image/bmp", "image/x-windows-bmp",
                    "image/x-ms-bmp", "image/gif", "image/jpeg", "image/png", "image/tiff"
                };
                intent.PutExtra(Intent.ExtraLocalOnly, true);
                intent.SetType(mimeTypes.Length == 1 ? mimeTypes[0] : "*/*");
                intent.PutExtra(Intent.ExtraMimeTypes, mimeTypes);
            }

            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            StartActivityForResult(intent, (int)ActivityRequestCodes.FilePickerCode);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (data == null)
            {
                Finish();
                return;
            }

            if (requestCode == (int)ActivityRequestCodes.FilePickerCode && resultCode == Result.Ok)
            {
                var uris = new List<string>();

                if (data.ClipData != null)
                {
                    for (int i = 0; i < data.ClipData.ItemCount; i++)
                    {
                        uris.Add(data.ClipData.GetItemAt(i).Uri.ToString());
                    }
                }
                else
                {
                    if (data.Data != null)
                    {
                        uris.Add(data.Data.ToString());
                    }
                }

                var fileService = (LoadFileService)Mvx.IoCProvider.Resolve<ILoadFileService>();
                fileService.SetUris(uris);

                fileService.FinishFilePicking();
            }

            Finish();
        }
    }
}