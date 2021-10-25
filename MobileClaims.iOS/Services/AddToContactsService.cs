using Contacts;
using Foundation;
using MobileClaims.Core.Services;

namespace MobileClaims.iOS.Services
{
    public class AddToContactsService : IAddToContactsService
    {
        public string SaveContact(string name, string number)
        {
            bool isSuccess = true;

            var contact = new CNMutableContact();
            var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(number));
            var phoneNumber = new[] { cellPhone };
            contact.PhoneNumbers = phoneNumber;
            contact.GivenName = name;

            var store = new CNContactStore();
            NSError error;

            var predicate = CNContact.GetPredicateForContacts(name);
            var fetchKey = new NSString[] { CNContactKey.GivenName };

            var existingContact = store.GetUnifiedContacts(predicate, fetchKey, out error);
            if (existingContact.Length > 0)
            {
                return $"Contact {name} already exists";
            }

            var saveRequest = new CNSaveRequest();
            saveRequest.AddContact(contact, store.DefaultContainerIdentifier);

            isSuccess = store.ExecuteSaveRequest(saveRequest, out error);
            store.RequestAccess(CNEntityType.Contacts, (granted, nsError) =>
            {
                if (!granted)
                {
                    isSuccess = false;
                }
            });

            if (error == null && isSuccess)
            {
                isSuccess = true;
            }

            return isSuccess ? $"Contact {name} added" : $"Error adding {name}";
        }
    }
}