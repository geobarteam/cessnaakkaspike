using System;
using System.Collections.Generic;
using System.Text;

namespace CessnaAkkaSpike.Messages
{
    public class StartPipelineMessage
    {
        public StartPipelineMessage(string pipelineName, string installerName)
        {
            PipelineName = pipelineName;
            InstallerName = installerName;
        }

        public string PipelineName { get; }
        public string InstallerName { get; }

    }
}
