using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public enum GameState
    {
        FirstWin,
        SecondWin,
        Tie,
        NotEnded,
        NotValid
    }

    public enum Players
    {
        FirstPlayer,
        SecondPlayer
    }

    public class GameLogic
    {
        public string PlayerSymbol { get; private set; }

        private int[][] _gameMap;

        public GameLogic()
        {
            PlayerSymbol = "X";
            InitGameMap();
        }

        public Players GetPlayer()
        {
            if (PlayerSymbol == "X")
            {
                return Players.FirstPlayer;
            }
            else
            {
                return Players.SecondPlayer;
            }
        }

        private void InitGameMap()
        {
            _gameMap = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                _gameMap[i] = new int[3];
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _gameMap[i][j] = 0;
                }
            }
        }

        public GameState AddMove(int row, int column)
        {
            if (row >= 3 || row < 0)
            {
                throw new ArgumentException();
            }
            if (column >= 3 || column < 0)
            {
                throw new ArgumentException();
            }

            if (_gameMap[row][column] == 0)
            {
                if (PlayerSymbol == "X")
                {
                    _gameMap[row][column] = 1;
                }
                else
                {
                    _gameMap[row][column] = -1;
                }
            }
            else
            {
                return GameState.NotValid;
            }

            return DoesGameEnd(_gameMap);
        }

        public void ChangeSymbol()
        {
            if (PlayerSymbol == "X")
            {
                PlayerSymbol = "O";
            }
            else
            {
                PlayerSymbol = "X";
            }
        }

        private GameState WinWithRow(int[][] gameMap)
        {
            for (int i = 0; i < 3; i++)
            {
                var rowSum = gameMap[i].Sum();
                var gameState = GameStateBySum(rowSum);
                if (gameState != GameState.NotEnded)
                {
                    return gameState;
                }
            }
            return GameState.NotEnded;
        }

        private GameState WinWithColumns(int[][] gameMap)
        {
            for (int i = 0; i < 3; i++)
            {
                var colSum = 0;
                for (int j = 0; j < 3; j++)
                {
                    colSum += gameMap[j][i];
                }

                var gameState = GameStateBySum(colSum);
                if (gameState != GameState.NotEnded)
                {
                    return gameState;
                }
            }
            return GameState.NotEnded;
        }

        private GameState WinWithDiagonal(int[][] gameMap)
        {
            var diagonalSum = 0;
            for (int i = 0; i < 3; i++)
            {
                diagonalSum += gameMap[i][i];
            }
            var gameState = GameStateBySum(diagonalSum);
            if (gameState == GameState.NotEnded)
            {
                diagonalSum = 0;
                for (int i = 0; i < 3; i++)
                {
                    diagonalSum += gameMap[i][2 - i];
                }
                return GameStateBySum(diagonalSum);
            }
            return gameState;
        }

        private GameState DoesGameEnd(int[][] gameMap)
        {
            var gameState = WinWithRow(gameMap);
            if (gameState != GameState.NotEnded)
            {
                return gameState;
            }
            gameState = WinWithColumns(gameMap);
            if (gameState != GameState.NotEnded)
            {
                return gameState;
            }
            gameState = WinWithDiagonal(gameMap);
            if (gameState != GameState.NotEnded)
            {
                return gameState;
            }
            var zerosExist = gameMap.SelectMany(x => x).Any(x => x == 0);
            if (zerosExist)
            {
                return GameState.NotEnded;
            }
            else
            {
                return GameState.Tie;
            }
        }

        private GameState GameStateBySum(int sum)
        {
            if (sum == 3)
            {
                return GameState.FirstWin;
            }
            else if (sum == -3)
            {
                return GameState.SecondWin;
            }
            return GameState.NotEnded;
        }
    }
}