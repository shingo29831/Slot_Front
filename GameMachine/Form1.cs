namespace GameMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //起動時フルスクリーン
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            //キーイベントをすべてのフォームで受け取る処理
            this.KeyPreview = true;
            //null許容参照型
            #nullable disable
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //アプリケーション終了処理
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("アプリケーションを終了しますか？", "ゲーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

    }
}