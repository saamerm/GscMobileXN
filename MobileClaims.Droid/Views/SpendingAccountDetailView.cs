using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Views;
using System;
using System.Globalization;

namespace MobileClaims.Droid
{
    [Region(Resource.Id.phone_main_region)]
   
	public class SpendingAccountDetailView : BaseFragment, IMvxView
	{
		SpendingAccountDetailViewModel _model;
		NonSelectableList _nonSelectableList;
        TextView noContributions;
        TextView total_remaining_text;
        CustomAdapter refAdapter;
       
        public static int Total;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			// Set the layout
         
			View _view= this.BindingInflate(Resource.Layout.SpendingAccountDetailFragment, null);
            _nonSelectableList = _view.FindViewById<NonSelectableList>(Resource.Id.lstAccountRollups);
            total_remaining_text = _view.FindViewById<TextView>(Resource.Id.dsTxtTotalRemaining_txt);
                    
            
            return _view;
		}

        public bool BackPressHandled { get; set; }
        public void OnBackPressed()
        {
            BackPressHandled = true;
            _model.BackBtnClickCommandDroid.Execute();
        }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                _model = (SpendingAccountDetailViewModel)this.ViewModel;
                CustomAdapter refAdapter = new CustomAdapter(Activity, (IMvxAndroidBindingContext)BindingContext, _model.SpendingAccountDetails);
                _nonSelectableList.Adapter = refAdapter;
                DollarSignTextView dsTxtTotalRemaining_sample = view.FindViewById<DollarSignTextView>(Resource.Id.dsTxtTotalRemaining_sample);
                Typeface nunitoFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");
                dsTxtTotalRemaining_sample.SetTypeface(nunitoFont, TypefaceStyle.Bold);
                Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
                total_remaining_text.SetTypeface(leagueFont, TypefaceStyle.Bold);
            
                if (_model != null && _model.SpendingAccountDetails != null)
                {
                    dsTxtTotalRemaining_sample.DollarString = _model.SpendingAccountDetails.TotalRemaining.ToString();
                }
                else
                {
                    dsTxtTotalRemaining_sample.DollarString = 0d.ToString();
                }


                if (this.Resources.GetBoolean(Resource.Boolean.isTablet))
                {
                    FrameLayout total_remaining_layout = this.Activity.FindViewById<FrameLayout>(Resource.Id.total_remaining_ll);
                    total_remaining_layout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 0);
                }

                if (_nonSelectableList.Adapter.Count == 0)
                {
                    noContributions = view.FindViewById<TextView>(Resource.Id.lblcontributions);
                    noContributions.Text = Activity.ApplicationContext.GetString(Resource.String.no_contributions_found);

                    
                    FrameLayout total_remaining_layout = view.FindViewById<FrameLayout>(Resource.Id.total_remaining_ll);
                    total_remaining_layout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent,1);
                }

                ProgressDialog progressDialog = null;
                if (_model.Busy)
                {
                    this.Activity.RunOnUiThread(() =>
                    {
                        progressDialog = ProgressDialog.Show(this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                    });
                }

                _model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {

                    try
                    {

                        if (e.PropertyName.Equals("SpendingAccountDetails"))
                        {
                            
                            CustomAdapter refAdapter1= new CustomAdapter(Activity, (IMvxAndroidBindingContext)BindingContext, _model.SpendingAccountDetails);
                            _nonSelectableList.Adapter = refAdapter1;
                            if (refAdapter1.Count > 0)
                            {
                                noContributions.Text = "";
                            }

                            if (_model != null && _model.SpendingAccountDetails != null)
                            {
                                dsTxtTotalRemaining_sample.DollarString = _model.SpendingAccountDetails.TotalRemaining.ToString();
                            }
                            else
                            {
                                dsTxtTotalRemaining_sample.DollarString = 0d.ToString();
                            }

                            if (this.Resources.GetBoolean(Resource.Boolean.isTablet))
                            {
                                FrameLayout total_remaining_layout = this.Activity.FindViewById<FrameLayout>(Resource.Id.total_remaining_ll);
                                total_remaining_layout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 0);
                            }

                            if (_nonSelectableList.Adapter.Count == 0)
                            {
                                noContributions = view.FindViewById<TextView>(Resource.Id.lblcontributions);
                                noContributions.Text = Activity.ApplicationContext.GetString(Resource.String.no_contributions_found);


                                FrameLayout total_remaining_layout = view.FindViewById<FrameLayout>(Resource.Id.total_remaining_ll);
                                total_remaining_layout.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent, 1);
                            }
                        }


                        if (e.PropertyName.Equals("Busy"))
                        {
                            if (_model.Busy)
                            {
                                this.Activity.RunOnUiThread(() =>
                                {
                                    if (progressDialog == null)
                                    {
                                        progressDialog = ProgressDialog.Show(this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                                    }
                                    else
                                    {
                                        if (!progressDialog.IsShowing)
                                        {
                                            progressDialog.Show();
                                        }
                                    }
                                });
                            }
                            else
                            {
                                this.Activity.RunOnUiThread(() =>
                                {
                                    if (progressDialog != null && progressDialog.IsShowing)
                                    {
                                        progressDialog.Dismiss();
                                    }

                                    if (noContributions == null)
                                        noContributions = view.FindViewById<TextView>(Resource.Id.lblcontributions);

                                    if (_model.SpendingAccountDetails == null || _model.SpendingAccountDetails.AccountRollups.Count == 0)
                                    {
                                        noContributions.Text = Activity.ApplicationContext.GetString(Resource.String.no_contributions_found);
                                    }
                                    else
                                    {
                                        noContributions.Text = "";
                                    }
                                });
                            }
                        }
                    }
                    catch (Exception exc)
                    {

                    }
                };
            }
            catch (Exception ex)
            {

            }
        }
	}
	public class CustomAdapter : MvxAdapter
	{
		Context ctx;
		SpendingAccountTypeRollup _spendingAccountTypeRollup;

		public CustomAdapter(Context context, IMvxAndroidBindingContext bindingContext,SpendingAccountTypeRollup _typeRollup) 
            : base(context, bindingContext)
		{
			ctx=context;
			_spendingAccountTypeRollup=_typeRollup;

		}
		public override int GetPosition(object item)
		{
			return base.GetPosition(item);
		}
		public override int Count
        {
			get
            {
               if (_spendingAccountTypeRollup != null && _spendingAccountTypeRollup.AccountRollups != null)
               {
                    return _spendingAccountTypeRollup.AccountRollups.Count;
                }
                else
                    return 0;
               
            }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
			Typeface nunitoFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");
			SpendingAccountPeriodRollup _periodRollup = (SpendingAccountPeriodRollup)dataContext;
			templateId = Resource.Layout.item_period_rollup;
			View v = base.GetBindableView(convertView, dataContext, parent, templateId);
            TextView txtcontribution1=v.FindViewById<TextView>(Resource.Id.contribution1);
			TextView txtcontribution2=v.FindViewById<TextView>(Resource.Id.contribution2);
			TextView txtcontribution4=v.FindViewById<TextView>(Resource.Id.contribution4);
			DollarSignTextView dsTxtTotalRemaining=v.FindViewById<DollarSignTextView>(Resource.Id.dsTxtTotalRemaining);
			LinearLayout ll_item_account_detail=v.FindViewById<LinearLayout>(Resource.Id.ll_item_account_detail);
			dsTxtTotalRemaining.SetTypeface (nunitoFont, TypefaceStyle.Bold);
			txtcontribution1.SetTypeface (leagueFont, TypefaceStyle.Bold);
			txtcontribution2.SetTypeface (leagueFont, TypefaceStyle.Bold);
			txtcontribution4.SetTypeface (leagueFont, TypefaceStyle.Bold);

			txtcontribution2.Text=" "+_periodRollup.Year+"";
            String _startEndDate = "( " + DateTime.ParseExact(_periodRollup.StartDateAsString, "MMMM dd, yyyy", CultureInfo.CurrentCulture).ToString("MMM dd, yyyy")+" - "+DateTime.ParseExact(_periodRollup.EndDateAsString, "MMMM dd, yyyy",CultureInfo.CurrentCulture).ToString("MMM dd, yyyy")+ " )";
			dsTxtTotalRemaining.DollarString = _periodRollup.TotalRemaining+"";
			txtcontribution4.Text = _startEndDate;

			bool tabletOrPhone=isTablet(ctx);
			int i = 0;
            
            ll_item_account_detail.RemoveAllViews(); 
			foreach (SpendingAccountDetail _spendingAccountDetail in _periodRollup.SpendingAccounts)
			{
			
				LayoutInflater _inflatorservice = (LayoutInflater)ctx.GetSystemService(Context.LayoutInflaterService);
				var viewContainer = _inflatorservice.Inflate(Resource.Layout.item_account_detail,null);
				if (viewContainer != null) {
					TextView txtRemainingAsStringtxt=viewContainer.FindViewById<TextView>(Resource.Id.txtRemainingAsStringtxt);
					TextView txtAccountAsStringtxt=viewContainer.FindViewById<TextView>(Resource.Id.txtAccountAsStringtxt);
					View line_separatord=viewContainer.FindViewById<View>(Resource.Id.line_separatord);

					TextView txtDepositeAsStringtxt=viewContainer.FindViewById<TextView>(Resource.Id.txtDepositeAsStringtxt);
					TextView txtUsedToAsStringtxt=viewContainer.FindViewById<TextView>(Resource.Id.txtUsedToAsStringtxt);

                    txtDepositeAsStringtxt.Text = ctx.Resources.GetString(Resource.String.deposited);
					txtRemainingAsStringtxt.SetTypeface (nunitoFont, TypefaceStyle.Bold);
					txtUsedToAsStringtxt.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					txtDepositeAsStringtxt.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					if (tabletOrPhone == true) {
                        txtDepositeAsStringtxt.Text = ctx.Resources.GetString(Resource.String.deposited_tab);
                        txtUsedToAsStringtxt.Text = ctx.Resources.GetString(Resource.String.usedToDate_tab);                    
                        txtRemainingAsStringtxt.Text = ctx.Resources.GetString(Resource.String.remaining_tab);
						if (i == 0) {
							if (txtAccountAsStringtxt != null) {
								txtAccountAsStringtxt.Visibility = ViewStates.Visible;
								txtAccountAsStringtxt.SetTypeface (nunitoFont, TypefaceStyle.Normal);
							}
							if (line_separatord != null) {
								line_separatord.Visibility = ViewStates.Visible;
							}
							txtRemainingAsStringtxt.Visibility = ViewStates.Visible;
							txtDepositeAsStringtxt.Visibility = ViewStates.Visible;
							txtUsedToAsStringtxt.Visibility = ViewStates.Visible;
						} else {
							if (txtAccountAsStringtxt != null) {
								txtAccountAsStringtxt.Visibility = ViewStates.Gone;
								txtAccountAsStringtxt.SetTypeface (nunitoFont, TypefaceStyle.Normal);
							}
							if (line_separatord != null) {
								line_separatord.Visibility = ViewStates.Gone;
							}
							txtRemainingAsStringtxt.Visibility = ViewStates.Gone;
							txtDepositeAsStringtxt.Visibility = ViewStates.Gone;
							txtUsedToAsStringtxt.Visibility = ViewStates.Gone;
						}
					}

					TextView txtAccountName=viewContainer.FindViewById<TextView>(Resource.Id.txtAccountName);
					txtAccountName.Text = _spendingAccountDetail.AccountName;
					txtAccountName.SetTypeface (leagueFont, TypefaceStyle.Bold);
					DollarSignTextView txtDepositeAsString=viewContainer.FindViewById<DollarSignTextView>(Resource.Id.txtDepositeAsString);
					DollarSignTextView txtRemainingAsString=viewContainer.FindViewById<DollarSignTextView>(Resource.Id.txtRemainingAsString);
					DollarSignTextView txtUsedToAsString=viewContainer.FindViewById<DollarSignTextView>(Resource.Id.txtUsedToAsString);

					txtDepositeAsString.DollarString = _spendingAccountDetail.Deposited.ToString();
					txtUsedToAsString.DollarString = _spendingAccountDetail.UsedToDate.ToString();
                    txtRemainingAsString.DollarString = _spendingAccountDetail.Remaining.ToString();
						
					txtRemainingAsString.SetTypeface (nunitoFont, TypefaceStyle.Bold);
					txtUsedToAsString.SetTypeface (nunitoFont, TypefaceStyle.Bold);
					txtDepositeAsString.SetTypeface (nunitoFont, TypefaceStyle.Bold);

                    TextView txtEndDateAsString = viewContainer.FindViewById<TextView>(Resource.Id.txtEndDateAsString);
                    txtEndDateAsString.SetTypeface(nunitoFont, TypefaceStyle.Normal);
                    var forfeitedText = string.Format(ctx.Resources.GetString(Resource.String.forfeited_lower) + " " + ("<b>" + DateTime.ParseExact(_spendingAccountDetail.UseByDateAsString, "MMMM dd, yyyy", CultureInfo.CurrentCulture).ToString("MMM dd, yyyy") + "</b>"));
                    txtEndDateAsString.TextFormatted = Html.FromHtml(forfeitedText); 

                    TextView txtEndDateAsStringDate = viewContainer.FindViewById<TextView>(Resource.Id.txtEndDateAsStringDate);
                    txtEndDateAsStringDate.Text = " ";
                    txtEndDateAsStringDate.SetTypeface(nunitoFont, TypefaceStyle.Bold); 
                    
					ll_item_account_detail.AddView (viewContainer);
					i++;
				}
			}
			return v;
		}
		public bool isTablet(Context context) {
			bool xlarge = ((context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) == ScreenLayout.SizeXlarge);
			bool large = ((context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) == ScreenLayout.SizeLarge);
			return (xlarge || large);
		}
	}
}
