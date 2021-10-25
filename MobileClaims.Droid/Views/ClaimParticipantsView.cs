using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using System.Drawing;
using System.Globalization;
using static Android.Widget.TextView;
using Color = Android.Graphics.Color;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.ADD)]
    public class ClaimParticipantsView : BaseFragment
    {
        private ClaimParticipantsViewModel _model;
        private bool _dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimParticipantsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimParticipantsViewModel)ViewModel;
            _model.ChangeParticipantRequest += HandleChangeParticipantRequest;

            var selectedParticipant = _model.SelectedParticipant;
            if (selectedParticipant != null)
            {
                var participantlist = Activity.FindViewById(Resource.Id.claim_participant) as MvxListView;
                var index = _model.ParticipantsViewModel.Participants.FindIndex(x => x.PlanMemberID == selectedParticipant.PlanMemberID);
                participantlist.SetItemChecked(index, true);

                var SecondMvXList = View.FindViewById<SingleSelectMvxListView>(Resource.Id.claim_participant_under_19);
                var indexUnder19 = _model.ParticipantsViewModel.OtherParticipants.FindIndex(x => x.PlanMemberID == selectedParticipant.PlanMemberID);
                SecondMvXList.SetItemChecked(indexUnder19, true);
            }

            var claimPlanParticipantTitle = Activity.FindViewById<TextView>(Resource.Id.claim_plan_participant_title);
            claimPlanParticipantTitle.Text = string.Format(Resources.GetString(Resource.String.claimPlanParticipantTitle), (_model.ClaimSubmissionType.Name).ToLower());
            _model.SelectOnlyOneItemAtATimeClick += ViewModel_SelectOnlyOneItemAtATimeClick;
            applyColor();
        }

        private void applyColor()
        {
            NunitoTextView FinancialAssistance = View.FindViewById<NunitoTextView>(Resource.Id.financial_asistance);
            SpannableStringBuilder builder = new SpannableStringBuilder();
            String Black1 = null;
            String Branded1 = null;
            String Black2 = null;
            String Branded2 = null;

            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Equals("fr"))
            {
                Black1 = "Le programme ";
                Branded1 = "Mieux voir pour réussir";
                Black2 = " offre un remboursement de 250 $ par 24 mois à la suite de l’achat de lunettes ou de verr​es de contact (lentilles), prescrits par un professionnel, visant à corriger la vision de tout enfant de moins de 18 ans. Seuls les achats effectués depuis le 1er septembre 2019, chez un opticien ou un optométriste au Québec, sont admissibles. ";
                Branded2 = "Vous devez soumettre votre réclamation même si le montant offert par le gouvernement couvre le montant total de l’achat.";
            }
            else
            {
                Black1 = "The ";
                Branded1 = "See better to succeed";
                Black2 = " program offers a $250 reimbursement per 24-month period for purchases of glasses or contact lenses prescribed by a professional to correct the vision of any child under 18 years of age. Only purchases made after September 1, 2019 from an optician or optometrist in Québec are eligible.";
                Branded2 = " You must submit your claim even if the amount offered by the government covers the total purchase amount."; 
            }
           SpannableString Black1Spannable = new SpannableString(Black1);
            SpannableString Branded1Spannable = new SpannableString(Branded1);
            SpannableString Black2Spannable = new SpannableString(Black2);
            SpannableString Branded2Spannable = new SpannableString(Branded2);

            Black1Spannable.SetSpan(new ForegroundColorSpan(Color.Black), 0, Black1.Length, 0);
            Branded1Spannable.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.highlight_color)), 0, Branded1.Length, 0);
            Branded1Spannable.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, Branded1.Length, 0);
            Black2Spannable.SetSpan(new ForegroundColorSpan(Color.Black), 0, Black2.Length, 0);
            Branded2Spannable.SetSpan(new ForegroundColorSpan(Resources.GetColor(Resource.Color.highlight_color)), 0, Branded2.Length, 0);
            Branded2Spannable.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, Branded2.Length, 0);

            builder.Append(Black1Spannable);
            builder.Append(Branded1Spannable);
            builder.Append(Black2Spannable);
            builder.Append(Branded2Spannable);

            FinancialAssistance.SetText(builder, BufferType.Spannable);
        }

        private string GetHexColor(int resourceIdColor)
        {
            var color = Resources.GetColor(resourceIdColor);
            return string.Format("#{0:X}{1:X}{2:X}", color.R, color.G, color.B);
        }

        void ViewModel_SelectOnlyOneItemAtATimeClick(Object sender, EventArgs e, bool type)
        {
            var FirstMvxList = View.FindViewById<SingleSelectMvxListView>(Resource.Id.claim_participant);
            var SecondMvXList = View.FindViewById<SingleSelectMvxListView>(Resource.Id.claim_participant_under_19);

            if (type)
            {
                FirstMvxList.ClearChoices();
            }
            else
            {
                SecondMvXList.ClearChoices();
            }
        }

        private void HandleChangeParticipantRequest(object sender, EventArgs e)
        {
            if (_dialogShown)
            {
                return;
            }

            Activity.RunOnUiThread(() =>
                {
                    _dialogShown = true;
                    var res = Resources;
                    var builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimPlanParticipantChangedTitle)));
                    builder.SetMessage(string.Format(res.GetString(Resource.String.claimPlanParticipantChangedContent), _model.RequestedParticipant.FullName));
                    builder.SetCancelable(false);
                    builder.SetNegativeButton(string.Format(res.GetString(Resource.String.cancel)), delegate
                    {
                        _dialogShown = false;
                        var index = _model.ParticipantsViewModel.Participants.IndexOf(_model.SelectedParticipant);
                        if (index >= 0)
                        {
                            var participantList = Activity.FindViewById<ListView>(Resource.Id.claim_participant);
                            participantList.SetItemChecked(index, true);
                        }

                        if (!Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            _model.ChangeParticipantCommand.Execute(_model.SelectedParticipant);
                        }
                    });
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                    {
                        _dialogShown = false;
                        _model.ChangeParticipantCommand.Execute(_model.RequestedParticipant);
                    });
                    builder.Show();
                }
            );
        }
    }
}