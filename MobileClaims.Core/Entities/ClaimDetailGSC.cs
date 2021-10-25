using System;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimDetailGSC
    {
        [JsonProperty("procedureCode")]
        public string ProcedureCode { get; set; }

        [JsonProperty("lengthOfTreatment")]
        public string LengthOfTreatment { get; set; }

        [JsonProperty("treatmentDate")]
        public string TreatmentDate { get; set; }

        [JsonProperty("claimAmount")]
        public double ClaimAmount { get; set; }

        [JsonProperty("otherPaidAmount")]
        public double OtherPaidAmount { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("isGstIncluded")]
        public bool IsGSTIncluded { get; set; }

        [JsonProperty("isPstIncluded")]
        public bool IsPSTIncluded { get; set; }

        [JsonProperty("isReferralSubmitted")]
        public bool IsReferralSubmitted { get; set; }

        [JsonProperty("referralDate")]
        public string ReferralDate { get; set; }

        [JsonProperty("medicalProfessionalId")]
        public string MedicalProfessionalID { get; set; }

        [JsonProperty("lensTypeCode")]
        public string LensTypeCode { get; set; }

        [JsonProperty("isCorrectiveEyewear")]
        public bool IsCorrectiveEyewear { get; set; }

        [JsonProperty("rightSphere")]
        public string RightSphere { get; set; }

        [JsonProperty("leftSphere")]
        public string LeftSphere { get; set; }

        [JsonProperty("rightCylinder")]
        public string RightCylinder { get; set; }

        [JsonProperty("leftCylinder")]
        public string LeftCylinder { get; set; }

        [JsonProperty("rightAxis")]
        public string RightAxis { get; set; }

        [JsonProperty("leftAxis")]
        public string LeftAxis { get; set; }

        [JsonProperty("rightPrism")]
        public string RightPrism { get; set; }

        [JsonProperty("leftPrism")]
        public string LeftPrism { get; set; }

        [JsonProperty("rightBifocal")]
        public string RightBifocal { get; set; }

        [JsonProperty("leftBifocal")]
        public string LeftBifocal { get; set; }

        [JsonProperty("rightTrifocal")]
        public string RightTrifocal { get; set; }

        [JsonProperty("leftTrifocal")]
        public string LeftTrifocal { get; set; }

        [JsonProperty("frameAmount")]
        public string FrameAmount { get; set; }

        [JsonProperty("lensAmount")]
        public string LensAmount { get; set; }

        [JsonProperty("feeAmount")]
        public string FeeAmount { get; set; }

        [JsonProperty("toothCode")]
        public String ToothCode { get; set; }

        [JsonProperty("toothSurface")]
        public string ToothSurface { get; set; }

        [JsonProperty("dentalFee")]
        public double DentalFee { get; set; }

        [JsonProperty("dentalLabFee")]
        public double DentalLabFee { get; set; }

        [JsonProperty("publicOrGvtAmount")]
        public double PublicOrGvtAmount { get; set; }
    }
}