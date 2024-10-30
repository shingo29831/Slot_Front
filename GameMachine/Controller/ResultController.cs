using GameMachine.Model;
using GameMachine.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMachine.Controller
{
    public partial class ResultController : UserControl
    {

        private ResultView resultView;
        private byte SetValue = 5;


        private sbyte EndResultCount = 0;
        public ResultController()
        {
            InitializeComponent();
            resultView = new ResultView(ResultPictureBox);
        }

        private void ResultController_Load(object sender, EventArgs e)
        {
            label1.Parent = ResultPictureBox;
            label1.BackColor = Color.Transparent;
            label2.Parent = ResultPictureBox;
            label2.BackColor = Color.Transparent;

            EndResultDisplayTimer.Enabled = true;
            resultView.ResultPictureSwitching(SetValue);
        }
        //ボーナスが終了して呼び出される
        public void ResultsDisplay()
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowResultScreen();
            }
            
        }

        private void EndResultDisplayTimer_Tick(object sender, EventArgs e)
        {
            EndResultCount++;
            if (EndResultCount == 7)
            {
                EndResultCount = 0;
                EndResultDisplayTimer.Enabled = true;
                //スクリーン切り替え
                var mainForm = this.Parent as StartUp;
                if (mainForm != null)
                {
                    mainForm.HideResultScreen();
                }
            }
        }
    }
}
