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
    public partial class CounterController : UserControl
    {
        //変数宣言



        public CounterController()
        {
            InitializeComponent();
            // フォームのロード時にテキストボックスを無効に設定
            BigCounterTxb.Enabled = false;
            RegularCounterTxb.Enabled = false;
            BetweenBonusTbx.Enabled = false;
        }

        private void CounterController_Load(object sender, EventArgs e)
        {

        }

        
    }
}

