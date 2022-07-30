using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValidatorNew
{
    public class BonusTimer
    {
        public int[] durat;
        public bool[] start_stopBonus = new bool[] { false, false, false };
        public Timer[] timerBonus;
        private Card[] curCard;
        public BonusTimer(int plasticCnt, Card[] curCard)
        {
            this.curCard = curCard;
            durat = new int[] { SetTime(), SetTime(), SetTime() };
            TimerArray(plasticCnt);

        }

        //Установка бонусного времени
        public int SetTime()
        {
            return 60;
        }

        //Создание массива таймеров
        private void TimerArray(int plasticCnt)
        {
            timerBonus = new Timer[plasticCnt];
            for (int i = 0; i < timerBonus.Length; i++)
            {
                timerBonus[i] = new Timer();
                timerBonus[i].Interval = 1000;
                timerBonus[i].Tick += TimerTick;

            }
        }

        //Событие отсчёта таймера
        private void TimerTick(object sender, EventArgs e)
        {

            if (start_stopBonus[0])
            {

                TimerContent(0);
            }

            if (start_stopBonus[1])
            {

                TimerContent(1);
            }
            if (start_stopBonus[2])
            {

                TimerContent(2);
            }
        }

        //Содержание таймера
        private void TimerContent(int id)
        {
            durat[id]--;
            if (durat[id] == 0)
            {
                durat[id] = SetTime();
                timerBonus[id].Stop();
                start_stopBonus[id] = false;
            }
            curCard[id].cardsData[5] = durat[id].ToString();
            curCard[id].currentCard.Text = curCard[id].cardsData[0] +
                curCard[id].cardsData[1] + "\n" + curCard[id].cardsData[2]
                + curCard[id].cardsData[3] + "\n" + curCard[id].cardsData[4]
                + curCard[id].cardsData[5];

        }

    }
}
