using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int currGen = 0;
        private Graphics graphics;
        private int resolve;
        private bool[,] field;
        private int rows;
        private int columns;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()// заполение первичных клеток
        {

            if (timer1.Enabled)
                return;
            currGen = 0;
            Text = $"Generation{currGen}";

            nudRes.Enabled = false;
            nudDens.Enabled = false;
            resolve = (int)nudRes.Value;
            rows = pictureBox1.Height/resolve;
            columns = pictureBox1.Width/resolve;
            field = new bool[columns, rows];

            Random rand = new Random();

            for(int x =0; x<columns; x++) 
            {
                for(int y = 0; y < rows; y++)
                {
                    field[x, y] = rand.Next((int)nudDens.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void NextGeneraton()
        {
            graphics.Clear(Color.Black);
            var newField = new bool[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighCount = CountNeighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighCount==3)
                        newField[x, y] = true;
                     else if(hasLife &&(neighCount<2 || neighCount> 3))
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x,y];

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Blue, x * resolve, y * resolve, resolve-1, resolve-1);
                }
            }
            field = newField; 
            pictureBox1.Refresh();
            Text = $"Generation{++currGen}";
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for(int i =-1; i<2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + columns) % columns ;
                    var row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row];

                    if (hasLife && !isSelfChecking)
                        count++;

                }
            }
            return count;
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudRes.Enabled = true;
            nudDens.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneraton();
        }
       

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if(e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolve;
                var y = e.Location.Y / resolve;
                var valPass = ValidateMousPos(x,y);
                if(valPass)
                field[x, y] = true; 
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolve;
                var y = e.Location.Y / resolve;
                 var valPass = ValidateMousPos(x,y);
                if(valPass)
                field[x, y] = true; 
            }

        }
        private bool ValidateMousPos(int x,int y)
        {
            return x >=0 && y >=0 && x<columns && y< rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation{currGen}";
        }
    }
}
