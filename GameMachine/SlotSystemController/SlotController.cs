using System.Threading;
using GameMachine.SlotView;

namespace GameMachine.SlotSystemController
{
    internal class SlotController
    {
        private Lever lever;

        public SlotController(Lever lever)
        {
            this.lever = lever;
        }

        public void DownLever()
        {
            lever.StartReels();
        }

        public void StopAllReels()
        {
            lever.StopAllReels();
        }

        public void StopSpecificReel(int reelNumber)
        {
            lever.StopSpecificReel(reelNumber);
        }
    }
}
