using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Skins.Controls;
using Modules.Application.Database;
using Modules.Application.DataTransfer;
using Modules.Controls;
using Modules.VSaleKit.Business;
using Modules.VSaleKit.Database;
using Modules.VSaleKit.DataTransfer;
using Modules.VSaleKit.Global;
using Telerik.Web.UI;
using Website.Library.Global;

namespace DesktopModules.Modules.VSaleKit
{
    public partial class BatchUpload : VSaleKitModuleBase
    {
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            BindData();
        }

        private void BindData()
        {
            ddlApplicationType.EmptyMessage = GetResource("lblApplicationType.Help");
            ddlApplicationType.ClearSelection();
            ddlPolicy.EmptyMessage = GetResource("lblPolicy.Help");
            ddlPolicy.ClearSelection();

            BindApplicationTypeData(ddlApplicationType);
            LoadPolicyData(ddlPolicy);

            divResult.Visible = gridData.Visible = gridException.Visible = false;
            divForm.Visible = false;
        }

        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                // Reset state
                divResult.Visible = true;
                gridData.Visible = gridException.Visible = false;
                divForm.Visible = false;
                lblTotalRows.Text = lblTotalExceptions.Text = lblMessage.Text = string.Empty;

                // Upload file csv => list object
                List<BatchExceptionData> listException = new List<BatchExceptionData>();
                List<BatchData> listData = FunctionBase.ImportExcel<BatchData>(fupFile.FileContent);

                // Validate upload data
                for (int i = 0; i < listData.Count; i++)
                {
                    string lineNumber = (i + 2).ToString();
                    BatchData item = listData[i];

                    // Check require fields
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { BatchDataTable.IdentityTypeCode, item.IdentityTypeCode },
                        { BatchDataTable.CustomerID, item.CustomerID },
                        { BatchDataTable.CustomerName, item.CustomerName },
                        { BatchDataTable.UserName, item.UserName },
                        { BatchDataTable.Priority, item.Priority }
                    };
                    if (IsRequireFieldEmpty(lineNumber, dictionary, listException))
                    {
                        continue;
                    }

                    // Check numeric field
                    dictionary = new Dictionary<string, string>
                    {
                        { BatchDataTable.CreditLimit, item.CreditLimit }
                    };
                    if (IsInvalidNumberField(lineNumber, dictionary, listException, 0, 900000000))
                    {
                        continue;
                    }
                    dictionary = new Dictionary<string, string>
                    {
                        { BatchDataTable.Priority, item.Priority }
                    };
                    if (IsInvalidNumberField(lineNumber, dictionary, listException, 0, 1))
                    {
                        continue;
                    }

                    // Check option field
                    if (CacheBase.Find<IdentityTypeData>(IdentityTypeTable.IdentityTypeCode, item.IdentityTypeCode) == null)
                    {
                        listException.Add(new BatchExceptionData
                        {
                            LineNumber = lineNumber,
                            FieldName = BatchDataTable.IdentityTypeCode,
                            Error = $"Không tìm thấy giá trị {item.IdentityTypeCode}."
                        });
                        continue;
                    }

                    // Check user role
                    IsInvalidUser(lineNumber, item.UserName, listException);
                }

                // Show result
                lblTotalRows.Text = listData.Count.ToString();
                lblTotalExceptions.Text = listException.Count.ToString();
                if (listException.Count == 0)
                {
                    lblMessage.Text =
                        @"Vui lòng chọn <b>Loại hồ sơ</b>, <b>Chính sách</b> và click <b>XÁC NHẬN</b> để tiếp tục.";
                    ViewState[BatchDataTable.TableName] = listData;
                    BindGrid(gridData, listData);
                    divForm.Visible = true;
                }
                else
                {
                    lblMessage.Text = @"Vui lòng điều chỉnh dữ liệu và thực hiện <b>UPLOAD</b> lại.";
                    ViewState[BatchDataTable.TableName] = listException;
                    BindGrid(gridException, listException);
                }
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                ShowMessage("Có lỗi khi xử lý", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void Process(object sender, EventArgs e)
        {
            try
            {
                List<BatchData> list = ViewState[BatchDataTable.TableName] as List<BatchData>;
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { BatchDataTable.ApplicationTypeID, ddlApplicationType.SelectedValue },
                    { BatchDataTable.PolicyID, ddlPolicy.SelectedValue },
                    { BatchDataTable.ImportUserID, UserInfo.UserID.ToString() }
                };

                DataTable dataTable = BatchDataBusiness.Insert(list, dictionary);
                ViewState[BatchDataTable.TableName] = dataTable;
                BindGrid(gridData, dataTable);

                // Update state
                lblTotalExceptions.Text =
                    dataTable.AsEnumerable()
                        .Count(item => item[BatchDataTable.ProcessStatus].ToString() == "-1")
                        .ToString();
                lblMessage.Visible = false;
                divForm.Visible = false;
                if (dataTable.Rows.Count > 0)
                {
                    ShowMessage("Quá trình xử lý đã hoàn tất. Vui lòng kiểm tra thông tin chi tiết.", ModuleMessage.ModuleMessageType.GreenSuccess);
                }
            }
            catch (Exception exception)
            {
                FunctionBase.LogError(exception);
                ShowMessage("Có lỗi khi xử lý", ModuleMessage.ModuleMessageType.RedError);
            }
        }

        protected void LoadPolicyDetail(object sender, EventArgs e)
        {
            PolicyData policy = CacheBase.Receive<PolicyData>(ddlPolicy.SelectedValue);
            string title = $"Chính sách: {policy.PolicyCode} - {policy.Name}";
            string message = policy.Remark.Replace(Environment.NewLine, string.Empty);
            ShowAlertDialog(message, title);
        }

        private static bool IsRequireFieldEmpty(string lineNumber, Dictionary<string, string> dictionary,
            ICollection<BatchExceptionData> listException)
        {
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                if (string.IsNullOrWhiteSpace(pair.Value) == false)
                {
                    continue;
                }

                listException.Add(new BatchExceptionData
                {
                    LineNumber = lineNumber,
                    FieldName = pair.Key,
                    Error = "Dữ liệu rỗng."
                });
                return true;
            }
            return false;
        }


        private static bool IsInvalidNumberField(string lineNumber, Dictionary<string, string> dictionary,
            ICollection<BatchExceptionData> listException, decimal min, decimal max)
        {
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                decimal value;
                if (decimal.TryParse(pair.Value, out value) && value >= min && value <= max)
                {
                    continue;
                }

                listException.Add(new BatchExceptionData
                {
                    LineNumber = lineNumber,
                    FieldName = pair.Key,
                    Error = $@"Dữ liệu không hợp lệ.
                        <br>Bắt buộc phải >= {FunctionBase.FormatDecimal(min)} và <= {FunctionBase.FormatDecimal(max)}."
                });
                return true;
            }
            return false;
        }

        private void IsInvalidUser(string lineNumber, string userName, ICollection<BatchExceptionData> listException)
        {
            UserInfo user = UserController.GetUserByName(PortalId, userName);
            if (user == null || user.Membership.Approved == false)
            {
                listException.Add(new BatchExceptionData
                {
                    LineNumber = lineNumber,
                    FieldName = BatchDataTable.UserName,
                    Error = $"User không tồn tại hoặc đã bị khoá. ({userName})"
                });
                return;
            }

            if (IsRoleInsert(user.UserID))
            {
                return;
            }

            listException.Add(new BatchExceptionData
            {
                LineNumber = lineNumber,
                FieldName = BatchDataTable.UserName,
                Error = $"User không có quyền thêm mới hồ sơ. ({userName})"
            });
        }


        private void BindGrid(Grid grid, object dataSource)
        {
            grid.Visible = true;
            grid.DataSource = dataSource;
            grid.LocalResourceFile = LocalResourceFile;
            grid.DataBind();
        }

        protected string FormatIdentityTypeCode(string value)
        {
            return CacheBase.Find<IdentityTypeData>(IdentityTypeTable.IdentityTypeCode, value)?.Name;
        }

        protected void OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null)
            {
                return;
            }
            grid.DataSource = ViewState[BatchDataTable.TableName];
        }
    }
}