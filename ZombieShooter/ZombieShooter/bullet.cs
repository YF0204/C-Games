using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ZombieShooter
{
    class bullet
    {
        // game variables
        public string direction;
        public int speed = 20;
        PictureBox Bullet = new PictureBox();
        Timer tm = new Timer();

        public int bulletLeft;
        public int bulletTop;
        // end of variables

        public void mkBullet(Form form)
        {
            // this function will add bullets to the game play
            // called from this class
            // set color to white
            Bullet.BackColor = System.Drawing.Color.White;
            // change size to 5 by 5 pixels
            Bullet.Size = new Size(5, 5);
            // set tag to bullet
            Bullet.Tag = "bullet";
            // set bullet left
            Bullet.Left = bulletLeft;
            // set bullet right
            Bullet.Top = bulletTop;
            // bring bullet to front of other objects
            Bullet.BringToFront();
            // add bullet to screen
            form.Controls.Add(Bullet);

            tm.Interval = speed; // set the timer interval to speed
            tm.Tick += new EventHandler(tm_tick); // assign timer with an event
            tm.Start(); // start the timer
        }

        public void tm_tick(object sender, EventArgs e)
        {
            // if direction equals to left
            if (direction == "left")
            {
                Bullet.Left -= speed; // move bullet towards left
            }
            // if direction equals right
            if (direction == "right")
            {
                Bullet.Left += speed; // move bullet towards right
            }
            // if direction is up
            if (direction == "up")
            {
                Bullet.Top -= speed; // move bullet towards top
            }
            // if direction is down
            if (direction == "down")
            {
                Bullet.Top += speed; // move bullet towards bottom
            }

            // if bullet is less than 16 pixels to the left or
            // if the bullet is more than 860 pixels to the right or
            // if the bullet is 10 pixels from the top or
            // if the bullet is 616 pixels to the bottom or
            // if any one of the conditions are met than the following will be executed

            if (Bullet.Left < 16 || Bullet.Left > 860 || Bullet.Top < 10 || Bullet.Top > 616)
            {
                // stop the timer
                tm.Stop();
                // dispose the timer event
                tm.Dispose();
                // dispose the bullet
                Bullet.Dispose();
                // nullify the timer
                tm = null;
                // nullify the bullet
                Bullet = null;
            }
        }
    }
}
