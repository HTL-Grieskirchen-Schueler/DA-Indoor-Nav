using EditorLib;

using Karteneditor.Services;

using System.Windows;
using System.Windows.Controls;

namespace Karteneditor;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  private readonly string _contentPath = "..\\..\\..\\Content\\";

  //private CommunicatorService _communicator;
  private SaveService _saveService;
  private MessageService _messageService;

  public MainWindow() => InitializeComponent();

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    //_communicator = new("https://localhost:5001");
    _saveService = new(_contentPath, ImageControl);
    _messageService = new(Dispatcher, lblMessage);

    _saveService.LoadImage();
    _saveService.LoadContent();

    Enum.GetValues(typeof(EditModes)).OfType<EditModes>().ToList().ForEach(x => cboModes.Items.Add(x));
    cboModes.SelectionChanged += CboModes_SelectionChanged;
    cboModes.SelectedItem = ImageControl.Mode;
  }

  private void CboModes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    => ImageControl.Mode = (EditModes)cboModes.SelectedItem;

  private void OnSaveClicked(object sender, RoutedEventArgs e)
  {
    _saveService.SaveContent();
    _messageService.WriteMessage("Saved!");
  }
}
