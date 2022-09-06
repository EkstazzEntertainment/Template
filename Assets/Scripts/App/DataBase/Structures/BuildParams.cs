namespace App.DataBase.Structures
{
    using System;

    [Serializable]
    public class BuildParams
    {
        public string id;
        public string name;
        public string platform;
        public bool development;
        public string checkoutPath;
    }
}
