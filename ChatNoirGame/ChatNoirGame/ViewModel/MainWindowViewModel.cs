using ChatNoirGame.Command;
using ChatNoirGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatNoirGame.ViewModel
{
  class MainWindowViewModel : INotifyPropertyChanged

  {
    private SolidColorBrush CatCircleBackground = Brushes.Red;
    private SolidColorBrush GreenBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ccff00"));////9fcb04, "#ccff00"));
    private ObservableCollection<ObservableCollection<Circle>> board;
    HashSet<Circle> EdgeCircles = new HashSet<Circle>();
    private Circle Cat;
    public List<Circle> SelectedCircles = new List<Circle>();

    public ObservableCollection<ObservableCollection<Circle>> Board
    {
      get { return board; }
      set
      {
        if (board == value) return;
        board = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Board"));
      }
    }
    public ICommand ResetCommand { get; set; }
    public string CatIcon = @"pack://application:,,,/ChatNoirGame;component/Resources/cat.png";

    public MainWindowViewModel()
    {
      ResetCommand = new RelayCommand(ResetCommandExecute);
    }

    private void ResetCommandExecute()
    {
      LoadData();
    }

    public void LoadData()
    {
      EdgeCircles.Clear();
      SelectedCircles.Clear();
      Board = new ObservableCollection<ObservableCollection<Circle>>();
      int id = 0;
      for (int i = 0; i < 11; i++)
      {
        ObservableCollection<Circle> listOfCircles = new ObservableCollection<Circle>();
        for (int j = 0; j < 11; j++)
        {
          Circle circle = new Circle(id.ToString(), GreenBackground, new Tuple<int, int>(i, j));
          circle.Id2 = i.ToString() + "," + j.ToString();
          listOfCircles.Add(circle);
          id++;
        }
        Board.Add(listOfCircles);
      }

      Cat = getCircle("60");

      Cat.Icon = CatIcon;
      //Cat.Background = CatCircleBackground;
      HashSet<int> listOfGeneratedNumbers = new HashSet<int>();
      Random rand0 = new Random();
      int NumberOfSelectedCircles = rand0.Next(5, 15);
      while (listOfGeneratedNumbers.Count() < NumberOfSelectedCircles)
      {
        Random rand = new Random();
        int x = rand.Next(1, 121);
        if (!listOfGeneratedNumbers.Contains(x) && x != 60)
        {
          listOfGeneratedNumbers.Add(x);
          var circle = getCircle(x.ToString());
          circle.IsClicked = true;
          circle.number = -1;
          SelectedCircles.Add(circle);
        }
      }

    }

    public void SelectCircle(string id)
    {

      Circle circle = getCircle(id);
      if (Cat.Coordinates == circle.Coordinates)
      {
        return;
      }
      if (Cat.Coordinates.Item1 == 0 || Cat.Coordinates.Item1 == 10 || Cat.Coordinates.Item2 == 0 || Cat.Coordinates.Item2 == 10)
      {
        Cat.Icon = null;
        return;
      }
      circle.IsClicked = true;
      circle.Number = -1;
      SelectedCircles.Add(circle);

      getNextCoordinate(Cat);
      if (!EdgeCircles.Any())
      {
        List<int> x = new List<int>() { -1, -1, 0, 1, 1, 0 };
        List<int> yEven = new List<int>() { -1, 0, 1, 0, -1, -1 };
        List<int> yOdd = new List<int>() { 0, 1, 1, 1, 0, -1 };

        for (var i = 0; i < 6; i++)
        {
          var x2 = Cat.Coordinates.Item1 + x[i];
          int y2;
          if (Cat.Coordinates.Item1 % 2 == 0)
          {
            y2 = Cat.Coordinates.Item2 + yEven[i];
          }
          else
          {
            y2 = Cat.Coordinates.Item2 + yOdd[i];
          }
          if (x2 >= 0 && x2 < 11 && y2 >= 0 && y2 < 11)
          {

            if (Board[x2][y2].Number != -1)
            {
              MoveCat(Board[x2][y2]);
              //Cat.Background = GreenBackground;
              //Cat = new Circle();
              //Cat = Board[x2][y2];
              //Cat.Background = CatCircleBackground;
              return;
            }
          }

        }

        Cat.Icon = @"pack://application:,,,/ChatNoirGame;component/Resources/cat_left.png";
        return;
      }

      int minNumber = EdgeCircles.Min(c => c.Number);
      var listMinCircle = EdgeCircles.Where(c => c.Number == minNumber);
      var minCircle = listMinCircle.FirstOrDefault(c => c.Coordinates.Item1 == 0 || c.Coordinates.Item1 == 10 ||
            c.Coordinates.Item2 == 0 || c.Coordinates.Item2 == 10);
      if (minCircle == null)
        minCircle = listMinCircle.FirstOrDefault();

      if (minNumber != -1 && minCircle != null)
      {

        if (minCircle.Number == 1 && (minCircle.Coordinates.Item1 == 0 || minCircle.Coordinates.Item1 == 10 || minCircle.Coordinates.Item2 == 0 || minCircle.Coordinates.Item2 == 10))
        {
          MoveCat(minCircle);
          //Cat.Background = GreenBackground;
          //Cat = new Circle();
          //Cat = getCircle(minCircle.Id);
          //Cat.Background = CatCircleBackground;
          return;
        }
        else
        {
          MoveCat(getReversePath(minCircle));
          //Cat.Background = GreenBackground;
          //Circle cat = getReversePath(minCircle);
          //Cat = new Circle();
          //Cat = cat;
          //Cat.Background = CatCircleBackground;
        }


      }
      //getLeftSection(circle);
    }

    private Circle getCircle(string id)
    {
      Circle circle = null;
      foreach (var l in Board)
      {
        if (circle == null)
        {
          circle = l.FirstOrDefault(c => c.Id == id);
        }
        else
        {
          break;
        }
      }
      return circle;
    }


    void getNextCoordinate(Circle start)
    {
      Queue<Circle> queueOfCoordinates = new Queue<Circle>();

      for (int i = 0; i < 11; i++)
      {
        for (int j = 0; j < 11; j++)
        {
          if (Board[i][j].Number > 0)
            Board[i][j].Number = 0;
        }
      }

      queueOfCoordinates.Enqueue(start);


      while (queueOfCoordinates.Any())
      {
        var current = queueOfCoordinates.Dequeue();
        List<int> x = new List<int>() { -1, -1, 0, 1, 1, 0 };
        List<int> yEven = new List<int>() { -1, 0, 1, 0, -1, -1 };
        List<int> yOdd = new List<int>() { 0, 1, 1, 1, 0, -1 };

        for (var i = 0; i < 6; i++)
        {
          var x2 = current.Coordinates.Item1 + x[i];
          int y2;
          if (current.Coordinates.Item1 % 2 == 0)
          {
            y2 = current.Coordinates.Item2 + yEven[i];
          }
          else
          {
            y2 = current.Coordinates.Item2 + yOdd[i];
          }
          if (x2 >= 0 && x2 < 11 && y2 >= 0 && y2 < 11)
          {
            if ((x2 == 0 || x2 == 10 || y2 == 0 || y2 == 10) && Board[x2][y2].Number > 0)
            {
              EdgeCircles.Add(Board[x2][y2]);
            }
            if (Board[x2][y2].Number == 0 && Board[x2][y2] != start)
            {
              Board[x2][y2].Number = current.Number + 1;
              queueOfCoordinates.Enqueue(Board[x2][y2]);
            }
          }

        }
        var h = current.Number;
      }
    }

    Circle getReversePath(Circle current)
    {
      EdgeCircles.Clear();
      while (true)
      {

        List<int> x = new List<int>() { -1, -1, 0, 1, 1, 0 };
        List<int> yEven = new List<int>() { -1, 0, 1, 0, -1, -1 };
        List<int> yOdd = new List<int>() { 0, 1, 1, 1, 0, -1 };

        for (var i = 0; i < 6; i++)
        {
          var x2 = current.Coordinates.Item1 + x[i];
          int y2;
          if (current.Coordinates.Item1 % 2 == 0)
          {
            y2 = current.Coordinates.Item2 + yEven[i];
          }
          else
          {
            y2 = current.Coordinates.Item2 + yOdd[i];
          }
          if (x2 >= 0 && x2 < 11 && y2 >= 0 && y2 < 11)
          {
            if (Board[x2][y2].Number == current.Number - 1)
            {
              current = Board[x2][y2];
              if (current.Number <= 1)
                return current;
              break;
            }
          }

        }
        var h = current.Number;
      }
    }

    private void getLeftSection(Circle circle)
    {

      for (int i = 0; i < 11; i++)
      {
        for (int j = 0; j < 11; j++)
        {
          var c = circle;

          if (i <= c.Coordinates.Item1 && j <= c.Coordinates.Item2 / 2)
          {
            Board[i][j].Background = Brushes.Yellow;
          }
          if (i >= c.Coordinates.Item1 && j <= c.Coordinates.Item2 / 2)
          {
            Board[i][j].Background = Brushes.Yellow;
          }
        }
      }
    }

    private void MoveCat(Circle circle)
    {
      Cat.Background = GreenBackground;
      Cat.Icon = null;
      Cat = new Circle();
      Cat = circle;
      //Cat.Background = CatCircleBackground;
      Cat.Icon = CatIcon;
    }

    //private void MoveCat(Circle from, Circle to)
    //{
    //  from.Background = GreenBackground;
    //  from = new Circle();
    //  from = to;
    //  from.Background = CatCircleBackground;
    //}


    public event PropertyChangedEventHandler PropertyChanged;
  }
}
