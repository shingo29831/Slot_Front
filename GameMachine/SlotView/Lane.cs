using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMachine.SlotView
{
    internal class Lane
    {
        public PictureBox[] picBoxes; // 各リールに対応する PictureBox 配列
        public Bitmap[] images; // リールの画像を管理する配列
        public int[] reelIndexes; // 各リールの現在のインデックスを追跡する配列
        public Random random = new Random(); // ランダム生成用

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        public Lane(PictureBox[] picBoxes, Bitmap[] images)
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        {
            this.picBoxes = picBoxes;
            this.images = images;
            reelIndexes = new int[picBoxes.Length];
            StartSpin(); // 初期化時にリールの位置をランダムに設定
        }

        public void StartSpin()
        {
            for (int i = 0; i < reelIndexes.Length; i++)
            {
                reelIndexes[i] = random.Next(images.Length);
                UpdateImage(i); // 初期位置を表示
            }
        }

        public void UpdateImage(int index)
        {
            if (picBoxes[index].IsHandleCreated) // ハンドルが作成されているか確認
            {
                picBoxes[index].Invoke((Action)(() =>
                {
                    picBoxes[index].Image = images[reelIndexes[index]];
                }));
            }
        }

        public void SetReelIndex(int index, int newIndex)
        {
            reelIndexes[index] = newIndex;
            UpdateImage(index);
        }
    }
}
