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
using static GameMachine.Model.Game;

namespace GameMachine.Controller
{
    public partial class GameEndController : UserControl
    {
        public GameEndController()
        {
            InitializeComponent();
        }

        private void GameEndController_Load(object sender, EventArgs e)
        {
            Credit.Text = Game.GetHasCoin().ToString();
        }

        private void EndButton_Click(object sender, EventArgs e)
        {
            var mainForm = this.Parent as StartUp;
            if (mainForm != null)
            {
                mainForm.ShowUserSelectionScreen();
            }
        }
    }
}
