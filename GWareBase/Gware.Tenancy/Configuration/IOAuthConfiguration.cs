using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Tenancy.Configuration
{
    public interface IOAuthConfiguration
    {
        string AuthUrl { get; }
        string CallBackUrl { get; }
        string ClientID { get; }
        string Secret { get; }
    }
}
