using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZombieShooter
{
    public partial class Form1 : Form
    {
        // list of game variables
        bool goup;
        bool godown;
        bool goleft;
        bool goright;
        string facing = "up"; // used to guide bullets
        double playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        int score = 0;
        bool gameOver = false;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (gameOver) return; // if game over is true, do nothing

            // if left key is pressed
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
                facing = "left";
                player.Image = Properties.Resources.left; // change player to left position
            }

            // if right key is pressed
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
                facing = "right";
                player.Image = Properties.Resources.right; // change player to right position
            }

            // if up key is pressed
            if (e.KeyCode == Keys.Up)
            {
                facing = "up";
                goup = true;
                player.Image = Properties.Resources.up; // change player to up position
            }

            // if down key is pressed
            if (e.KeyCode == Keys.Down)
            {
                facing = "down";
                godown = true;
                player.Image = Properties.Resources.down; // change player to down position
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (gameOver) return; // if game is over do nothing

            // if left key is up
            if (e.KeyCode == Keys.Left)
            {
                goleft = false; // change bool to false
            }

            // if right key is up
            if (e.KeyCode == Keys.Right)
            {
                goright = false; // change bool to false
            }

            // if up key is up
            if (e.KeyCode == Keys.Up)
            {
                goup = false; // change bool to false
            }

            // if down key is up
            if (e.KeyCode == Keys.Down)
            {
                godown = false; // change bool to false
            }

            // if space bar key is up and ammo is more than 0
            if (e.KeyCode == Keys.Space && ammo > 0)
            {
                ammo--; // reduce ammo by 1 from total number
                shoot(facing); // execute shoot function with facing string, bullet will move in same direction

                // if ammo is less than 1 
                if (ammo < 1)
                {
                    DropAmmo(); // add more to the screen
                }
            }
        }

        private void gameEngine(object sender, EventArgs e)
        {
            if (playerHealth > 1) // if player health is greater than 1
            {
                // assign the progess bar to player health int
                progressBar1.Value = Convert.ToInt32(playerHealth);
            }

            else
            {
                // if the player health is below 1
                player.Image = Properties.Resources.dead; // show the player dead
                timer1.Stop(); // stop the timer
                gameOver = true; // change game over bool to true
            }

            label1.Text = "   Ammo:  " + ammo; // show ammo amount
            label2.Text = "Kills: " + score; // show total kills

            // if player health is less than 20
            if (playerHealth < 20)
            {
                // change progress bar to red
                progressBar1.ForeColor = System.Drawing.Color.Red;
            }

            if (goleft && player.Left > 0)
            {
                // if moving left is true and player is 1 pixel more from left
                // move player to left
                player.Left -= speed;
            }

            if (goright && player.Left + player.Width < 930)
            {
                // if moving right is true & player width/left is less than 930 pixels
                // move player to the right
                player.Left += speed;
            }

            if (goup && player.Top > 60)
            {
                // if moving top is true and player is 60 pixels more from top
                // move player to top
                player.Top -= speed;
            }

            if (godown && player.Top + player.Height < 700)
            {
                player.Top += speed;
                // if moving down is true and player top + player height is less than 700 pixels
                // move player down
            }

            // run the loop below
            foreach (Control x in this.Controls)
            {
                // if x is picture box and x has ammo tag

                if (x is PictureBox && x.Tag == "ammo")
                {
                    // check is X hitting player picture box

                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                    {
                        // once the player picks up ammo

                        // remove the ammo box pic
                        this.Controls.Remove(((PictureBox)x));

                        ((PictureBox)x).Dispose(); // dispose pic box from program
                        ammo += 5; // add 5 ammo to int
                    }

                }
            
                // if the bullets hit the borders
                // if x is a pic box and x has the bullet tag

                if (x is PictureBox && x.Tag == "bullet")
                {
                    // if bullet is less then 1 pixel to the left
                    // if the bullet is more than 930 pixels to the right
                    // if the bullet is 10 pixels from the top
                    // if the bullet is 700 pixels to the bottom

                    if (((PictureBox)x).Left < 1 || ((PictureBox)x).Left > 930 || ((PictureBox)x).Top < 10 ||
                                ((PictureBox)x).Top > 700)
                    {
                        // remove bullet from display
                        this.Controls.Remove(((PictureBox)x));
                        // dispose bullet from program
                        ((PictureBox)x).Dispose();
                    }
                }

                // if the player hits a zombie
                if (x is PictureBox && x.Tag == "zombie")
                {
                    // check boundaries between player and zombie
                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                    {
                        // if zombie hits player, decrease health by 1
                        playerHealth -= 1;
                    }
                    
                    // move zombie towards player
                    if (((PictureBox)x).Left > player.Left)
                    {
                        // move zombie towards left of player
                        ((PictureBox)x).Left -= zombieSpeed;
                        // change zombie image to left
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }

                    if (((PictureBox)x).Top > player.Top)
                    {
                        // move zombie upwards towards player top
                        ((PictureBox)x).Top -= zombieSpeed;
                        // change zombie image to top
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }

                    if (((PictureBox)x).Left < player.Left)
                    {
                        // move zombie towards right side of player
                        ((PictureBox)x).Left += zombieSpeed;
                        // change zombie image to right
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }

                    if (((PictureBox)x).Top < player.Top)
                    {
                        // move zombie towards bottom of player
                        ((PictureBox)x).Top += zombieSpeed;
                        // change image to down zombie
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }
                }

                // second for loop, nested inside first one
                // bullet and zombie needs to be different than each other
                // then we can use that to determine if they hit each other

                foreach (Control j in this.Controls)
                {
                    // this is the selection that identifies the bullet and zombie
                    if ((j is PictureBox && j.Tag == "bullet") && (x is PictureBox && x.Tag == "zombie"))
                    {
                        // this if statement checks if bullet hits a zombie
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++; // increase the score by 1
                            this.Controls.Remove(j); // remove bullet from screen
                            j.Dispose(); // dispose bullet from program
                            this.Controls.Remove(x); // remove zombie from screen
                            x.Dispose(); // this will dispose zombie from program
                            makeZombies(); // this function will add more zombies to the screen
                        }
                    }
                }
            }
        }

        // this functions adds ammo to the screen
        private void DropAmmo()
        {
            // new instance of pic box
            PictureBox ammo = new PictureBox();
            // assign ammo image to pic box
            ammo.Image = Properties.Resources.ammo_Image;
            // set size to auto
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            // set location to rnd left
            ammo.Left = rnd.Next(10, 890);
            // set location to rnd top
            ammo.Top = rnd.Next(50, 600);
            // set tag to ammo
            ammo.Tag = "ammo";
            // add ammo pic box to screen
            this.Controls.Add(ammo);
            // bring ammo to front
            ammo.BringToFront();
            // bring player to front
            player.BringToFront();
        }

        private void shoot(string direct)
        {
            // this function makes new bullets
            // new instance of bullet class
            bullet shoot = new bullet();
            // assign direction to bullet
            shoot.direction = direct;
            // place the bullet to left half of player
            shoot.bulletLeft = player.Left + (player.Width / 2);
            // place the bullet on top half of player
            shoot.bulletTop = player.Top + (player.Height / 2);
            // run the make bullet function from bullet class
            shoot.mkBullet(this);
        }

        private void makeZombies()
        {
            // this function is called to make zombies in game
            PictureBox zombie = new PictureBox();
            // add tag to call it zombie
            zombie.Tag = "zombie";
            // set default to zdown
            zombie.Image = Properties.Resources.zdown;
            // generate number between 0 and 900 and assign to new zombie left
            zombie.Left = rnd.Next(0, 900);
            // generate number between 0 and 800 and assign to new zombie top
            zombie.Top = rnd.Next(0, 800);
            // set pic box to autosize
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            // add pic box to screen
            this.Controls.Add(zombie);
            // bring player to front
            player.BringToFront();
        }
    }
}
