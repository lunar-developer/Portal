using System;
using System.Drawing;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Modules.Controls
{
    public sealed class Grid : DnnGrid
    {
        public string LocalResourceFile { get; set; }


        public Grid()
        {
            RenderMode = RenderMode.Lightweight;
            AllowFilteringByColumn = false;
            AllowMultiRowSelection = true;
            AllowPaging = true;
            AllowSorting = true;
            CssClass = "dnnGrid";
            EnableViewState = true;
            GroupingEnabled = false;
            GroupingSettings.CaseSensitive = false;
            HeaderStyle.VerticalAlign = VerticalAlign.Middle;
            HorizontalAlign = HorizontalAlign.Center;
            SortingSettings.EnableSkinSortStyles = true;

            #region Paging Override
            PagerStyle.Mode = GridPagerMode.Advanced;
            PagerStyle.Position = GridPagerPosition.Bottom;
            PagerStyle.PageSizeControlType = PagerDropDownControlType.RadComboBox;

            PagerStyle.LastPageText = "Trang cuối";
            PagerStyle.FirstPageText = "Trang đầu";

            PagerStyle.NextPageText = "Trang kế tiếp";
            PagerStyle.NextPageToolTip = "Trang kế tiếp";
            PagerStyle.NextPagesToolTip = "Hiển thị trang ẩn";

            PagerStyle.PrevPageText = "Trang trước";
            PagerStyle.PrevPageToolTip = "Trang trước";
            PagerStyle.PrevPagesToolTip = "Hiển thị trang ẩn";
            PagerStyle.PageSizeLabelText = "Dòng";

            PagerStyle.GoToPageButtonToolTip = string.Empty;
            PagerStyle.GoToPageTextBoxToolTip = string.Empty;

            /*
             * No matter which pager you choose, the meaning of the different parameters is as follows:

                {0} is used to display current page number.

                {1} is total number of pages.

                {2} is the number of the first item (record) in the current page.

                {3} is the number of the last item (record) in the current page.

                {4} indicates where the set of controls for the corresponding pager type (positioned on the left) appear.

                {5} is the total number of items (records) in the data source.
            */
            PagerStyle.PagerTextFormat = "{4} Trang {0} trên {1} trang, Tổng số dòng {5}";
            PagerStyle.PageSizeLabelText = "Số dòng: ";
            ClientSettings.ClientMessages.PagerTooltipFormatString = " Trang {0} trên {1} trang";
            PagerStyle.ChangePageSizeButtonToolTip = "Thay đổi số dòng trên trang";
            #endregion

            #region Sort Setting
            SortingSettings.SortToolTip = "Sắp xếp (Sort)";
            SortingSettings.SortedDescToolTip = "Thứ tự giảm dần";
            SortingSettings.SortedAscToolTip = "Thứ tự tăng dần";
            #endregion

            #region ClientSetting
            ClientSettings.AllowColumnsReorder = true;
            ClientSettings.AllowDragToGroup = true;
            ClientSettings.AllowRowsDragDrop = true;
            ClientSettings.ReorderColumnsOnClient = true;
            ClientSettings.Resizing.AllowColumnResize = true;
            ClientSettings.Resizing.EnableRealTimeResize = true;
            ClientSettings.Selecting.AllowRowSelect = true;
            ClientSettings.Selecting.EnableDragToSelectRows = true;
            ClientSettings.Scrolling.EnableVirtualScrollPaging = false;
            #endregion
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            MasterTableView.FilterExpression = string.Empty;
            MasterTableView.SortExpressions.Clear();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AutoGenerateColumns)
            {
                return;
            }
            foreach (GridColumn column in Columns)
            {
                SetColumnFilterSetting(column);
            }
        }

        protected override void OnColumnCreated(GridColumnCreatedEventArgs e)
        {
            base.OnColumnCreated(e);
            if (AutoGenerateColumns == false)
            {
                return;
            }
            SetColumnFilterSetting(e.Column);
        }

        protected override void OnItemDataBound(GridItemEventArgs e)
        {
            base.OnItemDataBound(e);
            GridPagerItem gridPagerItem = e.Item as GridPagerItem;
            if (gridPagerItem == null)
            {
                return;
            }

            System.Web.UI.WebControls.Label lblPageSize =
                (System.Web.UI.WebControls.Label) gridPagerItem.FindControl("ChangePageSizeLabel");
            if (lblPageSize != null)
            {
                lblPageSize.Text = "Số dòng:";
            }


            RadComboBox rdComboBox = (RadComboBox)gridPagerItem.FindControl("PageSizeComboBox");
            if (rdComboBox != null)
            {
                rdComboBox.Text = "Số dòng:";
            }


            System.Web.UI.WebControls.Label lblGotoPage =
                (System.Web.UI.WebControls.Label) gridPagerItem.FindControl("GoToPageLabel");
            if (lblGotoPage != null)
            {
                lblGotoPage.Text = "Trang: ";
            }


            System.Web.UI.WebControls.Label lblPageOf =
                (System.Web.UI.WebControls.Label) gridPagerItem.FindControl("PageOfLabel");
            if (lblPageOf != null)
            {
                GridPagerItem item = gridPagerItem;
                lblPageOf.Text = "/ " + item.Paging.PageCount;
            }


            Button lnkGotoPage = (Button) gridPagerItem.FindControl("GoToPageLinkButton");
            if (lnkGotoPage != null)
            {
                lnkGotoPage.Text = "Chuyển Trang";
            }


            Button lnkChangePageSize = (Button) gridPagerItem.FindControl("ChangePageSizeLinkButton");
            if (lnkChangePageSize != null)
            {
                lnkChangePageSize.Text = "Áp dụng";
            }


            System.Web.UI.WebControls.Label lblChangePage = 
                (System.Web.UI.WebControls.Label) gridPagerItem.FindControl("ChangePageSizeLabel");
            if (lblChangePage != null)
            {
                lblChangePage.Text = "Số dòng:";
            }


            RadSlider rdSliderPage = (RadSlider)gridPagerItem.FindControl("GridSliderPager");
            if (rdSliderPage != null)
            {
                rdSliderPage.DecreaseText = "Trở về trang trước";
            }


            RadNumericTextBox goToPageText = (RadNumericTextBox)gridPagerItem.FindControl("GoToPageTextBox");
            goToPageText.Width = Unit.Pixel(70);
            goToPageText.ShowSpinButtons = true;


            RadNumericTextBox changePageSizeTextBox = (RadNumericTextBox)gridPagerItem.FindControl("ChangePageSizeTextBox");
            changePageSizeTextBox.IncrementSettings.Step = 10;
            changePageSizeTextBox.Width = Unit.Pixel(70);
            changePageSizeTextBox.ShowSpinButtons = true;
        }

        private void SetColumnFilterSetting(GridColumn column)
        {
            string header = Localization.GetString(column.HeaderText + ".Header", LocalResourceFile);
            if (string.IsNullOrWhiteSpace(header) == false)
            {
                column.HeaderText = header;
            }
            if (Math.Abs(column.HeaderStyle.Width.Value) <= 0)
            {
                column.HeaderStyle.Width = 200;
            }
            column.SortedBackColor = Color.Transparent;
            column.ShowFilterIcon = false;
            column.ShowSortIcon = true;
            column.FilterDelay = 1500;

            switch (column.ColumnType)
            {
                case "GridNumericColumn":
                    column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                    break;

                case "GridCheckBoxColumn":
                    column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                    column.AutoPostBackOnFilter = true;
                    break;

                case "GridDateTimeColumn":
                    GridDateTimeColumn columnDateTime = (GridDateTimeColumn)column;
                    columnDateTime.EnableRangeFiltering = true;
                    columnDateTime.PickerType = GridDateTimeColumnPickerType.DatePicker;
                    columnDateTime.CurrentFilterFunction = GridKnownFunction.Between;
                    columnDateTime.ShowFilterIcon = true;
                    break;

                default:
                    column.CurrentFilterFunction = GridKnownFunction.Contains;
                    break;
            }
        }
    }
}