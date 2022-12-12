using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
   public interface IRule
   {
        string Rename(string name);
        IRule Parse(string data);
        string name { get; }
   }
}
