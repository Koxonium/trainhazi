using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace trainú
{
    public partial class Form1 : Form
    {
        public Timer carSpawn;
        public Timer trainTimer;
        public Timer movement;
        public List<PictureBox> cars;
        public PictureBox train;
        public PictureBox barrier;
        public Button barrierButton;
        public bool barrierClosed = false;
        public int score = 0;
        public Random r;
        Label scoreLabel = new Label();

        public Form1()
        {
            InitializeComponent();
            this.Width = 1080;
            this.Height = 720;
            cars = new List<PictureBox>();
            r = new Random();

            
            barrier = new PictureBox
            {
                Size = new Size(10, 50),
                Location = new Point(380, 150),
                BackColor = Color.Gray
            };
            this.Controls.Add(barrier);

            
            barrierButton = new Button
            {
                Text = "STOP",
                Location = new Point(10, 10)
            };
            barrierButton.Click += BarrierButton_Click;
            this.Controls.Add(barrierButton);

            
            train = new PictureBox
            {
                Size = new Size(100, 30),
                Location = new Point(400, 0),
                BackColor = Color.Black
            };
            this.Controls.Add(train);


            carSpawn = new Timer();
            carSpawn.Interval = 1000;
            carSpawn.Tick += CarTimer_Tick;
            carSpawn.Start();

            Timer trainMove = new Timer();
            trainMove.Interval = 25;
            trainMove.Tick += (s, e) =>
            {
                train.Top += 4;
            };
            trainMove.Start();


            movement = new Timer();
            movement.Interval = 25;
            movement.Tick += MovementTimer_Tick;
            movement.Start();

            trainTimer = new Timer();
            trainTimer.Tick += TrainTimer_Tick;
            SetNewTrainInterval();
        }

        public void BarrierButton_Click(object sender, EventArgs e)
        {
            barrierClosed = !barrierClosed;
            barrier.BackColor = barrierClosed ? Color.Black : Color.Gray;
            if (!barrierClosed)
            {
                movement.Start();
            }
        }

        public void CarTimer_Tick(object sender, EventArgs e)
        {
            var car = new PictureBox
            {
                Size = new Size(40, 20),
                Location = new Point(-50, 150),
                BackColor = GetRandomColor()
            };
            cars.Add(car);
            this.Controls.Add(car);
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
        }

        public void MovementTimer_Tick(object sender, EventArgs e)
        {
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                var car = cars[i];

                if (barrierClosed && car.Right >= barrier.Left - 20)
                {
                    continue;
                }

                car.Left += 2;

                if (car.Right > this.ClientSize.Width)
                {
                    this.Controls.Remove(car);
                    cars.RemoveAt(i);
                    score++;
                    scoreLabel.Text = "Autó átjutott: " + score;
                    scoreLabel.Location = new Point(100, 40);
                    this.Controls.Add(scoreLabel);
                }
            }

            
            foreach (var car in cars)
            {
                if (train.Bounds.IntersectsWith(car.Bounds))
                {
                    carSpawn.Stop();
                    trainTimer.Stop();
                    movement.Stop();
                    MessageBox.Show("Game Over! Score: " + score);
                    return;
                }
            }
        }

        public void SetNewTrainInterval()
        {
            trainTimer.Interval = r.Next(10000, 30000);
            trainTimer.Start();
        }

        public void TrainTimer_Tick(object sender, EventArgs e)
        {
            if (train.Top < 0)
            {
                train.Top = -50;
            }

            if (train.Top > this.Height)
            {
                train.Top = -50;
                trainTimer.Stop();
                SetNewTrainInterval();
            }
        }
    }
}
