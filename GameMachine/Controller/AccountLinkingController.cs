using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void RegistrationBtn_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowUserSelectionScreen();
            }
        }
    }
}
