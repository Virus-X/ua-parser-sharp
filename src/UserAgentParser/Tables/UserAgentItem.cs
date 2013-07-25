using System;
using System.Linq;

namespace UserAgentStringLibrary.Tables
{
    public abstract class UserAgentItem
    {
        public int ID { get; set; }

        public abstract int GetNumberItems();

        protected abstract void LoadFields(string[] fields);

        public virtual void Intialize(string s)
        {
            string[] lines = s.Trim().Replace("\r", string.Empty).Replace("\"", string.Empty).Split('\n');

            string sID = lines[0].Split('=')[0].Trim().Replace("[]", string.Empty);
            int id;
            if (!int.TryParse(sID, out id))
            {
                return;
            }

            ID = id;
            var fields = (from l in lines
                          select l.Split(new[] { "[] =" }, StringSplitOptions.None)[1].Trim()).ToArray();

            LoadFields(fields);
        }
    }
}
