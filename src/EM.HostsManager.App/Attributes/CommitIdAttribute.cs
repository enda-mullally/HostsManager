//
// Copyright © 2021-2022 Enda Mullally.
//

namespace EM.HostsManager.App.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class CommitIdAttribute : Attribute
{
    public string CommitId { get; set; }

    public CommitIdAttribute(string commitId)
    {
        CommitId = commitId;
    }
}