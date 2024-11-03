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
    public partial class SelectionController : UserControl
    {
        public SelectionController()
        {
            InitializeComponent();
        }

        //アカウント
        private void selectAccountPlay_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowAccountLinkingScreen();
            }
        }

        //ゲストプレイ
        private void selectGustPlay_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowWaitLinkScreen();
            }
        }

        private void UserSelectionScren_Load(object sender, EventArgs e)
        {

        }

        private void SettingCheck(object sender, KeyPressEventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (e.KeyChar == (char)Keys.Enter)
            {
                string s = textBox1.Text;
                s = s.Replace(Environment.NewLine, "");
                if (s.Equals("Setting"))
                {
                    if(mainForm != null)
                    {
                        mainForm.ShowSettingScreen();
                    }
                    
                }
                else if (s.Equals("Exit"))
                {
                    if(DialogResult.Yes == MessageBox.Show("アプリケーションを終了しますか？","ゲーム",MessageBoxButtons.YesNo,MessageBoxIcon.Question))
                    {
                        if(mainForm != null)
                        {
                            mainForm.SetExitFlg(true);
                        }
                    }
                }
                textBox1.Clear();
            }
            
        }
    }
}
