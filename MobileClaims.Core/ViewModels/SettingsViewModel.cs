using MobileClaims.Core.Services;

namespace MobileClaims.Core.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        readonly IDataService _dataService;
        public SettingsViewModel(IDataService dataService)
        {
            _dataService = dataService;

			var biometricsSetting = dataService.GetUseBiometricsSetting();
			_useBiometricsSetting = biometricsSetting ?? false;
        }

		bool _useBiometricsSetting;

        public bool UseBiometricsSetting
        {
            get => _useBiometricsSetting;

            set
            {
                if (_useBiometricsSetting != value)
                {
                    _useBiometricsSetting = value;
                    RaisePropertyChanged(() => UseBiometricsSetting);
                    _dataService.PersistUseBiometricsSetting(value);
                }
            }
        }
    }
}
