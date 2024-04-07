//
// Copyright © 2021-2024 Enda Mullally.
//

namespace EM.HostsManager.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildDateAttribute(string buildDateString) : Attribute
{
    public string BuildDate { get; set; } = buildDateString;
}