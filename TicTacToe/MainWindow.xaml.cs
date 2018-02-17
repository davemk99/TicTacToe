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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameLogic gameLogic;

        public MainWindow()
        {
            InitializeComponent();
            gameGrid.Loaded += GameGrid_Loaded;
            gameLogic = new GameLogic();
        }

        private void GameGrid_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button btn = new Button();
                    btn.Name = string.Format("gameButton_{0}_{1}", i, j);
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    btn.FontSize = 20;
                    btn.Click += Btn_Click;
                    gameGrid.Children.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var rowAndColumn = button.Name.Split('_').Skip(1).Take(2).Select(x => int.Parse(x)).ToArray();
            var gameState = gameLogic.AddMove(rowAndColumn[0], rowAndColumn[1]);
            ChangeFromGameState(gameState, button);
        }

        public void ChangeFromGameState(GameState gameState, Button button)
        {
            button.Foreground = GetForgroungByPlayer();
            if (gameState == GameState.NotValid)
            {
                MessageBox.Show("Move is not valid");
            }
            else if (gameState == GameState.FirstWin)
            {
                button.Content = gameLogic.PlayerSymbol;
                MessageBox.Show("First Player Wins");
                InitFirstState();
            }
            else if (gameState == GameState.SecondWin)
            {
                button.Content = gameLogic.PlayerSymbol;
                MessageBox.Show("Second Player Wins");
                InitFirstState();
            }
            else if (gameState == GameState.Tie)
            {
                button.Content = gameLogic.PlayerSymbol;
                MessageBox.Show("Tie");
                InitFirstState();
            }
            else
            {
                button.Content = gameLogic.PlayerSymbol;
                gameLogic.ChangeSymbol();
            }
        }

        private Brush GetForgroungByPlayer()
        {
            var getPlayer = gameLogic.GetPlayer();
            if (getPlayer == Players.FirstPlayer)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Blue);
            }
        }

        private void InitFirstState()
        {
            for (int i = 0; i < gameGrid.Children.Count; i++)
            {
                var control = gameGrid.Children[i] as Button;
                if (control != null && control.Name.Contains("gameButton"))
                {
                    control.Content = "";
                }
            }
            gameLogic = new GameLogic();
        }
    }
}