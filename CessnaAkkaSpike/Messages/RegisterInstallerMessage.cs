using System;
using System.Collections.Generic;
using System.Text;

namespace CessnaAkkaSpike.Messages
{
    public class RegisterInstallerMessage
    {
        public RegisterInstallerMessage(string installerFullName)
        {
            InstallerFullName = installerFullName;
        }

        public string InstallerFullName { get; }
    }
}
