namespace CessnaAkkaSpike.Application.Messages
{
    public class PipelineMessage
    {
        public PipelineMessage(string pipelineName, string installerName)
        {
            PipelineName = pipelineName;
            InstallerName = installerName;
        }

        public string PipelineName { get; }
        public string InstallerName { get; }

    }
}
