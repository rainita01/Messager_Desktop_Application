using System.Windows.Media.Imaging;

namespace demo_158.Services.Interfaces;

public interface IProfileServices
{
    public BitmapImage ByteArrayToImage(byte[]? bytes);
}