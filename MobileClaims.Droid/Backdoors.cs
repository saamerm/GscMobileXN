//using Java.Interop;
//using MobileClaims.Core.Services;
//using MvvmCross;
//using MvvmCross.Platforms.Android.Views.Fragments;
//using MvvmCross.Plugin.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using MvvmCross.Commands;
//using MvvmCross.Platforms.Android.Core;
//using MvvmCross.Platforms.Android.Views;
//using MvvmCross.ViewModels;

//namespace MobileClaims.Droid
//{
//    public partial class Application
//    {
//#if DEBUG
//        #region Backdoors
//        #region IsCommandEnabled
//        [Android.Runtime.Preserve]
//        [Export("IsCommandEnabled")]
//        public string IsCommandEnabled(string param)
//        {
//            var parameters = param.Split(';');
//            string resultMessage = string.Empty;
//            var commandName = parameters[0];
//            IMvxCommand command = null;
//            IMvxViewModel leftVM;
//            IMvxViewModel rightVM;
//            leftVM = GetTopOrLeftViewModel(out resultMessage);
//            rightVM = GetRightViewModel(out resultMessage);
//            MvxJsonConverter converter = new MvxJsonConverter();
//            command = GetParamaterlessCommandOnVM(leftVM, commandName, out resultMessage);
//            if (command == null)
//            {
//                command = GetParamaterlessCommandOnVM(rightVM, commandName, out resultMessage);
//                if (command == null)
//                {
//                    return string.Format("Couldn't find a command with the name {0} on any visible VM", commandName);
//                }
//            }
//            if (command.GetType().GenericTypeArguments.Count() > 0)
//            {
//                Type t = command.GetType().GenericTypeArguments[0];
//                var parameter = converter.DeserializeObject(t, parameters[1]);
//                return command.CanExecute(parameter).ToString();
//            }
//            else
//            {
//                return command.CanExecute(null).ToString();
//            }
//        }
//        #endregion

//        #region ExecuteParamaterizedCommandOnVM
//        [Android.Runtime.Preserve]
//        [Export("ExecuteParamaterizedCommandOnVM")]
//        public string ExecuteParamaterizedCommandOnVM(string parameters)
//        {
//            string resultMessage;

//            var param = parameters.Split(';');
//            IMvxViewModel leftVM = null;
//            IMvxViewModel rightVM = null;
//            bool fromLeft = false;
//            bool fromRight = false;
//            IMvxCommand command = null;
//            leftVM = GetTopOrLeftViewModel(out resultMessage);
//            rightVM = GetRightViewModel(out resultMessage);
//            MvxJsonConverter converter = new MvxJsonConverter();
//            command = GetParamaterlessCommandOnVM(leftVM, param[0].ToString(), out resultMessage);
//            if (command != null)
//            {
//                fromLeft = true;
//            }
//            else
//            {
//                command = GetParamaterlessCommandOnVM(rightVM, param[0].ToString(), out resultMessage);
//                if (command != null)
//                {
//                    fromRight = true;
//                }
//                else
//                {
//                    return string.Format("Couldn't find a Command with the name {0} on any visible VM", param[0].ToString());
//                }
//            }
//            Type t = command.GetType().GenericTypeArguments[0];
//            if (t == null)
//            {
//                return "No generic parameters for the command were found";
//            }
//            var commandParameter = converter.DeserializeObject(t, param[1].ToString());
//            if (commandParameter == null)
//            {
//                return string.Format("Couldn't deserialize the parameter: {0}", param[1].ToString());
//            }
//            IMvxCommand icommand = command as IMvxCommand;
//            if (icommand == null)
//            {
//                return "Couldn't cast command to IMvxCommand";
//            }
//            try
//            {
//                icommand.Execute(commandParameter);
//                return string.Empty;
//            }
//            catch (Exception ex)
//            {
//                return ex.ToString();
//            }
//        }
//        #endregion

//        #region ExecuteParameterlessCommandOnVM
//        [Android.Runtime.Preserve]
//        [Export("ExecuteParameterlessCommandOnVM")]
//        public string ExecuteParameterlessCommandOnVM(string param)
//        {
//            string message=string.Empty;
//            try
//            {
//                IMvxCommand cmd=null;
//                var vm = GetTopOrLeftViewModel(out message);
//                if (vm != null)
//                {
//                    cmd = GetParamaterlessCommandOnVM(vm, param, out message);
//                    if (cmd != null)
//                    {
//                        cmd.Execute(null);
//                        return string.Empty;
//                    }

//                }
//                if(cmd==null)
//                {
//                    string rightMessage = string.Empty;
//                    string rightCommandMessage = string.Empty;
//                    vm = GetRightViewModel(out rightMessage);
//                    if (vm != null)
//                    {
//                        cmd = GetParamaterlessCommandOnVM(vm, param, out rightCommandMessage);
//                        if (cmd != null)
//                        {
//                            cmd.Execute(null);
//                            return string.Empty;
//                        }
//                        else
//                        {
//                            string popupMessage = string.Empty;
//                            string popupCommandMessage = string.Empty;
//                            vm = GetPopupViewModel(out popupMessage);
//                            if(vm!=null)
//                            {
//                                cmd = GetParamaterlessCommandOnVM(vm, param, out popupCommandMessage);
//                                if(cmd != null)
//                                {
//                                    cmd.Execute(null);
//                                    return string.Empty;
//                                }
//                                else
//                                {
//                                    message += "\n" + rightMessage + "\n" + popupMessage;
//                                }
//                            }
//                        }
//                    }
//                }
//                return string.Format("{0}\n{1}","VM is null", message);
//            }
//            catch(Exception ex)
//            {
//                return string.Format("{0}\n{1}", message, ex.ToString());
//            }
//        }
//        #endregion

//        #region ChangePropertyValue
//        [Export("ChangePropertyValue")]
//        public string ChangePropertyValue(string parameters)
//        {
//            try
//            {
//                var Parameters = parameters.Split(';');
//                var activityMonitor = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>();
//                if (activityMonitor == null) return ("No activity monitor!");
//                var monitor = activityMonitor as MvxAndroidLifetimeMonitor;
//                if (monitor == null) return "Couldn't cast IMvxAndroidActivityLifetimeListener to MvxAndroidLifetimeMonitor";
//                var activity = monitor.Activity as MvxActivity;
//                if (activity == null) return ("No activity!");
//                var vm = activity.ViewModel;
//                if (vm == null) return ("No vm!");
//                var propertyInfo = vm.GetType().GetProperties().Where(p => p.Name == Parameters[0]).FirstOrDefault();
//                propertyInfo.SetValue(vm, Parameters[1]);
//                return ("success!");
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
//        #endregion

//        #region GetSystemLanguage
//        [Export("GetSystemLanguage")]
//        [Android.Runtime.Preserve]
//        public string GetSystemLanguage(string param)
//        {
//            try
//            {
//                var languageService = Mvx.IoCProvider.Resolve<ILanguageService>();
//                return languageService.CurrentLanguage;
//            }
//            catch (Exception ex)
//            {
//                return ex.ToString();
//            }
//        }
//        #endregion

//        #region SetBooleanPropertyValue
//        [Export("SetBooleanPropertyValue")]
//        [Android.Runtime.Preserve]
//        public string SetBooleanPropertyValue(string param)
//        {
//            var parameters = param.Split(';');
//            string resultMessage = string.Empty;
//            var vm = GetTopOrLeftViewModel(out resultMessage);
//            if (vm == null)
//            {
//                //get right side VM
//            }
//            if (vm == null)
//            {
//                resultMessage = "Couldn't get a VM!";

//            }
//            var pinfo = vm.GetType().GetProperties().Where(pi => pi.Name.Equals(parameters[0])).FirstOrDefault();
//            if (pinfo == null)
//            {
//                resultMessage = string.Format("Couldn't find a property named {0} on ViewModel: {1}", parameters[0], vm.GetType().Name);
//                return resultMessage;
//            }
//            try
//            {
//                pinfo.SetValue(vm, System.Convert.ToBoolean(parameters[1]));
//                resultMessage = string.Empty;
//            }
//            catch (Exception ex)
//            {
//                resultMessage = string.Format("An error occurred while setting the property {0} on VM {1}.  The exception encountered was \r\n", parameters[0], vm.GetType().Name, ex.ToString());
//            }
//            return resultMessage;
//        }
//        #endregion

//        #region SetPropertyValue
//        [Android.Runtime.Preserve]
//        [Export("SetPropertyValue")]
//        public string SetPropertyValue(string param)
//        {
//            try
//            {
//                var parameters = param.Split(';');
//                string leftMessage;
//                string rightMessage;
//                var leftVM = GetTopOrLeftViewModel(out leftMessage);
//                var rightVM = GetRightViewModel(out rightMessage);

//                var pinfo = GetPropertyInfoOnVM(parameters[0], leftVM);
//                if (pinfo != null)
//                {
//                    Type t = pinfo.PropertyType;
//                    var val = new MvxJsonConverter().DeserializeObject(t,parameters[1]);
//                    pinfo.SetValue(leftVM, val);
//                    return string.Empty;
//                }
//                pinfo = GetPropertyInfoOnVM(parameters[0], rightVM);
//                if (pinfo != null)
//                {
//                    Type t = pinfo.PropertyType;
//                    var val = new MvxJsonConverter().DeserializeObject(t, parameters[1]);
//                    pinfo.SetValue(rightVM, val);
//                    return string.Empty;
//                }
//                return string.Format("Couldn't find a property with the name {0} on left viewmodel of type {1} or right view model of type{2}", parameters[0], leftVM.GetType().Name, rightVM.GetType().Name);
//            }
//            catch (Exception ex)
//            {
//                return ex.ToString();
//            }
//        }
//        #endregion
//        #region SetStringPropertyValue
//        [Android.Runtime.Preserve]
//        [Export("SetStringPropertyValue")]
//        public string SetStringPropertyValue(string param)
//        {
//            try
//            {
//                var parameters = param.Split(';');
//                string leftMessage;
//                string rightMessage;
//                var leftVM = GetTopOrLeftViewModel(out leftMessage);
//                var rightVM = GetRightViewModel(out rightMessage);

//                var pinfo = GetPropertyInfoOnVM(parameters[0], leftVM);
//                if (pinfo != null)
//                {
//                    Type t = pinfo.PropertyType;
//                    var val = Convert.ChangeType(parameters[1], t);
//                    pinfo.SetValue(leftVM, val);
//                    return string.Empty;
//                }
//                pinfo = GetPropertyInfoOnVM(parameters[0], rightVM);
//                if (pinfo != null)
//                {
//                    Type t = pinfo.PropertyType;
//                    var val = Convert.ChangeType(parameters[1], t);
//                    pinfo.SetValue(rightVM, val);
//                    return string.Empty;
//                }
//                return string.Format("Couldn't find a property with the name {0} on left viewmodel of type {1} or right view model of type{2}", parameters[0], leftVM.GetType().Name, rightVM.GetType().Name);
//            }
//            catch (Exception ex)
//            {
//                return ex.ToString();
//            }
//        }
//        #endregion
//        #region GetPopupViewModel
//        public IMvxViewModel GetPopupViewModel(out string message)
//        {
//            var activityMonitor = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>() as MvxAndroidLifetimeMonitor;
//            var frag = activityMonitor.Activity as MvxFragmentActivity;
//            message = string.Empty;

//            if (frag == null)
//            {
//                message = "What the hell";
//                return null;
//            }
//            var fragments = frag.FragmentManager.Fragments;
//            IEnumerable<MvxFragment> popupQry = new List<MvxFragment>();
//            if (fragments != null)
//            {
//                try
//                {
//                    if (popupQry.Count() > 0)
//                    {
//                        return (popupQry.Last().ViewModel as IMvxViewModel);
//                    }
//                    else
//                    {
//                        message += "Couldn't find a popup view model";
//                        return null;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    message += string.Format("Popup query failed with exception \n {0}", ex.ToString());
//                    return null;
//                }
//            }
//            else
//            {
//                message = "Couldn't find a popup view model";
//                return null;
//            }
//        }
//        #endregion
//        #region GetPopupViewModel
//        [Export("GetPopupViewModel")]
//        [Android.Runtime.Preserve]
//        public string GetPopupViewModel()
//        {
//            string message;
//            var vm = GetPopupViewModel(out message);
//            if (vm == null)
//            {
//                return message;
//            }
//            else
//            {
//                return vm.GetType().Name;
//            }
//        }
//        #endregion
//        #region GetRightViewModel
//        [Export("GetRightViewModel")]
//        [Android.Runtime.Preserve]
//        public string GetRightViewModel()
//        {
//            try
//            {
//                string message;
//                var vm = GetRightViewModel(out message);
//                if (vm == null)
//                {
//                    return string.Empty;
//                }
//                return vm.GetType().Name;
//            }
//            catch(Exception ex)
//            {
//                return ex.ToString();
//            }
//        }
//        #endregion

//        #region GetTopOrLeftViewModel
//        [Export("GetTopOrLeftViewModel")]
//        [Android.Runtime.Preserve]
//        public string GetTopOrLeftViewModel()
//        {
//            string message;
//            var vm = GetTopOrLeftViewModel(out message);
//            if (vm == null)
//            {
//                return message;
//            }
//            return vm.GetType().Name;
//        }
//        #endregion

//        #region IsTablet
//        [Export("IsTablet")]
//        [Android.Runtime.Preserve]
//        public bool IsTablet()
//        {
//            var deviceService = Mvx.IoCProvider.Resolve<IDeviceService>();
//            return this.Resources.GetBoolean(Resource.Boolean.isTablet);
//            //return deviceService.IsTablet;
//        }
//        #endregion

//        #region GetPropertyValue
//        [Android.Runtime.Preserve]
//        [Export("GetPropertyValue")]
//        public string GetPropertyValue(string propertyName)
//        {
//            StringBuilder trace = new StringBuilder();
//            try
//            {
//                string message;
//                string[] dottedPropertyName = null;
//                trace.AppendLine("Initialized dottedPropertyName");
//                if (propertyName.Contains("."))
//                {
//                    trace.AppendLine("propertyName contains .");
//                    dottedPropertyName = propertyName.Split('.');
//                    trace.AppendLine(string.Format("{0} elements in dottedPropertyName", dottedPropertyName.Count().ToString()));
//                }
//                if (dottedPropertyName != null)
//                {
//                    trace.AppendLine(string.Format("Setting propertyName to {0}", dottedPropertyName[0]));
//                    propertyName = dottedPropertyName[0];
//                }
//                var leftVM = GetTopOrLeftViewModel(out message);
//                trace.AppendLine(string.Format("leftVM {0} null.  Name is {1}", leftVM == null ? "is" : "is not", leftVM != null ? leftVM.GetType().Name : "not available"));
//                var rightVM = GetRightViewModel(out message);
//                trace.AppendLine(string.Format("rightVM {0} null. Name is {1}", rightVM == null ? "is" : "is not", rightVM != null ? rightVM.GetType().Name : "not available"));
//                trace.AppendLine(string.Format("propertyName is {0}", propertyName));
//                PropertyInfo pinfo;
//                //MvxJsonConverter converter = new MvxJsonConverter ();
//                object value = null;
//                pinfo = GetPropertyInfoOnVM(propertyName, leftVM);
//                if (pinfo != null)
//                {
//                    value = pinfo.GetValue(leftVM);
//                }
//                else
//                {
//                    pinfo = GetPropertyInfoOnVM(propertyName, rightVM);
//                    if (pinfo != null)
//                    {
//                        value = pinfo.GetValue(rightVM);
//                    }
//                }
//                trace.AppendLine(string.Format("value {0} null", value == null ? "is" : "is not"));
//                if (dottedPropertyName == null && value != null)
//                {
//                    return new MvxJsonConverter().SerializeObject(value);
//                }
//                else
//                {
//                    if (dottedPropertyName != null && value != null)
//                    {
//                        pinfo = GetPropertyInfoOnVM(dottedPropertyName[1], (IMvxViewModel)value);
//                        value = pinfo.GetValue((IMvxViewModel)value);
//                        return new MvxJsonConverter().SerializeObject(value);
//                    }
//                }
//                return trace.ToString();
//            }
//            catch (Exception ex)
//            {
//                return string.Format("{0}\n{1}",ex.ToString(), trace.ToString());
//            }
//        }
//        #endregion
//        #endregion
//#endif
//    }
//}
