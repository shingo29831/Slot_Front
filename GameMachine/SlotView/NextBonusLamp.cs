using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMachine.SlotView
{
    internal class NextBonusLamp : Lamp
    {
        private bool isNextBonus;

        public void LightOn(bool isNextBonus)
        {
            this.isNextBonus = isNextBonus;
            base.LightOn();
        }
    }
}
