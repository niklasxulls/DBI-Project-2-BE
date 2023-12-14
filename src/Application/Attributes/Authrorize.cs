using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Attributes
{


    [AttributeUsage(AttributeTargets.Class)]
    public class Authorize : Attribute
    {
        public readonly bool AllowUnverified;
        public Authorize(bool allowUnverified = false)
        {
            AllowUnverified = allowUnverified;
        }
    }
}
