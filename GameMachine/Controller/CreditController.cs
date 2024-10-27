using System;
using static GameMachine.Model.Game;
using static GameMachine.Model.Setting;

namespace GameMachine
{
    public partial class CreditController : UserControl
    {
        private int remaingCredit;
        private int returnBonusCredit;
        private int returnCredit;
        private int payout = 0;
        private int bonusCount = 0;
        private int credit = 0;

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

        public void ShowCredit()
        {
            creditTxb.Text = GetHasCoin().ToString();
        }

        public void ShowBonusCount()
        {
            if (GetInBonus())
            {
                countTxb.Text = GetIncreasedCoin().ToString();
            }
            else
            {
                countTxb.Text = "0";
            }
            
        }

        public void ShowPayOut()
        {
            payoutTxb.Text = GetRoleReturn(GetEstablishedRole()).ToString();
        }


        private void SetPayOut()
        {

        }

        private void SetBonusCount()
        {
            bonusCount = GetIncreasedCoin();
        }

        private void SetCredit() 
        {
            credit = GetHasCoin();
        }
    }
}
