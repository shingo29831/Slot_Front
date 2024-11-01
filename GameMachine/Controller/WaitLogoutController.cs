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
    public partial class WaitLogoutController : UserControl
    {
        bool logoutFlg;
        StartUp mainForm;
        public WaitLogoutController()
        {
            InitializeComponent();
        }
        public void ActivateController()
        {
            this.Focus();
            mainForm = this.Parent as StartUp;
        }

        async public void WaitLogout()
        {
            
            ////////////////////////////////変更箇所//////////////////////////////
            try
            {
                await StartUp.Account.logout_request();
                while (!await StartUp.Account.logout_isdone())
                {
                    await Task.Delay(2000);
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
            /////////////////////////////////変更箇所//////////////////////////////
        }

        public void EndLocalGame()
        {
            MessageBox.Show("ログアウト完了しました", "success", MessageBoxButtons.OK);
            mainForm.ShowUserSelectionScreen();
        }
    }
}
