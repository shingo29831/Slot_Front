using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Constants;

namespace GameMachine.View
{
    class ResultView
    {
        private PictureBox ResultPicture;
        private readonly Dictionary<byte, Bitmap> ResultPictureData;
        
        public ResultView(PictureBox resultpicture)
        {
            ResultPicture = resultpicture;
            //画像の追加はここから
            ResultPictureData = new Dictionary<byte, Bitmap>
        {
            { 1, new Bitmap(Properties.Resources.AmusementPark_1)},
            { 2, new Bitmap(Properties.Resources.AmusementPark_2)},
            { 3, new Bitmap(Properties.Resources.AmusementPark_3)},
            { 4, new Bitmap(Properties.Resources.AmusementPark_4)},
            { 5, new Bitmap(Properties.Resources.AmusementPark_5_6)},
            { 6, new Bitmap(Properties.Resources.AmusementPark_5_6)}
        };
        }
        //設定値が送られてくるとそれに合わせた画像を表示する
        public void ResultPictureSwitching(byte SetValue)
        {
            ResultPicture.Image = ResultPictureData[SetValue];
            ResultPicture.Invalidate(); // 画面の再描画を強制する
            ResultPicture.Refresh();
        }
    }
}
