using Gware.Common.Networking.Connection;
using Gware.Common.Networking.FramedConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Networking.AuthenticatedConnection
{

    public class AuthenticationServer : FramedServer
    {
        private static Random m_rand = new Random();

        public AuthenticationServer(int port,ClientServerConnectionType type) :base(port,type)
        {
            
        }

        protected override void OnDataCompleted(System.Net.IPEndPoint sender, byte[] result)
        {
            AuthenticatedCommand rxCmd = new AuthenticatedCommand();
            rxCmd.FromBytes(result);

            switch ((eAuthenticatedCommand)rxCmd.CommandID)
            {
                case eAuthenticatedCommand.AuthenticationRequest:
                    HandleIncomingAuthenticationRequest(sender, rxCmd);
                    break;
                case eAuthenticatedCommand.DataTransfer:
                    HandleIncomingDataCommand(sender, rxCmd);
                    break;
                default:
                    Send(sender, BuildCommand(eAuthenticatedCommand.None, eErrors.UnknownCommand));
                    break;
            }
        }

        protected virtual eErrors OnAuthenticationResult(string username, string password)
        {
            return eErrors.None ;
        }
        private AuthenticatedCommand BuildCommand(eAuthenticatedCommand command, eErrors error)
        {
            AuthenticatedCommand retVal = new AuthenticatedCommand(command,error);            
            return retVal;
        }
        private void Send(System.Net.IPEndPoint to,AuthenticatedCommand cmd)
        {
            base.Send(to, cmd.ToBytes());
        }
        protected override void Send(System.Net.IPEndPoint to, byte[] bytes)
        {
            AuthenticatedCommand cmd = BuildCommand(eAuthenticatedCommand.None, eErrors.None);
            Send(to, cmd);
        }
        private void HandleIncomingDataCommand(System.Net.IPEndPoint sender, AuthenticatedCommand cmd)
        {
            if ((eAuthenticatedCommand)cmd.CommandID == eAuthenticatedCommand.DataTransfer)
            {
                eErrors authenticationResponse = ConfirmSessionToken(cmd.AuthenticationToken, sender);
                if (authenticationResponse == eErrors.None)
                {
                    OnSessionDataRx(sender, cmd.Data);
                }
                else if (authenticationResponse == eErrors.ValidExpiredToken)
                {
                    string newToken = GenerateSessionToken();

                    RefreshSessionToken(cmd.AuthenticationToken,newToken, sender);

                    AuthenticatedCommand retCmd = new AuthenticatedCommand(eAuthenticatedCommand.TokenRefresh,eErrors.None);
                    retCmd.AuthenticationToken = newToken;
                    Send(sender, retCmd);

                    OnSessionDataRx(sender, cmd.Data);

                }
                else
                {
                    Send(sender, new AuthenticatedCommand(eAuthenticatedCommand.DataTransferResponse, authenticationResponse));
                }
            }

        }
        private void HandleIncomingAuthenticationRequest(System.Net.IPEndPoint sender, AuthenticatedCommand cmd)
        {
            if ((eAuthenticatedCommand)cmd.CommandID == eAuthenticatedCommand.AuthenticationRequest)
            {
                eErrors authenticationResponse = OnAuthenticationResult(cmd.Username, cmd.Password);
                AuthenticatedCommand retCmd = new AuthenticatedCommand(eAuthenticatedCommand.AuthenticationResult, authenticationResponse);

                if (authenticationResponse == eErrors.None)
                {
                    string newToken = GenerateSessionToken();
                    StoreSessionToken(cmd.Username, cmd.Password, newToken, sender);
                    retCmd.AuthenticationToken = newToken;
                }

                Send(sender, retCmd);
            }
        }
        protected virtual void OnSessionDataRx(System.Net.IPEndPoint sender, byte[] result)
        {

        }

        #region ----- Session Management -----

        protected virtual string GenerateSessionToken()
        {
            byte[] tokenBytes = new byte[128];
            m_rand.NextBytes(tokenBytes);

            return Application.ApplicationBase.c_ApplicationEncoding.GetString(tokenBytes);
        }
        protected virtual void RefreshSessionToken(string oldToken, string newToken, System.Net.IPEndPoint ep)
        {

        }
        protected virtual void StoreSessionToken(string username,string password,string token, System.Net.IPEndPoint ep)
        {

        }

        protected virtual eErrors ConfirmSessionToken(string token, System.Net.IPEndPoint ep)
        {
            return eErrors.None;
        }

        protected virtual void RemoveSessionToken(System.Net.IPEndPoint ep)
        {

        }

        

        #endregion ----- Session Management -----
    }
}
