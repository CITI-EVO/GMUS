using System;

namespace CITI.EVO.Rpc.Common
{
    public class AuthorizationEventArg : EventArgs
    {
        public AuthorizationEventArg(String userName, String password, String encryption)
        {
            UserName = userName;
            Password = password;
            Encryption = encryption;

            CanContinue = true;
        }

        public String UserName { get; private set; }
        public String Password { get; private set; }
        public String Encryption { get; private set; }

        public bool CanContinue { get; set; }
    }
}
