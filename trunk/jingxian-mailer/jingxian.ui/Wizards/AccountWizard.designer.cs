namespace jingxian.ui
{
    partial class AccountWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountWizard));
            this.wizardControl1 = new jingxian.ui.controls.wizards.WizardControl();
            this.licenceStep1 = new jingxian.ui.controls.wizards.LicenceStep();
            this.intermediateStep1 = new jingxian.ui.controls.wizards.IntermediateStep();
            this.finishStep1 = new jingxian.ui.controls.wizards.FinishStep();
            this.wizardControl = new jingxian.ui.controls.wizards.WizardControl();
            this.welcomeStep = new jingxian.ui.controls.wizards.StartStep();
            this.proxyButton = new System.Windows.Forms.Button();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.MailBox_textBox = new System.Windows.Forms.TextBox();
            this.labelMail = new System.Windows.Forms.Label();
            this.smtpStep = new jingxian.ui.controls.wizards.IntermediateStep();
            this.smtpControl = new jingxian.ui.controls.ServerItemControl();
            this.popStep = new jingxian.ui.controls.wizards.IntermediateStep();
            this.popControl = new jingxian.ui.controls.ServerItemControl();
            this.finishStep = new jingxian.ui.controls.wizards.FinishStep();
            this.label1 = new System.Windows.Forms.Label();
            this.welcomeStep.SuspendLayout();
            this.smtpStep.SuspendLayout();
            this.popStep.SuspendLayout();
            this.finishStep.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackButtonEnabled = false;
            this.wizardControl1.BackButtonVisible = true;
            this.wizardControl1.CancelButtonEnabled = true;
            this.wizardControl1.CancelButtonVisible = true;
            this.wizardControl1.EulaButtonEnabled = true;
            this.wizardControl1.EulaButtonText = "eula";
            this.wizardControl1.EulaButtonVisible = true;
            this.wizardControl1.HelpButtonEnabled = true;
            this.wizardControl1.HelpButtonVisible = true;
            this.wizardControl1.Location = new System.Drawing.Point(187, 138);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.NextButtonEnabled = true;
            this.wizardControl1.NextButtonVisible = true;
            this.wizardControl1.Size = new System.Drawing.Size(534, 403);
            this.wizardControl1.WizardSteps.AddRange(new jingxian.ui.controls.wizards.WizardStep[] {
            this.licenceStep1,
            this.intermediateStep1,
            this.finishStep1});
            // 
            // licenceStep1
            // 
            this.licenceStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("licenceStep1.BindingImage")));
            this.licenceStep1.LicenseFile = "";
            this.licenceStep1.Name = "licenceStep1";
            this.licenceStep1.Title = "License Agreement.";
            this.licenceStep1.Warning = "Please read the following license agreement. You must accept the terms  of this a" +
                "greement before continuing.";
            this.licenceStep1.WarningFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            // 
            // intermediateStep1
            // 
            this.intermediateStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep1.BindingImage")));
            this.intermediateStep1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep1.Name = "intermediateStep1";
            // 
            // finishStep1
            // 
            this.finishStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("finishStep1.BindingImage")));
            this.finishStep1.Name = "finishStep1";
            // 
            // wizardControl
            // 
            this.wizardControl.BackButtonEnabled = true;
            this.wizardControl.BackButtonText = "< 上一步";
            this.wizardControl.BackButtonVisible = true;
            this.wizardControl.CancelButtonEnabled = true;
            this.wizardControl.CancelButtonText = "取消";
            this.wizardControl.CancelButtonVisible = true;
            this.wizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl.EulaButtonEnabled = true;
            this.wizardControl.EulaButtonText = "";
            this.wizardControl.EulaButtonVisible = true;
            this.wizardControl.FinishButtonText = "完成";
            this.wizardControl.HelpButtonEnabled = true;
            this.wizardControl.HelpButtonText = "帮助";
            this.wizardControl.HelpButtonVisible = true;
            this.wizardControl.Location = new System.Drawing.Point(0, 0);
            this.wizardControl.Name = "wizardControl";
            this.wizardControl.NextButtonEnabled = true;
            this.wizardControl.NextButtonText = "下一步 >";
            this.wizardControl.NextButtonVisible = true;
            this.wizardControl.Size = new System.Drawing.Size(564, 314);
            this.wizardControl.WizardSteps.AddRange(new jingxian.ui.controls.wizards.WizardStep[] {
            this.welcomeStep,
            this.smtpStep,
            this.popStep,
            this.finishStep});
            this.wizardControl.FinishButtonClick += new System.EventHandler(this.wizardControl_FinishButtonClick);
            this.wizardControl.NextButtonClick += new jingxian.ui.controls.wizards.GenericCancelEventHandler<jingxian.ui.controls.wizards.WizardControl>(this.wizardControl_NextButtonClick);
            this.wizardControl.CancelButtonClick += new System.EventHandler(this.wizardControl_CancelButtonClick);
            // 
            // welcomeStep
            // 
            this.welcomeStep.BindingImage = ((System.Drawing.Image)(resources.GetObject("welcomeStep.BindingImage")));
            this.welcomeStep.Controls.Add(this.proxyButton);
            this.welcomeStep.Controls.Add(this.Password_textBox);
            this.welcomeStep.Controls.Add(this.labelPassword);
            this.welcomeStep.Controls.Add(this.MailBox_textBox);
            this.welcomeStep.Controls.Add(this.labelMail);
            this.welcomeStep.Icon = ((System.Drawing.Image)(resources.GetObject("welcomeStep.Icon")));
            this.welcomeStep.Name = "welcomeStep";
            this.welcomeStep.Subtitle = "请输入你的邮件地址和密码，以便收发邮件。";
            this.welcomeStep.Title = "添加邮件地址";
            // 
            // proxyButton
            // 
            this.proxyButton.BackColor = System.Drawing.SystemColors.Control;
            this.proxyButton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.proxyButton.Location = new System.Drawing.Point(336, 153);
            this.proxyButton.Name = "proxyButton";
            this.proxyButton.Size = new System.Drawing.Size(164, 33);
            this.proxyButton.TabIndex = 4;
            this.proxyButton.Text = "代理设置...";
            this.proxyButton.UseVisualStyleBackColor = false;
            this.proxyButton.Click += new System.EventHandler(this.proxyButton_Click);
            // 
            // Password_textBox
            // 
            this.Password_textBox.Location = new System.Drawing.Point(255, 111);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.Size = new System.Drawing.Size(245, 21);
            this.Password_textBox.TabIndex = 3;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(214, 114);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(35, 12);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "密码:";
            // 
            // MailBox_textBox
            // 
            this.MailBox_textBox.Location = new System.Drawing.Point(255, 80);
            this.MailBox_textBox.Name = "MailBox_textBox";
            this.MailBox_textBox.Size = new System.Drawing.Size(245, 21);
            this.MailBox_textBox.TabIndex = 1;
            // 
            // labelMail
            // 
            this.labelMail.AutoSize = true;
            this.labelMail.Location = new System.Drawing.Point(190, 83);
            this.labelMail.Name = "labelMail";
            this.labelMail.Size = new System.Drawing.Size(59, 12);
            this.labelMail.TabIndex = 0;
            this.labelMail.Text = "邮箱地址:";
            // 
            // smtpStep
            // 
            this.smtpStep.BindingImage = ((System.Drawing.Image)(resources.GetObject("smtpStep.BindingImage")));
            this.smtpStep.Controls.Add(this.smtpControl);
            this.smtpStep.ForeColor = System.Drawing.SystemColors.ControlText;
            this.smtpStep.Name = "smtpStep";
            this.smtpStep.Subtitle = "请验证你的 SMTP 服务器的设置，如果你确定它是正确的请点击 下一步 按钮。";
            this.smtpStep.Title = "SMTP 设置";
            // 
            // smtpControl
            // 
            this.smtpControl.Location = new System.Drawing.Point(10, 70);
            this.smtpControl.Name = "smtpControl";
            this.smtpControl.Size = new System.Drawing.Size(550, 200);
            this.smtpControl.TabIndex = 0;
            // 
            // popStep
            // 
            this.popStep.BindingImage = ((System.Drawing.Image)(resources.GetObject("popStep.BindingImage")));
            this.popStep.Controls.Add(this.popControl);
            this.popStep.ForeColor = System.Drawing.SystemColors.ControlText;
            this.popStep.Name = "popStep";
            this.popStep.Subtitle = "请验证你的 POP 服务器的设置，如果你确定它是正确的请点击 下一步 按钮。";
            this.popStep.Title = "POP 设置";
            // 
            // popControl
            // 
            this.popControl.Location = new System.Drawing.Point(10, 70);
            this.popControl.Name = "popControl";
            this.popControl.Size = new System.Drawing.Size(550, 200);
            this.popControl.TabIndex = 0;
            // 
            // finishStep
            // 
            this.finishStep.BindingImage = ((System.Drawing.Image)(resources.GetObject("finishStep.BindingImage")));
            this.finishStep.Controls.Add(this.label1);
            this.finishStep.Name = "finishStep";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "邮件帐户设置完成了，点击“确定”退出设置。";
            // 
            // AccountWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 314);
            this.Controls.Add(this.wizardControl);
            this.Controls.Add(this.wizardControl1);
            this.Name = "AccountWizard";
            this.Text = "新增邮件帐户向导...";
            this.welcomeStep.ResumeLayout(false);
            this.welcomeStep.PerformLayout();
            this.smtpStep.ResumeLayout(false);
            this.popStep.ResumeLayout(false);
            this.finishStep.ResumeLayout(false);
            this.finishStep.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private jingxian.ui.controls.wizards.WizardControl wizardControl1;
        private jingxian.ui.controls.wizards.LicenceStep licenceStep1;
        private jingxian.ui.controls.wizards.IntermediateStep intermediateStep1;
        private jingxian.ui.controls.wizards.FinishStep finishStep1;
        private jingxian.ui.controls.wizards.WizardControl wizardControl;
        private jingxian.ui.controls.wizards.StartStep welcomeStep;
        private jingxian.ui.controls.wizards.IntermediateStep smtpStep;
        private jingxian.ui.controls.wizards.IntermediateStep popStep;
        private jingxian.ui.controls.wizards.FinishStep finishStep;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox MailBox_textBox;
        private System.Windows.Forms.Label labelMail;
        private System.Windows.Forms.Button proxyButton;
        private jingxian.ui.controls.ServerItemControl smtpControl;
        private jingxian.ui.controls.ServerItemControl popControl;
        private System.Windows.Forms.Label label1;

    }
}