﻿//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.IO
{
    public interface IFile
    {
        bool CopyFileTo(string sourceFileName, string destFileName, bool append = false);

        long FileSize(string fileName);

        bool ReplaceContentWith(string destFileName, string newContent);

        bool DeleteIfExists(string fileName);
    }
}