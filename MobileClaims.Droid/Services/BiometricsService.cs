using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Security.Keystore;
using Android.Util;

using Java.Security;
using Javax.Crypto;
using MobileClaims.Services;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace MobileClaims.Droid.Services
{
    public class BiometricsService : IBiometricsService
    {
        public async Task<bool> Authenticate(string reason, CancellationToken token = default(CancellationToken))
        {
            var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration(reason), token);
            return result.Authenticated;
        }

        public async Task<bool> BiometricsAvailable()
        {
            // Biometrics only available for API 23+
            var fingerprintAvailability = FingerprintAvailability.Unknown;
            try
            {
                fingerprintAvailability = await CrossFingerprint.Current.GetAvailabilityAsync();
            }
            catch (Exception e)
            {
                Log.Error("Could not use Fingerprint module", e.StackTrace);
            }

            return Build.VERSION.SdkInt >= BuildVersionCodes.M && fingerprintAvailability == FingerprintAvailability.Available;
        }

        public async Task<bool> CanLoginWithBiometrics()
        {
            if (!(await BiometricsAvailable()))
            {
                return false;
            }
            var preferences = Application.Context.GetSharedPreferences("GSC_Shared", FileCreationMode.Private);
            return preferences.GetString("encryptedPassword", null) != null;
        }

        public async Task<string> GetPasswordFromKeychain(CancellationToken token)
        {
            if (await Authenticate(Application.Context.Resources.GetString(Resource.String.fingerprintLoginInstruction), token))
            {
                var ks = KeyStore.GetInstance("AndroidKeyStore");
                ks.Load(null);

                var preferences = Application.Context.GetSharedPreferences("GSC_Shared", FileCreationMode.Private);
                var encryptedPassword = preferences.GetString("encryptedPassword", null);
                if (encryptedPassword == null)
                    return null;

                var password = decryptString("gscuser", ks, encryptedPassword);
                return password;
            }
            return null;
        }

        public async Task<bool> SavePasswordToKeychain(string password, CancellationToken token)
        {
            if (await Authenticate(Application.Context.Resources.GetString(Resource.String.fingerprintLoginInstruction), token))
            {
                var ks = KeyStore.GetInstance("AndroidKeyStore");
                ks.Load(null);
                if (!ks.ContainsAlias("gscuser"))
                {
                    var kpg = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa, "AndroidKeyStore");
                    kpg.Initialize(new KeyGenParameterSpec.Builder("gscuser", KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                        .SetDigests(KeyProperties.DigestSha256, KeyProperties.DigestSha512)
                        .SetRandomizedEncryptionRequired(true)
                        .SetEncryptionPaddings(KeyProperties.EncryptionPaddingRsaPkcs1)
                        .Build());
                    var kp = kpg.GenerateKeyPair();
                }

                var encryptedPassword = encryptString("gscuser", ks, password);
                var preferences = Application.Context.GetSharedPreferences("GSC_Shared", FileCreationMode.Private);

                using (var editor = preferences.Edit())
                {
                    editor.PutString("encryptedPassword", encryptedPassword);
                    editor.Commit();
                }
                return true;
            }
            return false;
        }

        public void RemoveStoredCredentials()
        {
            var preferences = Application.Context.GetSharedPreferences("GSC_Shared", FileCreationMode.Private);
            using (var editor = preferences.Edit())
            {
                editor.Remove("encryptedPassword");
                editor.Commit();
            }
        }

        private string encryptString(string alias, KeyStore keyStore, string password)
        {
            try
            {
                var privateKeyEntry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(alias, null);
                var publicKey = privateKeyEntry.Certificate.PublicKey;

                Cipher inCipher = Cipher.GetInstance("RSA/ECB/PKCS1Padding");
                inCipher.Init(CipherMode.EncryptMode, publicKey);

                using (var outputStream = new MemoryStream())
                {
                    using (var cipherOutputStream = new CipherOutputStream(outputStream, inCipher))
                    {
                        cipherOutputStream.Write(Encoding.UTF8.GetBytes(password));
                        cipherOutputStream.Close();
                        byte[] vals = outputStream.ToArray();
                        return Convert.ToBase64String(vals);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GSC", e.StackTrace);
            }
            return null;
        }

        private string decryptString(string alias, KeyStore keyStore, string encryptedPassword)
        {
            try
            {
                KeyStore.PrivateKeyEntry privateKeyEntry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(alias, null);
                var privateKey = privateKeyEntry.PrivateKey;

                Cipher output = Cipher.GetInstance("RSA/ECB/PKCS1Padding");
                output.Init(CipherMode.DecryptMode, privateKey);

                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                using (var cipherInputStream = new CipherInputStream(ms, output))
                {
                    var values = new List<byte>();
                    int nextByte;
                    while ((nextByte = cipherInputStream.Read()) != -1)
                    {
                        values.Add((byte)nextByte);
                    }
                    return Encoding.UTF8.GetString(values.ToArray());
                }
            }
            catch (Exception e)
            {
                Log.Error("GSC", e.StackTrace);
            }
            return null;
        }

        public string GetConfirmMessage()
        {
            return "Do you want to use your fingerprint?";
        }
    }
}