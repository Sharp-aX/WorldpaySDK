using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAx.Worldpay.API.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class WorldpaySensitivePropertyAttribute : Attribute
    {
        public WorldpaySensitivePropertyAttribute()
        {
            Mask = "*****";
        }

        public string Mask { get; private set; }
    }
}
