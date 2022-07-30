using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace ValidatorNew
{
    public class Card
    {
        public Button currentCard = new Button();
        public string[] cardsData = new string[] { "id ", "10", "         balance ", "50", "       bonus ", "60" };
       
        public Card()
        {
            rnd = new Random();
            rnd2 = new Random();
            Thread.Sleep(400);
            cardsData[3] = Balance().ToString();
            cardsData[1] = Id().ToString();
            currentCard.Text = cardsData[0] + cardsData[1] + "\n" + cardsData[2]
                + cardsData[3] + "\n" + cardsData[4] + cardsData[5];
           
            currentCard.BackgroundImage = Properties.Resources.bus_card;
            currentCard.BackgroundImageLayout = ImageLayout.Stretch;
            currentCard.Dock = DockStyle.Fill;
            currentCard.ForeColor = Color.White;
            currentCard.FlatAppearance.BorderSize = 0;
            currentCard.FlatStyle = FlatStyle.Flat;
            currentCard.Padding = new Padding(currentCard.Width/5 , 0, 0, currentCard.Height * 5/ 4);
            currentCard.Font = new Font("", 10);
            currentCard.Cursor = Cursors.Hand;
        }

        //установка начального баланса на карточке
        private Random rnd;
        private int Balance()
        {
            
            int balance = rnd.Next(12, 20);
         
            return balance;
        }

        //установка Id на карточке
        private Random rnd2;
        private int Id()
        {

            int balance = rnd2.Next(100, 200);

            return balance;
        }
    }
}
