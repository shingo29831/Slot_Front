using System;
using GameMachine.Model;

namespace GameMachine.View
{
    public class CounterView
    {


        CounterController counterDisplay;
        public CounterView(CounterController counterDisplay)
        {
            this.counterDisplay = counterDisplay;
        }

        public void SwitchCounterUpdate()
        {
            SwitchBigCounterUpdate(Counter.GetBigCnt());
            SwitchRegularCounterUpdate(Counter.GetRegCnt());
            SwitchBetweenBonus(Counter.GetBetweenBonusCnt());
        }

        private void SwitchBigCounterUpdate(int bigCount)
        {
            counterDisplay.BigCounterTxb.Text = bigCount.ToString("D3");
        }

        private void SwitchRegularCounterUpdate(int regCount)
        {
            counterDisplay.RegularCounterTxb.Text = regCount.ToString("D3");
        }

        private void SwitchBetweenBonus(int betweenBonusCount)
        {
            counterDisplay.BetweenBonusTbx.Text = betweenBonusCount.ToString("D3");
        }

        
    }
}
