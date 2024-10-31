using GameMachine.Controller;
using GameMachine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Constants;

namespace GameMachine.View
{
    public class ResultView
    {
        private bool nextResultEnabled;
        //private PictureBox ResultPicture;
        private readonly Dictionary<byte, Bitmap> ResultPictureData;

        ResultController resultController;
        public ResultView(ResultController resultController)
        {
            this.resultController = resultController;
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
            resultController.ResultPictureBox.Image = ResultPictureData[SetValue];
            resultController.ResultPictureBox.Invalidate(); // 画面の再描画を強制する
            resultController.ResultPictureBox.Refresh();
        }

        public void Result()
        {
            //resultController.ResultsDisplay();
            bool currentBonusState = Game.GetInBonus();

            // ボーナスが終了したときのみリザルト画面を表示
            if (!currentBonusState)
            {
                ResultPictureSwitching(Game.GetSuggestionImage(Setting.GetExpected()));
                resultController.ResultsDisplay();
            }


            //previousBonusState = currentBonusState; //状態を更新
        }

        public void SetNextResultEnabled(bool enabled)
        {
            //this.setNextResultEnabled = enabled;
        }
    }
}
