using ChatNoirGame.Model;
using ChatNoirGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatNoirGame
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private MainWindowViewModel viewModel;
    private double width;
    private double height;

    public MainWindow()
    {
      InitializeComponent();

      viewModel = new MainWindowViewModel();
      DataContext = viewModel;
      viewModel.LoadData();
      this.WindowState = WindowState.Maximized;
      //var desktopWorkingArea = SystemParameters.WorkArea;
      ////window.Width = desktopWorkingArea.Right;
      //width = desktopWorkingArea.Right / 2;
      //height = desktopWorkingArea.Bottom / 2;
      //height -= 65;
      //width -= 30;
      //Canvas.SetLeft(Cat, width);
      //Canvas.SetTop(Cat, height);
    }
    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, KeyEventArgs e)
    {

    }
    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var listViewItem = sender as ListBoxItem;
      List<Circle> l = new List<Circle>();

      //if (!viewModel.SelectedCircles.Any(c => c.Id == listViewItem.Content.ToString()))
      //{
      //  var x = Cat.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
      //  height += 55;
      //  width += 55;
      //  Canvas.SetTop(Cat, height);
      //  Canvas.SetLeft(Cat, width);
      //}
      viewModel.SelectCircle(listViewItem.Content.ToString());
    }

    


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }
  }
}
