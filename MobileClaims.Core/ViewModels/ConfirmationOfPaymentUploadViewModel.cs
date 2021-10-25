using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class ConfirmationOfPaymentUploadViewModel : ViewModelBase<IViewModelParameters>, IFileNamesContainer, ICanDeleteFile, IClaimUploadProperties
    {
        private const int MaxFileSizeB = 25165824;
        private readonly IUserDialogs _userDialogService;
        private readonly ILoadFileService _loadFileService;
        private readonly DocumentSelectionHelper _documentSelectionHelper;

        private IClaimUploadProperties _modelType;
        private bool _activityIsPaused;
        private UploadDocumentsFormData _uploadDocumentsFormData;
        private List<Action> _pendingActions = new List<Action>();

        public event EventHandler AttachmentsAdded;
        public event EventHandler AttachmentRemoved;
        public event EventHandler ViewModelWillDestroy;

        // This is maintain the information about where to go back to.
        public NavigationCatalog NavigationCatalog { get; set; }

        public string Title { get; private set; }
        public string ActionSheetTitle { get; private set; }
        public string DocumentsToUpload => Resource.DocumentsToUpload;
        public string AddAnotherDocument => Resource.AddAnotherDocument;
        public string Next => Resource.Next;
        public string CombinedSizeOfFilesMustBe => Resource.CombinedSizeOfFilesMustBe;
        public string TwentyFourMb => Resource.TwentyFourMb;
        public bool IsCommentVisible { get; }

        public ObservableCollection<DocumentInfo> Attachments { get; } = new ObservableCollection<DocumentInfo>();

        public bool ActivityIsPaused
        {
            get { return _activityIsPaused; }
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

        public IMvxCommand TakePhotoCommand { get; }

        public IMvxCommand LoadFilesCommand { get; }

        public IMvxCommand SubmitAttachmentsCommand { get; }

        public IMvxCommand ShowDocumentSelectionPopoverCommand { get; }

        public IMvxCommand DeleteCommand { get; }

        public IMvxCommand NavigateToPreviousPageCommand { get; }

        public ConfirmationOfPaymentUploadViewModel(IUserDialogs userDialogService, ILoadFileService loadFileService)
        {
            Attachments.CollectionChanged += OnAttachmentCollectionChanged;

            _userDialogService = userDialogService;
            _loadFileService = loadFileService;

            _documentSelectionHelper = new DocumentSelectionHelper(
                _userDialogService,
                _loadFileService,
                ActionSheetTitle,
                AddFileToAttachments
            );

            TakePhotoCommand = new MvxCommand(_documentSelectionHelper.TakePhoto);
            LoadFilesCommand = new MvxCommand<bool>(_documentSelectionHelper.LoadFile);
            ShowDocumentSelectionPopoverCommand =
                new MvxCommand(_documentSelectionHelper.ExecuteShowDocumentSelectionPopoverCommand);
            SubmitAttachmentsCommand = new MvxCommand(ExecuteSubmitAttachment);
            DeleteCommand = new MvxCommand<int>(DeleteAttachment);
            NavigateToPreviousPageCommand = new MvxCommand(NavigateToPreviousPage);
        }

        private void NavigateToPreviousPage()
        {
            //ShowViewModel(Type.GetType(NavigationCatalog.NavigateFrom));
        }

        protected override void GoBack()
        {
            ViewModelWillDestroy?.Invoke(this, EventArgs.Empty);
            Attachments.CollectionChanged -= OnAttachmentCollectionChanged;
            base.GoBack();
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

        private void ExecuteSubmitAttachment()
        {
            if (!Attachments.Any())
            {
                _userDialogService.Alert(Resource.AddAtLeastOneFile);
                return;
            }

            IClaimSubmitProperties uploadable = (IClaimSubmitProperties)UploadFactory.Create(TopCardViewData.ClaimActionState,
                nameof(ConfirmationOfPaymentSubmitViewModel));

            ShowViewModel<ConfirmationOfPaymentSubmitViewModel, ConfirmationOfPaymentSubmitViewModelParameters>(new ConfirmationOfPaymentSubmitViewModelParameters(TopCardViewData, _uploadDocumentsFormData, uploadable, Attachments));
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
            SubmitAttachmentsCommand.RaiseCanExecuteChanged();
            AttachmentRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void DeleteAttachment(int index)
        {
            Attachments.RemoveAt(index);
        }

        public override void Prepare(IViewModelParameters param)
        {
            ConfirmationOfPaymentUploadViewModelParameters parameter = (ConfirmationOfPaymentUploadViewModelParameters)param;
            _uploadDocumentsFormData = parameter.PlanMemberData;
            TopCardViewData = parameter.TopCardViewData;
            Title = parameter.ClaimUploadProperties.Title;
            ActionSheetTitle = parameter.ClaimUploadProperties.ActionSheetTitle;

            if(parameter.NavigationCatalog != null)
            {
                NavigationCatalog = parameter.NavigationCatalog;
            }
        }
    }
}