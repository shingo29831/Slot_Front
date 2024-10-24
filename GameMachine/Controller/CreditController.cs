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
            creditTxb.Enabled = false;
            countTxb.Enabled = false;
            payoutTxb.Enabled = false; 
        }
        //クレジット残高を表示　更新？？
        private void CreditUpdate()
        {

        }

    }
}
