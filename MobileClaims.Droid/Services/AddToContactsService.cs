using System;
using System.Collections.Generic;
using Android.Content;
using Android.Provider;


using MobileClaims.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Services
{
    public class AddToContactsService : IAddToContactsService
    {
        public string SaveContact(string name, string number)
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            ContentResolver resolver = activity.ContentResolver;

            var uri = ContactsContract.Contacts.ContentUri;
            string[] projection = { ContactsContract.Contacts.InterfaceConsts.DisplayName };
            var selection = string.Format("{0} = '{1}'", ContactsContract.ContactsColumns.DisplayName, name);
            var cursor = resolver.Query(uri, projection, selection, null, null);

            if (cursor.Count > 0)
            {
                return $"{name}" + Resource.String.contactAlreadyExists;
            }

            var contentProviderOperations = new List<ContentProviderOperation>();

            ContentProviderOperation.Builder builder =
                ContentProviderOperation.NewInsert(ContactsContract.RawContacts.ContentUri);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountType, null);
            builder.WithValue(ContactsContract.RawContacts.InterfaceConsts.AccountName, null);
            contentProviderOperations.Add(builder.Build());

            //Name
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.StructuredName.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.StructuredName.GivenName, name);
            contentProviderOperations.Add(builder.Build());

            //Number
            builder = ContentProviderOperation.NewInsert(ContactsContract.Data.ContentUri);
            builder.WithValueBackReference(ContactsContract.Data.InterfaceConsts.RawContactId, 0);
            builder.WithValue(ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.Phone.ContentItemType);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.Number, number);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Type,
                ContactsContract.CommonDataKinds.Phone.InterfaceConsts.TypeCustom);
            builder.WithValue(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Label, "Work");
            contentProviderOperations.Add(builder.Build());

            try
            {
                resolver.ApplyBatch(ContactsContract.Authority, contentProviderOperations);
            }
            catch (Exception e)
            {
                return $"Error adding {name}";
            }

            return $"Contact {name} added";
        }
    }
}