using System.Threading;
using System.Threading.Tasks;
using GameMachine.SlotView;

namespace GameMachine.SlotView
{
    internal class Lever
    {
        private Lane lane1;
        private Lane lane2;
        private Lane lane3;
        private CancellationTokenSource[] cancellationTokenSources;

        public Lever(Lane lane1, Lane lane2, Lane lane3)
        {
            this.lane1 = lane1;
            this.lane2 = lane2;
            this.lane3 = lane3;
            cancellationTokenSources = new CancellationTokenSource[3]; // 各リールのキャンセル用
        }

        public void StartReels()
        {
            for (int i = 0; i < 3; i++)
            {
                cancellationTokenSources[i] = new CancellationTokenSource();
            }

            Task.Run(() => SpinReel(lane1, cancellationTokenSources[0].Token));
            Task.Run(() => SpinReel(lane2, cancellationTokenSources[1].Token));
            Task.Run(() => SpinReel(lane3, cancellationTokenSources[2].Token));
        }

        private async Task SpinReel(Lane lane, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < lane.picBoxes.Length; i++)
                {
                    lane.SetReelIndex(i, (lane.reelIndexes[i] + 1) % lane.images.Length);
                }

                try
                {
                    await Task.Delay(120, cancellationToken); // 速度調整
                }
                catch (TaskCanceledException)
                {
                    break; // タスクがキャンセルされた場合、ループを終了
                }
            }
        }

        public void StopAllReels()
        {
            foreach (var cancellationTokenSource in cancellationTokenSources)
            {
                cancellationTokenSource?.Cancel();
            }
        }

        public void StopSpecificReel(int reelNumber)
        {
            cancellationTokenSources[reelNumber-1]?.Cancel(); // 指定されたリールのキャンセル
        }
    }
}
