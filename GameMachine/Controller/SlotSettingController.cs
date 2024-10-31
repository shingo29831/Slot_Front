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

        private void OkButton_Click(object sender, EventArgs e)
        {
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
    }
}
