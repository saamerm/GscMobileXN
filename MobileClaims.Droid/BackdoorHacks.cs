//using Android.Runtime;
//using Java.Interop;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using MobileClaims.Core.Entities;
//using MvvmCross.Plugin.Json;

//namespace MobileClaims.Droid
//{
//    public partial class Application
//    {
//#if DEBUG
//        #region SetSelectedParticipantByNameFromListProperty
//        [Preserve]
//        [Export("SetSelectedParticipantByNameFromListProperty")]
//        public string SetSelectedParticipantByNameFromListProperty(string param)
//        {
//            var parameters = param.Split(';');
//            string leftMessage = string.Empty;
//            string rightMessage = string.Empty;
//            string resultMessage = string.Empty;
//            bool fromLeft = false;
//            bool fromRight = false;
//            IEnumerable<Participant> participantList = null;
//            var leftVM = GetTopOrLeftViewModel(out leftMessage);
//            var rightVM = GetRightViewModel(out rightMessage);
//            var listPropertyInfo = GetPropertyInfoOnVM(parameters[0], leftVM);
//            PropertyInfo selectedParticipantPropertyInfo = null;
//            if (listPropertyInfo != null)
//            {
//                participantList = listPropertyInfo.GetValue(leftVM) as IEnumerable<Participant>;
//                selectedParticipantPropertyInfo = GetPropertyInfoOnVM(parameters[1], leftVM);
//                fromLeft = true;
//            }
//            else
//            {
//                listPropertyInfo = GetPropertyInfoOnVM(parameters[0], rightVM);
//                if (listPropertyInfo != null)
//                {
//                    participantList = listPropertyInfo.GetValue(rightVM) as IEnumerable<Participant>;
//                    selectedParticipantPropertyInfo = GetPropertyInfoOnVM(parameters[1], rightVM);
//                    fromRight = true;
//                }
//                else
//                {
//                    resultMessage = string.Format("Could not find the properties {0} and {1} on any visible VM", parameters[0], parameters[1]);
//                }
//            }
//            if (participantList != null && selectedParticipantPropertyInfo != null)
//            {
//                var qry = from Participant p in participantList
//                          where p.FullName.ToLower().Equals(parameters[2].ToLower())
//                          select p;
//                if (qry.Count() == 0)
//                {
//                    resultMessage = string.Format("Couldn't find a participant with FullName: {0}.  There are {1} participants in the list", parameters[2], participantList.Count().ToString());
//                }
//                else
//                {
//                    selectedParticipantPropertyInfo.SetValue(fromLeft ? leftVM : rightVM, qry.FirstOrDefault());
//                }
//            }
//            return resultMessage;
//        }
//        #endregion

//        #region GetSerializedDrugInfoFromList
//        [Preserve]
//        [Export("GetSerializedDrugInfoFromList")]
//        public string GetSerializedDrugInfoFromList(string param)
//        {
//            string resultMessage = string.Empty;
//            string leftResult = string.Empty;
//            string rightResult = string.Empty;
//            var parameters = param.Split(';');
//            PropertyInfo listPropertyInfo = null;
//            DrugInfo drugToSerialize = null;
//            IEnumerable<DrugInfo> listProperty = null;
//            MvxJsonConverter converter = new MvxJsonConverter();
//            var leftVM = GetTopOrLeftViewModel(out resultMessage);
//            var rightVM = GetRightViewModel(out resultMessage);
//            bool fromLeft = false;
//            bool fromRight = false;
//            leftVM = GetTopOrLeftViewModel(out leftResult);
//            rightVM = GetRightViewModel(out rightResult);
//            listPropertyInfo = GetPropertyInfoOnVM(parameters[0], leftVM);

//            if (listPropertyInfo != null)
//            {
//                fromLeft = true;
//            }
//            else
//            {
//                listPropertyInfo = GetPropertyInfoOnVM(parameters[0], rightVM);
//                if (listPropertyInfo != null)
//                {
//                    fromRight = true;
//                }
//                else
//                {
//                    return string.Format("Couldn't find a list property with the name {0} on any visible VM", parameters[0]);
//                }
//            }
//            listProperty = listPropertyInfo.GetValue(fromLeft ? leftVM : rightVM) as IEnumerable<DrugInfo>;

//            var qry = from DrugInfo di in listProperty
//                      where di.DIN.ToString().Equals(parameters[1])
//                      select di;
//            drugToSerialize = qry.FirstOrDefault();
//            if (drugToSerialize == null)
//            {
//                return string.Format("Couldn't find a drug in the list that matches the name {0}.  There are {1} drugs in the list of search results", parameters[1], listProperty.Count().ToString());
//            }
//            return converter.SerializeObject(drugToSerialize);

//        }
//        #endregion

//        #region GetSerializedParticipantFromList
//        [Preserve]
//        [Export("GetSerializedParticipantFromList")]
//        public string GetSerializedParticipantFromList(string param)
//        {
//            string resultMessage = string.Empty;
//            string leftResult = string.Empty;
//            string rightResult = string.Empty;
//            var parameters = param.Split(';');
//            PropertyInfo listPropertyInfo = null;
//            Participant participantToSerialize = null;
//            IEnumerable<Participant> listProperty = null;
//            MvxJsonConverter converter = new MvxJsonConverter();
//            var leftVM = GetTopOrLeftViewModel(out resultMessage);
//            var rightVM = GetRightViewModel(out resultMessage);
//            bool fromLeft = false;
//            bool fromRight = false;
//            leftVM = GetTopOrLeftViewModel(out leftResult);
//            rightVM = GetRightViewModel(out rightResult);
//            listPropertyInfo = GetPropertyInfoOnVM(parameters[0], leftVM);

//            if (listPropertyInfo != null)
//            {
//                fromLeft = true;
//            }
//            else
//            {
//                listPropertyInfo = GetPropertyInfoOnVM(parameters[0], rightVM);
//                if (listPropertyInfo != null)
//                {
//                    fromRight = true;
//                }
//                else
//                {
//                    return string.Format("Couldn't find a list property with the name {0} on any visible VM", parameters[0]);
//                }
//            }
//            listProperty = listPropertyInfo.GetValue(fromLeft ? leftVM : rightVM) as IEnumerable<Participant>;

//            var qry = from Participant p in listProperty
//                      where p.FullName.ToLower().ToString().Equals(parameters[1].ToLower())
//                      select p;
//            participantToSerialize = qry.FirstOrDefault();
//            if (participantToSerialize == null)
//            {
//                return string.Format("Couldn't find a participant in the list that matches the name {0}.  There are {1} participants in the list of search results", parameters[1], listProperty.Count().ToString());
//            }
//            return converter.SerializeObject(participantToSerialize);

//        }
//        #endregion
//#endif
//    }
//}
