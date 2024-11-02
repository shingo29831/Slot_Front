using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMachine.Model;
using GameMachine;
using static Constants;

namespace GameMachine.View
{
    

    public class CreditView
    {

        private int payout = 0;
        private int bonusCount = 0;
        private int credit = 0;
        public CreditController creditDisplay;
        public CreditView(CreditController creditDisplay)
        {
            this.creditDisplay = creditDisplay;
        }


        public void ShowCreditDisp()
        {
            ShowCredit();
            ShowBonusCount();
            ShowPayOut();
        }

        public void ShowCredit()
        {
            creditDisplay.creditTxb.Text = Game.GetHasCoin().ToString();
        }

        public void ShowBonusCount()
        {
            if (Game.GetInBonus() && Game.GetHasCoin() != Game.GetIncreasedCoin())
            {
                creditDisplay.countTxb.Text = Game.GetIncreasedCoin().ToString();
            }
            else
            {
                creditDisplay.countTxb.Text = "0";
            }

        }

        public void ShowPayOut()
        {
            Roles establishedRole = Game.GetEstablishedRole();
            if (Game.GetInBonus() && establishedRole != Roles.NONE)
            {
                creditDisplay.payoutTxb.Text = "15";
            }
            else if(establishedRole != Roles.NONE)
            {
                creditDisplay.payoutTxb.Text = Setting.GetRoleReturn(Game.GetEstablishedRole()).ToString();
            }
            else if(establishedRole == Roles.NONE)
            {
                creditDisplay.payoutTxb.Text = "0";
            }
           
        }



        public void SetPayOut(int payOut)
        {
            creditDisplay.payoutTxb.Text = payOut.ToString();
        }

        private void SetBonusCount()
        {
            bonusCount = Game.GetIncreasedCoin();
        }

        private void SetCredit()
        {
            credit = Game.GetHasCoin();
        }

        public void ResetAll()
        {
            payout = 0;
            bonusCount = 0;
            credit = 0;
        }



    }

    

}
