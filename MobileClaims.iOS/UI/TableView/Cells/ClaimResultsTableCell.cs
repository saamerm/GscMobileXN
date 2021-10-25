using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("ClaimResultsTableCell")]
    public class ClaimResultsTableCell : MvxTableViewCell
    {
        public UIView contentContainer;

        public UILabel titleLabel;

        public UILabel planInformationLabel;
        public UILabel totalsLabel;
        public UILabel claimDetailsLabel;
        public UILabel limitationLabel;

        public UILabel idTitleLabel;
        public UILabel nameTitleLabel;
        public UILabel dateTitleLabel;
        public UILabel claimedAmountTitleLabel;
        public UILabel otherPaidAmountTitleLabel;
        public UILabel paidAmountTitleLabel;
        public UILabel idLabel;
        public UILabel nameLabel;
        public UILabel dateLabel;
        public UILabel claimedAmountLabel;
        public UILabel otherPaidAmountLabel;
        public UILabel paidAmountLabel;

        protected UITableView limitationsTableView;
        private MvxLimitationsTableViewSource limitationsTableSource;

        protected UITableView tableView;
        private MvxResultsDetailsTableViewSource tableSource;

        protected UILabel awaitingPaymentNote;

        protected UIView cellBackingView;

        public ClaimResultsTableCell() : base()
        {
            CreateLayout();
            InitializeBindings();
        }

        public ClaimResultsTableCell(IntPtr handle) : base(handle)
        {
            CreateLayout();
            InitializeBindings();
        }

        public override void LayoutSubviews()
        {
            CreateLayout();
            base.LayoutSubviews();
        }

        private float redrawCount = 0;

        public void CreateLayout()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;

            float fieldPadding = 20;
            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float contentWidth = (float)this.Frame.Width - sidePadding * 2;
            float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth / 2 + fieldPadding;
            float topPadding = 15;
            float yPos = 0;
            float initialY = topPadding;

            if (cellBackingView == null)
                cellBackingView = new UIView();
            cellBackingView.Frame = new CGRect(0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
            cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            BackgroundView = cellBackingView;

            if (contentContainer == null)
                contentContainer = new UILabel();
            AddSubview(contentContainer);

            if (titleLabel == null)
                titleLabel = new UILabel();
            titleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            titleLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            titleLabel.BackgroundColor = Colors.Clear;
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.Lines = 0;
            contentContainer.AddSubview(titleLabel);

            switch (ResultTypeID)
            {
                case 1:
                    titleLabel.Text = "spouseResults".tr();
                    titleLabel.Frame = new CGRect(Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, 0, Frame.Width - sidePadding * 2, (float)titleLabel.Frame.Height);
                    titleLabel.SizeToFit();
                    titleLabel.Frame = new CGRect(Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, 0, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height);
                    yPos += (float)titleLabel.Frame.Height + fieldPadding;
                    titleLabel.Hidden = false;
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(SpendingAccountModelName))
                    {
                        titleLabel.Text = "spendingAccountResults".tr() + "-" + _spendingAccountModelName.ToUpper();
                    }
                    else
                    {
                        titleLabel.Text = "spendingAccountResults".tr() + _awaitingPaymentMessage;
                    }
                    titleLabel.Frame = new CGRect(Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, 0, Frame.Width - sidePadding * 2, (float)titleLabel.Frame.Height);
                    titleLabel.SizeToFit();
                    titleLabel.Frame = new CGRect(Frame.Width / 2 - (float)titleLabel.Frame.Width / 2, 0, (float)titleLabel.Frame.Width, (float)titleLabel.Frame.Height);
                    yPos += (float)titleLabel.Frame.Height + fieldPadding;
                    titleLabel.Hidden = false;
                    break;

                default:
                    titleLabel.Hidden = true;
                    break;
            }

            if (planInformationLabel == null)
                planInformationLabel = new UILabel();
            planInformationLabel.Text = "planInformation".tr();
            planInformationLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            planInformationLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            planInformationLabel.TextColor = Colors.DARK_GREY_COLOR;
            planInformationLabel.BackgroundColor = Colors.Clear;
            planInformationLabel.TextAlignment = UITextAlignment.Left;
            planInformationLabel.Lines = 0;
            planInformationLabel.SizeToFit();
            planInformationLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(planInformationLabel);

            yPos += (float)planInformationLabel.Frame.Height + 5 + fieldPadding;

            if (idLabel == null)
                idLabel = new UILabel();           
            idLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            idLabel.TextColor = Colors.DARK_GREY_COLOR;
            idLabel.TextAlignment = UITextAlignment.Left;
            idLabel.BackgroundColor = Colors.Clear;
            idLabel.Lines = 0;
            idLabel.SizeToFit();
            idLabel.AdjustsFontSizeToFitWidth = true;
            idLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE + 5);
            contentContainer.AddSubview(idLabel);

            if (idTitleLabel == null)
                idTitleLabel = new UILabel();
            idTitleLabel.Text = "greenShieldId".FormatWithBrandKeywords(LocalizableBrand.GreenShield);
            idTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            idTitleLabel.TextAlignment = UITextAlignment.Left;
            idTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            idTitleLabel.BackgroundColor = Colors.Clear;
            idTitleLabel.Lines = 0;
            idTitleLabel.SizeToFit();
            idTitleLabel.AdjustsFontSizeToFitWidth = true;
            idTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE + 5);
            contentContainer.AddSubview(idTitleLabel);

            yPos += (float)idTitleLabel.Frame.Height + fieldPadding;

            if (nameLabel == null)
                nameLabel = new UILabel();
            nameLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            nameLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            nameLabel.TextAlignment = UITextAlignment.Left;
            nameLabel.TextColor = Colors.DARK_GREY_COLOR;
            nameLabel.BackgroundColor = Colors.Clear;
            nameLabel.Lines = 0;
            nameLabel.SizeToFit();
            nameLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(nameLabel);

            if (nameTitleLabel == null)
                nameTitleLabel = new UILabel();
            nameTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            nameTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            nameTitleLabel.TextAlignment = UITextAlignment.Left;
            nameTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            nameTitleLabel.BackgroundColor = Colors.Clear;
            nameTitleLabel.Lines = 0;
            nameTitleLabel.Text = "participantName".tr();
            nameTitleLabel.SizeToFit();
            nameTitleLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(nameTitleLabel);

            yPos += (float)nameTitleLabel.Frame.Height + fieldPadding;

            if (dateLabel == null)
                dateLabel = new UILabel();
            dateLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            dateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            dateLabel.TextAlignment = UITextAlignment.Left;
            dateLabel.TextColor = Colors.DARK_GREY_COLOR;
            dateLabel.BackgroundColor = Colors.Clear;
            dateLabel.Lines = 0;
            dateLabel.SizeToFit();
            dateLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(dateLabel);

            if (dateTitleLabel == null)
                dateTitleLabel = new UILabel();
            dateTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            dateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            dateTitleLabel.TextAlignment = UITextAlignment.Left;
            dateTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            dateTitleLabel.BackgroundColor = Colors.Clear;
            dateTitleLabel.Lines = 0;
            dateTitleLabel.Text = "submissionDate".tr();
            dateTitleLabel.SizeToFit();
            dateTitleLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(dateTitleLabel);

            yPos += (float)dateTitleLabel.Frame.Height + fieldPadding * 2;

            if (totalsLabel == null)
                totalsLabel = new UILabel();
            totalsLabel.Text = "totals".tr();
            totalsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            totalsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            totalsLabel.TextColor = Colors.DARK_GREY_COLOR;
            totalsLabel.BackgroundColor = Colors.Clear;
            totalsLabel.TextAlignment = UITextAlignment.Left;
            totalsLabel.Lines = 0;
            totalsLabel.SizeToFit();
            totalsLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(totalsLabel);

            yPos += (float)totalsLabel.Frame.Height + fieldPadding;

            if (claimedAmountLabel == null)
                claimedAmountLabel = new UILabel();
            claimedAmountLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            claimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            claimedAmountLabel.TextAlignment = UITextAlignment.Left;
            claimedAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            claimedAmountLabel.BackgroundColor = Colors.Clear;
            claimedAmountLabel.Lines = 0;
            claimedAmountLabel.SizeToFit();
            claimedAmountLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(claimedAmountLabel);

            if (claimedAmountTitleLabel == null)
                claimedAmountTitleLabel = new UILabel();
            claimedAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            claimedAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            claimedAmountTitleLabel.TextAlignment = UITextAlignment.Left;
            claimedAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            claimedAmountTitleLabel.BackgroundColor = Colors.Clear;
            claimedAmountTitleLabel.Lines = 0;
            claimedAmountTitleLabel.Text = "claimedAmount".tr();
            claimedAmountTitleLabel.SizeToFit();
            claimedAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(claimedAmountTitleLabel);

            yPos += (float)claimedAmountTitleLabel.Frame.Height + fieldPadding;

            if (otherPaidAmountLabel == null)
                otherPaidAmountLabel = new UILabel();
            otherPaidAmountLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            otherPaidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            otherPaidAmountLabel.TextAlignment = UITextAlignment.Left;
            otherPaidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            otherPaidAmountLabel.BackgroundColor = Colors.Clear;
            otherPaidAmountLabel.Lines = 0;
            otherPaidAmountLabel.SizeToFit();
            otherPaidAmountLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(otherPaidAmountLabel);

            if (otherPaidAmountTitleLabel == null)
                otherPaidAmountTitleLabel = new UILabel();
            otherPaidAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            otherPaidAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            otherPaidAmountTitleLabel.TextAlignment = UITextAlignment.Left;
            otherPaidAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            otherPaidAmountTitleLabel.BackgroundColor = Colors.Clear;
            otherPaidAmountTitleLabel.Lines = 0;
            otherPaidAmountTitleLabel.Text = "otherPaidAmount".tr();
            otherPaidAmountTitleLabel.SizeToFit();
            otherPaidAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(otherPaidAmountTitleLabel);

            yPos += (float)otherPaidAmountTitleLabel.Frame.Height + fieldPadding;

            if (paidAmountLabel == null)
                paidAmountLabel = new UILabel();
            paidAmountLabel.Frame = new CGRect(resultPos, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
            paidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            paidAmountLabel.TextAlignment = UITextAlignment.Left;
            paidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            paidAmountLabel.BackgroundColor = Colors.Clear;
            paidAmountLabel.Lines = 0;
            paidAmountLabel.SizeToFit();
            paidAmountLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(paidAmountLabel);

            if (paidAmountTitleLabel == null)
                paidAmountTitleLabel = new UILabel();
            paidAmountTitleLabel.Frame = new CGRect(sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            paidAmountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            paidAmountTitleLabel.TextAlignment = UITextAlignment.Left;
            paidAmountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
            paidAmountTitleLabel.BackgroundColor = Colors.Clear;
            paidAmountTitleLabel.Lines = 0;
            paidAmountTitleLabel.Text = "paidAmount".tr();
            paidAmountTitleLabel.SizeToFit();
            paidAmountTitleLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(paidAmountTitleLabel);

            yPos += (float)paidAmountTitleLabel.Frame.Height + fieldPadding;

            if (claimDetailsLabel == null)
                claimDetailsLabel = new UILabel();
            claimDetailsLabel.Text = "claimDetailsTitle".tr();
            claimDetailsLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
            claimDetailsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            claimDetailsLabel.TextColor = Colors.DARK_GREY_COLOR;
            claimDetailsLabel.BackgroundColor = Colors.Clear;
            claimDetailsLabel.TextAlignment = UITextAlignment.Left;
            claimDetailsLabel.Lines = 0;
            claimDetailsLabel.SizeToFit();
            claimDetailsLabel.AdjustsFontSizeToFitWidth = true;
            contentContainer.AddSubview(claimDetailsLabel);

            yPos += (float)claimDetailsLabel.Frame.Height + fieldPadding;

            if (tableView == null)
                tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            tableView.TableHeaderView = new UIView();
            tableView.SeparatorColor = Colors.Clear;
            //tableView.RowHeight = tableRowHeight;
            tableView.UserInteractionEnabled = false;
            tableView.ShowsVerticalScrollIndicator = true;
            contentContainer.AddSubview(tableView);

            tableView.SetNeedsLayout();
            tableView.Frame = new CGRect(0, yPos, contentWidth, (nfloat)tableView.ContentSize.Height + fieldPadding * 2);

            yPos += (float)tableView.Frame.Height;

            if (limitationLabel == null)
                limitationLabel = new UILabel();
            limitationLabel.Text = "planLimitations".tr();
            limitationLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
            limitationLabel.TextColor = Colors.DARK_GREY_COLOR;
            limitationLabel.BackgroundColor = Colors.Clear;
            limitationLabel.TextAlignment = UITextAlignment.Left;
            limitationLabel.Lines = 0;
            limitationLabel.SizeToFit();
            limitationLabel.AdjustsFontSizeToFitWidth = true;

            if (limitationsTableView == null)
                limitationsTableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            limitationsTableView.TableHeaderView = new UIView();
            limitationsTableView.SeparatorColor = Colors.Clear;
            limitationsTableView.UserInteractionEnabled = false;
            limitationsTableView.ShowsVerticalScrollIndicator = true;

            if (IsPlanLimitationVisible)
            {
                contentContainer.AddSubview(limitationLabel);
                contentContainer.AddSubview(limitationsTableView);

                limitationLabel.Frame = new CGRect(sidePadding, yPos, Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

                float limitationLabelHeight = (float)limitationLabel.Frame.Height;//+ fieldPadding;
                yPos += limitationLabelHeight;

                nfloat contentHeight = 0;
                for (int i = 0; i < PlanLimitations.Count; i++)
                {
                    var rowRect = limitationsTableView.RectForRowAtIndexPath(NSIndexPath.FromRowSection(i, 0));
                    contentHeight += rowRect.Height;
                }
                limitationsTableView.Frame = new CGRect(0, yPos + 5, contentWidth, contentHeight + 5);
                yPos += (float)limitationsTableView.Frame.Height + fieldPadding;
            }

            if (awaitingPaymentNote == null)
                awaitingPaymentNote = new UILabel();
            awaitingPaymentNote.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            awaitingPaymentNote.TextColor = Colors.DARK_GREY_COLOR;
            awaitingPaymentNote.TextAlignment = UITextAlignment.Left;
            awaitingPaymentNote.BackgroundColor = Colors.Clear;
            awaitingPaymentNote.LineBreakMode = UILineBreakMode.WordWrap;
            awaitingPaymentNote.Lines = 0;

            switch (ResultTypeID)
            {
                case 1:
                    awaitingPaymentNote.Text = "awaitingPaymentNote".tr();
                    break;

                case 2:
                    awaitingPaymentNote.Text = AwaitingPaymentNote;
                    break;

                default:
                    awaitingPaymentNote.Text = "awaitingPaymentNote".tr();
                    break;
            }

            awaitingPaymentNote.Frame = new CGRect(sidePadding, yPos, Frame.Width - sidePadding * 2, (float)awaitingPaymentNote.Frame.Height + fieldPadding);
            awaitingPaymentNote.SizeToFit();
            awaitingPaymentNote.Frame = new CGRect(sidePadding, yPos, (float)awaitingPaymentNote.Frame.Width, (float)awaitingPaymentNote.Frame.Height + fieldPadding);

            contentContainer.AddSubview(awaitingPaymentNote);

            yPos += (float)awaitingPaymentNote.Frame.Height + fieldPadding;
            contentContainer.Frame = new CGRect(0, initialY, Frame.Width, yPos);

            if (redrawCount < 2)
            {
                redrawCount++;
                this.SetNeedsLayout();
            }
        }

        public virtual bool ShowsDeleteButton()
        {
            return false;
        }

        public virtual void InitializeBindings()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<ClaimResultsTableCell, ClaimResultGSC>();
                    set.Bind(this.idLabel).To(item => item.PlanMemberDisplayID).OneWay();
                    set.Bind(this.nameLabel).To(item => item.ParticipantFullName).OneWay();
                    set.Bind(this.dateLabel).To(item => item.SubmissionDate).OneWay();
                    set.Bind(this.claimedAmountLabel).To(item => item.ClaimedAmountTotal).WithConversion("DollarSignDoublePrefix").OneWay();
                    set.Bind(this.otherPaidAmountLabel).To(item => item.OtherPaidAmountTotal).WithConversion("DollarSignDoublePrefix").OneWay();
                    set.Bind(this.paidAmountLabel).To(item => item.PaidAmountTotal).WithConversion("DollarSignDoublePrefix").OneWay();
                    set.Bind(this).For(v => v.ClaimDetails).To(vm => vm.ClaimResultDetails);
                    set.Bind(this).For(v => v.PlanLimitations).To(item => item.PlanLimitations);
                    set.Bind(this).For(v => v.IsPlanLimitationVisible).To(item => item.IsPlanLimitationVisible);
                    set.Bind(this).For(v => v.ResultTypeID).To(item => item.ResultTypeID);
                    set.Bind(this).For(v => v.AwaitingPaymentMessage).To(item => item.AwaitingPaymentMessage);
                    set.Bind(this).For(v => v.SpendingAccountModelName).To(item => item.SpendingAccountModelName);
                    set.Bind(this).For(v => v.AwaitingPaymentNote).To(item => item.AwaitingPaymentNote);
                    set.Apply();

                    limitationsTableSource = new MvxLimitationsTableViewSource(PlanLimitations, limitationsTableView, "ClaimLimitationsTableCell", typeof(ClaimLimitationsTableCell));
                    limitationsTableView.Source = limitationsTableSource;
                    set.Bind(limitationsTableSource).To(vm => vm.PlanLimitations);
                    set.Apply();

                    tableView.ReloadData();

                    this.SetNeedsLayout();
                });
        }

        private float tableRowHeight = Constants.CLAIM_RESULTS_DETAILS_HEIGHT;

        private List<ClaimResultDetailGSC> _claimDetails;

        public List<ClaimResultDetailGSC> ClaimDetails
        {
            get
            {
                return _claimDetails;
            }
            set
            {
                _claimDetails = value;

                if (value != null)
                {
                    tableSource = new MvxResultsDetailsTableViewSource(value, tableView, "ClaimResultDetailTableCell", typeof(ClaimResultDetailTableCell));
                    tableView.Source = tableSource;

                    tableView.ReloadData();
                    var set = this.CreateBindingSet<ClaimResultsTableCell, ClaimResultGSC>();
                    set.Bind(tableSource).To(vm => vm.ClaimResultDetails);
                    set.Apply();

                    //					tableView.RowHeight = tableRowHeight;
                    //					tableView.Frame = new RectangleF (0, tableView.Frame.Y, tableView.Frame.Width, tableHeight);

                    this.SetNeedsLayout();
                }
            }
        }

        private bool _isPlanLimitationsVisible;

        public bool IsPlanLimitationVisible
        {
            get
            {
                return _isPlanLimitationsVisible;
            }
            set
            {
                _isPlanLimitationsVisible = value;

                if (value)
                {
                    limitationsTableView.ReloadData();
                    this.SetNeedsLayout();
                }
            }
        }

        private int _resultTypeID = 0;

        public int ResultTypeID
        {
            get
            {
                return _resultTypeID;
            }
            set
            {
                _resultTypeID = value;

                if (value != null)
                {
                    this.SetNeedsLayout();
                }
            }
        }

        public List<ClaimPlanLimitationGSC> PlanLimitations { get; set; }

        private string _awaitingPaymentMessage = "";

        public string AwaitingPaymentMessage
        {
            get
            {
                return _awaitingPaymentMessage;
            }
            set
            {
                _awaitingPaymentMessage = value;

                if (value != null)
                {
                    this.SetNeedsLayout();
                }
            }
        }

        private string _spendingAccountModelName = "";

        public string SpendingAccountModelName
        {
            get
            {
                return _spendingAccountModelName;
            }
            set
            {
                _spendingAccountModelName = value;

                if (value != null)
                {
                    this.SetNeedsLayout();
                }
            }
        }

        private string _awaitingPaymentNote = "";

        public string AwaitingPaymentNote
        {
            get
            {
                return _awaitingPaymentNote;
            }
            set
            {
                _awaitingPaymentNote = value;

                if (value != null)
                {
                    this.SetNeedsLayout();
                }
            }
        }
    }
}