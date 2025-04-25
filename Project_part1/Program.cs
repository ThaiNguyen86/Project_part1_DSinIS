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
                        // Set credentials for the new login
                        Utils.ConnectionHelper.SetCredentials(
                            loginForm.Username,
                            loginForm.Password,
                            loginForm.IsSysDba,
                            loginForm.ConnectionType,
                            loginForm.ServiceOrSidValue
                        );

                        // Show MainForm non-modally
                        using (var mainForm = new MainForm())
                        {
                            mainForm.Show();
                            // Keep the application running until MainForm is closed
                            Application.DoEvents(); // Process events to show MainForm
                            while (mainForm.Visible)
                            {
                                Application.DoEvents(); // Keep the UI responsive
                                System.Threading.Thread.Sleep(10); // Prevent CPU overuse
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