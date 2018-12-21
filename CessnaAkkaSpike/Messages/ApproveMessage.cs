using System;
using System.Collections.Generic;
using System.Text;

namespace CessnaAkkaSpike.Messages
{
    public class ApproveMessage
    {
        public ApproveMessage(string installerName)
        {
            InstallerName = installerName;
        }

        public string InstallerName { get; }
    }
}
