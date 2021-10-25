using MobileClaims.Core.Entities.HCSA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services.HCSA
{
    public interface IHCSAClaimService
    {
        /// <summary>
        /// Populates the possible Claim Types available to the specified plan member
        /// </summary>
        /// <param name="planMemberID">The complete plan-member ID including the Participant suffix ("12345678-00")</param>
        /// <param name="success">A paramaterless delegate to execute if the invocation succeeds (200 from web-service)</param>
        /// <param name="failure">A parameterless delegate to execute if the invocation fails (any other HTTP response)</param>
        Task GetClaimTypes(string planMemberID, Action success, Action<string, int> failure);

        /// <summary>
        /// Populates the possible expense type available for the selected claim type based to the specified plan member
        /// </summary>
        /// <param name="planMemberID">The complete plan-member ID including the Participant suffix ("12345678-00")</param>
        /// <param name="claimTypeID">The ID of the claim type.  Pass in HCSAService.SelectedClaimType.ID unless you have a super-awesome reason to do something else.  That reason would have to be really awesome though</param>
        /// <param name="success">A paramaterless delegate to execute if the invocation succeeds (200 from web-service)</param>
        /// <param name="failure">A parameterless delegate to execute if the invocation fails (any other HTTP response)</param>
        Task GetClaimExpenseTypes(string planMemberID, long claimTypeID, Action success, Action<string,int> failure);

        /// <summary>
        /// Submits a claim to GSC web services
        /// </summary>
        /// <param name="PlanMemberID">The complete plan-member ID including the Participant suffix ("12345678-00")</param>
        /// <param name="claimTypeID">The ID of the selected Claim Type</param>
        /// <param name="expenseTypeID">The ID of the selected expense type</param>
        /// <param name="medicalProfessionalID">The ID of the medical professional who provided the service being claimed</param>
        /// <param name="claimDetails">The claim details</param>
        /// <param name="success">A paramaterless delegate to execute if the invocation succeeds (200 from web-service)</param>
        /// <param name="failure">A parameterless delegate to execute if the invocation fails (any other HTTP response)</param>
        Task SubmitClaim(string PlanMemberID, long claimTypeID, long expenseTypeID, string medicalProfessionalID, ObservableCollection<ClaimDetail> claimDetails, Action success, Action failure);

        /// <summary>
        /// All available Claim Submission Types.  Includes a null item at the start of the list.  
        /// List is guaranteed not null
        /// </summary>
        List<ClaimType> ClaimSubmissionTypes { get; set; }

        /// <summary>
        /// All available Expense types for the selected claim submission type.  Includes a null item at the start of the list. 
        /// List is guaranteed not null
        /// </summary>
        List<ExpenseType> ExpenseTypes { get; set; }

        /// <summary>
        /// Indicates the expense type the user has selected either during this session or during the previous, restored session. No guarantees are made as to whether this property does or does not return null.
        /// </summary>
        ExpenseType SelectedExpenseType { get; set; }

        /// <summary>
        /// Indicates the claim type the user has selected either during this session or during the previous, restored session. No guarantees are made as to whether this property does or does not return null.
        /// </summary>
        ClaimType SelectedClaimType { get; set; }

        bool TermsAndConditionsHaveBeenShown { get; set; }

        /// <summary>
        /// The current HCSA claim the plan member is working with
        /// </summary>
        Claim Claim { get; set; }
        bool SelectedExpenseTypeRequiresReferral { get; }

        /// <summary>
        /// Serializes the current HCSA claim to local storage
        /// </summary>
        void PersistClaim();

        /// <summary>
        /// Resets the current HCSA claim to an empty state 
        /// </summary>
        void ClearClaim();

        /// <summary>
        /// Addresses issues caused when the user has added, then removed claim details. This messes up the back flow
        /// </summary>
        bool HaveClaimDetailsAlreadyBeenInitialized { get; set; }
    }
}
