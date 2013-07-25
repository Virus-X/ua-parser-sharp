using System;

using UAParserSharp;

namespace UserAgentStringLibrary.Tables
{
    public abstract class UserAgentItem
    {
      public int ID { get; set; }

      public abstract void Intialize(string s);
      public abstract ParserState GetState();
      public abstract int GetNumberItems();

    }
}
