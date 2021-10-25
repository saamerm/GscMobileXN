using System;

namespace MobileClaims.Core
{
	public class StringUtil
	{
		public static String fileNameWithoutExtension (String fileName)
		{
			int pos = fileName.LastIndexOf (".");

			if (pos > -1) {
				return fileName.Substring (0, pos);
			}

			return fileName;
		}

		public static String filenameWithoutPathAndExtension (String fileName )
		{
            int pos = 0;
            if (fileName.Contains("/"))
            {
                pos = fileName.LastIndexOf("/");
            }
            else
                pos = fileName.LastIndexOf(@"\");
			if (pos > -1) {
				return StringUtil.fileNameWithoutExtension (fileName.Substring (pos + 1));
			}

			return StringUtil.fileNameWithoutExtension (fileName);
		}
	}
}

