using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EditorLib.Nodes;

using EditorUserControll;

namespace EditorLib.ExtensionMethods;
public static class GenExtensionMethods
{
  public static int GetNextId(this IEnumerable<Node> nodes)
    => nodes.Any()
      ? nodes.Select(x => x.Id).Max() + 1
      : 0;

  public static WriteableBitmap ToWritableImage(this string base64Sting)
  {
    byte[] byteBuffer = Convert.FromBase64String(base64Sting);
    using var memoryStream = new MemoryStream(byteBuffer);
    {
      memoryStream.Position = 0;

      var bmpReturn = (Bitmap)System.Drawing.Image.FromStream(memoryStream);

      using var memory = new MemoryStream();
      {
        bmpReturn.Save(memory, ImageFormat.Png);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return new WriteableBitmap(bitmapImage);
      }
    }
  }

  public static void RemoveConnectionsWithNode(this UIElementCollection uieCollection, Node node)
    => uieCollection.OfType<NodeConnection>()
      .Where(x => x.ConnectionNode1 == node || x.ConnectionNode2 == node)
      .ToList()
      .ForEach(uieCollection.Remove);
}
