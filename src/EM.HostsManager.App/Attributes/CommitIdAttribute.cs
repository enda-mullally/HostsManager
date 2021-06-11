namespace EM.HostsManager.App.Attributes
{
    public class CommitIdAttribute : System.Attribute
    {
        public string CommitId { get; set; }

        public CommitIdAttribute(string commitId)
        {
            CommitId = commitId;
        }
    }
}