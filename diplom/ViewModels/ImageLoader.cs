using System.Reflection;
using Avalonia.Media.Imaging;

namespace diplom.ViewModels;

public static class ImageLoader
{
    public static Bitmap? LoadLevelImage(int level)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = $"diplom.Assets.zvaniya.{level}.png";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            return null;
        }

        return new Bitmap(stream);
    }
}