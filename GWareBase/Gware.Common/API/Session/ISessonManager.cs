using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.API.Session
{
    public interface ISessonManager
    {
        eLoginResult Login(string username, string password);
        bool LogOut(string username);

        eUserCreationResult CreateUser(string username, string password);
        bool DeleteUser(string username);
        bool ChangeUserPassword(string username, string password);
        bool AuthenticateUser(string username, string password);

        bool StoreKey(string username, string key);
        bool RemoveKey(string username);
        string GetKeyUsername(string username);
        eKeyStatus GetKeyStatus(string key);
        bool IsValidKey(string key);
    }
}
