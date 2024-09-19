using GameMachine.InitialSettingView;
using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        private SelectionController userSelectionScren; // �t�B�[���h�Ƃ��Ē�`
        private SlotController userGameScreen;         // �t�B�[���h�Ƃ��Ē�`
        private AccountLinkingController accountLinkingScreen; //��`
        private CounterController counterDisplay;         // �t�B�[���h�Ƃ��Ē�`
        private CreditController creditDisplay;

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
            counterDisplay = new CounterController();
            userSelectionScren = new SelectionController();
            userGameScreen = new SlotController();
            accountLinkingScreen = new AccountLinkingController();
            creditDisplay = new CreditController();

            // counterDisplay �̃T�C�Y��ʒu��ݒ�
            counterDisplay.Size = new Size(1920, 1080);
            counterDisplay.Location = new Point(0, 0);

            // userSelectionScren �̃T�C�Y��ʒu��ݒ�
            userSelectionScren.Size = new Size(1275, 700);
            userSelectionScren.Location = new Point(325, 200);

            // userGameScreen �̃T�C�Y��ʒu��ݒ�
            userGameScreen.Size = new Size(1275, 875);
            userGameScreen.Location = new Point(325, 200);

            // accountLinkingScreen �̃T�C�Y��ʒu��ݒ�
            accountLinkingScreen.Size = new Size(1275, 875);
            accountLinkingScreen.Location = new Point(325, 200);

            // creditDisplayn �̃T�C�Y��ʒu��ݒ�
            creditDisplay.Size = new Size(1275, 175);
            creditDisplay.Location = new Point(325, 900);

            // �t�H�[���ɒǉ�
            this.Controls.Add(counterDisplay);
            this.Controls.Add(userSelectionScren);

            userSelectionScren.BringToFront(); // �����\���� userSelectionScren
        }

        public void ShowUserGameScreen()
        {
            // UserSelectionScren ���\���ɂ��� UserGameScreen �Ɓ@creditDisplay��\������
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(userGameScreen);
            this.Controls.Add(creditDisplay);
            userGameScreen.BringToFront();
            creditDisplay.BringToFront();
        }

        public void ShowUserSelectionScren()
        {
            // UserGameScreen ���\���ɂ��� UserSelectionScren ��\������
            if (accountLinkingScreen.Visible)
            {
                // userScreen1 ���\������Ă���ꍇ�̏���
                this.Controls.Remove(accountLinkingScreen);
                this.Controls.Add(userSelectionScren);
                userSelectionScren.BringToFront();
            }
            else if (userGameScreen.Visible)
            {
                // userScreen2 ���\������Ă���ꍇ�̏���
                this.Controls.Remove(userGameScreen);
                this.Controls.Add(userSelectionScren);
                userSelectionScren.BringToFront();
            }
        }

        public void ShowAccountLinkingScreen()
        {
            //UserSelectionScren ���\���ɂ��� AccountLinkingScreen ��\������
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(accountLinkingScreen);
            accountLinkingScreen.BringToFront();
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
