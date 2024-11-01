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

namespace GameMachine.Controller
{
    public partial class InCreditController : UserControl
    {
        public InCreditController()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
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

                checkFlg = Game.SetHasCoin(credit /2);
                if (checkFlg)
                {
                    MessageBox.Show("入金に成功しました。", "success", MessageBoxButtons.OK);
                    textBox1.Clear();
                    var mainForm = this.Parent as StartUp;
                    if (mainForm != null)
                    {
                        mainForm.ShowUserGameScreen();
                    }
                    else
                    {
                        MessageBox.Show("入金に失敗しました。","error",MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (e.KeyChar < '0' || '9' < e.KeyChar)
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }
    }
}
