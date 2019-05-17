using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace Sumulacja
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        int Map_width = 0;
        int Map_height = 0;
        int numberOfStates;
        static int click_counter = 0;
        int rows = 0;
        int columns = 0;
        Grid Map;
        System.Timers.Timer aTimer;
        int BC = 0;
        Color[] colorTable;
        Random random = new Random();


        public Form1()
        {
            InitializeComponent();

        }

        void setColorTable()
        {
            numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
            colorTable = new Color[numberOfStates + 10];
            for (int i = 0; i < numberOfStates + 1; i++)
            {
                colorTable[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
        }

        private void DrawCells(Cell[,] Map)
        {
            GridPictureBox.Image = new Bitmap(GridPictureBox.Width, GridPictureBox.Height);
            graphics = Graphics.FromImage(this.GridPictureBox.Image);
            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Black, (float)0.0001);
            float x = 0, y = 0;

            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }

            Random random = new Random();
            int transition = 255 / numberOfStates;
            for (int i = 0; i < Map_height; i++)
            {

                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() != 0)
                    {
                        for (int k = 1; k < numberOfStates + 1; k++)
                        {
                            if (Map[i, j].getCellState() == k)
                            {
                                brush.Color = colorTable[k];
                            }
                        }
                        //brush.Color = Color.FromArgb(Map[i, j].getCellState() * transition / 2, Map[i, j].getCellState() * transition / 3, Map[i, j].getCellState() * transition / 4);
                    }
                    else
                    {
                        brush.Color = Color.LightPink;
                    }
                    graphics.FillRectangle(brush, x, y, RecWidth, RecHeight);
                    //graphics.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            aTimer.Enabled = false;
        }

        private void GridPictureBox_Click(object sender, EventArgs e)
        {
            float x;
            float y;

            MouseEventArgs clickOnPB = (MouseEventArgs)e;
            PointF coordinates = clickOnPB.Location;

            x = coordinates.X;
            y = coordinates.Y;

            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }

            int counterX = -1;
            int counterY = -1;
            while (x > 0)
            {
                x -= RecWidth;
                counterX++;
            }

            while (y > 0)
            {
                y -= RecWidth;
                counterY++;
            }


            if (click_counter <= numberOfStates)
            {
                Map.Map[counterY, counterX].setCellState(click_counter);
                DrawCells(Map.Map);
                click_counter++;
            }
        }

        private void DrawGrid_Click(object sender, EventArgs e)
        {
            setColorTable();
            click_counter = 0;
            GridPictureBox.Refresh();
            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki");
            }

            Map = new Grid(Map_width, Map_height, numberOfStates + 1);
            DrawCells(Map.Map);
        }

        private void OnTimedEventPeriodical(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalVonNeuman();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);
                }));
            }

            if (endOfSimulation())
            {
                aTimer.Enabled = false;
            }
        }

        private void OnTimedEventAbsorbing(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingVonNeuman();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);
                }));
            }
            if (endOfSimulation())
            {
                aTimer.Enabled = false;
            }
        }

        private void SimulationButton_Click(object sender, EventArgs e)
        {
            BC = bcComboBox.SelectedIndex;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            if (BC == 0)
            {
                aTimer.Elapsed += OnTimedEventPeriodical;
            }
            else if (BC == 1)
            {
                aTimer.Elapsed += OnTimedEventAbsorbing;
            }

            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        bool endOfSimulation()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map.Map[i, j].getCellState() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void JednorodnyButton_Click(object sender, EventArgs e)
        {
            setColorTable();
            Random rand = new Random();
            GridPictureBox.Refresh();
            click_counter = 0;
            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
                columns = int.Parse(columnTextBox.Text);
                rows = int.Parse(rowsTextBox.Text);
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki");
                return;
            }

            Map = new Grid(Map_width, Map_height, numberOfStates + 1);
            int deltaRows = Map_height / rows;
            int deltaColumns = Map_width / columns;
            if (deltaRows == 0)
            {
                deltaRows = 1;
            }
            if (deltaColumns == 0)
            {
                deltaRows = 1;
            }

            int cellCounter = 1;
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    if (cellCounter < numberOfStates + 1)
                    {
                        Map.Map[(i * deltaRows), (j * deltaColumns)].setCellState(cellCounter);
                        cellCounter++;
                    }
                }
            }

            DrawCells(Map.Map);
        }

        private void PromieniowyCheckBox_Click(object sender, EventArgs e)
        {
            setColorTable();
            click_counter = 0;
        }

        private void RandomStateButton_Click(object sender, EventArgs e)
        {
            setColorTable();
            click_counter = 0;
            Random rand = new Random();
            GridPictureBox.Refresh();

            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki");
                return;
            }

            int RandomCounter = 1;

            int indexI, indexJ;

            Map = new Grid(Map_width, Map_height, numberOfStates + 1);

            while (RandomCounter < numberOfStates + 1)
            {
                indexI = rand.Next(0, Map_height);
                indexJ = rand.Next(0, Map_width);
                if (Map.Map[indexI, indexJ].getCellState() == 0)
                {
                    Map.Map[indexI, indexJ].setCellState(RandomCounter);
                    RandomCounter++;
                }
                else
                {
                    indexI = rand.Next(0, Map_height);
                    indexJ = rand.Next(0, Map_width);
                }

            }
            DrawCells(Map.Map);
        }

    }
}

