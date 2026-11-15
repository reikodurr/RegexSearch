using System;
using System.Collections.Generic;
using System.Text;

namespace RegexSearch.Core.Models
{
    internal enum State
    {
        Start,
        Digits,
        Hyphen,
        Digits1,
        Hyphen1,
        Digits2,
        Space,
        Digits3,
        Accept,
        Error
    }
}
