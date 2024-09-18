using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class CreditController : UserControl
    {
        private int remaingCredit;
        private int returnBonusCredit;
        private int returnCredit;

        public CreditController()
        {
            InitializeComponent();
        }
        private void CreditDisplay_Load(object sender, EventArgs e)
        {

        }
        public void switchRemainingCreditDisplay(int remainingCredit) { }
        public void switchReturnBonusCreditDisplay(int returnBonusCredit) { }
        public void switchReturnCredit(int returnCredit) { }

    }
}
