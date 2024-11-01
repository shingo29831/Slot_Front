using System;
using static GameMachine.Model.Game;
using static GameMachine.Model.Setting;

namespace GameMachine
{
    public partial class CreditController : UserControl
    {
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
    }
}
