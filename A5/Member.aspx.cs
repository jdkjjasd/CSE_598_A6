using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace A5
{
    public partial class Member : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in
            //if (!User.Identity.IsAuthenticated)
            if (Global.get_role() == 0)
            {
                Response.Redirect("~/Login.aspx"); // Redirect to the login page if not logged in
            }
            //else if (new LoginControl().GetUserRole(User.Identity.Name) == 2)
            else if (Global.get_role() == 2)
            {
                //lblUsername.Text = User.Identity.Name;
                lblUsername.Text = Global.get_user();
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack) 
            {
                LoginControl.Instance.Read_User_Xml(); 
                BindMemoGrid(); 
            }
        }
        protected void gvMemos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteMemo")
            {
                string username = lblUsername.Text;
                int rowIndex = Convert.ToInt32(e.CommandArgument); 
                int memoId = (int)gvMemos.DataKeys[rowIndex].Value; 

                if (LoginControl.Instance.accounts.ContainsKey(username))
                {
                    LoginControl.Instance.RemoveMemo(username, memoId); 
                    BindMemoGrid(); 
                }
            }
        }

        private void BindMemoGrid()
        {
            string username = Global.get_user();
            var memos = LoginControl.Instance.GetMemos(username); // 使用 GetMemos 方法获取用户的 Memos

            if (memos != null)
            {
                gvMemos.DataSource = memos
                    .OrderByDescending(kvp => DateTime.TryParseExact(kvp.Value.Timestamp, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var parsedDate)
                                                  ? parsedDate
                                                  : DateTime.MinValue) // 按时间戳降序排序
                    .Select(kvp => new
                    {
                        Key = kvp.Key, // Memo ID
                        Timestamp = DateTime.TryParseExact(kvp.Value.Timestamp, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var parsedDate)
                                    ? parsedDate.ToString("yyyy-MM-dd HH:mm:ss") // 格式化时间戳
                                    : kvp.Value.Timestamp,
                        Reminder = kvp.Value.Reminder
                    })
                    .ToList();

                gvMemos.DataBind();
            }
        }

        protected void btnAddMemo_Click(object sender, EventArgs e)
        {
            string username = lblUsername.Text;
            if (!string.IsNullOrEmpty(username) && LoginControl.Instance.accounts.ContainsKey(username))
            {
                string reminder = txtReminder.Text.Trim();

                if (!string.IsNullOrEmpty(reminder))
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    LoginControl.Instance.AddMemo(username, timestamp, reminder);
                    txtReminder.Text = string.Empty;
                    BindMemoGrid();
                }
                else
                {
                    lblMessage.Text = "Reminder cannot be empty.";
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}
