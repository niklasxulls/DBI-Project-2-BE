using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace stackblob.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool Valid(this DateTime d)
        {
            return d != default(DateTime) && (bool) (d >= SqlDateTime.MinValue) && (bool)(d <= SqlDateTime.MaxValue);
        }
    }
}
