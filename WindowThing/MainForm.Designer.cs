namespace WindowThing
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hotKeyLeft = new FireAnt.Windows.Forms.Util.HotKey(this.components);
            this.hotKeyRight = new FireAnt.Windows.Forms.Util.HotKey(this.components);
            this.ratio = new System.Windows.Forms.NumericUpDown();
            this.ratioLabel = new System.Windows.Forms.Label();
            this.hotKeyMakeMain = new FireAnt.Windows.Forms.Util.HotKey(this.components);
            this.hotKeyUnGet = new FireAnt.Windows.Forms.Util.HotKey(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ratio)).BeginInit();
            this.SuspendLayout();
            // 
            // hotKeyLeft
            // 
            this.hotKeyLeft.Key = System.Windows.Forms.Keys.None;
            this.hotKeyLeft.Modifiers = FireAnt.Windows.Forms.Util.Modifiers.None;
            this.hotKeyLeft.HotKeyPressed += new System.EventHandler(this.HotKeyLeftHotKeyPressed);
            // 
            // hotKeyRight
            // 
            this.hotKeyRight.Key = System.Windows.Forms.Keys.None;
            this.hotKeyRight.Modifiers = FireAnt.Windows.Forms.Util.Modifiers.None;
            this.hotKeyRight.HotKeyPressed += new System.EventHandler(this.HotKeyRightHotKeyPressed);
            // 
            // ratio
            // 
            this.ratio.Location = new System.Drawing.Point(139, 23);
            this.ratio.Name = "ratio";
            this.ratio.Size = new System.Drawing.Size(40, 20);
            this.ratio.TabIndex = 0;
            this.ratio.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.ratio.ValueChanged += new System.EventHandler(this.RatioValueChanged);
            // 
            // ratioLabel
            // 
            this.ratioLabel.AutoSize = true;
            this.ratioLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ratioLabel.Location = new System.Drawing.Point(34, 27);
            this.ratioLabel.Name = "ratioLabel";
            this.ratioLabel.Size = new System.Drawing.Size(92, 16);
            this.ratioLabel.TabIndex = 1;
            this.ratioLabel.Text = "Split Ratio (%)";
            // 
            // hotKeyMakeMain
            // 
            this.hotKeyMakeMain.Key = System.Windows.Forms.Keys.None;
            this.hotKeyMakeMain.Modifiers = FireAnt.Windows.Forms.Util.Modifiers.None;
            this.hotKeyMakeMain.HotKeyPressed += new System.EventHandler(this.HotKeyMakeMainHotKeyPressed);
            // 
            // hotKeyGetAll
            // 
            this.hotKeyUnGet.Key = System.Windows.Forms.Keys.None;
            this.hotKeyUnGet.Modifiers = FireAnt.Windows.Forms.Util.Modifiers.None;
            this.hotKeyUnGet.HotKeyPressed += new System.EventHandler(this.HotKeyUnGetHotKeyPressed);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 76);
            this.Controls.Add(this.ratioLabel);
            this.Controls.Add(this.ratio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "WindowThing";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ratio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FireAnt.Windows.Forms.Util.HotKey hotKeyLeft;
        private FireAnt.Windows.Forms.Util.HotKey hotKeyRight;
        private System.Windows.Forms.NumericUpDown ratio;
        private System.Windows.Forms.Label ratioLabel;
        private FireAnt.Windows.Forms.Util.HotKey hotKeyMakeMain;
        private FireAnt.Windows.Forms.Util.HotKey hotKeyUnGet;
    }
}

