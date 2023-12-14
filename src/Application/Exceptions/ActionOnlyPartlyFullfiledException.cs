
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Exceptions
{
    public class ActionOnlyPartlyFullfiledException : Exception
    {
        public ActionOnlyPartlyFullfiledException() : base("This action could only be fullfilled partly")
        {

        }

        public ActionOnlyPartlyFullfiledException(string msg) : base(msg)
        {

        }
    }
}
