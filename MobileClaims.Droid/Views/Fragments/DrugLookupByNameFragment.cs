using Android.OS;
using Android.Views;

using MobileClaims.Core.ViewModels;
using System;
using Android.App;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views
{
	public class DrugLookupByNameFragment_ : BaseFragment
    {
		DrugLookupByNameViewModel _model;
		bool dialogShown;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.DrugLookupByNameFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (DrugLookupByNameViewModel)ViewModel;
			try{
			var list = Activity.FindViewById(Resource.Id.participant_list) as MvxListView;
			list.Adapter = new SingleSelectionMvxAdapter (this.Activity, (IMvxAndroidBindingContext)BindingContext);

			var item = this.View.FindViewById(Resource.Id.button_search_name);
			if(item != null)
			{
				item.Click+=(object newSender, EventArgs newE) => 
				{
					_model.SearchAndNavigateCommand.Execute(null);
				};
			}
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName=="DrugName" || e.PropertyName == "SelectedParticipant")
				{
					var theButton = this.View.FindViewById(Resource.Id.button_search_name);
					if(_model.SelectedParticipant!=null && !string.IsNullOrEmpty(_model.DrugName))
					{
						theButton.Enabled=true;
					}else{
						theButton.Enabled=false;
					}
                }else if(e.PropertyName == "ErrorInSearch" && _model.ErrorInSearch){
					ShowSearchError();
				}
			};
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}
		}

		void ShowSearchError ()
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.noresults)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.noresultsdetails)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

	}
}

