// StartVertexSelectionForm.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GraphEditor.Model.GraphStructure;

namespace GraphEditor
{
    public partial class StartVertexSelectionForm : Form
    {
        private List<Vertex> vertices;

        public Vertex SelectedVertex { get; private set; }

        public StartVertexSelectionForm(List<Vertex> vertices)
        {
            InitializeComponent();
            this.vertices = vertices;

            // Populate the ComboBox control with vertex IDs
            foreach (var vertex in vertices)
            {
                comboBoxStartVertex.Items.Add(vertex.ID);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (comboBoxStartVertex.SelectedItem != null)
            {
                // Get the selected vertex ID from the ComboBox
                int selectedVertexID = Convert.ToInt32(comboBoxStartVertex.SelectedItem);

                // Find the corresponding vertex
                SelectedVertex = vertices.Find(v => v.ID == selectedVertexID);

                // Close the form
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // Display an error message if no vertex is selected
                MessageBox.Show("Please select a vertex.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Close the form
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
