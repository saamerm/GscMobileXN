//using MvvmCross;
//using MvvmCross.Platforms.Android.Views.Fragments;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using MvvmCross.Commands;
//using MvvmCross.Platforms.Android.Core;
//using MvvmCross.Platforms.Android.Views;
//using MvvmCross.ViewModels;

//namespace MobileClaims.Droid
//{
//    public partial class Application
//    {
//#if DEBUG
//        #region Backdoor Helpers
//        #region GetPropertyOnVM
//        public PropertyInfo GetPropertyInfoOnVM(string propertyName, IMvxViewModel vm)
//        {
//            var pInfo = vm.GetType().GetProperties().Where(pi => pi.Name.Equals(propertyName)).FirstOrDefault();
//            return pInfo;
//        }
//        #endregion
//        #region GetPopupViewModel

//        #endregion
//        #region getRightViewModel
//        [Android.Runtime.Preserve]
//        public IMvxViewModel GetRightViewModel(out string resultMessage)
//        {
//            resultMessage = string.Empty;
//            var activityMonitor = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>() as MvxAndroidLifetimeMonitor;
//            var frag = activityMonitor.Activity as MvxFragmentActivity;

//            if (frag == null)
//            {
//                resultMessage = "What the hell";
//                return null;
//            }
//            var fragments = frag.FragmentManager.Fragments;
//            IEnumerable<MvxFragment> rightQry;
//            if (fragments != null)
//            {
//                rightQry = from MvxFragment f in fragments
//                           where f.HasRegionAttribute() &&
//                                  f.GetRegionId() == Resource.Id.right_region
//                           select f;
//                resultMessage += "\nExecuting Right Query";
//                if (rightQry.Count() > 0)
//                {
//                    var ret = rightQry.Last().ViewModel as IMvxViewModel;
//                    resultMessage += string.Format("\nRight View model type is {0}", ret != null ? ret.GetType().Name : "null");
//                    return ret;
//                }
//            }
//            else
//            {
//                var vm = frag.ViewModel;
//                if (vm != null)
//                {
//                    resultMessage = string.Empty;
//                    return vm as IMvxViewModel;
//                }
//            }
//            resultMessage = "No view model to return yet";
//            return null;
//        }
//        #endregion
//        #region GetModalViewModel

//        #endregion
//        #region GetCommandInAnyVisibleViewModel
//        public IMvxCommand GetCommandInAnyVisibleViewModel(string commandName, out string resultMessage)
//        {
//            var activityMonitor = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>() as MvxAndroidLifetimeMonitor;
//            var frag = activityMonitor.Activity as MvxFragmentActivity;


//            if (frag == null)
//            {
//                resultMessage = "What the hell";
//                return null;
//            }
//            var fragments = frag.FragmentManager.Fragments;
//            var qry = from MvxFragment f in fragments
//                      where f.IsVisible
//                      select f;
//            foreach(MvxFragment fragment in qry)
//            {
//                var cmd = GetParamaterlessCommandOnVM(fragment.ViewModel, commandName, out resultMessage);
//                if ( cmd != null)
//                {
//                    resultMessage = string.Empty;
//                    return cmd;
//                }
//            }
//            resultMessage = string.Format("Couldn't find a command named {0} on any visible viewmodel.  Sorry!", commandName);
//            return null;
//        }
//        #endregion
//        #region GetTopOrLeftViewModel
//        [Android.Runtime.Preserve]
//        public IMvxViewModel GetTopOrLeftViewModel(out string resultMessage)
//        {
//            resultMessage = string.Empty;
//            try
//            {

//                var activityMonitor = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>() as MvxAndroidLifetimeMonitor;
//                var frag = activityMonitor.Activity as MvxFragmentActivity;


//                if (frag == null)
//                {
//                    resultMessage = "What the hell";
//                    return null;
//                }
//                var fragments = frag.FragmentManager.Fragments;
//                IEnumerable<MvxFragment> leftQry;
//                IEnumerable<MvxFragment> modalQry;
//                IEnumerable<MvxFragment> rightQry;
//                IEnumerable<MvxFragment> popupQry = new List<MvxFragment>();
//                if (fragments != null)
//                {
//                    resultMessage += string.Format("\nBuilding Left Query. fragments {0} null", fragments == null ? "is" : "is not");
//                    leftQry = from MvxFragment f in fragments
//                              where f.HasRegionAttribute() &&
//                                     f.GetRegionId() == Resource.Id.left_region
//                              select f;
//                    resultMessage += string.Format("\nBuilding Right Query. fragments {0} null", fragments == null ? "is" : "is not");
//                    rightQry = from MvxFragment f in fragments
//                               where f.HasRegionAttribute() &&
//                                      f.GetRegionId() == Resource.Id.right_region
//                               select f;
//                    resultMessage += string.Format("\nBuilding Modal Query. fragments {0} null", fragments == null ? "is" : "is not");
//                    modalQry = from MvxFragment f in fragments
//                               where f.HasRegionAttribute() &&
//                                      f.GetRegionId() == Resource.Id.phone_main_region
//                               select f;
//                    resultMessage += string.Format("\nBuilding popup query.  fragments {0} null", fragments == null ? "is" : "is not");
//                    try
//                    {
//                        resultMessage += "\nExecuting Modal Query";
//                        if (modalQry.Count() > 0)
//                        {

//                            return modalQry.Last().ViewModel as IMvxViewModel;
//                        }
//                    }
//                    catch(Exception ex)
//                    {
//                        resultMessage += string.Format("Modal query failed with exception \n {0}", ex.ToString());
//                    }
//                    try
//                    {

//                        resultMessage += "\nExecuting Left Query";
//                        if (leftQry.Count() > 0)
//                        {

//                            var ret = leftQry.Last().ViewModel as IMvxViewModel;
//                            return ret;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        resultMessage += string.Format("Left query failed with exception \n {0}", ex.ToString());
//                    }
//                    try
//                    {
//                        resultMessage += "\nExecuting Popup Query";
//                        if(popupQry.Count() > 0)
//                        {
//                            return popupQry.Last().ViewModel as IMvxViewModel;
//                        }
//                    }
//                    catch(Exception ex)
//                    {
//                        resultMessage += string.Format("Popup query failed with exception \n {0}", ex.ToString());
//                    }
//                }
//                else
//                {
//                    var vm = frag.ViewModel;
//                    if (vm != null)
//                    {
//                        resultMessage = string.Empty;
//                        return vm as IMvxViewModel;
//                    }
//                }
//                resultMessage += "\nNo view model to return yet";
//                return null;
//            }
//            catch(Exception ex)
//            {
//                resultMessage += "\n" + ex.ToString();
//                return null;
//            }
//        }
//        #endregion

//        #region GetParamaterlessCommandOnVM
//        private IMvxCommand GetParamaterlessCommandOnVM(IMvxViewModel vm, string commandName, out string resultMessage)
//        {
//            resultMessage = string.Empty;
//            if (vm == null)
//            {
//                resultMessage += "Passed a null vm!";
//                return null;
//            }
//            try
//            {
//                var cmd = vm.GetType().GetProperties().Where(pi => pi.Name == commandName).FirstOrDefault();
//                if (cmd != null)
//                {
//                    var command = cmd.GetValue(vm) as IMvxCommand;
//                    if (command != null)
//                    {
//                        resultMessage = string.Empty;
//                        return command;
//                    }
//                    else
//                    {
//                        resultMessage += string.Format("\nCouldn't cast the property {0} to type IMvxCommand from Type {1}", commandName, cmd.PropertyType.Name);
//                    }
//                }
//                resultMessage += string.Format("\nCouldn't find a command named {0} on VM {1}", commandName, vm.GetType().Name);
//                return null;
//            }
//            catch(Exception ex)
//            {
//                resultMessage += ex.ToString();
//                return null;
//            }
//        }
//        #endregion
//        #endregion
//#endif
//    }
//}
