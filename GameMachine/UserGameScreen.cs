namespace GameMachine
{
    public partial class UserGameScreen : UserControl
    {
        Button btnStart = new Button();
        Button[] stopBtns = new Button[3];
        Bitmap[] imgPathReel1 = new Bitmap[5]; // 1リール用
        Bitmap[] imgPathReel2 = new Bitmap[5]; // 2リール用
        Bitmap[] imgPathReel3 = new Bitmap[5]; // 3リール用
        private PictureBox[] picBoxesReel1;
        private PictureBox[] picBoxesReel2;
        private PictureBox[] picBoxesReel3;
        private int[] reelIndexes; // 各リールの現在のインデックスを追跡する配列
        private Random random = new Random();

        public UserGameScreen()
        {
            InitializeComponent();
            picBoxesReel1 = new PictureBox[] { picBox1, picBox4, picBox7 };
            picBoxesReel2 = new PictureBox[] { picBox2, picBox5, picBox8 };
            picBoxesReel3 = new PictureBox[] { picBox3, picBox6, picBox9 };
            stopBtns = new Button[] { btnstop1, btnstop2, btnstop3 };
            reelIndexes = new int[] { 0, 0, 0 };
        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            // 1リールの画像
            imgPathReel1[0] = Properties.Resources.seven;
            imgPathReel1[1] = Properties.Resources.bar;
            imgPathReel1[2] = Properties.Resources.cherry;
            imgPathReel1[3] = Properties.Resources.bell;
            imgPathReel1[4] = Properties.Resources.watermelon;

            // 2リールの画像
            imgPathReel2[0] = Properties.Resources.cherry;
            imgPathReel2[1] = Properties.Resources.watermelon;
            imgPathReel2[2] = Properties.Resources.bar;
            imgPathReel2[3] = Properties.Resources.bell;
            imgPathReel2[4] = Properties.Resources.seven;

            // 3リールの画像
            imgPathReel3[0] = Properties.Resources.cherry;
            imgPathReel3[1] = Properties.Resources.bar;
            imgPathReel3[2] = Properties.Resources.watermelon;
            imgPathReel3[3] = Properties.Resources.seven;
            imgPathReel3[4] = Properties.Resources.bell;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            foreach (var btn in stopBtns)
            {
                btn.Enabled = true;
            }

            // 各リールのスタート位置をランダムに設定
            for (int i = 0; i < reelIndexes.Length; i++)
            {
                reelIndexes[i] = random.Next(imgPathReel1.Length);
            }

            while (true)
            {
                if (stopBtns.All(btn => !btn.Enabled))
                {
                    btnStart.Enabled = true;
                    break;
                }

                // 各リールの画像を表示
                for (int i = 0; i < stopBtns.Length; i++)
                {
                    if (stopBtns[i].Enabled)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            switch (i)
                            {
                                case 0: // 1リール
                                    picBoxesReel1[j].Image = imgPathReel1[(reelIndexes[i] + j) % imgPathReel1.Length];
                                    break;
                                case 1: // 2リール
                                    picBoxesReel2[j].Image = imgPathReel2[(reelIndexes[i] + j) % imgPathReel2.Length];
                                    break;
                                case 2: // 3リール
                                    picBoxesReel3[j].Image = imgPathReel3[(reelIndexes[i] + j) % imgPathReel3.Length];
                                    break;
                            }
                        }

                        //配列の末尾に到達したら先頭に移動
                        reelIndexes[i] = (reelIndexes[i] + 1) % imgPathReel1.Length;
                    }
                }
                await Task.Delay(120); //速度調整
            }
        }

        private void stopBtns_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
        }
    }
}
