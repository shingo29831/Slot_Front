using GameMachine.InitialSettingView;
using GameMachine.Model;
using Networks;
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
        ////////追加///////
        bool inCredit = false;
        ///////////////////
        int credit;
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
            StartUp.SetInOnline(true);
            inCredit = false;
            var mainForm = this.Parent as StartUp;
            credit = -1;
            if (StartUp.Account == null)
            {
                StartUp.Account = await Account_SYS.getAccount_SYS(StartUp.Network);
            }
            try
            {
                await StartUp.Account.request_payment();
                //クレジットの値が更新されるまで無限回繰り返す
                while (!inCredit) 
                {
                    CreditCheck();
                    await Task.Delay(2500);
                }
                /////////////////////追加//////////////////////
                credit = await StartUp.Account.get_user_money();
                ///////////////////////////////////////////////
                //更新できた場合
                Game.IncreaseHasCoin(credit);
                mainForm.ShowCreditDisp();
                StartUp.SetInOnline(true);
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
            inCredit = await StartUp.Account.request_payment_exists();
            await Task.Delay(1000);
        }
    }
}