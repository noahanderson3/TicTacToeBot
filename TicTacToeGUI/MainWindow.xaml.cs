using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToeGUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private Vars
		/// <summary>
		/// Holds current results of items
		/// </summary>
		private MarkType[] results;
		/// <summary>
		/// True if X player, false if O
		/// </summary>
		private bool mPlayer1Turn = true;
		/// <summary>
		/// True if game ended
		/// </summary>
		private bool endGame;
		/// <summary>
		/// Holds all first player's positions
		/// </summary>
		private List<int> firstPlayerPositions = new List<int>();
		/// <summary>
		/// Holds all computer player's positions
		/// </summary>
		List<int> computerPositions = new List<int>();


		#endregion
		#region Constructor
		/// <summary>
		/// Default Constructor
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			NewGame();
		}
		#endregion
		/// <summary>
		/// Starts a new game and resets everything to defaults
		/// </summary>
		private void NewGame()
		{
			// Create new blank array of all cells
			results = new MarkType[9];

			for (int i = 0; i < results.Length; i++)
			{
				results[i] = MarkType.Free;
			}

			//Makes sure player one is first player
			mPlayer1Turn = true;

			//Iterate every button on the grid
			Container.Children.Cast<Button>().ToList().ForEach(button =>
			{
				//Change background, foreground and content to default values
				button.Content = string.Empty;
				button.Background = Brushes.Black;
				button.Foreground = Brushes.Blue;
			});

			//Makes sure game isnt finished
			endGame = false;

			// Clear both position lists
			firstPlayerPositions.Clear();
			computerPositions.Clear();
		}
		/// <summary>
		/// Handles a button click event
		/// </summary>
		/// <param name="sender">The button that was clicked</param>
		/// <param name="e">The events of the click</param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//If game is over, start a new game with click
			if (endGame)
			{
				NewGame();
				return;
			}
			// Cast sender as a button
			var button = (Button)sender;
			// Column and Row get
			var column = Grid.GetColumn(button);
			var row = Grid.GetRow(button);

			// Converts them to index
			var index = column + (row * 3);

			// Dont do anything if the cell already has something in it
			if (results[index] != MarkType.Free)
				return;

			// Set the cell value based on player
			results[index] = MarkType.Cross;

			// Set button to show X
			button.Content = "X";

			// Add new position to list
			firstPlayerPositions.Add(index);

			// Check for winner
			CheckWinner();
			if (!endGame)
			{
				mPlayer1Turn = false;
				ComputerPlayer();
			}
		}
		/// <summary>
		/// Checks if there is 3 in a row
		/// </summary>
		private void CheckWinner()
		{
			#region Horizontal Wins
			// Check for horizontal wins
			//
			// - Row 0
			//
			if (results[0] != MarkType.Free && (results[0] & results[1] & results[2]) == results[0])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
			}
			// Check for horizontal wins
			//
			// - Row 1
			//
			if (results[3] != MarkType.Free && (results[3] & results[4] & results[5]) == results[3])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
			}
			// Check for horizontal wins
			//
			// - Row 2
			//
			if (results[6] != MarkType.Free && (results[6] & results[7] & results[8]) == results[6])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
			}
			#endregion
			#region Vertical Wins
			// Check for vertical wins
			//
			// - Column 0
			//
			if (results[0] != MarkType.Free && (results[0] & results[3] & results[6]) == results[0])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
			}
			// Check for veritcal wins
			//
			// - Column 1
			//
			if (results[1] != MarkType.Free && (results[1] & results[4] & results[7]) == results[1])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button1_0.Background = Button1_2.Background = Button1_1.Background = Brushes.Green;
			}
			// Check for vertical wins
			//
			// - Column 2
			//
			if (results[2] != MarkType.Free && (results[2] & results[5] & results[8]) == results[2])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
			}
			#endregion
			#region Diagonal Wins
			// Check for diagonal wins
			//
			// - Diagonal 0
			//
			if (results[0] != MarkType.Free && (results[0] & results[4] & results[8]) == results[0])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
			}
			// Check for diagonal wins
			//
			// - Diagonal 1
			//
			if (results[2] != MarkType.Free && (results[2] & results[4] & results[6]) == results[2])
			{
				// End game
				endGame = true;

				//Highlight winning cells
				Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
			}
			#endregion
			#region EndGame
			if (!results.Any(result => result == MarkType.Free) && !endGame)
			{
				// Game ended
				endGame = true;

				//Turn all cells orange

				Container.Children.Cast<Button>().ToList().ForEach(button =>
				{
					button.Background = Brushes.Orange;
				});
			}
			#endregion
		}
		private void ComputerPlayer()
		{
			// Generates random index to place
			Random random = new Random();
			int position = random.Next(0, 8);
			Button button = new Button();
			int chance = random.Next(1, 2);
			
				// Makes sure it's the bot's turn
				if (!mPlayer1Turn)
				{
					// If middle is available, best advantage
					if (results[4] == MarkType.Free)
					{
						position = 4;
						button = Button1_1;
					}
				#region O Horizontal Takes
				else if (computerPositions.Contains(0) && computerPositions.Contains(1) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				else if (computerPositions.Contains(1) && computerPositions.Contains(2) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				else if (computerPositions.Contains(0) && computerPositions.Contains(2) && results[1] == MarkType.Free)
				{
					position = 1;
					button = Button1_0;
				}
				//Row 1										 
				else if (computerPositions.Contains(3) && computerPositions.Contains(4) && results[5] == MarkType.Free)
				{
					position = 5;
					button = Button2_1;
				}
				else if (computerPositions.Contains(3) && computerPositions.Contains(5) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (computerPositions.Contains(4) && computerPositions.Contains(5) && results[3] == MarkType.Free)
				{
					position = 3;
					button = Button0_1;
				}
				// Row 2									
				else if (computerPositions.Contains(6) && computerPositions.Contains(7) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (computerPositions.Contains(6) && computerPositions.Contains(8) && results[7] == MarkType.Free)
				{
					position = 7;
					button = Button1_2;
				}
				else if (computerPositions.Contains(8) && computerPositions.Contains(7) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				#endregion
				#region O Vertical Takes
				// Column 0
				else if (computerPositions.Contains(0) && computerPositions.Contains(3) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				else if (computerPositions.Contains(0) && computerPositions.Contains(6) && results[3] == MarkType.Free)
				{
					position = 3;
					button = Button0_1;
				}
				else if (computerPositions.Contains(3) && computerPositions.Contains(6) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				// Column 1
				else if (computerPositions.Contains(1) && computerPositions.Contains(4) && results[7] == MarkType.Free)
				{
					position = 7;
					button = Button1_2;
				}
				else if (computerPositions.Contains(1) && computerPositions.Contains(7) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (computerPositions.Contains(7) && computerPositions.Contains(4) && results[1] == MarkType.Free)
				{
					position = 1;
					button = Button1_0;
				}
				// Column 2
				else if (computerPositions.Contains(2) && computerPositions.Contains(5) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (computerPositions.Contains(2) && computerPositions.Contains(8) && results[5] == MarkType.Free)
				{
					position = 5;
					button = Button2_1;
				}
				else if (computerPositions.Contains(8) && computerPositions.Contains(5) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				#endregion
				#region O Diagonal Takes
				// Top Left to Bottom Right
				else if (computerPositions.Contains(0) && computerPositions.Contains(4) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (computerPositions.Contains(4) && computerPositions.Contains(8) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				else if (computerPositions.Contains(0) && computerPositions.Contains(8) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				// Bottom Left to Top Right
				else if (computerPositions.Contains(2) && computerPositions.Contains(4) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				else if (computerPositions.Contains(2) && computerPositions.Contains(6) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (computerPositions.Contains(6) && computerPositions.Contains(4) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				#endregion
				#region X Horizontal Blocks
				// Row 0
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(1) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				else if (firstPlayerPositions.Contains(1) && firstPlayerPositions.Contains(2) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(2) && results[1] == MarkType.Free)
				{
					position = 1;
					button = Button1_0;
				}
				//Row 1
				else if (firstPlayerPositions.Contains(3) && firstPlayerPositions.Contains(4) && results[5] == MarkType.Free)
				{
					position = 5;
					button = Button2_1;
				}
				else if (firstPlayerPositions.Contains(3) && firstPlayerPositions.Contains(5) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (firstPlayerPositions.Contains(4) && firstPlayerPositions.Contains(5) && results[3] == MarkType.Free)
				{
					position = 3;
					button = Button0_1;
				}
				// Row 2
				else if (firstPlayerPositions.Contains(6) && firstPlayerPositions.Contains(7) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (firstPlayerPositions.Contains(6) && firstPlayerPositions.Contains(8) && results[7] == MarkType.Free)
				{
					position = 7;
					button = Button1_2;
				}
				else if (firstPlayerPositions.Contains(8) && firstPlayerPositions.Contains(7) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				#endregion
				#region X Vertical Blocks
				// Column 0
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(3) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(6) && results[3] == MarkType.Free)
				{
					position = 3;
					button = Button0_1;
				}
				else if (firstPlayerPositions.Contains(3) && firstPlayerPositions.Contains(6) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				// Column 1
				else if (firstPlayerPositions.Contains(1) && firstPlayerPositions.Contains(4) && results[7] == MarkType.Free)
				{
					position = 7;
					button = Button1_2;
				}
				else if (firstPlayerPositions.Contains(1) && firstPlayerPositions.Contains(7) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (firstPlayerPositions.Contains(7) && firstPlayerPositions.Contains(4) && results[1] == MarkType.Free)
				{
					position = 1;
					button = Button1_0;
				}
				// Column 2
				else if (firstPlayerPositions.Contains(2) && firstPlayerPositions.Contains(5) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (firstPlayerPositions.Contains(2) && firstPlayerPositions.Contains(8) && results[5] == MarkType.Free)
				{
					position = 5;
					button = Button2_1;
				}
				else if (firstPlayerPositions.Contains(8) && firstPlayerPositions.Contains(5) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				#endregion
				#region X Diagonal Blocks
				// Top Left to Bottom Right
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(4) && results[8] == MarkType.Free)
				{
					position = 8;
					button = Button2_2;
				}
				else if (firstPlayerPositions.Contains(4) && firstPlayerPositions.Contains(8) && results[0] == MarkType.Free)
				{
					position = 0;
					button = Button0_0;
				}
				else if (firstPlayerPositions.Contains(0) && firstPlayerPositions.Contains(8) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				// Bottom Left to Top Right
				else if (firstPlayerPositions.Contains(2) && firstPlayerPositions.Contains(4) && results[6] == MarkType.Free)
				{
					position = 6;
					button = Button0_2;
				}
				else if (firstPlayerPositions.Contains(2) && firstPlayerPositions.Contains(6) && results[4] == MarkType.Free)
				{
					position = 4;
					button = Button1_1;
				}
				else if (firstPlayerPositions.Contains(6) && firstPlayerPositions.Contains(4) && results[2] == MarkType.Free)
				{
					position = 2;
					button = Button2_0;
				}
				#endregion
				else
				{
					// Makes sure position is free
					while (results[position] != MarkType.Free)
					{
						position = random.Next(0, 8);
					}
					// Sets the button to position if it's free 
					switch (position)
					{
						case 0:
							button = Button0_0;
							break;
						case 1:
							button = Button1_0;
							break;
						case 2:
							button = Button2_0;
							break;
						case 3:
							button = Button0_1;
							break;
						case 4:
							button = Button1_1;
							break;
						case 5:
							button = Button2_1;
							break;
						case 6:
							button = Button0_2;
							break;
						case 7:
							button = Button1_2;
							break;
						case 8:
							button = Button2_2;
							break;
						default:
							button = new Button();
							break;
					}
				}
					// Mark the button/position
					results[position] = MarkType.Nought;
					button.Content = "O";
					button.Foreground = Brushes.Red;
					// Add new position to list 
					computerPositions.Add(position);
					// Check if bot won 
					CheckWinner();

					mPlayer1Turn = true; 
				}
			}
		}
	}