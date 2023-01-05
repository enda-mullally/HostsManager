//
// Copyright © 2021-2023 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildDateAttribute : Attribute
{
    public string BuildDate { get; set; }

    public BuildDateAttribute(string buildDateString)
    {
        BuildDate = buildDateString;
    }
}