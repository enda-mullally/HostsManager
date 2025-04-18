//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildDateAttribute(string buildDateString) : Attribute
{
    public string BuildDate { get; init; } = buildDateString;
}