using MobileClaims.Core.Messages;
using System;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Location;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly IMvxMessenger _messenger;
        private  IMvxLocationWatcher _location;
        private IMvxLog _log;
        #region ctors
        public LocationService(IMvxMessenger messenger, IMvxLocationWatcher location, IMvxLog log)
        {
            _messenger = messenger;
            _location = location;
            _log = log;

            LocationAvailable = false;
            MvxLocationOptions options = new MvxLocationOptions() {
                Accuracy = MvxLocationAccuracy.Fine,
                MovementThresholdInM = 100,
                TimeBetweenUpdates = new TimeSpan(0, 5, 0)
            };
            InitializeLocation(options);

        }
        #endregion
        private DateTime _locationAcquired;
        void InitializeLocation(MvxLocationOptions options)
        {
                _location.Start(options, loc =>
                {
                    this.Latitude = loc.Coordinates.Latitude;
                    this.Longitude = loc.Coordinates.Longitude;
#if DEBUGTestCloud
				 this.Latitude = 43.6477560;
                 this.Longitude = -79.3918450;
#endif

					LocationAvailable = true;
                    _locationAcquired = DateTime.Now;
                    _messenger.Publish<LocationUpdated>(new LocationUpdated(this));
                }, error =>
                {
                    if (options.Accuracy == MvxLocationAccuracy.Fine && (DateTime.Now - _locationAcquired).TotalSeconds > 30)
                    {
                        options.Accuracy = MvxLocationAccuracy.Coarse;
                        _location.Stop();
                        InitializeLocation(options);
                    }
                    if ((DateTime.Now - _locationAcquired).TotalSeconds > 30)
                    {
                        LocationAvailable = false;
                        _log.Trace(error.Code.ToString());
                        _messenger.Publish<GetLocationError>(new GetLocationError(this)
                        {
                            Message = error.ToString()
                        });
                    }
                });

        }

        #region Properties
        public bool LocationAvailable { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }
        #endregion

        #region Methods
        public void GetCurrentPosition()
        {
            MvxGeoLocation _current = null;
            try
            {
                _current = _location.CurrentLocation;
                this.Latitude = _current.Coordinates.Latitude;
                this.Longitude = _current.Coordinates.Longitude;
                LocationAvailable = true;
                _messenger.Publish<LocationUpdated>(new LocationUpdated(this));
            }
            catch (Exception ex)
            {
                _current = null;
                LocationAvailable = false;
                _log.Error(ex, ex.ToString());
                _messenger.Publish<GetLocationError>(new GetLocationError(this) { Message = ex.ToString() });
            }
        }
        #endregion
    }
}
