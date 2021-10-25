using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using MobileClaims.Services;
using Plugin.Fingerprint;
using Security;
using UIKit;

namespace MobileClaims.iOS.Services
{
    public class BiometricsService : IBiometricsService
    {
        public BiometricsService()
        {
        }

        public async Task<bool> SavePasswordToKeychain(string password, CancellationToken token)
        {
            if(await Authenticate(string.Format("{0} {1}",
                                                NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName"),
                                                "loginInstruction".tr())))
            {
                var existing = RetrievePassword();


                if (existing == null)
                {
                    var record = new SecRecord(SecKind.GenericPassword)
                    {
                        ValueData = NSData.FromString(password),
                        Account = "password",
                        Service = "GSC",
                        Label = "password"
                    };

                    var result = SecKeyChain.Add(record);

                    return result == SecStatusCode.Success && result != SecStatusCode.DuplicateItem;
                }
                else 
                {
                    var record = new SecRecord(SecKind.Identity)
                    {
                        Account = "password",
                        Service = "GSC",
                        Label = "password",
                        ValueData = NSData.FromString(password)
                    };

                    var result = SecKeyChain.Update(existing, record);
                    if(result != SecStatusCode.Success)
                    {
                        RemoveStoredCredentials();
                        var recordNew = new SecRecord(SecKind.GenericPassword)
                        {
                            ValueData = NSData.FromString(password),
                            Account = "password",
                            Service = "GSC",
                            Label = "password"
                        };
                        result = SecKeyChain.Add(recordNew);

                        return result == SecStatusCode.Success && result != SecStatusCode.DuplicateItem;
                    }
                    return result == SecStatusCode.Success;
                }
            }
            return false;
        }

        public async Task<string> GetPasswordFromKeychain(CancellationToken token)
        {
            if (await Authenticate("FingerprintLoginPrompt".tr()))
            {
                var result = RetrievePassword();
                if (result != null)
                {
                    return NSString.FromData(result.ValueData, NSStringEncoding.UTF8);
                }
            }
            return null;
        }

        private static SecRecord RetrievePassword()
        {
            var record = new SecRecord(SecKind.GenericPassword)
            {
                Account = "password",
                Service = "GSC",
                Label = "password"
            };
            if (record == null) return null;
            var result = SecKeyChain.QueryAsRecord(record, out var status);

            return status == SecStatusCode.Success ? result : null;
        }

        public async Task<bool> BiometricsAvailable()
        {
            return await CrossFingerprint.Current.IsAvailableAsync();
        }

        public async Task<bool> CanLoginWithBiometrics()
        {
            var result = RetrievePassword();
            return result != null && await BiometricsAvailable();
        }

        public async Task<bool> Authenticate(string reason, CancellationToken token = default(CancellationToken))
        {
            var result = await CrossFingerprint.Current.AuthenticateAsync(reason, token);

            return result.Authenticated;
        }

        public void RemoveStoredCredentials()
        {
            var existingPassword = RetrievePassword();
            if (existingPassword != null)
            {
                var record = new SecRecord(SecKind.GenericPassword)
                {
                    Account = "password",
                    Service = "GSC",
                    Label = "password"
                };
                SecKeyChain.Remove(record);
            }
        }

        public string GetConfirmMessage()
        {
            var context = new LAContext();
            NSError error;
            if(context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            {
                var strSystemVersion = UIDevice.CurrentDevice.SystemVersion.Split('.')[0];
                var systemVersion = Convert.ToInt32(strSystemVersion);

                if (systemVersion >= 11 && context.BiometryType == LABiometryType.FaceId)
                {
                    return "useFaceIdMessage".tr();
                }
                return "useTouchIdMessage".tr();
            }

            return string.Empty;
        }
    }
}
