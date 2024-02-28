using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldPayDemoSDK
{
    public abstract class DatabaseClass
    {

        public void SetPropertyValue<T>(string v, ref T Property, T value)
        {
            Property = value;
        }

        public void Save()
        {
            //STORE TO DATABASE

            OnSaved();
        }

        public virtual void OnSaved()
        {

        }
    }
}
