using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimDocumentsUploadViewModel : ViewModelBase<ClaimDocumentsUploadViewModelParameters>, ICanDeleteFile, IFileNamesContainer, INonRealTimeClaimUploadProperties
    {
        private readonly IUserDialogs _userDialogService;
        private readonly ILoadFileService _loadFileService;
        private readonly IClaimService _claimService;

        private const int MaxFileSizeB = 25165824;
        private bool _activityIsPaused;
        private List<Action> _pendingActions = new List<Action>();        
        private INonRealTimeClaimUploadProperties _properties;
        private DocumentSelectionHelper _documentSelectionHelper;

        public string Title { get; private set; } = Resource.GenericUploadDocuments;
        public string CombinedSizeOfFilesMustBe => Resource.CombinedSizeOfFilesMustBe;
        public string TwentyFourMb => Resource.TwentyFourMb;
        public string AddAnotherDocument => Resource.AddAnotherDocument;
        public string AdditionalInformation => Resource.AdditionalInformation.ToUpperInvariant();
        public string PleaseSubmitDocuments => Resource.PleaseSubmitDocuments;
        public string Next => Resource.Next;
        public string Comments { get; set; }
        public string ActionSheetTitle { get; private set; }

        public IMvxCommand TakePhotoCommand { get; }
        public IMvxCommand LoadFilesCommand { get; }
        public IMvxCommand ShowDocumentSelectionPopoverCommand { get; }
        public IMvxCommand DeleteCommand { get; }
        public IMvxCommand NavigateToPreviousPageCommand { get; }
        public IMvxCommand OpenDisclaimerCommand { get; }

        public event EventHandler AttachmentsAdded;
        public event EventHandler AttachmentRemoved;

        public bool ActivityIsPaused
        {
            get => _activityIsPaused;
            set
            {
                _activityIsPaused = value;
                if (!value)
                {
                    foreach (Action a in _pendingActions)
                    {
                        a.Invoke();
                    }

                    _pendingActions.Clear();
                }
            }
        }

        public TopCardViewData TopCardViewData { get; set; }
        public ObservableCollection<DocumentInfo> Attachments { get; } = new ObservableCollection<DocumentInfo>();
        public NonRealTimeClaimType ClaimType { get; private set; }

        public ClaimDocumentsUploadViewModel(
            IUserDialogs userDialogService,
            ILoadFileService loadFileService,
            IClaimService claimService)
        {
            Attachments.CollectionChanged += OnAttachmentCollectionChanged;

            _userDialogService = userDialogService;
            _loadFileService = loadFileService;
            _claimService = claimService;

            _documentSelectionHelper = new DocumentSelectionHelper(
                _userDialogService,
                _loadFileService,
                ActionSheetTitle,
                AddFileToAttachments
            );

            TakePhotoCommand = new MvxCommand(_documentSelectionHelper.TakePhoto);
            LoadFilesCommand = new MvxCommand<bool>(_documentSelectionHelper.LoadFile);
            ShowDocumentSelectionPopoverCommand = new MvxCommand(_documentSelectionHelper.ExecuteShowDocumentSelectionPopoverCommand);
            OpenDisclaimerCommand = new MvxCommand(ExecuteOpenDisclaimer);
            DeleteCommand = new MvxCommand<int>(DeleteAttachment);
        }

        private void ExecuteOpenDisclaimer()
        {
            if (!Attachments.Any())
            {
                _userDialogService.Alert(Resource.AddAtLeastOneFile);
                return;
            }

            IDisclaimerProperties properties = (IDisclaimerProperties)NonRealTimeUploadFactory.Create(ClaimType, nameof(ClaimSubmitTermsAndConditionsViewModel));

            ShowViewModel<ClaimSubmitTermsAndConditionsViewModel, ClaimSubmitTermsAndConditionsViewModelParameters>(new ClaimSubmitTermsAndConditionsViewModelParameters(ClaimType, properties, Attachments, Comments));
        }

        private async Task AddFileToAttachments(DocumentInfo file)
        {
            await Task.Yield();

            file.ByteContent = file.ByteContent; //.Compress(); TODO

            if (file.ByteContent.Length > MaxFileSizeB ||
                CalculateAttachmentsWeight(Attachments) + file.ByteContent.Length > MaxFileSizeB)
            {
                ExecuteOrSchedule(ShowMbExceededDialog);
            }
            else
            {
                Attachments.Add(file);
                AttachmentsAdded?.Invoke(this, EventArgs.Empty);
            }
        }

        private async void ShowMbExceededDialog()
        {
            await Task.Delay(400);
            _userDialogService.Alert(Resource.AttachmentsExceedsMbLimit, Resource.ErrorUploadingFile);
        }

        private static int CalculateAttachmentsWeight(IEnumerable<DocumentInfo> attachments)
        {
            return attachments.Sum(attachment => attachment.ByteContent.Length);
        }

        private void OnAttachmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => Attachments);
            OpenDisclaimerCommand.RaiseCanExecuteChanged();
            AttachmentRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void ExecuteOrSchedule(Action action)
        {
            if (ActivityIsPaused)
            {
                _pendingActions.Add(action);
            }
            else
            {
                action.Invoke();
            }
        }

        private void DeleteAttachment(int index)
        {
            Attachments.RemoveAt(index);
        }

        public override void Prepare(ClaimDocumentsUploadViewModelParameters parameter)
        {
            Title = parameter.NonRealTimeClaimUploadProperties.Title;
            ActionSheetTitle = parameter.NonRealTimeClaimUploadProperties.ActionSheetTitle;
            ClaimType = parameter.NonRealTimeClaimUploadProperties.ClaimType;
        }
    }
}