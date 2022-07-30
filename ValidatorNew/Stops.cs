using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace ValidatorNew
{
    public class Stops
    {
        private Panel[] stops;
        private int stopCnt = 12;
        private int stopSize = 25;
        private Panel bus = new Panel();
        public Timer busTimer = new Timer();
        private Timer timerPause = new Timer();
        public SoundPlayer playerMove = new SoundPlayer();
        private SoundPlayer playerAtent = new SoundPlayer();
        private Point busStertPosition;
        private AllCards allCards;
        private Panel panCheck;
        private Label labMonitor;
        private  Panel busMonitor;

        public Stops(Panel stopPan, TableLayoutPanel tbCards,
            Panel panCheck, Label labMonitor, Panel busMonitor)
        {
            this.busMonitor = busMonitor;
            this.labMonitor = labMonitor;
            AllStops(stopPan);
            Bus(stopPan);
            busTimer.Interval = 100;
            busTimer.Tick += Timer_Tick;
            timerPause.Interval = 1000;
            timerPause.Tick += TimerPause_Tick;
            playerMove.Stream = Properties.Resources.bus1;
            playerAtent.Stream = Properties.Resources.atention;
            allCards = new AllCards(tbCards);
            this.panCheck = panCheck;
        }

       
        //рестарт автобуса
        public void ReturnToStart()
        {
            bus.Location = busStertPosition;
            finish = 0;
            offset = 0;
            pause = 0;
            allCards.CardBlocked();
            allCards.CardAutoEllips();
        }
        
        //создание всех остановок
        private void AllStops(Panel stopPan)
        {
            stops = new Panel[stopCnt];
            for (int i = 0; i < stopCnt;i++ )
            {
                stops[i] = new Panel();
                stops[i].Size = new Size(stopSize, stopSize);
                stops[i].Location = new Point(stopSize*2 + stopPan.Width/12*i,
                    stopPan.Bottom - stopSize*8/3);
                stops[i].TabIndex = i;
                stops[i].Tag = i;
                stops[i].Cursor = Cursors.Hand;
                stops[i].BackColor = Color.Red;
                stops[i].Enabled = false;
                stops[i].BackgroundImage = Properties.Resources.stops;
                stops[i].BackgroundImageLayout = ImageLayout.Stretch;
                stops[i].Click += Stops_Click;
                stopPan.Controls.Add(stops[i]);
               
            }
        }

        //создание автобуса
        private void Bus(Panel stopPan)
        {
            bus.Size=new Size(45,25);
            bus.BackgroundImage=Properties.Resources.autobus;
            bus.BackgroundImageLayout=ImageLayout.Stretch;
            bus.Location = new Point(-bus.Width,
                stops[0].Location.Y);
            busStertPosition = bus.Location;
            stopPan.Controls.Add(bus);
        }

        //таймер движения автобуса
        int offset = 0;
        private void Timer_Tick(object sender,EventArgs e)
        {
                bus.Location = new Point(bus.Location.X + 1, bus.Location.Y);
                offset++;
                if (offset == bus.Width * 17 / 9)
                {
                    busTimer.Stop();
                    BusStop();
                    timerPause.Start();
                    offset = 0;
                    allCards.CardEnabled();
                    System.Threading.Thread.Sleep(500);
                    SoundDooR();
                    if (finish >= 12)
                        allCards.CardBlocked();
                }
           
        }

        //разблокировка клика остановок
        private void BusStop()
        {
            foreach(Panel pnl in stops){
                if (bus.Location.X > pnl.Location.X - 20 & bus.Location.X < pnl.Location.X + 20)
                {
                    stops[pnl.TabIndex].Enabled = true;
                    stops[pnl.TabIndex].BackgroundImage = Properties.Resources.sopBus;
                }
            }
        }

        //блокировка остановок
        private void BusStart()
        {
            foreach (Panel pnl in stops)
            {
                if (bus.Location.X < pnl.Location.X + 20)
                {
                    stops[pnl.TabIndex].Enabled = false;
                    stops[pnl.TabIndex].BackgroundImage = Properties.Resources.stops;
                }
            }
        }

        //таймер действий на остановке
        private int pause = 0;
        private int finish = 0;
        private void TimerPause_Tick(object sender, EventArgs e)
        {
            pause++;
           
            if(pause==6){
                if (finish >= 12)
                {
                    busTimer.Stop();
                    timerPause.Stop();
                    allCards.CardAutoEllips();
                    allCards.CardBlocked();
                }
                else
                {
                    allCards.Penalti(finish);
                    timerPause.Stop();
                    pause = 0;
                    playerAtent.Play();
                    System.Threading.Thread.Sleep(3500);
                    SoundDooR();
                    System.Threading.Thread.Sleep(3500);
                    playerMove.Play();
                    busTimer.Start();
                    finish++;
                    
                    BusStart();
                    allCards.CardBlocked();
                }
                
            }
        }

        //звук открытия-закрытия дверей
        private void SoundDooR()
        {
            SoundPlayer player = new SoundPlayer();
            player.Stream = Properties.Resources.door;
            player.Play();

        }

        //событие клика на остановку
        private void Stops_Click(object sender , EventArgs e)
        {
            Panel currentStop = (Panel)sender;
            Ent_Exit(currentStop);
            
        }

        //события с карточками на остановках
        private void Ent_Exit(Panel currentStop)
        {

            allCards.CardCheckingEnt(panCheck, currentStop, busMonitor,finish,labMonitor);
            allCards.CardCheckingExit(panCheck, currentStop,labMonitor,busMonitor);
        }

    }
}
