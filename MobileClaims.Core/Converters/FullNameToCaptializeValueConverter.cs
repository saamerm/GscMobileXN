using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class FullNameToCaptializeValueConverter: MvxValueConverter  
    {
        public override  object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value != null)
            {
                result = value.ToString();
                string[] Names=new string[2];
                string NewName = string.Empty;
                if (result.Contains(" "))
                { 
                    Names =result.Split(new char[]{' '},2);
                    foreach (string n in Names)
                    {
                        string nameStr = string.Empty;
                        int countOfName = n.Length;
                        if (countOfName > 0)
                        {
                            for (int i = 0; i < countOfName; i++)
                            {
                                string currentLetter = string.Empty;
                                if (i == 0)
                                {
                                    currentLetter = n.Substring(i, 1);
                                    currentLetter = currentLetter.ToUpper();
                                    nameStr += currentLetter;
                                }
                                else
                                {
                                    currentLetter = n.Substring(i, 1);
                                    currentLetter = currentLetter.ToLower();
                                    nameStr += currentLetter;
                                } 
                            } 
                            NewName += " " + nameStr;
                        } 
                    } 
                    result = NewName.Substring(1, NewName.Length - 1);
                } 
            }
            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty; 
            return result;
        }
    }
}

