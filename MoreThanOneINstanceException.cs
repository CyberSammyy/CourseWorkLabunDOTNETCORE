using System;
using System.Collections.Generic;
using System.Text;

namespace CourseWorkLabunDOTNETCORE
{
    class MoreThanOneINstanceException : Exception
    {
        public MoreThanOneINstanceException(string message) : base(message)
        {

        }
    }
}
