using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GraphicalTTT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GetBoardImages();
            GetBoardButtons();
        }

        private string currentPlayer = "O";
        private string[,] boardState = new string[3, 3] {
            { "empty", "empty", "empty" },
            { "empty", "empty", "empty" },
            { "empty", "empty", "empty" }};

        private Image[,] boardImages = new Image[3, 3];

        private void GetBoardImages()
        {
            boardImages[0, 0] = imgTopLeft;
            boardImages[0, 1] = imgTopMid;
            boardImages[0, 2] = imgTopRight;
            boardImages[1, 0] = imgCenterLeft;
            boardImages[1, 1] = imgCenterMid;
            boardImages[1, 2] = imgCenterRight;
            boardImages[2, 0] = imgBottomLeft;
            boardImages[2, 1] = imgBottomMid;
            boardImages[2, 2] = imgBottomRight;
        }

        private Button[,] boardButtons = new Button[3, 3];

        private void GetBoardButtons()
        {
            boardButtons[0, 0] = btnTopLeft;
            boardButtons[0, 1] = btnTopMid;
            boardButtons[0, 2] = btnTopRight;
            boardButtons[1, 0] = btnCenterLeft;
            boardButtons[1, 1] = btnCenterMid;
            boardButtons[1, 2] = btnCenterRight;
            boardButtons[2, 0] = btnBottomLeft;
            boardButtons[2, 1] = btnBottomMid;
            boardButtons[2, 2] = btnBottomRight;
        }

        private void btnTile_Click(object sender, RoutedEventArgs e)
        {

            Button btnTile = (Button)sender;
            Image imgTile = (Image)btnTile.Content;

            imgTile.Opacity = 100;

            if (currentPlayer == "O")
            {
                imgTile.Source = new BitmapImage(new Uri("ms-appx:///Assets/O.png"));
            }
            else
            {
                imgTile.Source = new BitmapImage(new Uri("ms-appx:///Assets/X.png"));
            }

            btnTile.IsEnabled = false;
            UpdateBoardState(GetTileIndexes(imgTile.Name));
            CheckWinConditions();
            SwitchPlayers();
        }

        private void SwitchPlayers()
        {
            if (currentPlayer == "O")
            {
                SetCurrentPlayer("X");
            }
            else
            {
                SetCurrentPlayer("O");
            }
        }

        private void SetCurrentPlayer(string player = "O")
        {
            lblCurrentPlayer.Text = player;
            currentPlayer = player;
        }

        private void UpdateBoardState(int[] coords)
        {
            boardState[coords[0], coords[1]] = currentPlayer;
        }

        private int[] GetTileIndexes(string tileName)
        {
            int row = 0;
            int col = 0;

            int[] coords = new int[2] { 0, 0 };

            switch (tileName)
            {
                case "imgTopLeft":
                    {
                        row = 0;
                        col = 0;
                        break;
                    }
                case "imgTopMid":
                    {
                        row = 0;
                        col = 1;
                        break;
                    }
                case "imgTopRight":
                    {
                        row = 0;
                        col = 2;
                        break;
                    }
                case "imgCenterLeft":
                    {
                        row = 1;
                        col = 0;
                        break;
                    }
                case "imgCenterMid":
                    {
                        row = 1;
                        col = 1;
                        break;
                    }
                case "imgCenterRight":
                    {
                        row = 1;
                        col = 2;
                        break;
                    }
                case "imgBottomLeft":
                    {
                        row = 2;
                        col = 0;
                        break;
                    }
                case "imgBottomMid":
                    {
                        row = 2;
                        col = 1;
                        break;
                    }
                case "imgBottomRight":
                    {
                        row = 2;
                        col = 2;
                        break;
                    }
            }

            coords[0] = row;
            coords[1] = col;
            return coords;
        }

        private void CheckWinConditions()
        {
            string win = $"{currentPlayer}{currentPlayer}{currentPlayer}";

            if(
                //Test Top Left to Bottom Right Diagonal
                (boardState[0, 0] + boardState[1, 1] + boardState[2, 2] == win) ||
                //Test Top Right to Bottom Left Diagonal
                (boardState[0, 2] + boardState[1, 1] + boardState[2, 0] == win) ||
                //Test Center Row
                (boardState[1, 0] + boardState[1, 1] + boardState[1, 2] == win) ||
                //Test Mid Col
                (boardState[0, 1] + boardState[1, 1] + boardState[2, 1] == win) ||
                //Test Top Row
                (boardState[0,0] + boardState[0,1] + boardState[0,2] == win) ||
                //Test Bottom Row
                (boardState[2, 0] + boardState[2, 1] + boardState[2, 2] == win) ||
                //Test First Col
                (boardState[0, 0] + boardState[1, 0] + boardState[2, 0] == win) ||
                //Test Right Col
                (boardState[0, 2] + boardState[1, 2] + boardState[2, 2] == win)
                )
            {
                HandleWinCondition();
            }
            else if (
                    boardButtons[0, 0].IsEnabled == false &&
                    boardButtons[0, 1].IsEnabled == false &&
                    boardButtons[0, 2].IsEnabled == false &&
                    boardButtons[1, 0].IsEnabled == false &&
                    boardButtons[1, 1].IsEnabled == false &&
                    boardButtons[1, 2].IsEnabled == false &&
                    boardButtons[2, 0].IsEnabled == false &&
                    boardButtons[2, 1].IsEnabled == false &&
                    boardButtons[2, 2].IsEnabled == false
                    )
            {
                HandleDrawCondition();
            }
        }

        private void HandleDrawCondition()
        {
            lblWin.Text = "DRAW!";
            lblWin.Visibility = Visibility.Visible;
            rectWin.Visibility = Visibility.Visible;
        }

        private void HandleWinCondition()
        {
            lblWin.Text = $"Player {currentPlayer} Wins!";
            lblWin.Visibility = Visibility.Visible;
            rectWin.Visibility = Visibility.Visible;
        }

        private void Reset()
        {
            //Reset boardState array
            for (int i = 0; i < boardState.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.GetLength(1); j++)
                {
                    boardState[i, j] = "empty";
                }
            }

            foreach (Image x in boardImages)
            {
                x.Opacity = 0;
            }

            foreach (Button x in boardButtons)
            {
                x.IsEnabled = true;
            }
        }

        private void WinScreen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblWin.Visibility = Visibility.Collapsed;
            rectWin.Visibility = Visibility.Collapsed;
            Reset();
        }
    }
}
