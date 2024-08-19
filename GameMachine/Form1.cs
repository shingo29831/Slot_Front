using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model.BusinessLogic;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        String uniqueID = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Model.BusinessLogic.Setting.maketableID();
            txtbox1.Text = Model.BusinessLogic.Setting.getTableID();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}