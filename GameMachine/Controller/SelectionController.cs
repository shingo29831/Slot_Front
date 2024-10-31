﻿using System;
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
    }
}
