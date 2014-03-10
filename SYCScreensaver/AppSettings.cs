using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SYCScreensaver
{
    public class AppSettings
    {
        private bool settingsChanged;
        private string m_urldefault;
        private ArrayList m_urlhistory;

        public string UrlDefault
        {
            get { return m_urldefault; }

            set
            {
                if (m_urldefault != value)
                {
                    m_urldefault = value;
                    settingsChanged = true;
                }
            }
        }

        public ArrayList UrlHistory
        {
            get { return m_urlhistory; }

            set
            {
                if (m_urlhistory != value)
                {
                    m_urlhistory = value;
                    settingsChanged = true;
                }
            }
        }

        public bool SaveSettings()
        {
            if (this.settingsChanged)
            {
                StreamWriter writer = null;
                XmlSerializer serializer = null;
                try
                {
                    serializer = new XmlSerializer(typeof(AppSettings));
                    writer = new StreamWriter(Application.LocalUserAppDataPath + @"\SYCScreensaver.config", false);
                    serializer.Serialize(writer, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            
            return settingsChanged;
        }

        public bool LoadAppSettings()
        {
            XmlSerializer serializer = null;
            FileStream stream = null;
            bool fileExists = false;

            try
            {
                serializer = new XmlSerializer(typeof(AppSettings));
                FileInfo fi = new FileInfo(Application.LocalUserAppDataPath + @"\SYCScreensaver.config");

                if (fi.Exists)
                {
                    stream = fi.OpenRead();

                    AppSettings appSettings = (AppSettings)serializer.Deserialize(stream);

                    this.m_urldefault = appSettings.UrlDefault;
                    this.m_urlhistory = appSettings.UrlHistory;

                    fileExists = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return fileExists;
        }
    }
}
