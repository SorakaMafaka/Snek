using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snek
{
    public partial class Form1 : Form
    {

        private List<Square> Snake = new List<Square>();
        private Square food = new Square();
        public Form1()
        {
            InitializeComponent();

            new Global();

            gameTimer.Interval = 1500 / Global.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;
            //reset to default
            new Global();

            //create new player
            Snake.Clear();
            Square head = new Square();
            head.Y = 5;
            head.X = 10;
            Snake.Add(head);

            lblScore.Text = Global.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            int maxXPos = gameCanvas.Size.Width / Global.Width;
            int maxYPos = gameCanvas.Size.Height / Global.Height;

            Random random = new Random();
            food = new Square();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);

        }

        private void UpdateScreen(Object sender, EventArgs e)
        {
            if(Global.GameOver)
            {
                if(Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Global.direction != Direction.Left)
                { Global.direction = Direction.Right; }
                else if (Input.KeyPressed(Keys.Left) && Global.direction != Direction.Right)
                { Global.direction = Direction.Left; }
                else if (Input.KeyPressed(Keys.Up) && Global.direction != Direction.Down)
                { Global.direction = Direction.Up; }
                else if (Input.KeyPressed(Keys.Down) && Global.direction != Direction.Up)
                { Global.direction = Direction.Down; }

                MovePlayer();
            }

            gameCanvas.Invalidate();

        }

        private void gameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Global.GameOver)
            {
                
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;
                    else
                        snakeColour = Brushes.Green;

                    //draw snek
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Global.Width,
                                      Snake[i].Y * Global.Height,
                                      Global.Width, Global.Height));

                    //draw food
                    canvas.FillEllipse(Brushes.Red,
                                       new Rectangle(food.X * Global.Width,
                                       food.Y * Global.Height, Global.Width, Global.Height));
                }
            }
            else
            {
                string gameOver = "Game Over \nYour final score is: " + Global.Score + "\nPress Enter to try again!";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i>=0; i--)
            {
                //move body
                if(i == 0)
                {
                    switch(Global.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    //Get max x and y pos
                    int maxXPos = gameCanvas.Size.Width / Global.Width;
                    int maxYPos = gameCanvas.Size.Height / Global.Height;

                    //detect borders
                    if(Snake[i].X <0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    //detect body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //detect food
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    //move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            Square square = new Square
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(square);

            //update score
            Global.Score += Global.Points;
            lblScore.Text = Global.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Global.GameOver = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
        

        private void gameTimer_Tick(object sender, EventArgs e)
        {

        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }

        private void gameCanvas_Click(object sender, EventArgs e)
        {

        }
    }
}
