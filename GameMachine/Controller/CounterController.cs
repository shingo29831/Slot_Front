using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class CounterController : UserControl
    {
        //変数宣言
        private int BigBounusCount = 0;
        private int RegularBounusCount = 0;
        private int BetweenBounusCount = 0;


        public CounterController()
        {
            InitializeComponent();
            // フォームのロード時にテキストボックスを無効に設定
            BigBonusTxb.Enabled = false;
            RegularBonusTxb.Enabled = false;
            BetweenBonusTbx.Enabled = false;
        }

        private void CounterController_Load(object sender, EventArgs e)
        {

        }

        //メソッド
        /*public switchBigBonusDisplay()
        {

        }
        public switchRegularBonusDisplay()
        {

        }
        public switchBetweenBonusDisplay()
        {

        }*/
    }
}

