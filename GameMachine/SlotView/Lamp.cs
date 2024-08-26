using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMachine.SlotView
{
    internal class Lamp : SlotModule
    {
        protected bool isLight;
        protected int bet;

        public virtual void LightOn()
        {
            isLight = true;
        }

        public void LightOff()
        {
            isLight = false;
        }
    }
}
