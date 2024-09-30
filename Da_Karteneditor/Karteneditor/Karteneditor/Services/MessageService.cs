using System.Windows.Controls;
using System.Windows.Threading;

namespace Karteneditor.Services;
internal class MessageService(Dispatcher dispatcher, Label lblMessage)
{
  private Dispatcher _dispatcher = dispatcher;
  private Label _lblMessage = lblMessage;

  public void WriteMessage(string message, int millisecondsDelay = 2000)
  {
    var task = Task.Run(() =>
    {
      _dispatcher.Invoke(() => _lblMessage.Content = message);
      Task.Delay(millisecondsDelay).Wait();
      _dispatcher.Invoke(() => _lblMessage.Content = "");
    });
  }
}
