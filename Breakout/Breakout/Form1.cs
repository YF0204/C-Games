using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Breakout
{
    
    public partial class Form1 : Form
    {
        bool goRight;
        bool goLeft;
        int speed = 20;

        int ballx = 5;
        int bally = 5;

        int score = 0;

        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "block")
                {
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    x.BackColor = randomColor;
                }
            }
        }

        
        private void keyisdown(object sender, KeyEventArgs e)
        {
            // if the player presses the left key
            if (e.KeyCode == Keys.Left && player.Left > 0)
            {
                goLeft = true;
            }

            // if the player presses the right key
            if (e.KeyCode == Keys.Right && player.Left + player.Width < 920)
            {
                goRight = true;
            }
        }

        
        private void keyisup(object sender, KeyEventArgs e)
        {
            // if the left key is up, set to false
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            
            // if the right key is up, set to false
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ball.Left += ballx;
            ball.Top += bally;

            label1.Text = "Score " + score;

            if (goLeft) { player.Left -= speed; } // move left
            if (goRight) { player.Left += speed; } // move right

            if (player.Left < 1)
            {
                goLeft = false; // stop the player from going off the screen
            }
            else if (player.Left + player.Width > 920)
            {
                goRight = false;
            }

            if (ball.Left + ball.Width > ClientSize.Width || ball.Left < 0)
            {
                ballx = -ballx; // this will bounce the ball from the left or right side
            }

            if (ball.Top < 0 || ball.Bounds.IntersectsWith(player.Bounds))
            {
                bally = -bally; // this will bounce the ball from the player paddle and top of the screen
            }

            if (ball.Top + ball.Height > ClientSize.Height)
            {
                gameOver(); // if the ball reaches the bottom of the screen
            }

            // remove each block that is hit and increase the score
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "block")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        bally = -bally;
                        score++;
                    }
                }
            }

            // if all blocks are hit, the player wins
            if (score > 34)
            {
                gameOver();
                MessageBox.Show("You Win!");
            }
        }

        private void gameOver()
        {
            // stop the timer
            timer1.Stop();
        }
    }
}
