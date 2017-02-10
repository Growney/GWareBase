using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API.Session
{
    public interface ISessonServer
    {
        ILoginResult Login(string username, string password);
        bool Logout(string key);

    }
}
