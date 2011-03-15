namespace OCR
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsLblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsBarCompleted = new System.Windows.Forms.ToolStripProgressBar();
            this.tsLblCompleted = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tpClassifierTesting = new System.Windows.Forms.TabPage();
            this.tabControlTestClassifier = new System.Windows.Forms.TabControl();
            this.tpTestWithTestExamples = new System.Windows.Forms.TabPage();
            this.tpTestByDrawing = new System.Windows.Forms.TabPage();
            this.tabControlPickClassifier = new System.Windows.Forms.TabControl();
            this.tpTrainClassifier = new System.Windows.Forms.TabPage();
            this.tpLoadSaveClassifier = new System.Windows.Forms.TabPage();
            this.tpPickClassifier = new System.Windows.Forms.TabPage();
            this.btnLoadClassisifer = new System.Windows.Forms.Button();
            this.btnSaveClassifier = new System.Windows.Forms.Button();
            this.tabControlTrainClassifier = new System.Windows.Forms.TabControl();
            this.tpGaborSettings = new System.Windows.Forms.TabPage();
            this.tpSVMSettings = new System.Windows.Forms.TabPage();
            this.lblTrainClassifierMessage = new System.Windows.Forms.Label();
            this.btnTrainClassifier = new System.Windows.Forms.Button();
            this.btnTestClassifierWithData = new System.Windows.Forms.Button();
            this.rtbStatistics = new System.Windows.Forms.RichTextBox();
            this.lblStatistics = new System.Windows.Forms.Label();
            this.lblMaxFilesTrain = new System.Windows.Forms.Label();
            this.txtMaxFilesTrain = new System.Windows.Forms.TextBox();
            this.txtMaxFilesTest = new System.Windows.Forms.TextBox();
            this.lblMaxFilesTest = new System.Windows.Forms.Label();
            this.lblDrawingClassificationResult = new System.Windows.Forms.Label();
            this.btnClearDrawer = new System.Windows.Forms.Button();
            this.btnClassifyDrawing = new System.Windows.Forms.Button();
            this.charDrawer = new OCR.CharDrawer(this.components);
            this.statusStrip.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tpClassifierTesting.SuspendLayout();
            this.tabControlTestClassifier.SuspendLayout();
            this.tpTestWithTestExamples.SuspendLayout();
            this.tpTestByDrawing.SuspendLayout();
            this.tabControlPickClassifier.SuspendLayout();
            this.tpTrainClassifier.SuspendLayout();
            this.tpLoadSaveClassifier.SuspendLayout();
            this.tpPickClassifier.SuspendLayout();
            this.tabControlTrainClassifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLblMessage,
            this.tsBarCompleted,
            this.tsLblCompleted});
            this.statusStrip.Location = new System.Drawing.Point(0, 647);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(804, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsLblMessage
            // 
            this.tsLblMessage.Name = "tsLblMessage";
            this.tsLblMessage.Size = new System.Drawing.Size(602, 17);
            this.tsLblMessage.Spring = true;
            this.tsLblMessage.Text = "No Message...";
            this.tsLblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsBarCompleted
            // 
            this.tsBarCompleted.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsBarCompleted.AutoSize = false;
            this.tsBarCompleted.Name = "tsBarCompleted";
            this.tsBarCompleted.Size = new System.Drawing.Size(150, 16);
            this.tsBarCompleted.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tsLblCompleted
            // 
            this.tsLblCompleted.AutoSize = false;
            this.tsLblCompleted.Name = "tsLblCompleted";
            this.tsLblCompleted.Size = new System.Drawing.Size(35, 17);
            this.tsLblCompleted.Text = "0%";
            this.tsLblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tpPickClassifier);
            this.tabControlMain.Controls.Add(this.tpClassifierTesting);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(804, 647);
            this.tabControlMain.TabIndex = 1;
            // 
            // tpClassifierTesting
            // 
            this.tpClassifierTesting.Controls.Add(this.tabControlTestClassifier);
            this.tpClassifierTesting.Location = new System.Drawing.Point(4, 22);
            this.tpClassifierTesting.Name = "tpClassifierTesting";
            this.tpClassifierTesting.Padding = new System.Windows.Forms.Padding(3);
            this.tpClassifierTesting.Size = new System.Drawing.Size(796, 621);
            this.tpClassifierTesting.TabIndex = 1;
            this.tpClassifierTesting.Text = "Classifier testing";
            this.tpClassifierTesting.UseVisualStyleBackColor = true;
            // 
            // tabControlTestClassifier
            // 
            this.tabControlTestClassifier.Controls.Add(this.tpTestWithTestExamples);
            this.tabControlTestClassifier.Controls.Add(this.tpTestByDrawing);
            this.tabControlTestClassifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTestClassifier.Location = new System.Drawing.Point(3, 3);
            this.tabControlTestClassifier.Name = "tabControlTestClassifier";
            this.tabControlTestClassifier.SelectedIndex = 0;
            this.tabControlTestClassifier.Size = new System.Drawing.Size(790, 615);
            this.tabControlTestClassifier.TabIndex = 0;
            // 
            // tpTestWithTestExamples
            // 
            this.tpTestWithTestExamples.Controls.Add(this.txtMaxFilesTest);
            this.tpTestWithTestExamples.Controls.Add(this.lblMaxFilesTest);
            this.tpTestWithTestExamples.Controls.Add(this.lblStatistics);
            this.tpTestWithTestExamples.Controls.Add(this.rtbStatistics);
            this.tpTestWithTestExamples.Controls.Add(this.btnTestClassifierWithData);
            this.tpTestWithTestExamples.Location = new System.Drawing.Point(4, 22);
            this.tpTestWithTestExamples.Name = "tpTestWithTestExamples";
            this.tpTestWithTestExamples.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestWithTestExamples.Size = new System.Drawing.Size(782, 589);
            this.tpTestWithTestExamples.TabIndex = 0;
            this.tpTestWithTestExamples.Text = "Test with Test Data";
            this.tpTestWithTestExamples.UseVisualStyleBackColor = true;
            // 
            // tpTestByDrawing
            // 
            this.tpTestByDrawing.Controls.Add(this.btnClassifyDrawing);
            this.tpTestByDrawing.Controls.Add(this.btnClearDrawer);
            this.tpTestByDrawing.Controls.Add(this.lblDrawingClassificationResult);
            this.tpTestByDrawing.Controls.Add(this.charDrawer);
            this.tpTestByDrawing.Location = new System.Drawing.Point(4, 22);
            this.tpTestByDrawing.Name = "tpTestByDrawing";
            this.tpTestByDrawing.Padding = new System.Windows.Forms.Padding(3);
            this.tpTestByDrawing.Size = new System.Drawing.Size(782, 589);
            this.tpTestByDrawing.TabIndex = 1;
            this.tpTestByDrawing.Text = "Test by drawing";
            this.tpTestByDrawing.UseVisualStyleBackColor = true;
            // 
            // tabControlPickClassifier
            // 
            this.tabControlPickClassifier.Controls.Add(this.tpLoadSaveClassifier);
            this.tabControlPickClassifier.Controls.Add(this.tpTrainClassifier);
            this.tabControlPickClassifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPickClassifier.Location = new System.Drawing.Point(3, 3);
            this.tabControlPickClassifier.Name = "tabControlPickClassifier";
            this.tabControlPickClassifier.SelectedIndex = 0;
            this.tabControlPickClassifier.Size = new System.Drawing.Size(790, 615);
            this.tabControlPickClassifier.TabIndex = 0;
            // 
            // tpTrainClassifier
            // 
            this.tpTrainClassifier.BackColor = System.Drawing.Color.Transparent;
            this.tpTrainClassifier.Controls.Add(this.txtMaxFilesTrain);
            this.tpTrainClassifier.Controls.Add(this.lblMaxFilesTrain);
            this.tpTrainClassifier.Controls.Add(this.btnTrainClassifier);
            this.tpTrainClassifier.Controls.Add(this.lblTrainClassifierMessage);
            this.tpTrainClassifier.Controls.Add(this.tabControlTrainClassifier);
            this.tpTrainClassifier.Location = new System.Drawing.Point(4, 22);
            this.tpTrainClassifier.Name = "tpTrainClassifier";
            this.tpTrainClassifier.Padding = new System.Windows.Forms.Padding(3);
            this.tpTrainClassifier.Size = new System.Drawing.Size(782, 589);
            this.tpTrainClassifier.TabIndex = 1;
            this.tpTrainClassifier.Text = "Train classifer";
            this.tpTrainClassifier.UseVisualStyleBackColor = true;
            // 
            // tpLoadSaveClassifier
            // 
            this.tpLoadSaveClassifier.Controls.Add(this.btnSaveClassifier);
            this.tpLoadSaveClassifier.Controls.Add(this.btnLoadClassisifer);
            this.tpLoadSaveClassifier.Location = new System.Drawing.Point(4, 22);
            this.tpLoadSaveClassifier.Name = "tpLoadSaveClassifier";
            this.tpLoadSaveClassifier.Padding = new System.Windows.Forms.Padding(3);
            this.tpLoadSaveClassifier.Size = new System.Drawing.Size(782, 589);
            this.tpLoadSaveClassifier.TabIndex = 0;
            this.tpLoadSaveClassifier.Text = "Load/Save classifier from/to file";
            this.tpLoadSaveClassifier.UseVisualStyleBackColor = true;
            // 
            // tpPickClassifier
            // 
            this.tpPickClassifier.Controls.Add(this.tabControlPickClassifier);
            this.tpPickClassifier.Location = new System.Drawing.Point(4, 22);
            this.tpPickClassifier.Name = "tpPickClassifier";
            this.tpPickClassifier.Padding = new System.Windows.Forms.Padding(3);
            this.tpPickClassifier.Size = new System.Drawing.Size(796, 621);
            this.tpPickClassifier.TabIndex = 0;
            this.tpPickClassifier.Text = "Pick classifier";
            this.tpPickClassifier.UseVisualStyleBackColor = true;
            // 
            // btnLoadClassisifer
            // 
            this.btnLoadClassisifer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadClassisifer.Location = new System.Drawing.Point(295, 173);
            this.btnLoadClassisifer.Name = "btnLoadClassisifer";
            this.btnLoadClassisifer.Size = new System.Drawing.Size(155, 150);
            this.btnLoadClassisifer.TabIndex = 0;
            this.btnLoadClassisifer.Text = "Load Classisifer";
            this.btnLoadClassisifer.UseVisualStyleBackColor = true;
            // 
            // btnSaveClassifier
            // 
            this.btnSaveClassifier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveClassifier.Location = new System.Drawing.Point(295, 367);
            this.btnSaveClassifier.Name = "btnSaveClassifier";
            this.btnSaveClassifier.Size = new System.Drawing.Size(155, 44);
            this.btnSaveClassifier.TabIndex = 1;
            this.btnSaveClassifier.Text = "Save Classisifer";
            this.btnSaveClassifier.UseVisualStyleBackColor = true;
            // 
            // tabControlTrainClassifier
            // 
            this.tabControlTrainClassifier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlTrainClassifier.Controls.Add(this.tpGaborSettings);
            this.tabControlTrainClassifier.Controls.Add(this.tpSVMSettings);
            this.tabControlTrainClassifier.Location = new System.Drawing.Point(6, 49);
            this.tabControlTrainClassifier.Name = "tabControlTrainClassifier";
            this.tabControlTrainClassifier.SelectedIndex = 0;
            this.tabControlTrainClassifier.Size = new System.Drawing.Size(770, 496);
            this.tabControlTrainClassifier.TabIndex = 0;
            // 
            // tpGaborSettings
            // 
            this.tpGaborSettings.Location = new System.Drawing.Point(4, 22);
            this.tpGaborSettings.Name = "tpGaborSettings";
            this.tpGaborSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpGaborSettings.Size = new System.Drawing.Size(762, 470);
            this.tpGaborSettings.TabIndex = 0;
            this.tpGaborSettings.Text = "Gabor Feature Extractor Settings";
            this.tpGaborSettings.UseVisualStyleBackColor = true;
            // 
            // tpSVMSettings
            // 
            this.tpSVMSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSVMSettings.Name = "tpSVMSettings";
            this.tpSVMSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSVMSettings.Size = new System.Drawing.Size(762, 470);
            this.tpSVMSettings.TabIndex = 1;
            this.tpSVMSettings.Text = "Kernel SVM Settings";
            this.tpSVMSettings.UseVisualStyleBackColor = true;
            // 
            // lblTrainClassifierMessage
            // 
            this.lblTrainClassifierMessage.Location = new System.Drawing.Point(24, 561);
            this.lblTrainClassifierMessage.Name = "lblTrainClassifierMessage";
            this.lblTrainClassifierMessage.Size = new System.Drawing.Size(642, 23);
            this.lblTrainClassifierMessage.TabIndex = 1;
            this.lblTrainClassifierMessage.Text = "Picked classifier:";
            // 
            // btnTrainClassifier
            // 
            this.btnTrainClassifier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrainClassifier.Location = new System.Drawing.Point(672, 551);
            this.btnTrainClassifier.Name = "btnTrainClassifier";
            this.btnTrainClassifier.Size = new System.Drawing.Size(100, 32);
            this.btnTrainClassifier.TabIndex = 2;
            this.btnTrainClassifier.Text = "Train Classifier";
            this.btnTrainClassifier.UseVisualStyleBackColor = true;
            // 
            // btnTestClassifierWithData
            // 
            this.btnTestClassifierWithData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestClassifierWithData.Location = new System.Drawing.Point(659, 23);
            this.btnTestClassifierWithData.Name = "btnTestClassifierWithData";
            this.btnTestClassifierWithData.Size = new System.Drawing.Size(106, 37);
            this.btnTestClassifierWithData.TabIndex = 0;
            this.btnTestClassifierWithData.Text = "Load Test";
            this.btnTestClassifierWithData.UseVisualStyleBackColor = true;
            // 
            // rtbStatistics
            // 
            this.rtbStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbStatistics.DetectUrls = false;
            this.rtbStatistics.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbStatistics.Location = new System.Drawing.Point(6, 93);
            this.rtbStatistics.Name = "rtbStatistics";
            this.rtbStatistics.ReadOnly = true;
            this.rtbStatistics.Size = new System.Drawing.Size(770, 490);
            this.rtbStatistics.TabIndex = 1;
            this.rtbStatistics.Text = "";
            // 
            // lblStatistics
            // 
            this.lblStatistics.AutoSize = true;
            this.lblStatistics.Location = new System.Drawing.Point(6, 77);
            this.lblStatistics.Name = "lblStatistics";
            this.lblStatistics.Size = new System.Drawing.Size(52, 13);
            this.lblStatistics.TabIndex = 2;
            this.lblStatistics.Text = "Statistics:";
            // 
            // lblMaxFilesTrain
            // 
            this.lblMaxFilesTrain.AutoSize = true;
            this.lblMaxFilesTrain.Location = new System.Drawing.Point(7, 15);
            this.lblMaxFilesTrain.Name = "lblMaxFilesTrain";
            this.lblMaxFilesTrain.Size = new System.Drawing.Size(129, 13);
            this.lblMaxFilesTrain.TabIndex = 3;
            this.lblMaxFilesTrain.Text = "Max. files per single class:";
            // 
            // txtMaxFilesTrain
            // 
            this.txtMaxFilesTrain.Location = new System.Drawing.Point(142, 12);
            this.txtMaxFilesTrain.Name = "txtMaxFilesTrain";
            this.txtMaxFilesTrain.Size = new System.Drawing.Size(100, 20);
            this.txtMaxFilesTrain.TabIndex = 4;
            // 
            // txtMaxFilesTest
            // 
            this.txtMaxFilesTest.Location = new System.Drawing.Point(151, 32);
            this.txtMaxFilesTest.Name = "txtMaxFilesTest";
            this.txtMaxFilesTest.Size = new System.Drawing.Size(100, 20);
            this.txtMaxFilesTest.TabIndex = 6;
            // 
            // lblMaxFilesTest
            // 
            this.lblMaxFilesTest.AutoSize = true;
            this.lblMaxFilesTest.Location = new System.Drawing.Point(16, 35);
            this.lblMaxFilesTest.Name = "lblMaxFilesTest";
            this.lblMaxFilesTest.Size = new System.Drawing.Size(129, 13);
            this.lblMaxFilesTest.TabIndex = 5;
            this.lblMaxFilesTest.Text = "Max. files per single class:";
            // 
            // lblDrawingClassificationResult
            // 
            this.lblDrawingClassificationResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDrawingClassificationResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDrawingClassificationResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDrawingClassificationResult.Location = new System.Drawing.Point(632, 36);
            this.lblDrawingClassificationResult.Name = "lblDrawingClassificationResult";
            this.lblDrawingClassificationResult.Size = new System.Drawing.Size(115, 115);
            this.lblDrawingClassificationResult.TabIndex = 1;
            this.lblDrawingClassificationResult.Text = "?";
            this.lblDrawingClassificationResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClearDrawer
            // 
            this.btnClearDrawer.Location = new System.Drawing.Point(58, 157);
            this.btnClearDrawer.Name = "btnClearDrawer";
            this.btnClearDrawer.Size = new System.Drawing.Size(75, 23);
            this.btnClearDrawer.TabIndex = 2;
            this.btnClearDrawer.Text = "Clear";
            this.btnClearDrawer.UseVisualStyleBackColor = true;
            // 
            // btnClassifyDrawing
            // 
            this.btnClassifyDrawing.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClassifyDrawing.Location = new System.Drawing.Point(352, 54);
            this.btnClassifyDrawing.Name = "btnClassifyDrawing";
            this.btnClassifyDrawing.Size = new System.Drawing.Size(91, 74);
            this.btnClassifyDrawing.TabIndex = 3;
            this.btnClassifyDrawing.Text = "Classify Drawing";
            this.btnClassifyDrawing.UseVisualStyleBackColor = true;
            // 
            // charDrawer
            // 
            this.charDrawer.BackColor = System.Drawing.Color.Black;
            this.charDrawer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.charDrawer.Cursor = System.Windows.Forms.Cursors.Cross;
            this.charDrawer.Location = new System.Drawing.Point(18, 36);
            this.charDrawer.Name = "charDrawer";
            this.charDrawer.Size = new System.Drawing.Size(115, 115);
            this.charDrawer.TabIndex = 0;
            this.charDrawer.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 669);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.Text = "OCR - Gabor filters";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tpClassifierTesting.ResumeLayout(false);
            this.tabControlTestClassifier.ResumeLayout(false);
            this.tpTestWithTestExamples.ResumeLayout(false);
            this.tpTestWithTestExamples.PerformLayout();
            this.tpTestByDrawing.ResumeLayout(false);
            this.tabControlPickClassifier.ResumeLayout(false);
            this.tpTrainClassifier.ResumeLayout(false);
            this.tpTrainClassifier.PerformLayout();
            this.tpLoadSaveClassifier.ResumeLayout(false);
            this.tpPickClassifier.ResumeLayout(false);
            this.tabControlTrainClassifier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.charDrawer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsLblMessage;
        private System.Windows.Forms.ToolStripProgressBar tsBarCompleted;
        private System.Windows.Forms.ToolStripStatusLabel tsLblCompleted;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tpPickClassifier;
        private System.Windows.Forms.TabControl tabControlPickClassifier;
        private System.Windows.Forms.TabPage tpLoadSaveClassifier;
        private System.Windows.Forms.TabPage tpTrainClassifier;
        private System.Windows.Forms.TabPage tpClassifierTesting;
        private System.Windows.Forms.TabControl tabControlTestClassifier;
        private System.Windows.Forms.TabPage tpTestWithTestExamples;
        private System.Windows.Forms.TabPage tpTestByDrawing;
        private System.Windows.Forms.Button btnSaveClassifier;
        private System.Windows.Forms.Button btnLoadClassisifer;
        private System.Windows.Forms.Button btnTrainClassifier;
        private System.Windows.Forms.Label lblTrainClassifierMessage;
        private System.Windows.Forms.TabControl tabControlTrainClassifier;
        private System.Windows.Forms.TabPage tpGaborSettings;
        private System.Windows.Forms.TabPage tpSVMSettings;
        private System.Windows.Forms.Label lblStatistics;
        private System.Windows.Forms.RichTextBox rtbStatistics;
        private System.Windows.Forms.Button btnTestClassifierWithData;
        private System.Windows.Forms.TextBox txtMaxFilesTrain;
        private System.Windows.Forms.Label lblMaxFilesTrain;
        private System.Windows.Forms.TextBox txtMaxFilesTest;
        private System.Windows.Forms.Label lblMaxFilesTest;
        private System.Windows.Forms.Button btnClassifyDrawing;
        private System.Windows.Forms.Button btnClearDrawer;
        private System.Windows.Forms.Label lblDrawingClassificationResult;
        private CharDrawer charDrawer;

    }
}