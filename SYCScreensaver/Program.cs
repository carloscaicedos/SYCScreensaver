using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SYCScreensaver
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArg = args[0].ToLower().Trim();
                string secondArg = null;

                if (firstArg.Length > 2)
                {
                    secondArg = firstArg.Substring(3).Trim();
                    firstArg = firstArg.Substring(0, 2);
                }
                else if (args.Length > 1)
                {
                    secondArg = args[1];
                }

                if (firstArg == "/c")
                {
                    ShowConfig();
                    Application.Run();
                }
                else if (firstArg == "/p")
                {
                    if (secondArg == null)
                    {
                        MessageBox.Show("No se puede previsualizar, se esperaba un segundo parámetro","SYC Screensaver",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewHandle = new IntPtr(long.Parse(secondArg));
                    Application.Run(new MainForm(previewHandle));
                    
                }
                else if (firstArg == "/s")
                {
                    if (Properties.Settings.Default.urldefault == "")
                    {
                        ShowConfig();
                        Application.Run();
                    }
                    else
                    {
                        ShowScreenSaver();
                        Application.Run();
                    }
                }
            }
            else
            {
                ShowConfig();
                Application.Run();
            }            
        }

        static void ShowScreenSaver(Boolean preview = false)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                MainForm screensaver = new MainForm(screen.Bounds);
                screensaver.Show();
            }
        }

        static void ShowConfig()
        {
            ConfigForm config = new ConfigForm();
            config.Show();
        }
    }
}
