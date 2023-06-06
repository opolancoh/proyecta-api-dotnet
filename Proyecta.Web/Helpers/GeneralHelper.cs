namespace Proyecta.Web.Helpers;

public static class GeneralHelper
{
    public static string MemoryInBestUnit(long size)
    {
        const long megaByte = 1024 * 1024;
        const long gigaByte = megaByte * 1024;

        switch (size)
        {
            case < megaByte:
                return $"{size} bytes";
            case < gigaByte:
            {
                var megaBytes = (double)size / megaByte;
                return $"{megaBytes:N2} MiB";
            }
            default:
            {
                var gigaBytes = (double)size / gigaByte;
                return $"{gigaBytes:N2} GiB";
            }
        }
    }
}