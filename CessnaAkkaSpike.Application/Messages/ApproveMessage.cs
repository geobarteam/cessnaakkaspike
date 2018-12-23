namespace CessnaAkkaSpike.Application.Messages
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
