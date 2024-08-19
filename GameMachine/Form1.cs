using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        private UserSelectionScren userSelectionScren; // �t�B�[���h�Ƃ��Ē�`
        private UserGameScreen userGameScreen;         // �t�B�[���h�Ƃ��Ē�`
        private CounterDisplay counterDisplay;         // �t�B�[���h�Ƃ��Ē�`

        public Form1()
        {
            InitializeComponent();
            // �N�����t���X�N���[��
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // �L�[�C�x���g�����ׂẴt�H�[���Ŏ󂯎�鏈��
            this.KeyPreview = true;

#nullable disable
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ���[�U�[�R���g���[�����C���X�^���X��
            counterDisplay = new CounterDisplay();
            userSelectionScren = new UserSelectionScren();
            userGameScreen = new UserGameScreen();

            // counterDisplay �̃T�C�Y��ʒu��ݒ�
            counterDisplay.Size = new Size(1920, 1080);
            counterDisplay.Location = new Point(0, 0);

            // userSelectionScren �̃T�C�Y��ʒu��ݒ�
            userSelectionScren.Size = new Size(1425, 750);
            userSelectionScren.Location = new Point(250, 200);

            // userGameScreen �̃T�C�Y��ʒu��ݒ�
            userGameScreen.Size = new Size(1425, 750);
            userGameScreen.Location = new Point(250, 200);

            // �t�H�[���ɒǉ�
            this.Controls.Add(counterDisplay);
            this.Controls.Add(userSelectionScren);

            userSelectionScren.BringToFront(); // �����\���� userSelectionScren
        }

        public void ShowUserGameScreen()
        {
            // UserSelectionScren ���\���ɂ��� UserGameScreen ��\������
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(userGameScreen);
            userGameScreen.BringToFront();
        }

        public void ShowUserSelectionScren()
        {
            // UserGameScreen ���\���ɂ��� UserSelectionScren ��\������
            this.Controls.Remove(userGameScreen);
            this.Controls.Add(userSelectionScren);
            userSelectionScren.BringToFront();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // �A�v���P�[�V�����I������
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("�A�v���P�[�V�������I�����܂����H", "�Q�[��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}
