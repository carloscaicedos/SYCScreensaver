using System.Windows.Forms;
using System.Drawing;
using System;
using System.Runtime.InteropServices;

namespace SYCScreensaver
{
    public partial class MainForm : Form
    {
        private Point mouseLocation;
        private Boolean showpreview = false;
        private string previewURL = null;
        private AppSettings appSettings;

        public MainForm(Rectangle Bounds, Boolean ShowPreview = false)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            this.showpreview = ShowPreview;            
        }

        public MainForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Establecer la ventana de previsualización como la ventana padre
            SetParent(this.Handle, PreviewWndHandle);

            // Hacer que la ventana sea hijo, así que al cerrar el padre se cierre la ventana
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //Ubicar la ventana dentro del rectangulo
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            this.showpreview = true;
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            appSettings = new AppSettings();

            Cursor.Hide();
            TopMost = true;

            if (this.previewURL != null && this.previewURL.Length > 0)
            {
                browser.Url = new Uri(this.previewURL);
            }
            else
            {
                if (this.appSettings.LoadAppSettings())
                {
                    browser.Url = new Uri(appSettings.UrlDefault);
                }
                else
                {
                    browser.Url = new Uri(Properties.Settings.Default.urldefault);
                }
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            browser.Document.MouseMove += new HtmlElementEventHandler(this.browser_MouseMove);
            browser.Document.MouseDown += new HtmlElementEventHandler(this.browser_MouseDown);
            browser.PreviewKeyDown += new PreviewKeyDownEventHandler(this.browser_KeyDown);
        }

        private void browser_MouseMove(object sender, HtmlElementEventArgs e)
        {
            if (!mouseLocation.IsEmpty)
            {
                if (Math.Abs(mouseLocation.X - e.MousePosition.X) > 10 || Math.Abs(mouseLocation.Y - e.MousePosition.Y) > 10)
                {
                    if (this.showpreview)
                    {
                        this.Close();
                    }
                    else
                    {                        
                        Application.Exit();
                    }
                }
            }
            mouseLocation = e.MousePosition;
        }

        private void browser_MouseDown(object sender, HtmlElementEventArgs e)
        {
            if (this.showpreview)
            {
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        private void browser_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.showpreview)
            {
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Show();
        }

        public void SetURL(string url)
        {
            this.previewURL = url;
        }
    }
}
