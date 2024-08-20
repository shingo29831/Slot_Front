using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model;

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
            Model.Setting.maketableID();
            txtbox1.Text = Model.Setting.getTableID();

        }

        //test
        static int[] left = new int[3];
        static int[] center = { 1, 1, 2 };
        static int[] right = { 1, 1, 2 };

        static int[,] tes = new int[3,3];

        private void button1_Click(object sender, EventArgs e)
        {
            tes = Model.Game.GetReachRows(left, center, right);
            txtbox1.Text = Model.Game.GetReachRows(left,center,right).ToString();
        }
    }
}