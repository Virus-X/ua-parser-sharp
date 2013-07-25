using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAParserSharp
{
    public abstract class UserAgentItem
    {
      public int ID { get; set; }

      public abstract void Intialize(string s);
      public abstract State GetState();
      public abstract int GetNumberItems();

    }
}
