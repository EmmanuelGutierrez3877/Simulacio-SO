namespace Simulacion_SO
{
    partial class FormInicio
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
            this.numeric1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonContinuar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericQuantum = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numeric1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericQuantum)).BeginInit();
            this.SuspendLayout();
            // 
            // numeric1
            // 
            this.numeric1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric1.Location = new System.Drawing.Point(335, 64);
            this.numeric1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numeric1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numeric1.Name = "numeric1";
            this.numeric1.Size = new System.Drawing.Size(164, 27);
            this.numeric1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "Numero de procesos a crear:";
            // 
            // buttonContinuar
            // 
            this.buttonContinuar.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonContinuar.Location = new System.Drawing.Point(197, 166);
            this.buttonContinuar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonContinuar.Name = "buttonContinuar";
            this.buttonContinuar.Size = new System.Drawing.Size(132, 53);
            this.buttonContinuar.TabIndex = 13;
            this.buttonContinuar.Text = "Continuar";
            this.buttonContinuar.UseVisualStyleBackColor = true;
            this.buttonContinuar.Click += new System.EventHandler(this.buttonContinuar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(183, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 19);
            this.label1.TabIndex = 15;
            this.label1.Text = "Quantum:";
            // 
            // numericQuantum
            // 
            this.numericQuantum.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericQuantum.Location = new System.Drawing.Point(336, 114);
            this.numericQuantum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericQuantum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericQuantum.Name = "numericQuantum";
            this.numericQuantum.Size = new System.Drawing.Size(164, 27);
            this.numericQuantum.TabIndex = 14;
            // 
            // FormInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 254);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericQuantum);
            this.Controls.Add(this.buttonContinuar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numeric1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormInicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormInicio";
            this.Load += new System.EventHandler(this.FormInicio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numeric1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericQuantum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numeric1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonContinuar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericQuantum;
    }
}