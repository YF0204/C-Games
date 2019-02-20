using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        // global variables
        bool goleft;
        bool goright;
        int speed = 5;
        int score = 0;
        bool isPressed;
        int totalEnemies = 12;
        int playerSpeed = 6;

        public Form1()
        {
            InitializeComponent();
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // when player releases button, set to false
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }

            // stop player from shooting infinite bullets
            if (isPressed)
            {
                isPressed = false;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // when player presses button, set to true
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }

            if (e.KeyCode == Keys.Space && !isPressed)
            {
                isPressed = true;
                makeBullet();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //check whether goleft or go right is true, so player can move
            if (goleft)
            {
                player.Left -= playerSpeed;
            }

            else if (goright)
            {
                player.Left += playerSpeed;
            }
            
            // enemies moving on screen
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "aliens")
                {
                    if 
                    (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver();
                        MessageBox.Show("Game Over");
                    }

                    ((PictureBox)x).Left += speed;

                    if (((PictureBox)x).Left > 720)
                    {
                        ((PictureBox)x).Top +=
                            ((PictureBox)x).Height + 10;

                        ((PictureBox)x).Left = -50;
                    }
                }
            }

            // animating the bullets and removing them when they go off screen
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Tag == "bullet")
                {
                    y.Top -= 20;

                    if (((PictureBox)y).Top < this.Height - 490)
                    {
                        this.Controls.Remove(y);
                    }
                }
            }

            // bullet and enemy collisions
            foreach (Control i in this.Controls)
            {
                foreach (Control j in this.Controls)
                {
                    if (i is PictureBox && i.Tag == "aliens")
                    {
                        if (j is PictureBox && j.Tag == "bullet")
                        {
                            if (i.Bounds.IntersectsWith(j.Bounds))
                            {
                                score++;
                                this.Controls.Remove(i);
                                this.Controls.Remove(j);
                            }
                        }
                    }
                }
            }

            // show score
            label1.Text = "Score : " + score;
            
            if (score > totalEnemies - 1)
            {
                gameOver();
                MessageBox.Show("You Won!");
            }  
        }

        // function to create tank bullets
        private void makeBullet()
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet;
            bullet.Size = new Size(5, 20);
            bullet.Tag = "bullet";
            bullet.Left = player.Left + player.Width / 2;
            bullet.Top = player.Top - 20;
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        // function when game over occurs
        private void gameOver()
        {
            timer1.Stop();
            label1.Text += " Game Over";
        }
    }
}
