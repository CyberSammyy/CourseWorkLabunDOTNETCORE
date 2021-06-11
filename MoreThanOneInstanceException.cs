using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_DOTNETCORE
{
    class MoreThanOneInstanceDALException : Exception
    {
        public MoreThanOneInstanceDALException(string message) : base(message)
        {

        }
    }
}
