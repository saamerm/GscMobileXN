using System;
using System.Linq;
using System.Text;
using MobileClaims.Core.Entities;
using MvvmCross;
using MvvmCross.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using MvvmCross.Plugin.File;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class JSONDataService : IDataService
    {
        private readonly string FILE_NAME = string.Empty;//"contents.json";
        private PersistedData _contents;
        private readonly IMvxLog _log;
        private readonly IMvxFileStore _filesystem;
        private readonly IMvxMessenger _messenger;
        private object _sync = new object();
        byte[] buf = new byte[16];              //input buffer
        byte[] obuf = new byte[512];            //output buffer

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private string Decrypt(byte[] toDecrypt, string keyAsString)
        {
            string decryptedString;
            var key = Encoding.Unicode.GetBytes(keyAsString);
            var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");
            cipher.Init(false, new KeyParameter(key));

            using (System.IO.MemoryStream toDecryptStream = new System.IO.MemoryStream(toDecrypt))
            {
                using (System.IO.MemoryStream decryptedStream = new System.IO.MemoryStream())
                {
                    int noBytesRead = 0;
                    int noBytesProcessed = 0;
                    while ((noBytesRead = toDecryptStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        noBytesProcessed = cipher.ProcessBytes(buf, 0, noBytesRead, obuf, 0);
                        decryptedStream.Write(obuf, 0, noBytesProcessed);
                    }
                    toDecryptStream.Dispose();
                    noBytesProcessed = cipher.DoFinal(obuf, 0);
                    decryptedStream.Write(obuf, 0, noBytesProcessed);
                    decryptedStream.Flush();

                    decryptedString = Encoding.Unicode.GetString(decryptedStream.ToArray(), 0, decryptedStream.ToArray().Length);
                    decryptedStream.Dispose();
                }
            }
            return decryptedString;
        }
        private byte[] Encrypt(string toEncrypt, string keyAsString)
        {
            byte[] encryptedArray;
            var key = Encoding.Unicode.GetBytes(keyAsString);
            var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS5Padding");
            cipher.Init(true, new KeyParameter(key));

            var toEncryptBytes = Encoding.Unicode.GetBytes(toEncrypt);
            using (System.IO.MemoryStream toEncryptStream = new System.IO.MemoryStream(toEncryptBytes))
            {
                using (System.IO.MemoryStream encryptedStream = new System.IO.MemoryStream())
                {
                    int noBytesRead = 0;
                    int noBytesProcessed = 0;
                    while ((noBytesRead = toEncryptStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        noBytesProcessed = cipher.ProcessBytes(buf, 0, noBytesRead, obuf, 0);
                        encryptedStream.Write(obuf, 0, noBytesProcessed);

                    }
                    toEncryptStream.Dispose();
                    noBytesProcessed = cipher.DoFinal(obuf, 0);
                    encryptedStream.Write(obuf, 0, noBytesProcessed);
                    encryptedStream.Flush();

                    encryptedArray = encryptedStream.ToArray();
                    encryptedStream.Dispose();
                }
            }
            return encryptedArray;
        }

        public JSONDataService(IMvxFileStore filesystem,
            IMvxMessenger messenger,
            IMvxLog log)
        {
            _messenger = messenger;
            _filesystem = filesystem;
            _log = log;
            try
            {
                FILE_NAME = _filesystem.GetFilesIn(string.Empty).Where(s => s.EndsWith(".json")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Trace(ex, ex.Message);
            }
            string key;

            if (string.IsNullOrEmpty(FILE_NAME))
            {
                key = Guid.NewGuid().ToString().Replace("-", "").Substring(8, 16);
                FILE_NAME = _filesystem.NativePath(key + ".json");
            }
            else
            {
                key = StringUtil.filenameWithoutPathAndExtension(FILE_NAME);
            }
            byte[] content;
            _filesystem.TryReadBinaryFile(FILE_NAME, out content);

            try
            {
                _contents = content != null ? JsonConvert.DeserializeObject<PersistedData>(Decrypt(content, key)) : new PersistedData();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Couldnt decrypt old persisted data.");
                _contents = new PersistedData();
            }
        }

        public string AH
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
                string ahValue = "2JpbGUyMDE0IQ==";
                return ahValue;
            }
        }

        public string AHX
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
                string ahxValue = "GVyb3hJZFAh";
                return ahxValue;
            }
        }

        private void WriteContentsAsJson()
        {
            lock (_sync)
            {
                string json;
                json = Newtonsoft.Json.JsonConvert.SerializeObject(_contents);

                var cryptoJson = Encrypt(json, StringUtil.filenameWithoutPathAndExtension(FILE_NAME));

                try
                {
                    _filesystem.WriteFile(StringUtil.filenameWithoutPathAndExtension(FILE_NAME) + ".json", cryptoJson);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error occured:" + e.Data);
                }
            }
        }

        public void PersistLastLogin(DateTime lastLogin)
        {
            string strLastLoginUTC = lastLogin.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            _contents.lastloginUTC = strLastLoginUTC; // lastLogin.ToUniversalTime();
            WriteContentsAsJson();
        }

        public DateTime GetLastLogin()
        {
            DateTime.TryParse(_contents.lastloginUTC, out var lastLoginUTC);
            DateTime lastLoginLocal = lastLoginUTC == DateTime.MinValue ? lastLoginUTC : lastLoginUTC.ToLocalTime();
            return lastLoginLocal;
        }

        public void PersistCardPlanMember(PlanMember cardplanmember)
        {
            lock (_sync)
            {
                _contents.cardplanmember = cardplanmember;
                WriteContentsAsJson();
            }
        }

        public PlanMember GetCardPlanMember()
        {
            return _contents.cardplanmember;
        }

        public void PersistUserName(string username)
        {
            _contents.username = username;
            WriteContentsAsJson();
        }
        public string GetUserName()
        {
            return _contents.username;
        }

        public void PersistClaim(Claim claim)
        {
            lock (_sync)
            {
                _contents.claim = claim;
                WriteContentsAsJson();
            }
        }

        public void PersistHCSAClaim(Entities.HCSA.Claim claim)
        {
            lock (_sync)
            {
                _contents.hcsaclaim = claim;
                WriteContentsAsJson();
            }
        }
        public void PersistSelectedHCSAClaimType(Entities.HCSA.ClaimType claimType)
        {
            lock (_sync)
            {
                _contents.HCSAClaimType = claimType;
                WriteContentsAsJson();
            }
        }
        public void PersistSelectedHCSAExpenseType(Entities.HCSA.ExpenseType expenseType)
        {
            lock (_sync)
            {
                _contents.HCSAExpenseType = expenseType;
                WriteContentsAsJson();
            }
        }

        public Entities.HCSA.ExpenseType GetSelectedHCSAExpenseType()
        {
            return _contents.HCSAExpenseType;
        }
        public Entities.HCSA.ClaimType GetSelectedHCSAClaimType()
        {
            return _contents.HCSAClaimType;
        }
        public Entities.HCSA.Claim GetHCSAClaim()
        {
            return _contents.hcsaclaim;
        }
        public Claim GetClaim()
        {
            return _contents.claim;
        }

        public void PersistAcceptedTC(bool acceptedTC)
        {
            lock (_sync)
            {
                _contents.acceptedTC = acceptedTC;
                WriteContentsAsJson();
            }
        }

        public bool GetAcceptedTC()
        {
            return _contents.acceptedTC;
        }

        public void PersistIDCard(IDCard card)
        {
            lock (_sync)
            {
                _contents.idcard = card;
                WriteContentsAsJson();
            }
        }

        public IDCard GetIDCard()
        {
            return _contents.idcard;
        }

        public void PersistLoggedInState(bool loggedIn)
        {

            lock (_sync)
            {
                _contents.isloggedin = loggedIn;
                WriteContentsAsJson();
            }
        }
        public bool GetLoggedInState()
        {
            return _contents.isloggedin;
        }

        public void PersistAuthInfo(AuthInfo authInfo)
        {
            lock (_sync)
            {
                _contents.authinfo = authInfo;
            }
        }
        public AuthInfo GetAuthInfo()
        {
            return _contents.authinfo;
        }

        ///jf 7/16/2014
        ///Adding to handle the case where a *different* user logs into the device
        ///Wipe out all the existing data as a precaution
        public bool ClearPersistedData()
        {
            try
            {
                bool acceptedTC = _contents.acceptedTC;
                this._contents = new PersistedData();
                this._contents.acceptedTC = acceptedTC; //TODO: remove in phase 2? -- keep persistence of T&C
                this._contents.hcsaclaim = null;
                this._contents.HCSAClaimType = null;
                this._contents.HCSAExpenseType = null;
                this._contents.idcard = null;
                WriteContentsAsJson();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void PersistLastUserToLogin(string lastUserToLogin)
        {
            _contents.lastusertologin = lastUserToLogin;
            WriteContentsAsJson();
        }
        public string GetLastUserToLogin()
        {
            return _contents.lastusertologin;
        }

        public void PersistUseBiometricsSetting(bool activate)
        {
            _contents.usebiometrics = activate;
            WriteContentsAsJson();

            if (!activate)
            {
                Mvx.IoCProvider.Resolve<MobileClaims.Services.IBiometricsService>().RemoveStoredCredentials();
            }
        }

        public bool? GetUseBiometricsSetting()
        {
            return _contents.usebiometrics;
        }
    }
}
