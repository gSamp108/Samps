namespace Lameo
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labeledTextbox7 = new Lameo.LabeledTextbox();
            this.inputStepsPerTick = new Lameo.LabeledTextbox();
            this.inputStatus = new Lameo.LabeledTextbox();
            this.inputStepSpeed = new Lameo.LabeledTextbox();
            this.inputAddQueueCount = new Lameo.LabeledTextbox();
            this.inputClosedCount = new Lameo.LabeledTextbox();
            this.inputSizeThreshold = new Lameo.LabeledTextbox();
            this.inputSeed = new Lameo.LabeledTextbox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.labeledTextbox7);
            this.panel1.Controls.Add(this.inputStepsPerTick);
            this.panel1.Controls.Add(this.inputStatus);
            this.panel1.Controls.Add(this.inputStepSpeed);
            this.panel1.Controls.Add(this.inputAddQueueCount);
            this.panel1.Controls.Add(this.inputClosedCount);
            this.panel1.Controls.Add(this.inputSizeThreshold);
            this.panel1.Controls.Add(this.inputSeed);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(616, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 445);
            this.panel1.TabIndex = 1;
            // 
            // labeledTextbox7
            // 
            this.labeledTextbox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labeledTextbox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.labeledTextbox7.Location = new System.Drawing.Point(0, 140);
            this.labeledTextbox7.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.labeledTextbox7.Name = "labeledTextbox7";
            this.labeledTextbox7.Size = new System.Drawing.Size(201, 20);
            this.labeledTextbox7.TabIndex = 7;
            this.labeledTextbox7.XLabel = "Seed";
            this.labeledTextbox7.XReadOnly = false;
            this.labeledTextbox7.XText = "";
            // 
            // inputStepsPerTick
            // 
            this.inputStepsPerTick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputStepsPerTick.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputStepsPerTick.Location = new System.Drawing.Point(0, 120);
            this.inputStepsPerTick.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputStepsPerTick.Name = "inputStepsPerTick";
            this.inputStepsPerTick.Size = new System.Drawing.Size(201, 20);
            this.inputStepsPerTick.TabIndex = 6;
            this.inputStepsPerTick.XLabel = "Steps Per Tick";
            this.inputStepsPerTick.XReadOnly = false;
            this.inputStepsPerTick.XText = "1";
            // 
            // inputStatus
            // 
            this.inputStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputStatus.Location = new System.Drawing.Point(0, 100);
            this.inputStatus.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputStatus.Name = "inputStatus";
            this.inputStatus.Size = new System.Drawing.Size(201, 20);
            this.inputStatus.TabIndex = 5;
            this.inputStatus.XLabel = "Status";
            this.inputStatus.XReadOnly = true;
            this.inputStatus.XText = "";
            // 
            // inputStepSpeed
            // 
            this.inputStepSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputStepSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputStepSpeed.Location = new System.Drawing.Point(0, 80);
            this.inputStepSpeed.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputStepSpeed.Name = "inputStepSpeed";
            this.inputStepSpeed.Size = new System.Drawing.Size(201, 20);
            this.inputStepSpeed.TabIndex = 4;
            this.inputStepSpeed.XLabel = "Step Speed";
            this.inputStepSpeed.XReadOnly = false;
            this.inputStepSpeed.XText = "10";
            // 
            // inputAddQueueCount
            // 
            this.inputAddQueueCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputAddQueueCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputAddQueueCount.Location = new System.Drawing.Point(0, 60);
            this.inputAddQueueCount.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputAddQueueCount.Name = "inputAddQueueCount";
            this.inputAddQueueCount.Size = new System.Drawing.Size(201, 20);
            this.inputAddQueueCount.TabIndex = 3;
            this.inputAddQueueCount.XLabel = "Add Queue";
            this.inputAddQueueCount.XReadOnly = true;
            this.inputAddQueueCount.XText = "";
            // 
            // inputClosedCount
            // 
            this.inputClosedCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputClosedCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputClosedCount.Location = new System.Drawing.Point(0, 40);
            this.inputClosedCount.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputClosedCount.Name = "inputClosedCount";
            this.inputClosedCount.Size = new System.Drawing.Size(201, 20);
            this.inputClosedCount.TabIndex = 2;
            this.inputClosedCount.XLabel = "Closed";
            this.inputClosedCount.XReadOnly = true;
            this.inputClosedCount.XText = "";
            // 
            // inputSizeThreshold
            // 
            this.inputSizeThreshold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputSizeThreshold.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputSizeThreshold.Location = new System.Drawing.Point(0, 20);
            this.inputSizeThreshold.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.inputSizeThreshold.Name = "inputSizeThreshold";
            this.inputSizeThreshold.Size = new System.Drawing.Size(201, 20);
            this.inputSizeThreshold.TabIndex = 1;
            this.inputSizeThreshold.XLabel = "Size Threshold";
            this.inputSizeThreshold.XReadOnly = false;
            this.inputSizeThreshold.XText = "100";
            // 
            // inputSeed
            // 
            this.inputSeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.inputSeed.Location = new System.Drawing.Point(0, 0);
            this.inputSeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.inputSeed.Name = "inputSeed";
            this.inputSeed.Size = new System.Drawing.Size(201, 20);
            this.inputSeed.TabIndex = 0;
            this.inputSeed.XLabel = "Seed";
            this.inputSeed.XReadOnly = true;
            this.inputSeed.XText = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(817, 445);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Lameo";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LabeledTextbox inputSeed;
        private System.Windows.Forms.Panel panel1;
        private LabeledTextbox labeledTextbox7;
        private LabeledTextbox inputStepsPerTick;
        private LabeledTextbox inputStatus;
        private LabeledTextbox inputStepSpeed;
        private LabeledTextbox inputAddQueueCount;
        private LabeledTextbox inputClosedCount;
        private LabeledTextbox inputSizeThreshold;
    }
}

