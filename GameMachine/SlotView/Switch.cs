using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMachine.SlotView
{
    internal class Switch : SlotModule
    {
        private bool pressed;

        public void SwitchOn()
        {
            pressed = true;
        }

        public void SwitchOff()
        {
            pressed = false;
        }
    }
}
