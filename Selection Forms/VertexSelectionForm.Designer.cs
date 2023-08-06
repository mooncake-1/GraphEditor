namespace GraphEditor
{
    partial class VertexSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Other private fields for the controls
        private System.Windows.Forms.ComboBox comboBoxStartVertex;
        private System.Windows.Forms.ComboBox comboBoxDestinationVertex;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;

        // Other methods, events, and properties

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxStartVertex = new System.Windows.Forms.ComboBox();
            this.comboBoxDestinationVertex = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // comboBoxStartVertex
            // 
            this.comboBoxStartVertex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStartVertex.FormattingEnabled = true;
            this.comboBoxStartVertex.Location = new System.Drawing.Point(30, 30);
            this.comboBoxStartVertex.Name = "comboBoxStartVertex";
            this.comboBoxStartVertex.Size = new System.Drawing.Size(150, 24);
            this.comboBoxStartVertex.TabIndex = 0;
            // 
            // comboBoxDestinationVertex
            // 
            this.comboBoxDestinationVertex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDestinationVertex.FormattingEnabled = true;
            this.comboBoxDestinationVertex.Location = new System.Drawing.Point(30, 70);
            this.comboBoxDestinationVertex.Name = "comboBoxDestinationVertex";
            this.comboBoxDestinationVertex.Size = new System.Drawing.Size(150, 24);
            this.comboBoxDestinationVertex.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(30, 120);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(120, 120);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // VertexSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 180);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxDestinationVertex);
            this.Controls.Add(this.comboBoxStartVertex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vertex Selection";
            this.ResumeLayout(false);
        }
    }
}
