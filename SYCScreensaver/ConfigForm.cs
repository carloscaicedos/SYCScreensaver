using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace SYCScreensaver
{
    public partial class ConfigForm : Form
    {
        private AppSettings appSettings;

        public ConfigForm()
        {
            InitializeComponent();            
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Regex regexp = new Regex(@"^(https?|ftp|file)://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]$", RegexOptions.IgnoreCase);

            if (regexp.IsMatch(cmbURL.Text))
            {
                appSettings.UrlDefault = cmbURL.Text;
                appSettings.UrlHistory = new ArrayList();

                foreach (Object item in cmbURL.Items)
                {
                    appSettings.UrlHistory.Add(item.ToString());
                }

                if (!appSettings.UrlHistory.Contains(cmbURL.Text))
                {
                    appSettings.UrlHistory.Add(cmbURL.Text);
                }

                appSettings.SaveSettings();
                this.Close();
            }
            else
            {
                MessageBox.Show("La dirección ingresada no es válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbURL.Focus();
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            appSettings = new AppSettings();

            if (this.appSettings.LoadAppSettings())
            {
                foreach (Object urld in appSettings.UrlHistory)
                {
                    cmbURL.Items.Add(urld.ToString());
                }

                cmbURL.Text = appSettings.UrlDefault;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbURL_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete key
            if (e.KeyValue == 46 && cmbURL.Items.Count > 0)
            {
                cmbURL.DroppedDown = false;
                if (cmbURL.SelectedIndex >= 0)
                {
                    cmbURL.Items.Remove(cmbURL.SelectedItem);
                    cmbURL.DroppedDown = true;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Página Web|*.htm;*.html;*.asp;*.aspx;*.php;*.jsp;*.jspx";

            DialogResult result = file.ShowDialog();

            if (result == DialogResult.OK)
            {
                cmbURL.Text = "file:///" + file.FileName.Replace("\\","/");
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (cmbURL.Text == "")
            {
                MessageBox.Show("Por favor introduzca una URL", "SYC Screensaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MainForm mainForm = new MainForm(Screen.PrimaryScreen.Bounds, true);
            mainForm.SetURL(cmbURL.Text);
            mainForm.Show();
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
