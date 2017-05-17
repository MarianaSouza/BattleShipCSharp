using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipCSharp
{
    public partial class Form1 : Form
    {
        //int index;
        PictureBox[] Person = new PictureBox[100];
        PictureBox[] Computer = new PictureBox[100];
        char[] PersonArray = new char[100];
        char[] ComputerArray = new char[100];
        Random randomNumber = new Random();
        int ComputerHitPerson = 0;
        int PersonHitComputer = 0;

        int HitsToWin = 6;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnInitialize_Click(sender, e);
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            int PersonX, PersonY, ComputerX, ComputerY;
            PersonX = 20;
            PersonY = 100;
            ComputerX = 610;
            ComputerY = 100;
            InitGrid(PersonX, PersonY, Person, PersonArray);
            InitGrid(ComputerX, ComputerY, Computer, ComputerArray);

            var pos1 = getNextRandom(randomNumber);
            RenderHorizontalShip(getNextRandom(randomNumber), Person, PersonArray, displayImage: true);
            pos1 = getNextRandom(randomNumber);
            RenderHorizontalShip(pos1, Person, PersonArray, displayImage: true);
            pos1 = getNextRandom(randomNumber);
            RenderVerticalShip(pos1, Person, PersonArray, displayImage: true);

            pos1 = getNextRandom(randomNumber);
            RenderHorizontalShip(pos1, Computer, ComputerArray, displayImage: false);
            pos1 = getNextRandom(randomNumber);
            RenderVerticalShip(pos1, Computer, ComputerArray, displayImage: false);
            pos1 = getNextRandom(randomNumber);
            RenderVerticalShip(pos1, Computer, ComputerArray, displayImage: false);

            btnInitialize.Visible = false;
        }

        private void InitGrid(int x, int y, PictureBox[] player, char[] arrayBehind)
        {
            var initialX = x;
            for (int i = 0; i < 100; i++)
            {
                arrayBehind[i] = 'W';

                player[i] = new PictureBox();
                player[i].Image = new Bitmap(BattleShipCSharp.Properties.Resources.Water);
                player[i].Location = new Point(x, y);
                player[i].Size = new Size(50, 50);
                player[i].Tag = i;

                Controls.Add(player[i]);

                if (IsLastPictureInRow(i)) //is true
                {
                    x = initialX;
                    y = y + 50;
                }
                else
                {
                    x = x + 50;
                }
            }
        }

        private bool IsLastPictureInRow(int i)
        {
            var rest = (i + 1) % 10;
            if (rest == 0)
            {
                return true;
            }
            return false;
        }
      
        private void RenderVerticalShip(int pos1, PictureBox[] player, char[] arrayBehind, bool displayImage)
        {
            var pos2 = 0;
            var image1 = new Bitmap(Properties.Resources.Ship_1);
            var image2 = new Bitmap(Properties.Resources.Ship_2);

            if (pos1 > 89)
            {
                pos2 = pos1 - 10;
                image1 = new Bitmap(Properties.Resources.Ship_2);
                image2 = new Bitmap(Properties.Resources.Ship_1);
            }
            else
            {
                pos2 = pos1 + 10;

            }

            if (arrayBehind[pos1] == 'S' || arrayBehind[pos2] == 'S')
            {
                RenderVerticalShip(getNextRandom(randomNumber), player, arrayBehind, displayImage);
                return;
            }

            if (displayImage)
            {
                player[pos1].Image = image1;
                player[pos1].Image.RotateFlip((RotateFlipType.Rotate90FlipX));
                player[pos2].Image = image2;
                player[pos2].Image.RotateFlip((RotateFlipType.Rotate90FlipX));
            }

            arrayBehind[pos1] = 'S';
            arrayBehind[pos2] = 'S';
        }

        private void RenderHorizontalShip(int pos1, PictureBox[] player, char[] arrayBehind, bool displayImage)
        {
            var pos2 = 0;

            if (IsLastPictureInRow(pos1)) //if ship is the last in the line
            {
                pos1 = pos1 - 1;
            }
            pos2 = pos1 + 1;

            if (arrayBehind[pos1] == 'S' || arrayBehind[pos2] == 'S')
            {
                RenderHorizontalShip(getNextRandom(randomNumber), player, arrayBehind, displayImage);
                return;
            }

            if (displayImage)
            {
                player[pos1].Image = new Bitmap(Properties.Resources.Ship_1);
                player[pos2].Image = new Bitmap(Properties.Resources.Ship_2);
            }

            arrayBehind[pos1] = 'S';
            arrayBehind[pos2] = 'S';
        }

        private int getNextRandom(Random rand)
        {
            var ret = Convert.ToInt16(Math.Sqrt((double)rand.Next() / int.MaxValue) * 99);
            return ret;
        }

        
        private void OnComputerShoot(object sender, EventArgs e)
        {
            var which = (PictureBox)sender;

            var clicked = Convert.ToInt16(which.Tag);
            if (PersonArray[clicked] == 'S')
            {
                PersonArray[clicked] = 'H';
                which.Image = new Bitmap(BattleShipCSharp.Properties.Resources.Hit);
                PlaySound(Properties.Resources.Bomb_Sound);
                ComputerHitPerson++;
            }
            else
            {
                PersonArray[clicked] = 'M';
                which.Image = new Bitmap(BattleShipCSharp.Properties.Resources.Miss);
                PlaySound(Properties.Resources.Miss_Sound);
            }
            GameOver();
        }


        private void OnPersonShoot(object sender, EventArgs e)
        {
            if (GameOver())
            {
                return;
            }
            var which = (PictureBox)sender;

            var clicked = Convert.ToInt16(which.Tag);

            if (ComputerArray[clicked] == 'H' || ComputerArray[clicked] == 'M')
            {
                MessageBox.Show("This location was already shoot");
                return;
            }

            if (ComputerArray[clicked] == 'S')
            {
                ComputerArray[clicked] = 'H';
                which.Image = new Bitmap(BattleShipCSharp.Properties.Resources.Hit);
                PlaySound(Properties.Resources.Bomb_Sound);
                PersonHitComputer++;
            }
            else
            {
                ComputerArray[clicked] = 'M';
                which.Image = new Bitmap(BattleShipCSharp.Properties.Resources.Miss);
                PlaySound(Properties.Resources.Miss_Sound);
            }
            if (GameOver())
            {
                return;
            }
            ComputerShoot(e);
        }

        private void ComputerShoot(EventArgs e)
        {
            if (GameOver())
            {
                return;
            }
            var shoot = getNextRandom(randomNumber);
            if (PersonArray[shoot] == 'H' || PersonArray[shoot] == 'M')
            {
                ComputerShoot(e);
                return;
            }
            OnComputerShoot(Person[shoot], e);
        }

        private static void PlaySound(System.IO.Stream sound)
        {
            var systemSound = new System.Media.SoundPlayer(sound);
            systemSound.Play();
        }

        private bool GameOver()
        {
            if (ComputerHitPerson == HitsToWin)
            {
                MessageBox.Show("The game is over. Computer won.");
                return true;
            }
            if (PersonHitComputer == HitsToWin)
            {
                MessageBox.Show("The game is over. Person won.");
                return true;
            }
            return false;
        }

        private void btnShoot_Click(object sender, EventArgs e)
        {
            var pos = Convert.ToInt16(txtX.Text + "" + txtY.Text);
            OnPersonShoot(Computer[pos], e);
        }
    }
}
