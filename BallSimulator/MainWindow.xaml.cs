using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
//Ball jako MODEL, GameViewModel jako VIEWMODEL,XAML jako VIEW,
//BALLSERVICE jako logika . MainWindow.xaml.cs oraz .xaml jako  warstwa prezentacji
//z warstwa DANE nwm co i jak mozna to podpiac jako ball troche !!LUB!! -odnosnik w iballservice. mozna jedna rzecz dac oddzielnie do fcji "dane" ktora sie stworzy
//Zależność od zewnętrznych repozytoriów danych- nwm o co chodzi
//rodzielenie warstw - warstwy rozdzielilem zgonie z MVVM ale nie sa osbnymi projektami (a tak jest na wikamp :/ )
// ---
// !!! NIE MAMY TESTÓW: testowanie jednostkowe oraz integracyjne. Techniki MOCK nie mamy (nie trzeba ale bedzie latwiej z nia), testy jednostkoe dla warstw DANE I LOGIKA
// !!! "luzne powiązanie warstw:..." eee no nie wiem, powiazanie wydaje sie byc luzne ale bez testow nie sprawdze tego w praktyce
// !!! "Użycie DataBinding oraz ICommand:..." no databinding jest zaimplementowane i icommand jest chyba uzywane w MainWindow.xaml.cs dla przycisku start
// ---
//WAZNE: testy musimy miec bo sa potrzebne by zaliczyc zadanie. ORAZ nie rozumiem w "wytyczne do realizacji" zdania:
//"GitHub: utworzyć release, w którym tag zostanie nadany zgodnie z Semantic Versioning 2.0.0" tam jest link do tego sementic ale nie wiem o co chodzi
//dodatkowo:
//w mainwindow.xaml canvas ma wymiary 800x450 a border dla niego 802x452, poniewaz scianki nie zgrywaly sie (nie wiem czemu) ciezko wytlumaczyc ale wyglada git teraz
//!!!
//WAZNE2: nie wiem czy wgl zadanie jest dobrze zrobione, poniewaz w etapie1 ma nie byc odbijania sie od scian ale nie mialem pomyslu jak zrobic zeby te kulki tam byly,
//poruszaly sie i jednoczesnie nie wychodzimy poza zakres canvas. !!!TRZEBA ZAPYTAC NA ZAJECIACH CZY MOZE TAK ZOSTAC!!!
//!!!
//update1: dodalem issues na gita (dla nas i dla prowadzacego)
namespace BallSimulator
{
    public partial class MainWindow : Window
    {
        private GameViewModel gameViewModel;
        private DispatcherTimer gameTimer;

        public MainWindow()
        {
            InitializeComponent();
            gameViewModel = new GameViewModel(new BallService(800, 450)); //dajemy szerokosc i wysokosc canvas (tam gdzie kulki sie poruszaja)
            DataContext = gameViewModel;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameTimer?.Stop();
            if (int.TryParse(numBallsPicker.Text, out int numBalls))
            {
                gameViewModel.StartGame(numBalls, (int)canvas.ActualWidth, (int)canvas.ActualHeight);
            }

            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; //~60fps
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameViewModel.UpdateGame();
            DrawBalls();
        }

        private void DrawBalls()
        {
            canvas.Children.Clear();
            foreach (var ball in gameViewModel.Balls)
            {
                var ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = ball.Diameter,
                    Height = ball.Diameter
                };
                 
                Canvas.SetLeft(ellipse, ball.X - ball.Diameter / 2); //centrowanie kulki na jej wspolrz x
                Canvas.SetTop(ellipse, ball.Y - ball.Diameter / 2);  //centrowanie kulki na jej wspolrz y
                canvas.Children.Add(ellipse);
            }
        }
    }
}
