using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ChatNoirGame.Model
{
  class Circle : INotifyPropertyChanged
  {
    public string Id { get; set; }
    public string Id2 { get; set; }
    public Tuple<int, int> Coordinates { get; set; }
    public int number { get; set; } = 0;

    public int Number
    {
      get { return number; }
      set
      {
        if (number == value) return;
        number = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number"));
      }
    }
    public double Size { get; set; } = 50;

    private string icon = @"pack://application:,,,/ChatNoirGame;component/Resources/";

    public Circle()
    {

    }

    public Circle(string Id, SolidColorBrush Background, Tuple<int, int> Coordinates)
    {
      this.Id = Id;
      this.Background = Background;
      this.Coordinates = Coordinates;
    }
    public String Icon
    {
      get { return icon; }
      set
      {
        if (icon == value) return;
        icon = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Icon"));
      }
    }

    private SolidColorBrush background;

    private bool isClicked = false;

    public bool IsClicked
    {
      get { return isClicked; }
      set
      {
        if (isClicked == value) return;
        isClicked = value;
        if (isClicked)
          Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#748504"));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsClicked"));
      }
    }


    public SolidColorBrush Background
    {
      get { return background; }
      set
      {
        if (background == value) return;
        background = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Background"));
      }
    }


    public override string ToString()
    {
      return this.Id;
    }


    public event PropertyChangedEventHandler PropertyChanged;
  }
}
