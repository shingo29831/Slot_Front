using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMachine.SlotView
{
    internal class BonusLamp : Lamp
    {
        private bool isBonus;

        public void LightOn(bool isBonus)
        {
            this.isBonus = isBonus;
            base.LightOn();
        }
    }
}
