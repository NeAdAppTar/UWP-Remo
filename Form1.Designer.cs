namespace UWPCleaner
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.CheckedListBox listBox;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label label;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label = new Label();
            listBox = new CheckedListBox();
            progressBar = new ProgressBar();
            removeButton = new Button();
            refreshButton = new Button();
            logBox = new TextBox();
            SuspendLayout();
            // 
            // label
            // 
            label.Dock = DockStyle.Top;
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label.Location = new Point(0, 0);
            label.Name = "label";
            label.Size = new Size(640, 46);
            label.TabIndex = 5;
            label.Text = "Выберите UWP приложения, которые хотите удалить.\r\nОбратите внимание, что приложения удаляются не для всех пользователей!";
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listBox
            // 
            listBox.CheckOnClick = true;
            listBox.Dock = DockStyle.Top;
            listBox.Location = new Point(0, 46);
            listBox.Name = "listBox";
            listBox.Size = new Size(640, 436);
            listBox.TabIndex = 4;
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(0, 482);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(640, 76);
            progressBar.TabIndex = 3;
            // 
            // removeButton
            // 
            removeButton.Anchor = AnchorStyles.Top;
            removeButton.Location = new Point(12, 498);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(529, 40);
            removeButton.TabIndex = 2;
            removeButton.Text = "Удалить выбранные";
            removeButton.Click += removeButton_Click;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Top;
            refreshButton.Location = new Point(547, 498);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(81, 40);
            refreshButton.TabIndex = 1;
            refreshButton.Text = "Обновить список";
            refreshButton.Click += refreshButton_Click;
            // 
            // logBox
            // 
            logBox.Dock = DockStyle.Fill;
            logBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            logBox.Location = new Point(0, 558);
            logBox.Multiline = true;
            logBox.Name = "logBox";
            logBox.ReadOnly = true;
            logBox.ScrollBars = ScrollBars.Vertical;
            logBox.Size = new Size(640, 109);
            logBox.TabIndex = 0;
            // 
            // Form1
            // 
            ClientSize = new Size(640, 667);
            Controls.Add(logBox);
            Controls.Add(refreshButton);
            Controls.Add(removeButton);
            Controls.Add(progressBar);
            Controls.Add(listBox);
            Controls.Add(label);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            Text = "UWP Remo";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
