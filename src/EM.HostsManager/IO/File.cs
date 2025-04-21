//
// Copyright © 2021-2025 Enda Mullally.
//

using SysFile = System.IO.File;

namespace EM.HostsManager.IO;

public class File : IFile
{
    public bool CopyFileTo(string sourceFileName, string destFileName, bool append = false)
    {
        try
        {
            using var sr = new StreamReader(sourceFileName);
            using var sw = new StreamWriter(destFileName, append);
            var currentHostsFileContent = sr.ReadToEnd();
            sw.Write(currentHostsFileContent);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public long FileSize(string fileName)
    {
        return SysFile.Exists(fileName)
            ? new FileInfo(fileName).Length
            : 0;
    }

    public bool ReplaceContentWith(string destFileName, string newContent)
    {
        try
        {
            using var sw = new StreamWriter(destFileName, false);
            sw.Write(newContent);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool DeleteIfExists(string fileName)
    {
        try
        {
            if (SysFile.Exists(fileName))
            {
                SysFile.Delete(fileName);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}