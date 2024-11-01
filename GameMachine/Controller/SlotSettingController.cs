using GameMachine.Model;
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

    public partial class SlotSettingController : UserControl
    {
        private int tableId;
        int MachineSpec = -1;
        public SlotSettingController()
        {
            InitializeComponent();
        }
        private void SlotSetting_Load(object sender, EventArgs e)
        {
            //MachineSpec=(int)Setting.GetExpected();
            comboBox1.SelectedIndex = MachineSpec;
        }

        private async void OkButton_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                int slotExpected = await Networks.Table.get_probability(StartUp.Network);
                Model.Setting.SetExpected(Convert.ToByte(slotExpected));
            }
            //Setting.SetExpected(Convert.ToByte(comboBox1.Text));
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowUserSelectionScreen();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowSettingScreen();
            }

        }
        ///////////////////////////////////変更箇所////////////////////////////////////////

        private void PressKey_ValueCheck(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || '9' < e.KeyChar)
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }

        private void KeyUpCheck_Enter(object sender, KeyEventArgs e)
        {
            textBox1.Text = textBox1.Text.Replace(Environment.NewLine, "");
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("テーブル番号を登録しました", "success", MessageBoxButtons.OK);
                tableId = int.Parse(textBox1.Text);
                String table = new StringBuilder("table_").Append(tableId).ToString();
                StartUp.Network = new Network.Network_sys(table, table);
                textBox1.Clear();
            }
        }
        public int getTableId()
        {
            return tableId;
        }
        ///////////////////////////////////変更箇所////////////////////////////////////////
    }
}
