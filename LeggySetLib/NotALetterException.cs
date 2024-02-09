using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLeggySetLib
{
    public class NotALetterException : Exception
    {
        public char OffendingCharacter
        {
            get; private set;
        }
        public NotALetterException(char offendingChar, string msg = "") : base(msg)
        {
            OffendingCharacter = offendingChar;
        }
    }
}
