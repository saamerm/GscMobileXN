using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;

namespace MobileClaims.Droid
{
    [ContentProvider(new string[] { MobileClaims.Droid.GSCFileProvider.AUTHORITY },GrantUriPermissions =true,Exported =false)]
    [MetaData("android.support.FILE_PROVIDER_PATHS", Resource ="@xml/provider_paths")]
   
    public class GSCFileProvider : FileProvider
    {
        public const string AUTHORITY = "com.greenshield.mobileclaims";
           // Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath; 
 
    } 
}
