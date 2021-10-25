using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System;
using System.Collections.ObjectModel;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly ILoginService _loginservice;
        private readonly IDataService _dataservice;
        private readonly IDrugLookupService _drugservice;
        private readonly IProviderLookupService _providerservice;
        private readonly IParticipantService _participantservice;
        private readonly ISpendingAccountService _spendingaccountservice;
        private readonly IClaimService _claimservice;
        private readonly IEligibilityService _eligibilityservice;
        private readonly MvxSubscriptionToken _loggedin;

        public FirstViewModel(IMvxMessenger messenger, ILoginService loginservice, IDataService dataservice, IDrugLookupService drugservice, IProviderLookupService providerservice, IParticipantService particpantservice, ISpendingAccountService spendingaccountservice, IClaimService claimservice, IEligibilityService eligibilityservice)
        {
            _messenger = messenger;
            _loginservice = loginservice;
            _dataservice = dataservice;
            _drugservice = drugservice;
            _providerservice = providerservice;
            _participantservice = particpantservice;
            _spendingaccountservice = spendingaccountservice;
            _claimservice = claimservice;
            _eligibilityservice = eligibilityservice;

            DrugInfo di = new DrugInfo() { ID = 12346, DIN = 766224 };
            //Participant p = new Participant() { PlanMemberID = "12345678-00" };
            Participant p = new Participant() { PlanMemberID = _loginservice.CurrentPlanMemberID };
            //Participant p = new Participant() { PlanMemberID = "1774113-00" };
            UserProfile up = new UserProfile() { PlanMemberIDWithoutParticipantCode = 4889467, LanguageCode = "en-CA" };
            EmailRequest er = new EmailRequest()
            {
                RecipientAddress = "colin.mcguire@imason.com",
                SenderAddress = "",
                SenderName = "",
                Subject = "",
                Body = ""
            };

            Participant claimParticipant = new Participant() { PlanMemberID = "1774113-00" };
            ClaimSubmissionType claimSubmissionType = new ClaimSubmissionType() { ID = "CHIRO" };
            ServiceProvider claimProvider = new ServiceProvider()
            {
                ID = 140755,
                BusinessName = "DR S A ZYLICH",
                Address = "447 CHURCH ST",
                City = "Toronto",
                Province = "ON",
                PostalCode = "M4Y2C5",
                Phone = "4169251868",
                RegistrationNumber = null
            };
            ObservableCollection<TreatmentDetail> claimTDs = GetFakeTreatmentDetails();
            Claim c = new Claim()
            {
                Participant = claimParticipant,
                Type = claimSubmissionType,
                Provider = claimProvider,
                CoverageUnderAnotherBenefitsPlan = false,
                IsOtherCoverageWithGSC = false,
                HasClaimBeenSubmittedToOtherBenefitPlan = false,
                PayAnyUnpaidBalanceThroughOtherGSCPlan = false,
                OtherGSCNumber = null,
                PayUnderHCSA = false,
                IsTreatmentDueToAMotorVehicleAccident = false,
                DateOfMotorVehicleAccident = DateTime.Now,
                IsTreatmentDueToAWorkRelatedInjury = false,
                DateOfWorkRelatedInjury = DateTime.Now,
                WorkRelatedInjuryCaseNumber = 0,
                IsMedicalItemForSportsOnly = false,
                HasReferralBeenPreviouslySubmitted = false,
                DateOfReferral = DateTime.Now,
                TypeOfMedicalProfessional = new ClaimOption() { ID = "5003" },
                TreatmentDetails = claimTDs
            };

            //_loginservice.RefreshToken();

            /* Drug/DIN */
            //_drugservice.GetByName("tylenol");
            //_drugservice.GetByDIN(00766224);
            //_drugservice.CheckEligibility(p, di);
            //_drugservice.GetSpecialAuthorizationForm("autho-botox migraine therapy-100-en.pdf"); //"migraine-therapy-100-en.pdf");
            //_drugservice.EmailSpecialAuthorizationForm("autho-botox migraine therapy-100-en.pdf", er);

            /* ProviderLookup */
            //_providerservice.GetServiceProviderTypes();
            //_providerservice.FindProviders(new ServiceProviderType { ID = "CHIRO" }, 5, "", "M4K2G9", null, null); //Count = 16
            //_providerservice.FindProviderByBusinessName("TorChiro", 5, new ServiceProviderType { ID = "CHIRO" }, "", "", 43.7390976, -79.4058762);
            //_providerservice.FindProviderByPhoneNumber("4161234567", 5, new ServiceProviderType { ID = "CHIRO" }, "", "", 43.7390976, -79.4058762);
            //_providerservice.FindProviderByLastName("Bolton", 5, new ServiceProviderType { ID = "CHIRO" }, "", "", 43.7390976, -79.4058762);
            //_providerservice.FindProviderByCity("Toronto", 5, new ServiceProviderType { ID = "CHIRO" }, "", "", 43.7390976, -79.4058762); //Count = 10
            //_providerservice.GetPreviousServiceProviders("4889467-00", string.Empty);
            //_providerservice.GetPreviousServiceProviders("1774113-00", string.Empty);
            //_providerservice.FindProviderByPhoneNumber("4169251868", 50, new ServiceProviderType { ID = "ACUPUNCTURE" }, "", "M4Y2C5", null, null);
            //_providerservice.GetServiceProviderByName("CHIRO", "Reynolds", "L");
            //_providerservice.GetServiceProviderByPhoneNumber("CHIRO", "4166521695");

            /* Participant */
            //_participantservice.GetParticipant(_loginservice.CurrentPlanMemberID);
            //_participantservice.GetIDCard(p.PlanMemberID);
            //_participantservice.GetIDCardImage("gsc_front.jpg");
            //_participantservice.GetIDCardLogo("ge.jpg");
            //_participantservice.GetUserProfile(_loginservice.CurrentPlanMemberID);
            //_participantservice.PutUserProfile(up);

            /* Spending Account */
            //_spendingaccountservice.GetSpendingAccountTypes(_loginservice.CurrentPlanMemberID);
            //_spendingaccountservice.GetSpendingAccountDetails(new SpendingAccountType() { Id = 83400 });
            //_spendingaccountservice.GetAllSpendingAccountDetails(p.PlanMemberID);

            /* Claims */
            //_claimservice.GetClaimSubmissionTypes(p.PlanMemberID);
            //_claimservice.GetClaimSubmissionBenefits("EYEEXAM");
            //_claimservice.GetClaimOptions("medicalprofessional");
            //_claimservice.GetTypesOfMedicalProfessional();
            //_claimservice.GetLensTypes(_loginservice.CurrentPlanMemberID);
            //_claimservice.GetVisionAxes();
            //_claimservice.GetVisionBifocals();
            //_claimservice.GetVisionCylinders();
            //_claimservice.GetVisionPrisms();
            //_claimservice.GetVisionSpheres();
            //_claimservice.Claim = c;
            //_claimservice.SubmitClaim();

            /* Eligibility */
            //_eligibilityservice.GetEligibilityCheckTypes(_loginservice.CurrentPlanMemberID);
            //_eligibilityservice.GetEligibilityProvinces("CHIRO");
            //_eligibilityservice.GetEligibilityBenefits("CHIRO");
            //_eligibilityservice.GetEligibilityOptions("massagetime");
            //_eligibilityservice.GetEligibilityOptions("lenstype");
            //_eligibilityservice.EligibilityInquiry(_loginservice.CurrentPlanMemberID);

            //IDCard idcard = _dataservice.GetIDCard();
            //if (idcard != null && !string.IsNullOrEmpty(idcard.PlanMemberFullName))
            //{
            //    FrontImagePath = idcard.FrontImageFilePath;
            //    Hello = string.Format("Hello {0}", idcard.PlanMemberFullName);
            //}
        }

        private string _specialAuthFormPath;
        public string SpecialAuthFormPath
        {
            get { return _specialAuthFormPath; }
            set { _specialAuthFormPath = value; RaisePropertyChanged(() => SpecialAuthFormPath); }
        }

        private string _frontimagepath;
        public string FrontImagePath
        {
            get { return _frontimagepath; }
            set { _frontimagepath = value; }
        }

		private string _hello = "Hello MvvmCross";
        public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }
		}

        private ObservableCollection<TreatmentDetail> GetFakeTreatmentDetails()
        {
            ObservableCollection<TreatmentDetail> tds = new ObservableCollection<TreatmentDetail>();

            TreatmentDetail td1 = new TreatmentDetail()
            {
                TypeOfTreatment = new ClaimSubmissionBenefit() { ProcedureCode = "31001" },
                TreatmentDuration = new ClaimOption() { Name = "30" },
                TreatmentDate = new DateTime(2014, 01, 11),
                TreatmentAmount = 11,
                AlternateCarrierPayment = 0
            };

            TreatmentDetail td2 = new TreatmentDetail()
            {
                TypeOfTreatment = new ClaimSubmissionBenefit() { ProcedureCode = "31002" },
                TreatmentDuration = new ClaimOption() { Name = "30" },
                TreatmentDate = new DateTime(2014, 02, 22),
                TreatmentAmount = 22,
                AlternateCarrierPayment = 0
            };

            tds.Add(td1);
            tds.Add(td2);

            return tds;
        }
    }
}
