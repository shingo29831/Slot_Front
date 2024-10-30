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

            //クレジットの値の更新と待機処理
            for (int i = 0; i < 3 && credit == -1; i++)
            {
                CreditCheck();
                await Task.Delay(1500);                    
            }
            //Game.IncreaseHasCoin(credit);

            //更新できなかった場合
            if (credit == -1)
            {
                //ゲスト等選択画面に戻る
                if (mainForm != null)
                {
                    mainForm.ShowUserSelectionScreen();
                }
            }
            //更新できた場合
            else
            {
                //ゲーム開始画面に行く
                if (mainForm != null)
                {
                    mainForm.ShowUserGameScreen();
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