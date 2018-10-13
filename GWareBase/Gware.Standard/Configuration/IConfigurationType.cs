using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Configuration
{
    public interface IConfigurationType
    {
        byte[] GetBytes();
        void SetBytes(byte[] value);
    }
}
