using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TankShooter
{
    public partial class Form1 : Form
    {
        //Variables
        private int Score ,Distance , Kills , i , CounterSpawnPlayerToStartLocation , GetScoreMore , MoveSaveLabel , outNum;
        private bool keyUp, keyDown, keyLeft, keyRight, keyShot, hard, FireEnemy1, FireEnemy2, FireEnemy3, GameOverCheck
            , keyCntrl, Start, Pause, keyDelete;
        private string ReadCharScore;
        Random rnd = new Random();

        
        //Constructor
        public Form1()
        {
            InitializeComponent();
            outNum = 0;
            ReadCharScore = "";
            WallLocation();
            if (File.Exists(Application.StartupPath + "\\Log"))
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\Log", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                lblScore.Text = $"Score : {sr.ReadToEnd()}";
                sr.Close();
                fs.Close();
                for(int i = 0; i < lblScore.Text.Length; i++)
                {
                    if(int.TryParse(lblScore.Text[i].ToString(), out outNum))
                    {
                        ReadCharScore += lblScore.Text[i].ToString();
                    }
                }
                Score = Convert.ToInt32(ReadCharScore);
            }
            else
            {
                Score = 0;
                lblScore.Text = $"Score : {Score}";
            }
            MoveSaveLabel = 0;
            GameOverCheck = false;
            keyCntrl = false;
            Kills = 0;
            Distance = 0;
            i = 0;
            CounterSpawnPlayerToStartLocation = 0;
            GetScoreMore = 0;
            hard = false;
            FireEnemy1 = false;
            FireEnemy2 = false;
            FireEnemy3 = false;
            Application.EnableVisualStyles();
        }

        
        //Update Function And Check This Function By "timer1"
        private new void Update()
        {
            label1.Text = $"Health : {progressBar1.Value * 10}";
            PlayerMove();
            Block(hard);
            Bullet();
            Enemy();
            ProgressValueChangedEvent();
            if (Pause == true)
                PauseGame();
            if (keyDelete == true)
                Delete();
            if (keyCntrl == true)
                timer5.Start();
        }

        
        //Bullets Of Tanks
        private void Bullet()
        {
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(Properties.Resources.Pistol_Sound_Effect__mp3cut_net___1_);
            
            if (keyShot == false && buPlayer.Visible == true)
                buPlayer.Left += 40;

            if (keyShot == true)
            {
                buPlayer.Location = new Point(playerTank.Location.X + 104, playerTank.Location.Y + 24);
                buPlayer.Enabled = true;
                buPlayer.Visible = true;

            }
            if (buPlayer.Left > playerTank.Location.X + 700)
            {
                if (buPlayer.Left == playerTank.Location.X + 144)
                    snd.Play();
                buPlayer.Enabled = false;
                buPlayer.Visible = false;
            }
            if (FireEnemy1 == true)
            {
                en1.Visible = true;
                en1.Left -= 10;
                if (en1.Left < enemy1.Location.X - 500)
                {
                    en1.Location = new Point(enemy1.Location.X - 20, enemy1.Location.Y + 15);
                    snd.Play();
                }
                if (buPlayer.Visible == true)
                    if (buPlayer.Bounds.IntersectsWith(en1.Bounds))
                    {
                        en1.Location = new Point(enemy1.Location.X - 20, enemy1.Location.Y + 15);
                        snd.Play();
                    }

            }
            if (FireEnemy2 == true)
            {
                en2.Visible = true;
                en2.Left -= 10;
                if (en2.Left < enemy2.Location.X - 500)
                {
                    en2.Location = new Point(enemy2.Location.X - 20, enemy2.Location.Y + 15);
                    snd.Play();
                }
                if (buPlayer.Visible == true)
                    if (buPlayer.Bounds.IntersectsWith(en2.Bounds))
                    {
                        en2.Location = new Point(enemy2.Location.X - 20, enemy2.Location.Y + 15);
                        snd.Play();
                    }
            }
            if (FireEnemy3 == true)
            {
                en3.Visible = true;
                en3.Left -= 10;
                if (en3.Left < enemy3.Location.X - 500)
                {
                    en3.Location = new Point(enemy3.Location.X - 20, enemy3.Location.Y + 15);
                    snd.Play();
                }
                if (buPlayer.Visible == true)
                    if (buPlayer.Bounds.IntersectsWith(en3.Bounds))
                    {
                        en3.Location = new Point(enemy3.Location.X - 20, enemy3.Location.Y + 15);
                        snd.Play();
                    }
            }


            foreach (Control y in Controls)
            {
                if (y is PictureBox && y.Tag == "buen")
                {
                    if (y.Name == "en1")
                    {
                        if (y.Location.X < enemy1.Location.X - 700)
                        {
                            en1.Visible = false;
                            FireEnemy1 = false;
                        }

                    }
                    if (y.Name == "en2")
                    {
                        if (y.Location.X < enemy2.Location.X - 700)
                        {
                            en2.Visible = false;
                            FireEnemy2 = false;
                        }
                    }
                    if (y.Name == "en3")
                    {
                        if (y.Location.X < enemy3.Location.X - 700)
                        {
                            FireEnemy3 = false;
                            en1.Visible = false;
                        }
                    }
                    if (FireEnemy1 == false)
                    {
                        FireEnemy1 = true;
                        
                    }
                    if (FireEnemy2 == false)
                    {
                        FireEnemy2 = true;
                        
                    }
                    if (FireEnemy3 == false)
                    {
                        FireEnemy3 = true;
                        
                    }
                }
            }
            foreach (Control y in Controls)
            {
                int X = rnd.Next(Width, Width + 10);
                int Y = rnd.Next(pictureBox15.Location.Y + pictureBox15.Height + 10, pictureBox16.Location.Y);
                if (y is PictureBox && y.Tag == "enemy1" || y.Tag == "enemy2" || y.Tag == "enemy3")
                {
                    if (y.Name == "enemy1")
                    {
                        if (buPlayer.Bounds.IntersectsWith(y.Bounds))
                        {
                            y.Location = new Point(X, Y);
                            Kills++;
                            lblKills.Text = $"Kills : {Kills}";
                            lblResult.Text = "You Killed Enemy1";
                        }
                    }
                    if (y.Name == "enemy2")
                    {
                        if (buPlayer.Bounds.IntersectsWith(y.Bounds))
                        {
                            y.Location = new Point(X, Y);
                            Kills++;
                            lblKills.Text = $"Kills : {Kills}";
                            lblResult.Text = "You Killed Enemy2";
                        }
                    }
                    if (y.Name == "enemy3")
                    {
                        if (buPlayer.Bounds.IntersectsWith(y.Bounds))
                        {
                            y.Location = new Point(X, Y);
                            Kills++;
                            lblKills.Text = $"Kills : {Kills}";
                            lblResult.Text = "You Killed Enemy3";
                        }
                    }
                }
            }
        }

        
        //Start The Game
        private void StartGame()
        {
            if(Pause == true)
            {
                Pause = false;
                timer3.Start();
            }
            lblPaused.Visible = false;
            lblCap.Visible = false;
            foreach (Control x in Controls)
            {
                if (x is PictureBox)
                    x.Enabled = true;
            }
            lblHard.Enabled = true;
            timer1.Start();
            lblResult.Text = "Started Game";
        }

        
        //Pause The Game
        private void PauseGame()
        {
            if(Start == true)
            {
                Start = false;
                timer3.Stop();
            }
            lblPaused.Visible = true;
            lblCap.Visible = true;
            foreach (Control x in Controls)
            {
                if (x is PictureBox)
                    x.Enabled = false;
            }
            lblHard.Enabled = false;
            timer1.Stop();
            timer2.Stop();
            lblResult.Text = "Paused Game";
        }

        
        //This Function Defines The Enemies In The Game
        private void Enemy()
        {
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(Properties.Resources.Explosion_Sound_Effects);
            foreach (Control x in Controls)
            {
                if (x is PictureBox && x.Tag == "enemy1")
                {
                    enlbl1.Location = new Point(x.Location.X + x.Width + 3, x.Location.Y);
                    enlbl1.Text = $"Enemy1\n[x : {x.Location.X}] [y : {x.Location.Y}]";
                    x.Left -= 2;
                    if (x.Left < 0)
                    {
                        int X = rnd.Next(Width, Width + 300);
                        int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 100);
                        x.Location = new Point(X, Y);
                    }
                    if (buPlayer.Visible == true)
                        if (buPlayer.Bounds.IntersectsWith(x.Bounds))
                        {
                            Score += 5;
                            Kills++;
                            lblScore.Text = $"Score : {Score}";
                            lblKills.Text = $"Kills : {Kills}";
                            int X = rnd.Next(Width, Width + 300);
                            int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 73);
                            buPlayer.Location = new Point(playerTank.Location.X + 104, playerTank.Location.Y + 72);
                            buPlayer.Visible = false;
                            x.Location = new Point(X, Y);
                            snd.Play();
                            lblResult.Text = "You Killed Enemy1";
                        }
                }
                if (x is PictureBox && x.Tag == "enemy2")
                {
                    enlbl2.Location = new Point(x.Location.X + x.Width + 3, x.Location.Y);
                    enlbl2.Text = $"Enemy2\n[x : {x.Location.X}] [y : {x.Location.Y}]";
                    x.Left -= 2;
                    if (x.Left < 0)
                    {
                        int X = rnd.Next(Width, Width + 300);
                        int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 100);
                        x.Location = new Point(X, Y);
                    }
                    if (buPlayer.Visible == true)
                        if (buPlayer.Bounds.IntersectsWith(x.Bounds))
                        {
                            Score += 5;
                            Kills++;
                            lblScore.Text = $"Score : {Score}";
                            lblKills.Text = $"Kills : {Kills}";
                            int X = rnd.Next(Width, Width + 300);
                            int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 73);
                            buPlayer.Location = new Point(playerTank.Location.X + 104, playerTank.Location.Y + 72);
                            buPlayer.Visible = false;
                            x.Location = new Point(X, Y);
                            snd.Play();
                            lblResult.Text = "You Killed Enemy2";
                        }
                }
                if (x is PictureBox && x.Tag == "enemy3")
                {
                    enlbl3.Location = new Point(x.Location.X + x.Width + 3, x.Location.Y);
                    enlbl3.Text = $"Enemy3\n[x : {x.Location.X}] [y : {x.Location.Y}]";
                    x.Left -= 2;
                    if (x.Left < 0)
                    {
                        int X = rnd.Next(Width, Width + 300);
                        int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 100);
                        x.Location = new Point(X, Y);
                    }
                    if (buPlayer.Visible == true)
                        if (buPlayer.Bounds.IntersectsWith(x.Bounds))
                        {
                            Score += 5;
                            Kills++;
                            lblScore.Text = $"Score : {Score}";
                            lblKills.Text = $"Kills : {Kills}";
                            int X = rnd.Next(Width, Width + 10);
                            int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 73);
                            buPlayer.Location = new Point(playerTank.Location.X + 104, playerTank.Location.Y + 72);
                            buPlayer.Visible = false;
                            x.Location = new Point(X, Y);
                            snd.Play();
                            lblResult.Text = "You Killed Enemy3";
                        }
                }
            }
        }

        
        //This Function Will Be Executed If You Are Game-Over
        private void GameOver()
        {
            label3.Visible = true;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                    x.Enabled = false;
            }
            timer3.Stop();
            lblHard.Enabled = false;
            timer1.Stop();
            timer2.Stop();
            lblStart.Enabled = false;
            lblPause.Enabled = false;
            btnRestart.Show();
            GameOverCheck = true;
        }
        //This Function Can Check Is Hard Mode Or Not
        private void HardModeGame()
        {
            if (hard == false)
            {
                lblHard.BackColor = Color.Black;
                lblHard.Text = "HARD [✔]";
                hard = true;
            }
            else
            {
                lblHard.BackColor = Color.Gray;
                lblHard.Text = "HARD []";
                hard = false;
            }
        }

        
        //This Function Defines The Movement Of The Player's Tank
        private void PlayerMove()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "UpWall")
                {
                    if (keyUp == true)
                    {
                        if (playerTank.Top > x.Location.Y + 32.98)
                        {
                            playerTank.Top -= 3;
                        }
                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "DownWall")
                {
                    if (keyDown == true)
                    {
                        if (playerTank.Top < x.Location.Y - 73)
                        {
                            playerTank.Top += 3;
                        }
                    }
                }
            }
            if (keyRight == true)
            {
                if (playerTank.Right < Width - 15)
                {
                    playerTank.Left += 3;
                }
            }
            if (keyLeft == true)
            {
                if (playerTank.Right > 110)
                {
                    playerTank.Left -= 5;
                }
            }
        }

        
        //This Function Defines The Movement Of The Blockers (STONES), 
        //Their Position And Reaction Against The Player
        private void Block(bool hard)
        {
            Random rnd = new Random();
            int X = rnd.Next(Width, Width + 10);
            int Y = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 73);
            foreach(Control x in this.Controls)
            {
                switch (hard)
                {
                    case true:
                        if (x is PictureBox && x.Tag == "b")
                        {
                            i++;
                            Distance = i > 100 ? i / 100 : 0;
                            x.Left -= 3;
                            lblScore.Text = $"Score : {Score}";
                            lblDistance.Text = $"Distance : {Distance}";
                            if (x.Left < -30)
                            {
                                x.Location = new Point(X, Y);
                                Score++;
                            }

                        }
                        if (x is PictureBox && x.Tag == "DownWall")
                        {
                            x.Left -= 5;
                            if (x.Left < -626)
                            {
                                x.Left = Width + 225;
                            }

                        }
                        if (x is PictureBox && x.Tag == "UpWall")
                        {
                            x.Left -= 5;
                            if (x.Left < -626)
                            {
                                x.Left = Width + 225;
                            }

                        }
                        break;
                    case false:
                        if (x is PictureBox && x.Tag == "b")
                        {
                            if (keyRight == true)
                            {
                                i++;
                                Distance = i > 100 ? i / 100 : 0;
                                lblDistance.Text = $"Distance : {Distance}";
                                lblScore.Text = $"Score : {Score}";
                                x.Left -= 5;
                                pictureBox1.Left -= 1;
                            }
                            if (x.Left < -30)
                            {
                                x.Location = new Point(X, Y);
                                Score++;
                            }
                            if (pictureBox1.Left < -30)
                            {
                                pictureBox1.Visible = false;
                                pictureBox1.Enabled = false;
                                timer3.Start();
                            }

                        }
                        if (x is PictureBox)
                        {
                            if(x.Tag == "UpWall")
                            {
                                if (keyRight == true)
                                    x.Left -= 3;
                                if (WindowState != FormWindowState.Maximized)
                                {
                                    if (x.Left < -626)
                                    {
                                        x.Left = x.Name == "pictureBox15" ? pictureBox9.Location.X + 623 : x.Name == "pictureBox9"
                                            ? pictureBox8.Location.X + 623 : x.Name == "pictureBox8"
                                            ? pictureBox7.Location.X + 623 : x.Name == "pictureBox7"
                                            ? pictureBox15.Location.X + 623 : Width;
                                    }
                                }
                                else
                                {
                                    if (x.Left < -626)
                                    {
                                        x.Left = x.Name == "pictureBox15" ? pictureBox9.Location.X + 623 : x.Name == "pictureBox9" 
                                            ? pictureBox8.Location.X + 623 : x.Name == "pictureBox8"
                                            ? pictureBox7.Location.X + 623 : x.Name == "pictureBox7"
                                            ? pictureBox15.Location.X + 623 : Width;
                                    }
                                }
                            }
                            if(x.Tag == "DownWall")
                            {
                                if (keyRight == true)
                                    x.Left -= 3;
                                if (WindowState != FormWindowState.Maximized)
                                {
                                    if (x.Left < -626)
                                    {
                                        x.Left = x.Name == "pictureBox16" ? pictureBox14.Location.X + 623 : x.Name == "pictureBox14"
                                            ? pictureBox13.Location.X + 623 : x.Name == "pictureBox13"
                                            ? pictureBox11.Location.X + 623 : x.Name == "pictureBox11"
                                            ? pictureBox16.Location.X + 623 : Width;
                                    }
                                }
                                else
                                {
                                    if (x.Left < -626)
                                    {
                                        x.Left = x.Name == "pictureBox16" ? pictureBox14.Location.X + 623 : x.Name == "pictureBox14"
                                            ? pictureBox13.Location.X + 623 : x.Name == "pictureBox13"
                                            ? pictureBox11.Location.X + 623 : x.Name == "pictureBox11"
                                            ? pictureBox16.Location.X + 623 : Width;
                                    }
                                }
                            }

                        }
                        break;
                }
            }
        }

        
        //This function defines the movement of the walls
        private void WallLocation()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "DownWall")
                {
                    x.Location = new Point(x.Location.X, Height - 69);
                }
                if (x is PictureBox && x.Tag == "UpWall")
                {

                    x.Location = new Point(x.Location.X, panel1.Height);
                }
            }
        }

        
        //This Function Can Save The Game Data
        private void Save()
        {
            if(keyCntrl == true)
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\Log", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Score);
                sw.Close();
                fs.Close();
                timer5.Start();
                keyCntrl = false;
            }
            if (keyCntrl != true)
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\Log", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Score);
                sw.Close();
                fs.Close();
            }
        }

        
        //This function can restart the game when the player loses
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 100;
            label1.Text = $"Health : {progressBar1.Value * 10}";
            timer1.Enabled = true;
            GameOverCheck = false;
            label3.Visible = true;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                    x.Enabled = true;
            }
            lblHard.Enabled = true;
            lblStart.Enabled = true;
            lblPause.Enabled = true;
            btnRestart.Hide();
            label3.Hide();
            enemy1.Location = new Point(Width + 100, pictureBox15.Location.Y + pictureBox15.Height + 100);
            enemy2.Location = new Point(Width + 300, pictureBox15.Location.Y + pictureBox15.Height + 300);
            enemy3.Location = new Point(Width + 200, pictureBox15.Location.Y + pictureBox15.Height + 500);
        }

        
        //This Function Can Delete Save File Game Data
        private void Delete()
        {
            if (File.Exists(Application.StartupPath + "\\Log"))
                File.Delete(Application.StartupPath + "\\Log");
            keyDelete = false;
        }

        
        //When Played Closed The Game , Then Game Doing Save Score In Application Path
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }

        
        //When Played Closed The Game , Then Game Doing Save Score In Application Path
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
        }

        
        //This Is Game Main Function
        private void GameResult()
        {
            foreach (Control x in this.Controls)
            {
                int X, Y;
                if (x is PictureBox && x.Tag == "b")
                {
                    int accident = 0;
                    if (progressBar1.Value != 0)
                    {
                        if (playerTank.Bounds.IntersectsWith(x.Bounds))
                            accident++;
                        if (accident == 1)
                        {
                            accident = 0;
                            progressBar1.Value -= 5;
                            playerTank.Location = new Point(0, (pictureBox16.Location.Y - (pictureBox15.Location.Y + pictureBox15.Height)) / 2);
                            timer4.Start();
                        }
                    }
                    else
                    {
                        GameOver();
                    }
                }
                if (x is PictureBox && x.Tag == "GetScoreMore")
                {
                    int accident = 0;
                    if (playerTank.Bounds.IntersectsWith(x.Bounds) || buPlayer.Bounds.IntersectsWith(x.Bounds))
                        accident++;
                    if (accident == 1)
                    {
                        accident = 0;
                        Score += 30;
                        if (progressBar1.Value != 100)
                            progressBar1.Value += 5;
                        lblScore.Text = $"Score : {Score}";
                        timer3.Start();
                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "enemy1" || x.Tag == "enemy2" || x.Tag == "enemy3")
                {
                    int X1,Y1;
                    do
                    {
                        // تولید یک عدد تصادفی بین 10 و 20
                        X1 = rnd.Next(Width, Width + 10);
                        Y1 = rnd.Next(pictureBox15.Location.Y + 32, pictureBox16.Location.Y - 73);
                    } while (!(X1 < Width) && !(Y1 < pictureBox16.Location.Y - 73));
                    int accident = 0;
                    if (progressBar1.Value != 0)
                    {
                        switch (x.Name)
                        {
                            case "enemy1":

                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    playerTank.Location = new Point(0, (pictureBox16.Location.Y - (pictureBox15.Location.Y + pictureBox15.Height)) / 2);
                                    timer4.Start();
                                }
                                break;
                            case "enemy2":

                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    playerTank.Location = new Point(0, (pictureBox16.Location.Y - (pictureBox15.Location.Y + pictureBox15.Height)) / 2);
                                    timer4.Start();
                                }
                                break;
                            case "enemy3":
                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    playerTank.Location = new Point(0, (pictureBox16.Location.Y - (pictureBox15.Location.Y + pictureBox15.Height)) / 2);
                                    timer4.Start();
                                }
                                break;
                        }
                    }
                    else
                    {
                        GameOver();

                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "buen")
                {
                    int accident = 0;
                    if (progressBar1.Value != 0)
                    {
                        switch (x.Name)
                        {
                            case "en1":

                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    x.Location = new Point(enemy1.Location.X - 35, enemy1.Location.Y + 15);
                                }
                                break;
                            case "en2":

                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    x.Location = new Point(enemy2.Location.X - 35, enemy2.Location.Y + 15);
                                }
                                break;
                            case "en3":
                                if (playerTank.Bounds.IntersectsWith(x.Bounds))
                                    accident++;
                                if (accident == 1)
                                {
                                    accident = 0;
                                    progressBar1.Value -= 5;
                                    x.Location = new Point(enemy3.Location.X - 35, enemy3.Location.Y + 15);
                                }
                                break;
                        }
                    }
                    else
                    {
                        GameOver();

                    }
                }
            }
        }

        
        //Change Easy Mode To Hard Mode By Click The lblHard
        private void lblHard_Click(object sender, EventArgs e)
        {
            HardModeGame();
        }

        
        //Start The Game By Click The lblStart
        private void lblStart_Click(object sender, EventArgs e)
        {
            if (GameOverCheck != true && Start == true)
                StartGame();
        }

        
        //Timer 2
        private void timer2_Tick(object sender, EventArgs e)
        {
            int valueP = progressBar1.Value;
            GameResult();
            if (progressBar1.Value < valueP)
                timer2.Stop();
        }

        
        //Change Location Function
        private void playerTank_LocationChanged(object sender, EventArgs e)
        {
            timer2.Start();
        }

        
        //Key Codes If Not Be "GameOverCheck" Equal To "True"
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameOverCheck != true)
                switch (e.KeyCode)
                {
                    case Keys.W:
                        keyUp = true;
                        lblKeyUp.BackColor = Color.Gray;
                        break;
                    case Keys.S:
                        keyDown = true;
                        lblKeyDown.BackColor = Color.Gray;
                        break;
                    case Keys.D:
                        keyRight = true;
                        lblKeyRight.BackColor = Color.Gray;
                        break;
                    case Keys.A:
                        keyLeft = true;
                        lblKeyLeft.BackColor = Color.Gray;
                        break;
                    case Keys.Space:
                        lblShot.BackColor = Color.Gray;
                        keyShot = true;
                        break;
                    case Keys.E:
                        lblStart.BackColor = Color.Gray;
                        if (GameOverCheck != true)
                            Start = true;
                        if (Start == true)
                            StartGame();
                        break;
                    case Keys.Escape:
                        lblPause.BackColor = Color.Gray;
                        if (GameOverCheck != true)
                            Pause = true;
                        break;
                    case Keys.R:
                        keyCntrl = true;
                        break;
                    case Keys.T:
                        keyDelete = true;
                        break;
                    case Keys.H:
                        if (pnlHelp.Visible == false)
                            pnlHelp.Visible = true;
                        else
                            pnlHelp.Visible = false;
                        break;
                }
        }

        
        //Key Up Codes If Not Be "GameOverCheck" Equal To "True"
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (GameOverCheck != true)
                switch (e.KeyCode)
                {
                    case Keys.W:
                        keyUp = false;
                        lblKeyUp.BackColor = Color.Black;
                        break;
                    case Keys.S:
                        keyDown = false;
                        lblKeyDown.BackColor = Color.Black;
                        break;
                    case Keys.D:
                        keyRight = false;
                        lblKeyRight.BackColor = Color.Black;
                        break;
                    case Keys.A:
                        keyLeft = false;
                        lblKeyLeft.BackColor = Color.Black;
                        break;
                    case Keys.Space:
                        lblShot.BackColor = Color.Black;
                        keyShot = false;
                        break;
                    case Keys.E:
                        lblStart.BackColor = Color.Black;
                        break;
                    case Keys.R:
                        keyCntrl = false;
                        break;
                    case Keys.Escape:
                        lblPause.BackColor = Color.Black;
                        break;
                    case Keys.T:
                        keyDelete = false;
                        break;
                }
        }

        
        //Timer 6
        private void timer6_Tick(object sender, EventArgs e)
        {

                lblSave.Top-= 2;
            if(lblSave.Top <= pictureBox15.Location.Y)
            {
                if(lblSave.Visible == true)
                {
                    lblSave.Visible = false;
                    lblSave.Enabled = false;
                }
                timer6.Stop();
            }
        }

        
        //When Form Loading This Function Run
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            Intro intro = new Intro();
            intro.ShowDialog();
            this.Show();
        }

        
        //Timer 5
        private void timer5_Tick(object sender, EventArgs e)
        {
            if(lblSave.Visible == false)
            {
                lblSave.Visible = true;
                lblSave.Enabled = true;
            }
            if(lblSave.Top == pictureBox15.Location.Y + pictureBox15.Height + 50)
            {
                timer5.Stop();
                timer6.Start();
            }
                lblSave.Top+=2;
        }

        
        //Pause The Game By Click The lblPause
        private void lblPause_Click(object sender, EventArgs e)
        {
            if (GameOverCheck != true && Pause == true)
                PauseGame();
        }


        //Text Changed Function For lblScore
        private void lblScore_TextChanged(object sender, EventArgs e)
        {
            if (Score >= 2000)
                hard = true;
        }


        //Timer3
        private void timer3_Tick(object sender, EventArgs e)
        {
            int X = rnd.Next(50, Width - 150);
            int Y = rnd.Next(68, pictureBox16.Location.Y - 100);
            GetScoreMore++;
            if (GetScoreMore < 200)
            {
                pictureBox1.Location = new Point(0, 0);
                pictureBox1.Visible = false;
                pictureBox1.Enabled = false;
            }
            if(GetScoreMore == 200)
            {
                pictureBox1.Location = new Point(X, Y);
                pictureBox1.Visible = true;
                pictureBox1.Enabled = true;
                GetScoreMore = 0;
                timer3.Stop();
            }
        }
        //Timer 4
        private void Timer4StartSection(object sender, EventArgs e)
        {
            CounterSpawnPlayerToStartLocation++;
            playerTank.Visible = playerTank.Visible == true ? false : true;
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && x.Tag == "b" || x.Tag == "enemy1" || x.Tag == "enemy2" || x.Tag == "enemy3")
                {
                    if(x.Location.X > 0 && x.Location.X < 500 && x.Location.Y > 60 && x.Location.Y < pictureBox16.Location.Y)
                    {
                        int X = rnd.Next(Width, Width + 1000);
                        int Y = rnd.Next(pictureBox15.Location.Y + pictureBox15.Height, pictureBox16.Location.Y - 77);
                        x.Location = new Point(X, Y);
                    }
                }
            }
            if (CounterSpawnPlayerToStartLocation == 8)
            {
                CounterSpawnPlayerToStartLocation = 0;
                timer4.Stop();
            }
        }
        
        private void pictureBox11_SizeChanged(object sender, EventArgs e)
        {
            WallLocation();
        }
        //Timer 1
        private void timer1_Tick(object sender, EventArgs e)
        {
            Update();
            if (GameOverCheck == true)
                timer1.Enabled = false;
        }
        //Change Color The ProgressBar
        private void ProgressValueChangedEvent()
        {
            if(progressBar1.Value > 70)
                progressBar1.SetStatus(1);
            if (progressBar1.Value > 35 && progressBar1.Value < 70)
                progressBar1.SetStatus(3);
            if (progressBar1.Value > 0 && progressBar1.Value < 35)
                progressBar1.SetStatus(2);
        }
    }
}
