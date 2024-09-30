using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EditorLib.Nodes.EditNode;
/// <summary>
/// Interaction logic for EditNodeWindow.xaml
/// </summary>
public partial class EditNodeWindow : Window
{

  private Dictionary<string, string> _properties;

  public bool ApplyChanges = false;
  public Dictionary<string, string> PropValues = [];

  public EditNodeWindow(Dictionary<string, string> properties)
  {
    InitializeComponent();
    _properties = properties;
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    PreviewKeyUp += OnPreviewKeyUp;

    _properties.ToList().ForEach(prop =>
    {
      var lblName = new Label { Content = $"{prop.Key}:" };
      var txtValue = new TextBox { Name = prop.Key, Width = 200, Text = prop.Value };

      txtValue.PreviewKeyDown += TxtOnPreviesKeyDown;

      Grid.SetColumn(lblName, 0);
      Grid.SetColumn(txtValue, 1);

      grdPropElements.RowDefinitions.Add(new RowDefinition());
      Grid.SetRow(lblName, grdPropElements.RowDefinitions.Count - 1);
      Grid.SetRow(txtValue, grdPropElements.RowDefinitions.Count - 1);

      grdPropElements.Children.Add(lblName);
      grdPropElements.Children.Add(txtValue);
    });
  }

  private void TxtOnPreviesKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Enter)
    {
      int focusRow = Grid.GetRow((UIElement)Keyboard.FocusedElement);

      var nextTextBox = focusRow >= _properties.Count - 1
        ? grdPropElements.Children.OfType<TextBox>().Single(x => Grid.GetRow(x) == 0)
        : grdPropElements.Children.OfType<TextBox>().Single(x => Grid.GetRow(x) == focusRow + 1);
      Keyboard.Focus(nextTextBox);
    }
  }

  private void OnSaveClicked(object sender, RoutedEventArgs e)
  => CloseWindowWithSave();

  private void OnPreviewKeyUp(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Escape)
      CloseWindowWithoutSave();
  }

  private void OnCancleClicked(object sender, RoutedEventArgs e)
    => CloseWindowWithoutSave();

  private void CloseWindowWithSave()
  {
    ApplyChanges = true;

    _properties.ToList().ForEach(propName =>
    {
      var element = grdPropElements.Children.OfType<TextBox>().Single(x => x.Name == propName.Key);
      PropValues.Add(propName.Key, element.Text);
    });

    Close();
  }

  private void CloseWindowWithoutSave()
  {
    ApplyChanges = false;
    Close();
  }
}
