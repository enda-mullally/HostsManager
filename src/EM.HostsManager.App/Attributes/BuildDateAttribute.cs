namespace EM.HostsManager.App.Attributes
{
    public class BuildDateAttribute : System.Attribute
    {
        public string BuildDate { get; set; }

        public BuildDateAttribute(string buildDateString)
        {
            BuildDate = buildDateString;
        }
    }
}
