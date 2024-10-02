using GameMachine.InitialSettingView;
using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class StartUp : Form
    {
        private SelectionController userSelectionScreen; // ���[�U�[�I����ʂ̃R���g���[���[
        private SlotController userGameScreen;           // �X���b�g�Q�[����ʂ̃R���g���[���[
        private AccountLinkingController accountLinkingScreen; // �A�J�E���g�����N��ʂ̃R���g���[���[
        private CounterController counterDisplay;        // �J�E���^�[�\���R���g���[���[
        private CreditController creditDisplay;          // �N���W�b�g�\���R���g���[���[

        public StartUp()
        {
            InitializeComponent();

            // �N�����Ƀt���X�N���[��
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;

            // DPI �X�P�[�����O�Ή��ݒ�
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // �L�[�C�x���g�̐ݒ�
            this.KeyDown += new KeyEventHandler(StartUp_KeyDown);
        }

        private void StartUp_Load(object sender, EventArgs e)
        {
            //���[�U�[�R���g���[�� �C���X�^���X
            counterDisplay = new CounterController();        // �J�E���^�[�\�����
            userSelectionScreen = new SelectionController(); // ���[�U�[�I�����
            userGameScreen = new SlotController();           // �X���b�g�Q�[�����
            accountLinkingScreen = new AccountLinkingController(); // �A�J�E���g�����N���
            creditDisplay = new CreditController();          // �N���W�b�g�\�����

            // �e���[�U�[�R���g���[���̏����T�C�Y�ƈʒu��ݒ�
            InitializeControlSettings();

            ///////////////////////////
            // ������ʕ\�����\�b�h  //
            ///////////////////////////
            ShowUserSelectionScreen();
        }

        private void InitializeControlSettings()
        {
            //���T�C�Y�̎��ɒ��������悤�ɐݒ�
            SetControlProperties(counterDisplay, new Size(1920, 1080), new Point(0, 0)); // �J�E���^�[�\��
            SetControlProperties(userSelectionScreen, new Size(1275, 700), new Point(325, 200)); // ���[�U�[�I��
            SetControlProperties(userGameScreen, new Size(1275, 875), new Point(325, 200)); // �X���b�g�Q�[��
            SetControlProperties(accountLinkingScreen, new Size(1275, 875), new Point(325, 200)); // �A�J�E���g�����N
            SetControlProperties(creditDisplay, new Size(1275, 149), new Point(325, 930)); // �N���W�b�g�\��
        }

        private void SetControlProperties(Control control, Size size, Point location)
        {
            control.Size = size;      // �R���g���[���̃T�C�Y��ݒ�
            control.Location = location; // �R���g���[���̈ʒu��ݒ�
            control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // ���T�C�Y�Ή�
            this.Controls.Add(control); // �t�H�[���ɒǉ�
        }

        private void ShowUserControl(Control controlToShow)
        {
            //�\������
            foreach (Control control in this.Controls)
            {
                control.Visible = false; // ���̃R���g���[�����\���ɂ���
            }

            controlToShow.Visible = true; // �Ώۂ̃R���g���[����\��
            controlToShow.BringToFront(); // �Ώۂ̃R���g���[����O�ʂɕ\��
        }

        public void ShowUserSelectionScreen()
        {
            // counterDisplay �� userSelectionScreen ��\������
            counterDisplay.Visible = true;      // �J�E���^�[�\����L����
            userSelectionScreen.Visible = true; // ���[�U�[�I����ʂ�L����

            // ���ꂼ��O�ʂɕ\��
            counterDisplay.BringToFront();
            userSelectionScreen.BringToFront();
        }

        public void ShowUserGameScreen()
        {
            ShowUserControl(userGameScreen); // �Q�[����ʂ�\��
            counterDisplay.Visible = true;      // �J�E���^�[�\����L����
            creditDisplay.Visible = true;    // �N���W�b�g�\����L���ɂ���
            creditDisplay.BringToFront();    // �N���W�b�g�\����O�ʂɈړ�
        }

        public void ShowAccountLinkingScreen()
        {
            ShowUserControl(accountLinkingScreen); // �A�J�E���g�����N��ʂ�\��
        }

        private void StartUp_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc �L�[�������ꂽ��I���m�F�̃_�C�A���O��\��
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("�A�v���P�[�V�������I�����܂����H", "�Q�[��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close(); // �A�v���P�[�V�������I��
                }
            }
        }
    }
}
