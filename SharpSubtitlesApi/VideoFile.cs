using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSubtitlesApi;

public class VideoFile
{

    #region Extensions


    public static bool IsVideoFile(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(extension)) return false;
        // ReSharper disable once SuspiciousTypeConversion.Global
        return VideoExtension.Extensions.Any(videoExtension => videoExtension.Equals(extension));
    }
    #endregion

    #region Hash
    // https://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes

    public static byte[] ComputeHash(string filename)
    {
        using Stream input = File.OpenRead(filename);
        var result = ComputeHash(input);
        return result;
    }

    public static byte[] ComputeHash(Stream input)
    {
        var streamSize = input.Length;
        var hash = streamSize;

        long i = 0;
        var buffer = new byte[sizeof(long)];
        while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
        {
            i++;
            hash += BitConverter.ToInt64(buffer, 0);
        }

        input.Position = Math.Max(0, streamSize - 65536);
        i = 0;
        while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
        {
            i++;
            hash += BitConverter.ToInt64(buffer, 0);
        }
        input.Close();
        var result = BitConverter.GetBytes(hash);
        Array.Reverse(result);
        return result;
    }

    public static Task<byte[]> ComputeHashAsync(string filename)
    {
        using Stream input = File.OpenRead(filename);
        var result = ComputeHashAsync(input);
        return result;
    }

    public static async Task<byte[]> ComputeHashAsync(Stream input)
    {
        var streamSize = input.Length;
        var hash = streamSize;

        long i = 0;
        var buffer = new byte[sizeof(long)];
        while (i < 65536 / sizeof(long) && (await input.ReadAsync(buffer.AsMemory(0, sizeof(long))) > 0))
        {
            i++;
            hash += BitConverter.ToInt64(buffer, 0);
        }

        input.Position = Math.Max(0, streamSize - 65536);
        i = 0;
        while (i < 65536 / sizeof(long) && (await input.ReadAsync(buffer.AsMemory(0, sizeof(long))) > 0))
        {
            i++;
            hash += BitConverter.ToInt64(buffer, 0);
        }
        input.Close();
        var result = BitConverter.GetBytes(hash);
        Array.Reverse(result);
        return result;
    }

    private static string ToHexadecimal(byte[] bytes)
    {
        var hexBuilder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            hexBuilder.Append(bytes[i].ToString("x2"));
        }
        return hexBuilder.ToString();
    }
    #endregion
}