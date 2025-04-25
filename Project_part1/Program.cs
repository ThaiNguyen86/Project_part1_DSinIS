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

            bool exitApplication = false;

            while (!exitApplication)
            {
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Set credentials with hostname and port
                        Utils.ConnectionHelper.SetCredentials(
                            loginForm.Username,
                            loginForm.Password,
                            loginForm.IsSysDba,
                            loginForm.ConnectionType,
                            loginForm.ServiceOrSidValue,
                            loginForm.Hostname, // New
                            loginForm.Port      // New
                        );

                        // Show MainForm non-modally
                        using (var mainForm = new MainForm())
                        {
                            mainForm.Show();
                            Application.DoEvents();
                            while (mainForm.Visible)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(10);
                            }
                        }
                    }
                    else
                    {
                        // User canceled login or closed LoginForm
                        exitApplication = true;
                    }
                }
            }
        }
    }
}