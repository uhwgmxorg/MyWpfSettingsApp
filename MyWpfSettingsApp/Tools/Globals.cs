using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MyWpfSettingsApp.Tools
{
    public class Globals: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        readonly string FILENAME = "GlobalSerialization.xml";

        public static string Revison { get; } = " Revison 0";

        #region INotify Changed Properties  
        private string myPrivateStringSettings;
        [XmlElement("MyPrivateStringSettings")]
        public string MyPrivateStringSettings
        {
            get { return myPrivateStringSettings; }
            set { SetField(ref myPrivateStringSettings, value, nameof(MyPrivateStringSettings)); }
        }
        #endregion

        public void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Globals));

                var XmlWriter = new StreamWriter(FILENAME);
                serializer.Serialize(XmlWriter, this);
                XmlWriter.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Load()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Globals));

                using (StreamReader rd = new StreamReader(FILENAME))
                {
                    var XmlImport = serializer.Deserialize(rd) as Globals;
                    MyPrivateStringSettings = XmlImport.MyPrivateStringSettings;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
    }
}
