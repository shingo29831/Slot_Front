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
        private static StartUp StartUp;
        private static bool OnLineFlag = false;
        public GameEndController()
        {
            InitializeComponent();
            StartUp = new StartUp();
        }

        private void GameEndController_Load(object sender, EventArgs e)
        {
            //残高
            Credit.Text = Game.GetHasCoin().ToString();
        }

        
        private void EndButton_Click(object sender, EventArgs e)
        {
            if (true)//オンライン
            {
                StartUp.ShowWaitLogoutScreen();
            }
            else//オフライン
            {
                var mainForm = this.Parent as StartUp;
                if (mainForm != null)
                {
                    mainForm.ShowUserSelectionScreen();
                }
            }
            
        }

        //オンライン状態絵をセットする
        public void SetOnlineFlag(bool Flag)
        {
            OnLineFlag = Flag;
        }
    }
}
