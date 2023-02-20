namespace Bal.Converter.Common.Extensions ;

public static class FileSystemExtensions
{
    public static bool Wait(this FileInfo file, int tries = 0)
    {
        while (true)
        {
            ++tries;
            try
            {
                using var fs = new FileStream(file.FullName,
                                              FileMode.Open, FileAccess.ReadWrite,
                                              FileShare.None, 100);
                fs.ReadByte();

                break;
            }
            catch (Exception)
            {
                if (tries > 10)
                {
                    return false;
                }

                System.Threading.Thread.Sleep(500);
            }
        }

        return true;
    }

    public static void SafeDelete(this FileSystemInfo fs)
    {
        if (fs.Exists)
        {
            fs.Delete();
        }
    }
}