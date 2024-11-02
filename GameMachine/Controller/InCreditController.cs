using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMachine.Model;
using GameMachine.View;

namespace GameMachine.Controller
{
    public partial class InCreditController : UserControl
    {
        StartUp mainForm;
        private bool keyPushEnabled = true;
        private String preText = "";
        public InCreditController()
        {
            InitializeComponent();
            
            mainForm = this.Parent as StartUp;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowSettingScreen();
                textBox1.Clear();
            }
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            bool checkFlg = false;
            textBox1.Text = textBox1.Text.Replace(Environment.NewLine, "");
            try
            {
                int credit = int.Parse(textBox1.Text);

                checkFlg = Game.SetHasCoin(credit / 2);
                if (checkFlg)
                {
                    MessageBox.Show("入金に成功しました。", "success", MessageBoxButtons.OK);
                    StartUp.SetInOnline(false);
                    textBox1.Clear();
                    var mainForm = this.Parent as StartUp;
                    if (mainForm != null)
                    {
                        mainForm.ShowUserGameScreen();
                    }
                    else
                    {
                        MessageBox.Show("入金に失敗しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("入金に失敗しました。", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
            }
        }

        private void ValueCheck_KeyPress(object sender, KeyPressEventArgs e)
        {
            //keyPushEnabled = true;
            //if ((e.KeyChar < '0' || '9' < e.KeyChar) && keyPushEnabled)
            //{
            //    //押されたキーが 0～9でない場合は、イベントをキャンセルする
            //    e.Handled = true;
            //    keyPushEnabled = false;
            //}
            if((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z'))
            {
                e.Handled = true;
            }
        }

        private void InCreditKeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                OK_Button_Click(sender, e);
            }


        }

        public void TextBoxFocus()
        {
            this.textBox1.Focus();
        }

        
    }
}
