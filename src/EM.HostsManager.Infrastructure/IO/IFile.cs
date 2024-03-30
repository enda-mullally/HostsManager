namespace EM.HostsManager.Infrastructure.IO
{
    public interface IFile
    {
        bool CopyFileTo(string sourceFileName, string destFileName, bool append = false);

        long FileSize(string fileName);

        bool ReplaceContentWith(string destFileName, string newContent);

        bool DeleteIfExists(string fileName);
    }
}