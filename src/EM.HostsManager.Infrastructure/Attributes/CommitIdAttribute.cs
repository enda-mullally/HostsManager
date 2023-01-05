//
// Copyright © 2021-2023 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class CommitIdAttribute : Attribute
{
    public string CommitId { get; set; }

    public CommitIdAttribute(string commitId)
    {
        CommitId = commitId;
    }
}