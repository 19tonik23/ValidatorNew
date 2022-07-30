using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using WMPLib;
using System.IO;
using System.Threading;


namespace ValidatorNew
{
    public class AllCards
    {
        private Card[] curCard;
        private Button[] btnCrd;
        private const int CARD_CNT = 3;
        private TableLayoutPanel tableCards;
        private Panel[] panel;
        private WindowsMediaPlayer mPlayer = new WindowsMediaPlayer();
        public BonusTimer bnsTimer;
        
        public AllCards(TableLayoutPanel tableCards)
        {
           
            this.tableCards = tableCards;
            Sound();
            AllCrd();
           
            bnsTimer = new BonusTimer(CARD_CNT, curCard);
        }
        
        //создание всех карт
        private void AllCrd()
        {
            curCard = new Card[CARD_CNT];
            btnCrd = new Button[CARD_CNT];
            panel = new Panel[CARD_CNT];
            for (int i = 0; i < curCard.Length; i++)
            {
                panel[i] = new Panel();
                panel[i].Dock = DockStyle.Fill;
                curCard[i] = new Card();
                btnCrd[i] = curCard[i].currentCard;
                btnCrd[i].TabIndex = i;
                btnCrd[i].Enabled=false;
                btnCrd[i].Click += Check_Card;
                tableCards.Controls.Add(panel[i], i, 0);
                panel[i].Controls.Add(btnCrd[i]);
            }
        }

        //разблокировка карт на остановках
        public void CardEnabled()
        {
            for(int i=0;i<CARD_CNT;i++)
                btnCrd[i].Enabled = true;
        }

        //блокировка карт вне остановок
        public void CardBlocked()
        {
            for (int i = 0; i < CARD_CNT; i++)
                btnCrd[i].Enabled = false;
        }

       
        //установка разрешения на клик в первоночальное состояние
        public void CardAutoEllips()
        {
            for (int i = 0; i < CARD_CNT; i++)
                btnCrd[i].AutoEllipsis = false;
        }

        //клик на карту
        private int cardNumber;
        private void Check_Card(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.AutoEllipsis = !btn.AutoEllipsis;
            
            btn.FlatAppearance.BorderSize = 1;
            cardNumber = btn.TabIndex;
        
        }

        //действия на карте при входе в автобус(первый клик на карту)
        public void CardCheckingEnt(Panel panCheck, Panel currentStop, Panel busMonitor,
            int finish, Label labMonitor)
        {
            if (btnCrd[cardNumber].AutoEllipsis)
            {
             
            mPlayer.controls.play();
            panel[cardNumber].Controls.Remove(btnCrd[cardNumber]);
            panCheck.Controls.Add(btnCrd[cardNumber]);
           
            panCheck.Refresh();
            EntrIndication(busMonitor);
            System.Threading.Thread.Sleep(500);
            
            panCheck.Controls.Remove(btnCrd[cardNumber]);
            panel[cardNumber].Refresh();
            panel[cardNumber].Controls.Add(btnCrd[cardNumber]);
            btnCrd[cardNumber].Tag = currentStop.Tag;
            BonusTimerStop(cardNumber);
            NullBalance(cardNumber, labMonitor); 
            
           }
        }

        //иконка вход в автобус
        private void EntrIndication(Panel busMonitor)
        {
            busMonitor.BackgroundImage = Properties.Resources.pasEnt;
            busMonitor.Refresh();
            busMonitor.BackgroundImage = null;
        }

        //действия на карте при выходе из автобуса(второй клик на карту)
        public void CardCheckingExit(Panel panCheck, Panel currentStop,Label labMonitor,Panel busMonitor)
        {
            if (btnCrd[cardNumber].AutoEllipsis==false)
            {
                
                mPlayer.controls.play();
                panel[cardNumber].Controls.Remove(btnCrd[cardNumber]);
                panCheck.Controls.Add(btnCrd[cardNumber]);
                btnCrd[cardNumber].FlatAppearance.BorderSize = 0;
                CalculBalance(currentStop, labMonitor,busMonitor);
                
                panCheck.Refresh();
                System.Threading.Thread.Sleep(1000);

                panCheck.Controls.Remove(btnCrd[cardNumber]);
                panel[cardNumber].Refresh();
                panel[cardNumber].Controls.Add(btnCrd[cardNumber]);
                BonusTimerStart(cardNumber);
            }
        }

        //Старт таймера бонусного времени
        private void BonusTimerStart(int index)
        {
            bnsTimer.durat[index] =  bnsTimer.SetTime();
            bnsTimer.start_stopBonus[index] = true;
            bnsTimer.timerBonus[index].Start();
        }

        //Стоп таймера бонусного времени
        private void BonusTimerStop(int index)
        {
            bnsTimer.start_stopBonus[index] = false;
            bnsTimer.timerBonus[index].Stop();
            curCard[cardNumber].currentCard.Text = curCard[cardNumber].cardsData[0] +
               curCard[cardNumber].cardsData[1] + "\n" + curCard[cardNumber].cardsData[2]
               + curCard[cardNumber].cardsData[3] + "\n" + curCard[cardNumber].cardsData[4]
               + bnsTimer.SetTime();
        }

        //подсчёт баланса карты при выходе из автобуса
        private void CalculBalance(Panel currentStop, Label labMonitor, Panel busMonitor)
        {
            int sum1 = (int)(currentStop.Tag);
            int sum2 = (int)(btnCrd[cardNumber].Tag);
            int sum = sum1 - sum2;
            double balance;
            if (bnsTimer.durat[cardNumber] == bnsTimer.SetTime())
            {
                balance = 0.5 + sum;
            }
            else{
                balance =sum;
            }
            double crdBalance = double.Parse(curCard[cardNumber].cardsData[3]);
            
            curCard[cardNumber].cardsData[3] = (crdBalance - balance).ToString();
            curCard[cardNumber].currentCard.Text = curCard[cardNumber].cardsData[0] +
                curCard[cardNumber].cardsData[1] + "\n" + curCard[cardNumber].cardsData[2]
                + curCard[cardNumber].cardsData[3] + "\n" + curCard[cardNumber].cardsData[4]
                + curCard[cardNumber].cardsData[5];

            ExitIndication(labMonitor, busMonitor);
        }

        //индикация на табло при выходе из автобуса
        private void ExitIndication(Label labMonitor, Panel busMonitor)
        {
            labMonitor.Text = "На счету " + curCard[cardNumber].cardsData[3];
            labMonitor.ForeColor = Color.Red;
            labMonitor.Refresh();
            labMonitor.Text = "Приложите карточку";
            labMonitor.ForeColor = Color.White;
            busMonitor.BackgroundImage = Properties.Resources.pasExit;
            busMonitor.Refresh();
            busMonitor.BackgroundImage = null;
        }

        //штраф при выходе из автобуса,не приложив карточку
        public void Penalti(int finish)
        {

            if (finish == 11)
            {
               
                int penalSum = 11;
                for (int i = 0; i < CARD_CNT;i++ )
                {
                    if (btnCrd[i].AutoEllipsis)
                    {
                    double crdBalance = double.Parse(curCard[i].cardsData[3]);
                    curCard[i].cardsData[3] = (crdBalance - penalSum).ToString();
                    curCard[i].currentCard.Text = curCard[i].cardsData[0] +
                       curCard[i].cardsData[1] + "\n" + curCard[i].cardsData[2]
                       + curCard[i].cardsData[3] + "\n" + curCard[i].cardsData[4]
                       + curCard[i].cardsData[5];
                    btnCrd[i].FlatAppearance.BorderSize = 0;
                    }
                }
            }
        }
        
        //звук валидатора при прикладывании карточки
        private void Sound()
        {
             string path = Path.GetDirectoryName(Application.ExecutablePath) + "file.mp3";
             File.WriteAllBytes(path, Properties.Resources.check);
             mPlayer.URL = path;
        }

        //действия валидатора на баланс меньше минимальной при входе в автобус
        private void NullBalance(int cardNumber, Label labMonitor)
        {
            if (double.Parse(curCard[cardNumber].cardsData[3])<=1.5)
            {
            curCard[cardNumber].currentCard.Enabled = false;
            curCard[cardNumber].currentCard.BackgroundImage = Properties.Resources.not_money;
            labMonitor.Text = "Нет средств";
            labMonitor.ForeColor = Color.Red;
            btnCrd[cardNumber].Refresh();
            labMonitor.Refresh();
            System.Threading.Thread.Sleep(1500);
            
            labMonitor.Text = "Приложите карточку";
            labMonitor.ForeColor = Color.White;
            btnCrd[cardNumber].Visible = false;
            }

        }
        
    }
}
