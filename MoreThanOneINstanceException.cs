using System;
using System.Collections.Generic;
using System.Text;

namespace BLL_DOTNETCORE
{
    class MoreThanOneINstanceException : Exception
    {
        public MoreThanOneINstanceException(string message) : base(message)
        {

        }
    }
}
