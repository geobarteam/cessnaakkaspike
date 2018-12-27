namespace CessnaAkkaSpike.Application.Messages
{
    public class ApproveMessage
    {
        public ApproveMessage(string pipelineName, string installerName)
        {
            PipelineName = pipelineName;
            InstallerName = installerName;
        }

        public string PipelineName { get; }
        public string InstallerName { get; }
    }
}
