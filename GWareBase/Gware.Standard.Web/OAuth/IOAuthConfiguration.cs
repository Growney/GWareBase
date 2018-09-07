using System;
using System.Collections.Generic;
using System.Text;

namespace Gware.Standard.Web.OAuth
{
    public interface IOAuthConfiguration
    {
        string AuthUrl { get; }
        string CallBackUrl { get; }
        string ClientID { get; }
        string Secret { get; }
    }
}
