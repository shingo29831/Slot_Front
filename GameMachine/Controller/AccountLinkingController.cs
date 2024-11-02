using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Networks;

namespace GameMachine.InitialSettingView
{
    public partial class AccountLinkingController : UserControl
    {
        public AccountLinkingController()
        {
            InitializeComponent();
        }

        private void AccountLinkingScreen_Load(object sender, EventArgs e)
        {

        }
        //////////////////////////////////////変更箇所//////////////////////////////////////////////////
        async private void RegistrationBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(String.IsNullOrEmpty(UserNameTxb.Text)) && !(String.IsNullOrEmpty(PasswordTxb.Text)))
                {
                    UserNameTxb.Text = UserNameTxb.Text.Replace(Environment.NewLine, "");
                    PasswordTxb.Text = PasswordTxb.Text.Replace(Environment.NewLine, "");
                    StartUp.Account = await Account_SYS.getAccount_SYS(UserNameTxb.Text, PasswordTxb.Text, StartUp.Network);

                    var mainForm = this.Parent as StartUp;

                    if (mainForm != null)
                    {
                        mainForm.ShowWaitLinkScreen();
                    }
                }
                else
                {
                    MessageBox.Show("入力値に空があります。", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ユーザーネーム,パスワードが登録されたものと異なります。", "Error", MessageBoxButtons.OK);
                UserNameTxb.Clear();
                PasswordTxb.Clear();
            }


        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            UserNameTxb.Clear();
            PasswordTxb.Clear();
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowUserSelectionScreen();
            }
        }
        ////////////////////////////////////////////変更箇所/////////////////////////////////////
    }
}
