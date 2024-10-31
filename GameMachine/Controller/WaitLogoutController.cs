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
    public partial class WaitLogoutController : UserControl
    {
        bool logoutFlg;
        public WaitLogoutController()
        {
            InitializeComponent();
        }
        async public void WaitLogout()
        {
            logoutFlg = false;
            var mainForm = this.Parent as StartUp;
            try
            {
                while (!logoutFlg)
                {
                    LogoutCheck();
                    await Task.Delay(1500);
                    logoutFlg = true;
                }
                MessageBox.Show("ログアウト完了しました", "success", MessageBoxButtons.OK);
                if (mainForm != null)
                {
                    mainForm.ShowUserSelectionScreen();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("ログアウトに失敗しました。\nスタッフをお呼びください。", "Logout_Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(mainForm != null)
                {
                    mainForm.ShowUserSelectionScreen();
                }              
            }
        }

        async private void LogoutCheck()　
        {
            try
            {
                //蟹江君のバックへの通信処理 成功時true 失敗時false をlogoutflgに入れる
                await Task.Delay(1000);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
