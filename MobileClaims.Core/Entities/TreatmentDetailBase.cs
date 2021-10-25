using System;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.Entities
{
    public abstract class TreatmentDetailBase : MvxNotifyPropertyChanged
    {
        private Guid _id;
        private bool _isAlternateCarrierPaymentVisible;
        private double? _totalTreatmentAmount;
        private double _amountPaidByAlternateCarrier;
        private DateTime _treatmentDate;
        private ClaimSubmissionBenefit _typeOfTreatment;
        private ClaimSubmissionType _claimSubmissionType;

        public Guid ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime TreatmentDate
        {
            get => _treatmentDate;
            set
            {
                SetProperty(ref _treatmentDate, value);
                // RaisePropertyChanged("TreatmentDateListViewItem");
            }
        }

        public virtual ClaimSubmissionBenefit TypeOfTreatment
        {
            get => _typeOfTreatment;
            set
            {
                SetProperty(ref _typeOfTreatment, value);
                // RaisePropertyChanged("TypeOfTreatmentListViewItem");
            }
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get => _claimSubmissionType;
            set => SetProperty(ref _claimSubmissionType, value);
        }

        public virtual double? TotalTreatmentAmount
        {
            get => _totalTreatmentAmount;
            set => SetProperty(ref _totalTreatmentAmount, value);
        }

        public double AmountPaidByAlternateCarrier
        {
            get => _amountPaidByAlternateCarrier;
            set => SetProperty(ref _amountPaidByAlternateCarrier, value);
        }

        public bool IsAlternateCarrierPaymentVisible
        {
            get => _isAlternateCarrierPaymentVisible;
            set => SetProperty(ref _isAlternateCarrierPaymentVisible, value);
        }
    }

    public class SpeechTreatmentDetails : TreatmentDetailBase
    {
        private ClaimOption _treatmentDuration;

        public ClaimOption TreatmentDuration
        {
            get => _treatmentDuration;
            set => SetProperty(ref _treatmentDuration, value);
        }
    }

    public class DentalTreatmentDetails : TreatmentDetailBase
    {
        private int _procedureCode;
        private int _toothCode;
        private double _dentistFees;
        private double _laboratoryCharges;
        private string _toothSurface;

        public int ProcedureCode
        {
            get => _procedureCode;
            set
            {
                SetProperty(ref _procedureCode, value);
            }
        }

        public int ToothCode
        {
            get => _toothCode;
            set
            {
                SetProperty(ref _toothCode, value);
            }
        }

        public string ToothSurface
        {
            get => _toothSurface;
            set
            {
                SetProperty(ref _toothSurface, value);
            }
        }

        public double DentistFees
        {
            get => _dentistFees;
            set
            {
                SetProperty(ref _dentistFees, value);
            }
        }

        public double LaboratoryCharges
        {
            get => _laboratoryCharges;
            set
            {
                SetProperty(ref _laboratoryCharges, value);
            }
        }

        public override double? TotalTreatmentAmount
        {
            get => DentistFees + LaboratoryCharges;
        }
    }

    public class GlassesTreatmentDetail : EyewearTreatmentDetail
    {
        //ID = Guid.NewGuid(),
        //ClaimSubmissionType = this.ClaimSubmissionType,
        //DateOfPurchase = this.DateOfPurchase,
        //TypeOfEyewear = this.TypeOfEyewear,
        //TypeOfLens = this.TypeOfLens,
        //TreatmentAmount = this.TotalDollarValue,
        //TotalAmountCharged = tac,
        //IsTotalAmountChargedVisible = this.IsTotalAmountChargedVisible,
        //FrameAmount = fa,
        //EyeglassLensesAmount = ela,
        //FeeAmount = fee,
        //IsFeeAmountVisible = this.IsFrameLensAndFeeEntryVisible,
        //IsEyeglassLensesAmountVisible = this.IsFrameLensAndFeeEntryVisible,
        //IsFrameAmountVisible = this.IsFrameLensAndFeeEntryVisible,
        //IsAcknowledgeEyewearIsCorrectiveVisible = this.IsAcknowledgeEyewearIsCorrectiveVisible,
        //AcknowledgeEyewearIsCorrective = this.AcknowledgeEyewareIsCorrective,
        //AlternateCarrierPayment = acp,
        //IsAlternateCarrierPaymentVisible = this.AmountPaidByAlternateCarrierVisible

        private double _frameAmount;
        private double _eyeglassLensesAmount;
        private double _feeAmount;
        private double _totalAmountCharged;

        private bool _isTotalAmountChargedVisible;
        private bool _isFrameAmountVisible;
        private bool _isEyeglassLensesAmountVisible;
        private bool _isFeeAmountVisible;
        private LensType _typeOfLens;

        public bool IsFrameAmountVisible
        {
            get => _isFrameAmountVisible;
            set => SetProperty(ref _isFrameAmountVisible, value);
        }

        public bool IsEyeglassLensesAmountVisible
        {
            get =>_isEyeglassLensesAmountVisible;
            set => SetProperty(ref _isEyeglassLensesAmountVisible, value);
        }
        
        public bool IsFeeAmountVisible
        {
            get => _isFeeAmountVisible;
            set => SetProperty(ref _isFeeAmountVisible, value);
        }

        public bool IsTotalAmountChargedVisible
        {
            get => _isTotalAmountChargedVisible;
            set => SetProperty(ref _isTotalAmountChargedVisible, value);
        }

        public double FrameAmount
        {
            get => _frameAmount;
            set
            {
                SetProperty(ref _frameAmount, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public double EyeglassLensesAmount
        {
            get => _eyeglassLensesAmount;
            set
            {
                SetProperty(ref _eyeglassLensesAmount, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public double FeeAmount
        {
            get => _feeAmount;
            set
            {
                SetProperty(ref _feeAmount, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public double TotalAmountCharged
        {
            get => _totalAmountCharged;
            set
            {
                SetProperty(ref _totalAmountCharged, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public override double? TotalTreatmentAmount
        {
            get
            {
                if (IsTotalAmountChargedVisible)
                {
                    return TotalAmountCharged;
                }
                else
                {
                    return FrameAmount + EyeglassLensesAmount + FeeAmount;
                }
            }
        }

        public LensType TypeOfLens
        {
            get => _typeOfLens;
            set => SetProperty(ref _typeOfLens, value);
        }
    }

    public class EyewearTreatmentDetail : TreatmentDetailBase
    {
        private DateTime _dateOfPurchase;
        private ClaimSubmissionBenefit _typeOfEyewear;
        private bool _isAcknowledgeEyewearIsCorrectiveVisible;
        private bool _acknowledgeEyewearIsCorrective;
        private bool _isPrescriptionDetailsVisible;

        private ClaimOption _rightSphere;
        private ClaimOption _leftSphere;
        private ClaimOption _rightCylinder;
        private ClaimOption _leftCylinder;
        private ClaimOption _rightAxis;
        private ClaimOption _leftAxis;
        private ClaimOption _rightPrism;
        private ClaimOption _leftPrism;
        private ClaimOption _rightBifocal;
        private ClaimOption _leftBifocal;
        private bool _isRightSphereEnabled;
        private bool _isLeftSphereEnabled;
        private bool _isRightCylinderEnabled;
        private bool _isLeftCylinderEnabled;
        private bool _isRightAxisEnabled;
        private bool _isLeftAxisEnabled;
        private bool _isRightPrismEnabled;
        private bool _isLeftPrismEnabled;
        private bool _isRightBifocalEnabled;
        private bool _isLeftBifocalEnabled;

        public DateTime DateOfPurchase
        {
            get => _dateOfPurchase;
            set
            {
                SetProperty(ref _dateOfPurchase, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public ClaimSubmissionBenefit TypeOfEyewear
        {
            get
            {
                return _typeOfEyewear;
            }
            set
            {
                SetProperty(ref _typeOfEyewear, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public bool IsAcknowledgeEyewearIsCorrectiveVisible
        {
            get => _isAcknowledgeEyewearIsCorrectiveVisible;
            set => SetProperty(ref _isAcknowledgeEyewearIsCorrectiveVisible, value);
        }

        public bool AcknowledgeEyewearIsCorrective
        {
            get => _acknowledgeEyewearIsCorrective;
            set
            {
                SetProperty(ref _acknowledgeEyewearIsCorrective, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public bool IsPrescriptionDetailsVisible
        {
            get => _isPrescriptionDetailsVisible;
            set => SetProperty(ref _isPrescriptionDetailsVisible, value);
        }

        public ClaimOption RightSphere
        {
            get => _rightSphere;
            set => SetProperty(ref _rightSphere, value);
        }

        public ClaimOption LeftSphere
        {
            get => _leftSphere;
            set => SetProperty(ref _leftSphere, value);
        }

        public ClaimOption RightCylinder
        {
            get => _rightCylinder;
            set => SetProperty(ref _rightCylinder, value);
        }

        public ClaimOption LeftCylinder
        {
            get => _leftCylinder;
            set => SetProperty(ref _leftCylinder, value);
        }

        public ClaimOption RightAxis
        {
            get => _rightAxis;
            set => SetProperty(ref _rightAxis, value);
        }

        public ClaimOption LeftAxis
        {
            get => _leftAxis;
            set => SetProperty(ref _leftAxis, value);
        }

        public ClaimOption RightPrism
        {
            get => _rightPrism;
            set => SetProperty(ref _rightPrism, value);
        }

        public ClaimOption LeftPrism
        {
            get => _leftPrism;
            set => SetProperty(ref _leftPrism, value);
        }

        public ClaimOption RightBifocal
        {
            get => _rightBifocal;
            set => SetProperty(ref _rightBifocal, value);
        }

        public ClaimOption LeftBifocal
        {
            get => _leftBifocal;
            set => SetProperty(ref _leftBifocal, value);
        }

        public bool IsRightSphereEnabled
        {
            get => _isRightSphereEnabled;
            set => SetProperty(ref _isRightSphereEnabled, value);
        }

        public bool IsLeftSphereEnabled
        {
            get => _isLeftSphereEnabled;
            set => SetProperty(ref _isLeftSphereEnabled, value);
        }

        public bool IsRightCylinderEnabled
        {
            get => _isRightCylinderEnabled;
            set => SetProperty(ref _isRightCylinderEnabled, value);
        }

        public bool IsLeftCylinderEnabled
        {
            get => _isLeftCylinderEnabled;
            set => SetProperty(ref _isLeftCylinderEnabled, value);
        }

        public bool IsRightAxisEnabled
        {
            get => _isRightAxisEnabled;
            set => SetProperty(ref _isRightAxisEnabled, value);
        }

        public bool IsLeftAxisEnabled
        {
            get => _isLeftAxisEnabled;
            set => SetProperty(ref _isLeftAxisEnabled, value);
        }

        public bool IsRightPrismEnabled
        {
            get => _isRightPrismEnabled;
            set => SetProperty(ref _isRightPrismEnabled, value);
        }

        public bool IsLeftPrismEnabled
        {
            get => _isLeftPrismEnabled;
            set => SetProperty(ref _isLeftPrismEnabled, value);
        }

        public bool IsRightBifocalEnabled
        {
            get => _isRightBifocalEnabled;
            set => SetProperty(ref _isRightBifocalEnabled, value);
        }

        public bool IsLeftBifocalEnabled
        {
            get => _isLeftBifocalEnabled;
            set => SetProperty(ref _isLeftBifocalEnabled, value);
        }
    }

    public class ContactTreatmentDetail : EyewearTreatmentDetail
    {

    }

    public class MedicalItemTreatentDetail : TreatmentDetailBase
    {
        private string _itemDescriptiomn;
        private DateTime _pickupDate;

        public string ItemDescription
        {
            get => _itemDescriptiomn;
            set => SetProperty(ref _itemDescriptiomn, value);
        }

        public DateTime PickupDate
        {
            get => _pickupDate;
            set => SetProperty(ref _pickupDate, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
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
                SetProperty(ref _gstHstIncludedInTotal, value);
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
                SetProperty(ref _pstIncludedInTotal, value);
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
                SetProperty(ref _isReferralNotSubmitted, value);
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
                IsReferralNotSubmitted = !value;
                SetProperty(ref _hasReferralBeenPreviouslySubmitted, value);
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
                SetProperty(ref _typeOfMedicalProfessional, value);
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
                SetProperty(ref _dateOfReferral, value);
            }
        }
    }

    public class OmfTreatmentDetail : TreatmentDetailBase
    {
        private double _orthodonticMonthlyFee;
        private DateTime _dateOfMonthlyTreatment;

        public double OrthodonticMonthlyFee
        {
            get
            {
                return _orthodonticMonthlyFee;
            }
            set
            {
                SetProperty(ref _orthodonticMonthlyFee, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }

        public DateTime DateOfMonthlyTreatment
        {
            get
            {
                return _dateOfMonthlyTreatment;
            }
            set
            {
                SetProperty(ref _dateOfMonthlyTreatment, value);
                RaisePropertyChanged(nameof(TotalTreatmentAmount));
            }
        }
    }
}
