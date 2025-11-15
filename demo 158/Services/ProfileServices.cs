using System.IO;
using System.Windows.Media.Imaging;
using demo_158.Services.Interfaces;

namespace demo_158.Services;

public class ProfileServices :IProfileServices
{
    public BitmapImage ByteArrayToImage(byte[]? bytes)
    {
        if (bytes == null)
        {
            return null;
        }
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}