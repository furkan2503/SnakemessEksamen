using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace SnakeMess
{
	class Point
	{
		public const string Ok = "Ok";

		public int X; public int Y;
		public Point(int x = 0, int y = 0) { X = x; Y = y; }
		public Point(Point input) { X = input.X; Y = input.Y; }
	}

	class Setting
	{
		public int boardW;
		public int boardH;
		public bool gg;
		public bool pause;
		public bool inUse;
		public int gameStart;

		public Setting()
		{

			this.gg = false;
			this.pause = false;
			this.inUse = false;
			this.boardW = Console.WindowWidth;
			this.boardH = Console.WindowHeight - 5;
			this.gameStart = 0;
		}

		//First page berfore starting the game
		public int gameContinue()
		{

			Console.SetCursorPosition(50, 15);
			Console.WriteLine("Welcome to this boring game");
			Console.SetCursorPosition(50, 16);
			Console.WriteLine("|Press P to play the game|");
			Console.SetCursorPosition(50, 17);
			Console.WriteLine("|Press Q to quit the game|");

			ConsoleKeyInfo info = Console.ReadKey(true);
			if (info.Key == ConsoleKey.P)
				this.gameStart = 1;
			else if (info.Key == ConsoleKey.Q)
				this.gameStart = -1;
			else
			{
				Console.SetCursorPosition(60, 33);
				Console.WriteLine("Please Select Correct option");
				this.gameStart = 0;
			}

			return this.gameStart;
		}
	}

	class thisMain
	{
		public short newDir = 2; // 0 = up, 1 = right, 2 = down, 3 = left
		public short last;
		public Random rng;
		public Point app;
		public List<Point> snake;
		public Setting setting;
		public Stopwatch t;
		public int score;


		public thisMain()
		{
			this.newDir = 2; // 0 = up, 1 = right, 2 = down, 3 = left
			this.last = newDir;
			this.rng = new Random();
			this.app = new Point();
			this.snake = new List<Point>();
			this.setting = new Setting();
			this.snake.Add(new Point(10, 10)); this.snake.Add(new Point(10, 10)); this.snake.Add(new Point(10, 10)); this.snake.Add(new Point(10, 10));
			this.t = new Stopwatch();
			this.score = 0;



		}

		public void snakeIntilize()
		{
			while (true)
			{
				this.app.X = this.rng.Next(0, this.setting.boardW); this.app.Y = this.rng.Next(0, this.setting.boardH);
				bool spot = true;
				foreach (Point i in snake)
					if (i.X == app.X && i.Y == app.Y)
					{
						spot = false;
						break;
					}
				if (spot)
				{
					Console.ForegroundColor = ConsoleColor.Green; Console.SetCursorPosition(app.X, app.Y); Console.Write("$");
					break;
				}

			}
		}
		public void checkDirection()
		{
			ConsoleKeyInfo cki = Console.ReadKey(true);
			if (cki.Key == ConsoleKey.Escape)
				this.setting.gg = true;
			else if (cki.Key == ConsoleKey.Spacebar)
				this.setting.pause = !this.setting.pause;
			else if (cki.Key == ConsoleKey.UpArrow && this.last != 2)
				this.newDir = 0;
			else if (cki.Key == ConsoleKey.RightArrow && this.last != 3)
				this.newDir = 1;
			else if (cki.Key == ConsoleKey.DownArrow && this.last != 0)
				this.newDir = 2;
			else if (cki.Key == ConsoleKey.LeftArrow && this.last != 1)
				this.newDir = 3;
			else if (cki.Key == ConsoleKey.Q)
			{
				Console.Clear();
				Environment.Exit(0);

			}

		}


		public void directMove()
		{

			Point tail = new Point(this.snake.First());
			Point head = new Point(this.snake.Last());
			Point newH = new Point(head);
			switch (this.newDir)
			{
				case 0:
					newH.Y -= 1;
					break;
				case 1:
					newH.X += 1;
					break;
				case 2:
					newH.Y += 1;
					break;
				default:
					newH.X -= 1;
					break;
			}
			if (newH.X < 0 || newH.X >= this.setting.boardW)
			{
				this.setting.gg = true;
				gameOver();

			}
			else if (newH.Y < 0 || newH.Y >= this.setting.boardH)
			{
				this.setting.gg = true;
				gameOver();
			}

			if (newH.X == app.X && newH.Y == app.Y)
			{
				if (this.snake.Count + 1 >= this.setting.boardW * this.setting.boardH)
				{
					// No more room to place apples - game over.
					this.setting.gg = true;
					gameOver();


				}
				else
				{
					while (true)
					{
						app.X = this.rng.Next(0, this.setting.boardW); app.Y = this.rng.Next(0, this.setting.boardH);
						bool found = true;
						foreach (Point i in this.snake)
							if (i.X == app.X && i.Y == app.Y)
							{
								found = false;
								break;
							}
						if (found)
						{
							this.setting.inUse = true;
							break;
						}
					}
				}
			}
			if (!this.setting.inUse)
			{
				snake.RemoveAt(0);
				foreach (Point x in snake)
					if (x.X == newH.X && x.Y == newH.Y)
					{
						// Death by accidental self-cannibalism.
						this.setting.gg = true;
						gameOver();

						break;
					}
			}
			if (!this.setting.gg)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.SetCursorPosition(head.X, head.Y); Console.Write("0");
				if (!this.setting.inUse)
				{
					Console.SetCursorPosition(tail.X, tail.Y); Console.Write(" ");
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green; Console.SetCursorPosition(app.X, app.Y); Console.Write("$");
					this.score++;
					this.setting.inUse = false;
				}
				this.snake.Add(newH);
				Console.ForegroundColor = ConsoleColor.Yellow; Console.SetCursorPosition(newH.X, newH.Y); Console.Write("@");

				this.last = this.newDir;
			}





		}

		void gameOver()
		{
			if (this.setting.gg == true)
			{
				Console.SetCursorPosition(50, 14);
				Console.WriteLine("____________________________");
				Console.SetCursorPosition(50, 15);
				Console.WriteLine("         Game Over          ");

				Console.SetCursorPosition(50, 16);
				Console.WriteLine("       Your Score: " + this.score);

				Console.SetCursorPosition(50, 17);
				Console.WriteLine("    Please Q to Quite     ");
				Console.SetCursorPosition(50, 18);
				Console.WriteLine("____________________________");
				checkDirection();

			}
		}


	}

	public static class gamePropertyClass
	{

		public static void gameIntilize()
		{
			Console.CursorVisible = false;
			Console.Title = "Software design - SNAKE";
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(10, 10);

			Console.SetCursorPosition(0, Console.WindowHeight - 5);
			Console.WriteLine("________________________________________________________________________________________________________________________");

		}

	}



	class SnakeMess
	{
		public static void Main(string[] arguments)
		{
			gamePropertyClass.gameIntilize();
			thisMain SnakeObj = new thisMain();
			int option;
			option = SnakeObj.setting.gameContinue();
			Console.Clear();

			SnakeObj.t.Start();
			SnakeObj.snakeIntilize();
			Console.SetCursorPosition(0, Console.WindowHeight - 5);
			Console.WriteLine("________________________________________________________________________________________________________________________");
			Console.SetCursorPosition(30, Console.WindowHeight - 3);
			Console.WriteLine("Press Space To Pause,                Press Q to Quit the Game");
			Console.SetCursorPosition(30, Console.WindowHeight - 2);
			Console.WriteLine("             Use Arrow Keys to Move the Snake                ");
			while (!SnakeObj.setting.gg)
			{
				Console.SetCursorPosition(55, Console.WindowHeight - 4);
				Console.WriteLine("Score: " + SnakeObj.score);
				if (option == 1)
				{

					if (SnakeObj.t.ElapsedMilliseconds < 100)
						continue;
					if (Console.KeyAvailable)
					{
						SnakeObj.checkDirection();
					}
					if (!SnakeObj.setting.pause)
					{
						SnakeObj.directMove();
					}
					SnakeObj.t.Restart();
				}
				else if (option == -1)
				{
					Console.Clear();
					Environment.Exit(0);

				}
				else
				{
					option = SnakeObj.setting.gameContinue();
				}
			}
		}
	}
}
