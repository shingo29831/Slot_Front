using GameMachine.Controller;
using GameMachine.InitialSettingView;
using GameMachine.View;
using System;
using System.Windows.Forms;
using static Constants;
using Networks;

namespace GameMachine
{
    public partial class StartUp : Form
    {
        

        private static SelectionController userSelectionScreen = new SelectionController(); // ���[�U�[�I����ʂ̃R���g���[���[

        private static AccountLinkingController accountLinkingScreen = new AccountLinkingController(); // �A�J�E���g�����N��ʂ̃R���g���[���[

        private static WaitLinkController waitLinkScreen = new WaitLinkController(); //�ҋ@��ʂ̃R���g���[���[

        private static WaitLogoutController waitLogoutController = new WaitLogoutController(); //���O�A�E�g�ҋ@��ʂ̃R���g���[���[

        private static ResultController resultScreen = new ResultController();

        private static SettingController settingController = new SettingController();   //�ݒ��ʂ̃R���g���[���[

        private static InCreditController inCreditController = new InCreditController(); // ���[�J��������ʂ̃R���g���[���[

        private static SlotSettingController slotSettingController = new SlotSettingController();//��ݒ�ύX��ʂ̃R���g���[���[

        //�W�v�i�J�E���^�[�j���
        private static CounterController counterDisplay = new CounterController();        // �J�E���^�[�\���R���g���[���[
        private static CounterView counterView = new CounterView(counterDisplay);


        //�X���b�g��ʁi�N���W�b�g��ʂƃJ�E���^�[View���t���j
        private static CreditController creditDisplay = new CreditController();          // �N���W�b�g�\���R���g���[���[
        public static CreditView creditView = new CreditView(creditDisplay);
        private static SlotController userGameScreen = new SlotController(creditView, counterView);       // �X���b�g�Q�[����ʂ̃R���g���[���[
        public static SlotView slotView = new SlotView(userGameScreen);

        static Account_SYS? account_ = null;

        static Network_sys? ns = null;

        private bool exitFlg=false;

        public static Account_SYS Account { get => account_; set => account_ = value; }

        public static Network_sys Network { get => ns; set => ns = value; } 
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


            slotView = new SlotView(userGameScreen);
        }

        private void StartUp_Load(object sender, EventArgs e)
        {
            ////////////////////////�ύX�ӏ�/////////////////////////
            SetSlotExpected();
            ////////////////////////�ύX�ӏ�/////////////////////////

            //���[�U�[�R���g���[�� �C���X�^���X
            //counterDisplay = new CounterController();        // �J�E���^�[�\�����
            //userSelectionScreen = new SelectionController(); // ���[�U�[�I�����
            //accountLinkingScreen = new AccountLinkingController(); // �A�J�E���g�����N���
            //creditDisplay = new CreditController();          // �N���W�b�g�\�����
            //userGameScreen = new SlotController(creditView);           // �X���b�g�Q�[�����



            //creditView = new CreditView(creditDisplay); //�r���[�C���X�^���X
            //counterView = new CounterView(counterDisplay);



            // �e���[�U�[�R���g���[���̏����T�C�Y�ƈʒu��ݒ�
            InitializeControlSettings();

            ///////////////////////////
            // ������ʕ\�����\�b�h  //
            ///////////////////////////
            ShowUserSelectionScreen();
            
            counterView.SwitchCounterUpdate();
        }

        private void InitializeControlSettings()
        {
            //���T�C�Y�̎��ɒ��������悤�ɐݒ�
            SetControlProperties(counterDisplay, new Size(1920, 1080), new Point(0, 0)); // �J�E���^�[�\��
            SetControlProperties(userSelectionScreen, new Size(1275, 700), new Point(325, 200)); // ���[�U�[�I��
            SetControlProperties(userGameScreen, new Size(1275, 875), new Point(325, 200)); // �X���b�g�Q�[��
            SetControlProperties(accountLinkingScreen, new Size(1275, 700), new Point(325, 200)); // �A�J�E���g�����N
            SetControlProperties(creditDisplay, new Size(1275, 149), new Point(325, 930)); // �N���W�b�g�\��
            SetControlProperties(resultScreen, new Size(1275, 730), new Point(325, 200)); // �N���W�b�g�\��
            SetControlProperties(waitLinkScreen, new Size(1275, 875), new Point(325, 200));//�ҋ@���
            SetControlProperties(waitLogoutController, new Size(1275, 875), new Point(325, 200));//���O�A�E�g�҂����
            SetControlProperties(settingController, new Size(1275, 875), new Point(325, 200));//�ݒ���
            SetControlProperties(inCreditController, new Size(1275, 875), new Point(325, 200));//�������
            SetControlProperties(slotSettingController, new Size(1275, 875), new Point(325, 200));//��ݒ���
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

            userGameScreen.SlotViewLoad(slotView); //slotView��n��
            slotView.InitialPictureSet(1, 3, 20); // �����V���{���\���ʒu

            //�{�^���\���؂�ւ�
            slotView.BtnChange(Reels.LEFT);
            slotView.BtnChange(Reels.CENTER);
            slotView.BtnChange(Reels.RIGHT);
        }

        public void ShowAccountLinkingScreen()
        {
            ShowUserControl(accountLinkingScreen); // �A�J�E���g�����N��ʂ�\��
            counterDisplay.Visible = true;      // �J�E���^�[�\����L����
            creditDisplay.BringToFront();    // �N���W�b�g�\����O�ʂɈړ�
        }

        public void ShowResultScreen()
        {
            ShowUserControl(resultScreen);
            resultScreen.BringToFront();
        }

        public void HideResultScreen()
        {
            if (resultScreen != null)
            {
                resultScreen.Visible = false;
            }
        }
        public void ShowWaitLinkScreen()
        {
            ShowUserControl(waitLinkScreen);    //�ҋ@��ʕ\��
            counterDisplay.Visible = true;      //�J�E���^�[��ʕ\��
            waitLinkScreen.BringToFront();      //�ҋ@��ʂ�O�ʂɈړ�
            waitLinkScreen.WaitLink();          //�ҋ@�����J�n
        }
        public void ShowWaitLogoutScreen()
        {
            ShowUserControl(waitLogoutController);  //���O�A�E�g�ҋ@��ʕ\��
            counterDisplay.Visible = true;          //�J�E���^�[����ʂ�\��(���̔w�i��\�����邽��
            waitLogoutController.BringToFront();    //�ҋ@��ʂ�O�ʂɈړ�
            waitLogoutController.WaitLogout();      //�ҋ@�����J�n
        }

        public void ShowSettingScreen()
        {
            ShowUserControl(settingController); //�ݒ��ʕ\��
            counterDisplay.Visible = true;      //�J�E���^�[��ʂ�\��
            settingController.BringToFront();   //�ݒ��ʂ�O�ʂɈړ�
        }

        public void ShowInCreditScreen()
        {
            ShowUserControl(inCreditController);    //���[�J��������ʕ\��
            counterDisplay.Visible = true;          //�J�E���^�[��ʂ�\��
            inCreditController.BringToFront();      //���[�J��������ʂ�O�ʂɈړ�
        }

        public void ShowSlotSettingScreen()
        {
            ShowUserControl(slotSettingController); //��ݒ��ʕ\��
            counterDisplay.Visible = true;          //�J�E���^�[��ʕ\��
            slotSettingController.BringToFront();   //��ݒ��ʂ�O�ʂɈړ�
        }
        public void ShowCreditDisp()
        {
            creditView.ShowCreditDisp();
        }


        private void StartUp_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void StartUp_KeyUp(object sender, KeyEventArgs e)
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

            if (e.KeyCode == Keys.OemSemicolon)
            {
                if (MessageBox.Show("���O�A�E�g���܂����H", "�Q�[��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ShowWaitLogoutScreen();
                }
            }
        }

        async private void SetSlotExpected()
        {
            int slotExpected;
            while (!exitFlg)
            {
                if (Network != null)
                {
                    slotExpected = await Networks.Table.get_probability(Network);
                    //Model.Setting.SetExpected(Convert.ToByte(slotExpected));
                }
                await Task.Delay(30000);
            }
            this.Close();
        }

        public void SetExitFlg(bool flg)
        {
            exitFlg= flg;
        }
    }
}
