using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>(); // create an array list for the snake
        private Circle food = new Circle(); // creating an instance of the circle food

        public Form1()
        {
            InitializeComponent();

            new Settings(); // create a link to the settings class to this form

            gameTimer.Interval = 1000 / Settings.Speed; // change the game time to settings speed
            gameTimer.Tick += updateScreen; // link updateScreen function to timer
            gameTimer.Start(); // start the timer
            // start the game
            startGame();
        }

        private void updateScreen(object sender, EventArgs e)
        {
            // update screen function, each clock tick will run this function
            if (Settings.GameOver == true)
            {
                // if the game over is true and player presses enter
                // start the game again
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                // if the game is not over then the following commands will be executed
                if (Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }

                movePlayer(); // move player function
            }

            pbCanvas.Invalidate(); // refresh the picture box and update graphics
        }

        private void movePlayer()
        {
            // loop for the snake head and parts
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                // if the snake head is active
                if (i == 0)
                {
                    // move rest of the body according to which way the head is moving
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }

                    // do not allow snake to leave canvas
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYpos = pbCanvas.Size.Height / Settings.Height;

                    if (
                        Snake[i].X < 0 || Snake[i].Y < 0 ||
                        Snake[i].X > maxXpos || Snake[i].Y > maxYpos
                       )
                    {
                        // end the game if snake reaches edge of screen
                        die();
                    }

                    // detect collion with body
                    // this loop will check if snake has had collision with other body parts
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            // snake dies
                            die();
                        }
                    }

                    // detect food collision
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        // eat the food
                        eat();
                    }
                }
                else
                {
                    // if there are no collisions, then continue moving snake
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // this is the key down event, return the keys
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // this is the key up event, return the keys
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            // this function handles the snake and moving parts
            Graphics canvas = e.Graphics; // create a new graphics class

            if (Settings.GameOver == false)
            {
                // if the game is not over, do the following
                Brush snakeColour; // create a new brush class

                // run a loop to check the snake parts
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        // colour the head of the snake black
                        snakeColour = Brushes.Black;
                    }
                    else
                    {
                        // colour the body green
                        snakeColour = Brushes.Green;
                    }
                    // draw snake body and head
                    canvas.FillEllipse(snakeColour,
                                        new Rectangle(
                                            Snake[i].X * Settings.Width,
                                            Snake[i].Y * Settings.Height,
                                            Settings.Width, Settings.Height
                                            ));
                    // draw food
                    canvas.FillEllipse(Brushes.Red,
                                        new Rectangle(
                                            food.X * Settings.Width,
                                            food.Y * Settings.Height,
                                            Settings.Width, Settings.Height
                                            ));
                }
            }
            else
            {
                // if the game is over, display game over text and make score label visible on screen
                string gameOver = "Game Over \n" + "Final Score is " + Settings.Score + "\n Press enter to Restart \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }

        private void startGame()
        {
            // start game function
            label3.Visible = false; // make label invisible
            new Settings(); // create new instance of settings
            Snake.Clear(); // clear the snake parts
            Circle head = new Circle { X = 10, Y = 5 }; // create new head for snake
            Snake.Add(head); // add the head to snake array
            label2.Text = Settings.Score.ToString(); // show the score on label 2
            generateFood(); // generate food on screen
        }

        // generate food on screen function
        private void generateFood()
        {
            // create a max x position int with half the size of play area
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            // create a max y position int with half the size of play area
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            // create a random class
            Random rnd = new Random();
            // create new food with random x and y points
            food = new Circle { X = rnd.Next(0, maxXpos), Y = rnd.Next(0, maxYpos) };
        }

        // when snake gets food
        private void eat()
        {
            // add part to body when food is eaten
            Circle Body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            // add the part to snake array
            Snake.Add(Body);
            // increase score
            Settings.Score += Settings.Points;
            // show score on label
            label2.Text = Settings.Score.ToString();
            // create food on screen
            generateFood();
        }

        // when snake dies
        private void die()
        {
            // change game over bool to true
            Settings.GameOver = true;
        }
    }
}
