using GameMachine.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace GameMachine.Controller
{
    public partial class WaitLinkController : UserControl
    {
        int credit = -1;
        public WaitLinkController()
        {
            InitializeComponent();
        }

        private void WaitLinkController_Load(object sender, EventArgs e)
        {

        }

        //バックとフロントのクレジットの値を同期させる
        async public void WaitLink()
        {
            var mainForm = this.Parent as StartUp;

            try
            {
                //クレジットの値が更新されるまで無限回繰り返す
                while (credit == -1) 
                {
                    CreditCheck();
                    await Task.Delay(1500);
                    credit = 100;
                }
                //更新できた場合
                //Game.IncreaseHasCoin(credit);
                //ゲーム開始画面に行く
                if (mainForm != null)
                {
                    mainForm.ShowUserGameScreen();
                }
            }
            catch(Exception e)
            {
                //更新できなかった場合
                MessageBox.Show("クレジットを追加できませんでした。\nスタッフをお呼びください。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //ゲスト等選択画面に戻る
                    if (mainForm != null)
                {
                    mainForm.ShowUserSelectionScreen();
                }
            }              
        }

        //クレジットの値を取得
        async private void CreditCheck()
        {
            //credit=蟹江君の通信メソッド
            await Task.Delay(1000);
        }
    }
}