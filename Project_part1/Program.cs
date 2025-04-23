using OracleUserManagementApp.Forms;
using System;
using System.Windows.Forms;

namespace OracleUserManagementApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Utils.ConnectionHelper.SetCredentials(
                        loginForm.Username,
                        loginForm.Password,
                        loginForm.IsSysDba,
                        loginForm.ConnectionType,
                        loginForm.ServiceOrSidValue
                    );
                    Application.Run(new MainForm());
                }
                else
                {
                    return;
                }
            }
        }
    }
}