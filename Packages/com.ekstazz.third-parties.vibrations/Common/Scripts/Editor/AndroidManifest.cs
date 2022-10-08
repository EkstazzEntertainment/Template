namespace MoreMountains.NiceVibrations
{
    using System.Text;
    using System.Xml;

    internal class AndroidXmlDocument : XmlDocument
    {
        protected const string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
        private readonly string path;

        public AndroidXmlDocument(string path)
        {
            this.path = path;
            using (var reader = new XmlTextReader(this.path))
            {
                reader.Read();
                Load(reader);
            }
            var namespaceManager = new XmlNamespaceManager(NameTable);
            namespaceManager.AddNamespace("android", AndroidXmlNamespace);
        }

        public void Save()
        {
            SaveAt(path);
        }

        private void SaveAt(string path)
        {
            using var writer = new XmlTextWriter(path, new UTF8Encoding(false));
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
    }


    internal class AndroidManifest : AndroidXmlDocument
    {
        private readonly XmlElement applicationElement;

        public AndroidManifest(string path) : base(path)
        {
            applicationElement = SelectSingleNode("/manifest/application") as XmlElement;
        }

        internal void SetVibratePermission()
        {
            var manifest = SelectSingleNode("/manifest");
            var child = CreateElement("uses-permission");
            manifest.AppendChild(child);
            var newAttribute = CreateAndroidAttribute("name", "android.permission.VIBRATE");
            child.Attributes.Append(newAttribute);
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            var attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }
    }
}
