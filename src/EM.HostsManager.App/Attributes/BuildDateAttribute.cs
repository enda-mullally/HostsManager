//
// Copyright © 2021-2022 Enda Mullally.
//

namespace EM.HostsManager.App.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildDateAttribute : Attribute
{
    public string BuildDate { get; set; }

    public BuildDateAttribute(string buildDateString)
    {
        BuildDate = buildDateString;
    }
}