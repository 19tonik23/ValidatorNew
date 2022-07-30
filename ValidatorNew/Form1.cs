using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValidatorNew
{
    public partial class Form1 : Form
    {

        private Stops stops;
        public Form1()
        {
            InitializeComponent();
            tabPanGeneral.BackColor = ColorTranslator.FromHtml("#494949");
            stops = new Stops(panMaps, tbCards,panCheck,labMonitor,busMonitor);
            
        }
        
        //старт программы
        private void butStart_Click(object sender, EventArgs e)
        {
            stops.ReturnToStart();
            stops.playerMove.Play();
            stops.busTimer.Start();
        }

        
    }
}
