using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Entry couldn't be found")
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
