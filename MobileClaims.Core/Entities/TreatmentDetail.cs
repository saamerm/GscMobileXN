using System;
using System.ComponentModel;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Entities
{
    public class TreatmentDetail : MvxViewModel, INotifyPropertyChanged
    {
        public TreatmentDetail()
        {
            TreatmentDate = DateTime.Now;
            PickupDate = DateTime.Now;
            DateOfReferral = DateTime.MinValue;
            DateOfMonthlyTreatment = DateTime.Now;
            DateOfPurchase = DateTime.Now;
            DateOfExamination = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                InvokeOnMainThread(() =>
                    {
                        PropertyChanged(this, e);
                    }
                );

            }
        }

        private Guid _id;
        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        private ClaimSubmissionType _claimSubmissionType;
        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimSubmissionType;
            }
            set
            {
                _claimSubmissionType = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ClaimSubmissionType"));
            }
        }

        // Common fields for most claim submission types
        private DateTime _treatmentDate;
        public DateTime TreatmentDate
        {
            get
            {
                return _treatmentDate;
            }
            set
            {
                _treatmentDate = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDate"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDateListViewItem"));
            }
        }

        private ClaimOption _treatmentDuration; //aka Length of Treatment
        public ClaimOption TreatmentDuration
        {
            get
            {
                return _treatmentDuration;
            }
            set
            {
                _treatmentDuration = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDuration"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDurationListViewItem"));
            }
        }

        private ClaimSubmissionBenefit _typeOfTreatment;
        public ClaimSubmissionBenefit TypeOfTreatment
        {
            get
            {
                return _typeOfTreatment;
            }
            set
            {
                _typeOfTreatment = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfTreatment"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfTreatmentListViewItem"));
            }
        }

        private double _alternateCarrierPayment; //aka Amount Paid by Alternate Carrier
        public double AlternateCarrierPayment
        {
            get
            {
                return _alternateCarrierPayment;
            }
            set
            {
                _alternateCarrierPayment = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("AlternateCarrierPayment"));
            }
        }

        private double _amountPaidByPPorGP;
        public double AmountPaidByPPorGP
        {
            get => _amountPaidByPPorGP;
            set => SetProperty(ref _amountPaidByPPorGP, value);
        }

        private double _treatmentAmount; //aka Total Amount of Visit or Total Amount Charged or Total Amount of Examination
        public double TreatmentAmount
        {
            get
            {
                return _treatmentAmount;
            }
            set
            {
                _treatmentAmount = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmount"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }


        // Medical Item fields
        private ClaimSubmissionBenefit _itemDescription;
        public ClaimSubmissionBenefit ItemDescription
        {
            get
            {
                return _itemDescription;
            }
            set
            {
                _itemDescription = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ItemDescription"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfTreatmentListViewItem"));
            }
        }

        private DateTime _pickupDate;
        public DateTime PickupDate
        {
            get
            {
                return _pickupDate;
            }
            set
            {
                _pickupDate = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("PickupDate"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDateListViewItem"));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("Quantity"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        private bool _gstHstIncludedInTotal;
        public bool GSTHSTIncludedInTotal
        {
            get
            {
                return _gstHstIncludedInTotal;
            }
            set
            {
                _gstHstIncludedInTotal = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("GSTHSTIncludedInTotal"));
            }
        }

        private bool _pstIncludedInTotal;
        public bool PSTIncludedInTotal
        {
            get
            {
                return _pstIncludedInTotal;
            }
            set
            {
                _pstIncludedInTotal = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("PSTIncludedInTotal"));
            }
        }

        private bool _isReferralNotSubmitted;
        public bool IsReferralNotSubmitted
        {
            get
            {
                return _isReferralNotSubmitted;
            }
            set
            {
                _isReferralNotSubmitted = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsReferralNotSubmitted"));
            }
        }

        private bool _hasReferralBeenPreviouslySubmitted;
        public bool HasReferralBeenPreviouslySubmitted
        {
            get
            {
                return _hasReferralBeenPreviouslySubmitted;
            }
            set
            {
                _hasReferralBeenPreviouslySubmitted = value;
                IsReferralNotSubmitted = !value;
                RaisePropertyChanged(new PropertyChangedEventArgs("HasReferralBeenPreviouslySubmitted"));
            }
        }

        private ClaimOption _typeOfMedicalProfessional;
        public ClaimOption TypeOfMedicalProfessional
        {
            get
            {
                return _typeOfMedicalProfessional;
            }
            set
            {
                _typeOfMedicalProfessional = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfMedicalProfessional"));
            }
        }

        private DateTime _dateOfReferral;
        public DateTime DateOfReferral
        {
            get
            {
                return _dateOfReferral;
            }
            set
            {
                _dateOfReferral = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("DateOfReferral"));
            }
        }

        // Orthodontic Monthly Fee fields
        private DateTime _dateOfMonthlyTreatment;
        public DateTime DateOfMonthlyTreatment
        {
            get
            {
                return _dateOfMonthlyTreatment;
            }
            set
            {
                _dateOfMonthlyTreatment = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("DateOfMonthlyTreatment"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDateListViewItem"));
            }
        }

        private double _orthodonticMonthlyFee;
        public double OrthodonticMonthlyFee
        {
            get
            {
                return _orthodonticMonthlyFee;
            }
            set
            {
                _orthodonticMonthlyFee = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("OrthodonticMonthlyFee"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        // Prescription Contacts and Prescription Glasses fields
        private DateTime _dateOfPurchase;
        public DateTime DateOfPurchase
        {
            get
            {
                return _dateOfPurchase;
            }
            set
            {
                _dateOfPurchase = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("DateOfPurchase"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDateListViewItem"));
            }
        }

        private ClaimSubmissionBenefit _typeOfEyewear;
        public ClaimSubmissionBenefit TypeOfEyewear
        {
            get
            {
                return _typeOfEyewear;
            }
            set
            {
                _typeOfEyewear = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfEyewear"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfTreatmentListViewItem"));
            }
        }

        private bool _acknowledgeEyewearIsCorrective;
        public bool AcknowledgeEyewearIsCorrective
        {
            get
            {
                return _acknowledgeEyewearIsCorrective;
            }
            set
            {
                _acknowledgeEyewearIsCorrective = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("AcknowledgeEyewearIsCorrective"));
            }
        }

        private LensType _typeOfLens;
        public LensType TypeOfLens
        {
            get
            {
                return _typeOfLens;
            }
            set
            {
                _typeOfLens = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TypeOfLens"));
            }
        }

        private double _totalAmountCharged;
        public double TotalAmountCharged
        {

            get
            {
                return _totalAmountCharged;
            }
            set
            {
                _totalAmountCharged = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TotalAmountCharged"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        private double _frameAmount;
        public double FrameAmount
        {
            get
            {
                return _frameAmount;
            }
            set
            {
                _frameAmount = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("FrameAmount"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        private double _eyeglassLensesAmount;
        public double EyeglassLensesAmount
        {
            get
            {
                return _eyeglassLensesAmount;
            }
            set
            {
                _eyeglassLensesAmount = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("EyeglassLensesAmount"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        private double _feeAmount;
        public double FeeAmount
        {
            get
            {
                return _feeAmount;
            }
            set
            {
                _feeAmount = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("FeeAmount"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentAmountListViewItem"));
            }
        }

        // Routine Eye Examination fields
        private DateTime _dateOfExamination;
        public DateTime DateOfExamination
        {
            get
            {
                return _dateOfExamination;
            }
            set
            {
                _dateOfExamination = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("DateOfExamination"));
                RaisePropertyChanged(new PropertyChangedEventArgs("TreatmentDateListViewItem"));
            }
        }

        // Vision Claims Prescription fields
        private bool _isPrescriptionDetailsVisible;
        public bool IsPrescriptionDetailsVisible
        {
            get
            {
                return _isPrescriptionDetailsVisible;
            }
            set
            {
                _isPrescriptionDetailsVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsPrescriptionDetailsVisible"));
            }
        }

        private bool _isRightSphereEnabled = false;
        public bool IsRightSphereEnabled
        {
            get
            {
                return _isRightSphereEnabled;
            }
            set
            {
                _isRightSphereEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsRightSphereEnabled"));
            }
        }

        private ClaimOption _rightSphere;
        public ClaimOption RightSphere
        {
            get
            {
                return _rightSphere;
            }
            set
            {
                _rightSphere = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("RightSphere"));
            }
        }

        private bool _isRightCylinderEnabled = false;
        public bool IsRightCylinderEnabled
        {
            get
            {
                return _isRightCylinderEnabled;
            }
            set
            {
                _isRightCylinderEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsRightCylinderEnabled"));
            }
        }

        private ClaimOption _rightCylinder;
        public ClaimOption RightCylinder
        {
            get
            {
                return _rightCylinder;
            }
            set
            {
                _rightCylinder = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("RightCylinder"));
            }
        }

        private bool _isRightAxisEnabled = false;
        public bool IsRightAxisEnabled
        {
            get
            {
                return _isRightAxisEnabled;
            }
            set
            {
                _isRightAxisEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsRightAxisEnabled"));
            }
        }

        private ClaimOption _rightAxis;
        public ClaimOption RightAxis
        {
            get
            {
                return _rightAxis;
            }
            set
            {
                _rightAxis = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("RightAxis"));
            }
        }

        private bool _isRightPrismEnabled = false;
        public bool IsRightPrismEnabled
        {
            get
            {
                return _isRightPrismEnabled;
            }
            set
            {
                _isRightPrismEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsRightPrismEnabled"));
            }
        }

        private ClaimOption _rightPrism;
        public ClaimOption RightPrism
        {
            get
            {
                return _rightPrism;
            }
            set
            {
                _rightPrism = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("RightPrism"));
            }
        }

        private bool _isRightBifocalEnabled = false;
        public bool IsRightBifocalEnabled
        {
            get
            {
                return _isRightBifocalEnabled;
            }
            set
            {
                _isRightBifocalEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsRightBifocalEnabled"));
            }
        }

        private ClaimOption _rightBifocal;
        public ClaimOption RightBifocal
        {
            get
            {
                return _rightBifocal;
            }
            set
            {
                _rightBifocal = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("RightBifocal"));
            }
        }

        private bool _isLeftSphereEnabled = false;
        public bool IsLeftSphereEnabled
        {
            get
            {
                return _isLeftSphereEnabled;
            }
            set
            {
                _isLeftSphereEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsLeftSphereEnabled"));
            }
        }

        private ClaimOption _leftSphere;
        public ClaimOption LeftSphere
        {
            get
            {
                return _leftSphere;
            }
            set
            {
                _leftSphere = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LeftSphere"));
            }
        }

        private bool _isLeftCylinderEnabled = false;
        public bool IsLeftCylinderEnabled
        {
            get
            {
                return _isLeftCylinderEnabled;
            }
            set
            {
                _isLeftCylinderEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsLeftCylinderEnabled"));
            }
        }

        private ClaimOption _leftCylinder;
        public ClaimOption LeftCylinder
        {
            get
            {
                return _leftCylinder;
            }
            set
            {
                _leftCylinder = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LeftCylinder"));
            }
        }

        private bool _isLeftAxisEnabled = false;
        public bool IsLeftAxisEnabled
        {
            get
            {
                return _isLeftAxisEnabled;
            }
            set
            {
                _isLeftAxisEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsLeftAxisEnabled"));
            }
        }

        private ClaimOption _leftAxis;
        public ClaimOption LeftAxis
        {
            get
            {
                return _leftAxis;
            }
            set
            {
                _leftAxis = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LeftAxis"));
            }
        }

        private bool _isLeftPrismEnabled = false;
        public bool IsLeftPrismEnabled
        {
            get
            {
                return _isLeftPrismEnabled;
            }
            set
            {
                _isLeftPrismEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsLeftPrismEnabled"));
            }
        }

        private ClaimOption _leftPrism;
        public ClaimOption LeftPrism
        {
            get
            {
                return _leftPrism;
            }
            set
            {
                _leftPrism = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LeftPrism"));
            }
        }

        private bool _isLeftBifocalEnabled = false;
        public bool IsLeftBifocalEnabled
        {
            get
            {
                return _isLeftBifocalEnabled;
            }
            set
            {
                _isLeftBifocalEnabled = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsLeftBifocalEnabled"));
            }
        }

        private ClaimOption _leftBifocal;
        public ClaimOption LeftBifocal
        {
            get
            {
                return _leftBifocal;
            }
            set
            {
                _leftBifocal = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LeftBifocal"));
            }
        }

        private bool _isAlternateCarrierPaymentVisible;
        public bool IsAlternateCarrierPaymentVisible
        {
            get
            {
                return _isAlternateCarrierPaymentVisible;
            }
            set
            {
                _isAlternateCarrierPaymentVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsAlternateCarrierPaymentVisible"));
            }
        }

        private bool _isAmountPaidByPPorGPVisible;
        public bool IsAmountPaidByPPorGPVisible
        {
            get => _isAmountPaidByPPorGPVisible;
            set => SetProperty(ref _isAmountPaidByPPorGPVisible, value);
        }

        public bool IsTypeOfTreatmentVisible
        {
            get
            {
                if (ClaimSubmissionType != null)
                {
                    switch (ClaimSubmissionType.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                        case "DENTAL":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTreatementDateVisible
        {
            get
            {
                if (ClaimSubmissionType != null)
                {
                    switch (ClaimSubmissionType.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                        case "DENTAL":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTreatmentAmountVisible
        {
            get
            {
                if (ClaimSubmissionType != null)
                {
                    switch (ClaimSubmissionType.ID)
                    {
                        case "ACUPUNCTURE":
                        case "CHIROPODY":
                        case "CHIRO":
                        case "PHYSIO":
                        case "PODIATRY":
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                        case "CONTACTS":
                        case "EYEEXAM":
                        case "DENTAL":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsTotalAmountChargedForMIVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsTreatmentDurationVisible
        {
            get
            {
                if (ClaimSubmissionType != null)
                {
                    switch (ClaimSubmissionType.ID)
                    {
                        case "PSYCHOLOGY":
                        case "MASSAGE":
                        case "NATUROPATHY":
                        case "SPEECH":
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsItemDescriptionVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsPickupDateVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsQuantityVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsGSTHSTIncludedInTotalVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsPSTIncludedInTotalVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }

        public bool IsHasReferralBeenPreviouslySubmittedVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI")
                    return true;
                else
                    return false;
            }
        }
        //Used on confirmation page only
        public bool IsDateOfReferralVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI" && HasReferralBeenPreviouslySubmitted && DateOfReferral != DateTime.MinValue)
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfMedicalProfessionalVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "MI" && HasReferralBeenPreviouslySubmitted && TypeOfMedicalProfessional != null)
                    return true;
                else
                    return false;
            }
        }

        public bool IsDateOfMonthlyTreatmentVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "ORTHODONTIC")
                    return true;
                else
                    return false;
            }
        }

        public bool IsOrthodonticMonthlyFeeVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "ORTHODONTIC")
                    return true;
                else
                    return false;
            }
        }

        public bool IsDateOfPurchaseVisible
        {
            get
            {
                if (ClaimSubmissionType != null && (ClaimSubmissionType.ID == "CONTACTS" || ClaimSubmissionType.ID == "GLASSES"))
                    return true;
                else
                    return false;
            }
        }

        public bool IsTypeOfEyewearVisible
        {
            get
            {
                if (ClaimSubmissionType != null && (ClaimSubmissionType.ID == "CONTACTS" || ClaimSubmissionType.ID == "GLASSES"))
                    return true;
                else
                    return false;
            }
        }

        private bool _isAcknowledgeEyewearIsCorrectiveVisible = false;
        public bool IsAcknowledgeEyewearIsCorrectiveVisible
        {
            get
            {
                return _isAcknowledgeEyewearIsCorrectiveVisible;
            }
            set
            {
                _isAcknowledgeEyewearIsCorrectiveVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsAcknowledgeEyewearIsCorrectiveVisible"));
            }
        }

        public bool IsTypeOfLensVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "GLASSES")
                    return true;
                else
                    return false;
            }
        }

        private bool _isTotalAmountChargedVisible = false;
        public bool IsTotalAmountChargedVisible
        {
            get
            {
                return _isTotalAmountChargedVisible;
            }
            set
            {
                _isTotalAmountChargedVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsTotalAmountChargedVisible"));
            }
        }

        private bool _isFrameAmountVisible = false;
        public bool IsFrameAmountVisible
        {
            get
            {
                return _isFrameAmountVisible;
            }
            set
            {
                _isFrameAmountVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsFrameAmountVisible"));
            }
        }

        private bool _isEyeglassLensesAmountVisible = false;
        public bool IsEyeglassLensesAmountVisible
        {
            get
            {
                return _isEyeglassLensesAmountVisible;
            }
            set
            {
                _isEyeglassLensesAmountVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsEyeglassLensesAmountVisible"));
            }
        }

        private bool _isFeeAmountVisible = false;
        public bool IsFeeAmountVisible
        {
            get
            {
                return _isFeeAmountVisible;
            }
            set
            {
                _isFeeAmountVisible = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("IsFeeAmountVisible"));
            }
        }

        public bool IsDateOfExaminationVisible
        {
            get
            {
                if (ClaimSubmissionType != null && ClaimSubmissionType.ID == "EYEEXAM")
                    return true;
                else
                    return false;
            }
        }

        public DateTime? TreatmentDateListViewItem
        {
            get
            {
                switch (ClaimSubmissionType.ID)
                {
                    case "ACUPUNCTURE":
                    case "CHIROPODY":
                    case "CHIRO":
                    case "PHYSIO":
                    case "PODIATRY":
                        return TreatmentDate;
                    case "PSYCHOLOGY":
                    case "MASSAGE":
                    case "NATUROPATHY":
                    case "SPEECH":
                        return TreatmentDate;
                    case "MI":
                        return PickupDate;
                    case "ORTHODONTIC":
                        return DateOfMonthlyTreatment;
                    case "CONTACTS":
                        return DateOfPurchase;
                    case "GLASSES":
                        return DateOfPurchase;
                    case "EYEEXAM":
                        return DateOfExamination;
                    case "DENTAL":
                        return TreatmentDate;
                    default:
                        return null;
                }
            }
        }

        public ClaimOption TreatmentDurationListViewItem
        {
            get
            {
                switch (ClaimSubmissionType.ID)
                {
                    case "ACUPUNCTURE":
                    case "CHIROPODY":
                    case "CHIRO":
                    case "PHYSIO":
                    case "PODIATRY":
                        return null;
                    case "PSYCHOLOGY":
                    case "MASSAGE":
                    case "NATUROPATHY":
                    case "SPEECH":
                        return TreatmentDuration;
                    case "MI":
                        return null;
                    case "ORTHODONTIC":
                        return null;
                    case "CONTACTS":
                        return null;
                    case "GLASSES":
                        return null;
                    case "EYEEXAM":
                        return null;
                    case "DENTAL":
                        return null;
                    default:
                        return null;
                }
            }
        }

        public ClaimSubmissionBenefit TypeOfTreatmentListViewItem
        {
            get
            {
                switch (ClaimSubmissionType.ID)
                {
                    case "ACUPUNCTURE":
                    case "CHIROPODY":
                    case "CHIRO":
                    case "PHYSIO":
                    case "PODIATRY":
                        return TypeOfTreatment;
                    case "PSYCHOLOGY":
                    case "MASSAGE":
                    case "NATUROPATHY":
                    case "SPEECH":
                        return TypeOfTreatment;
                    case "MI":
                        return ItemDescription;
                    case "ORTHODONTIC":
                        TypeOfTreatment.Name = ClaimSubmissionType.Name;
                        return TypeOfTreatment;
                    case "CONTACTS":
                        return TypeOfEyewear;
                    case "GLASSES":
                        return TypeOfEyewear;
                    case "EYEEXAM":
                        TypeOfTreatment.Name = ClaimSubmissionType.Name;
                        return TypeOfTreatment;
                    case "DENTAL":
                        return DentalSubmisionBenefit;
                    default:
                        return null;
                }
            }
        }

        public double? TreatmentAmountListViewItem
        {
            get
            {
                switch (ClaimSubmissionType.ID)
                {
                    case "ACUPUNCTURE":
                    case "CHIROPODY":
                    case "CHIRO":
                    case "PHYSIO":
                    case "PODIATRY":
                        return TreatmentAmount;
                    case "PSYCHOLOGY":
                    case "MASSAGE":
                    case "NATUROPATHY":
                    case "SPEECH":
                        return TreatmentAmount;
                    case "MI":
                        return TreatmentAmount;
                    case "ORTHODONTIC":
                        return OrthodonticMonthlyFee;
                    case "CONTACTS":
                        return TreatmentAmount;
                    case "GLASSES":
                        if (IsTotalAmountChargedVisible)
                            return TotalAmountCharged;
                        else
                            return FrameAmount + EyeglassLensesAmount + FeeAmount;
                    case "EYEEXAM":
                        return TreatmentAmount;
                    case "DENTAL":
                        return (DentistFees ?? 0) + (LaboratoryCharges ?? 0);
                    default:
                        return null;
                }
            }
        }

        private ClaimSubmissionBenefit _dentalSubmissionBenefit;
        public ClaimSubmissionBenefit DentalSubmisionBenefit
        {
            get => _dentalSubmissionBenefit;
            set => SetProperty(ref _dentalSubmissionBenefit, value);
        }

        private string _procedureCode;
        public string ProcedureCode
        {
            get => _procedureCode;
            set
            {
                SetProperty(ref _procedureCode, value);
            }
        }

        public bool IsToothCodeRequired { get; set; }
        public bool IsToothSurfaceRequired { get; set; }
        public bool IsLaboratoryChargesRequired { get; set; }

        private int? _toothCode;
        public int? ToothCode
        {
            get => _toothCode;
            set
            {
                SetProperty(ref _toothCode, value);
            }
        }

        private string _toothSurface;
        public string ToothSurface
        {
            get => _toothSurface;
            set
            {
                SetProperty(ref _toothSurface, value);
            }
        }

        private double? _dentistFees;
        public double? DentistFees
        {
            get => _dentistFees;
            set
            {
                SetProperty(ref _dentistFees, value);
            }
        }

        private double? _laboratoryCharges;
        public double? LaboratoryCharges
        {
            get => _laboratoryCharges;
            set
            {
                SetProperty(ref _laboratoryCharges, value);
            }
        }
    }
}
