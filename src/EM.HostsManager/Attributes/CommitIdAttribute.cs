//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class CommitIdAttribute(string commitId) : Attribute
{
    public string CommitId { get; init; } = commitId;
}