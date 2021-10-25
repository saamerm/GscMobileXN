using System;
using System.Text;
using MvvmCross.Plugin.File;

namespace MobileClaims.Core.Services
{
    public class LoggerService : ILoggerService
    {
        DateTime datet = DateTime.Now;
        private readonly string FILE_NAME = string.Empty;
        private readonly IMvxFileStore _filesystem;
        private object _sync = new object();
        private StringBuilder sb;
        
        public LoggerService(IMvxFileStore filesystem)
        {
            _filesystem = filesystem;
            FILE_NAME = _filesystem.NativePath("GSCLogger" + datet.ToString("-MMMdd-hhmmss") + ".log");
            sb = new StringBuilder();
        }

        public void WriteLine(string line)
        {
            datet = DateTime.Now;
            sb.AppendLine(datet.ToString("MM/dd hh:mm:ss") + "> " + line);
            try
            {
                lock (_sync)
                {
                    _filesystem.WriteFile(FILE_NAME, sb.ToString());
                }
            }
            catch
            {

            }

        }

        public string AH
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
				string ahValue = "Z3NjbW9iaW";
                return ahValue;
            }
        }

        public string AHX // New Property
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
                string ahxValue = "aW50WGVyb3";
                return ahxValue;
            }
        }

    }
}
