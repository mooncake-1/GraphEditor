// VertexSelectionForm.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GraphEditor.Model.GraphStructure;

namespace GraphEditor
{
    public partial class VertexSelectionForm : Form
    {
        private List<Vertex> vertices;

        public Vertex StartVertex { get; private set; }
        public Vertex DestinationVertex { get; private set; }

        public VertexSelectionForm(List<Vertex> vertices)
        {
            InitializeComponent();
            this.vertices = vertices;

            // Populate the ComboBox controls with vertex IDs
            foreach (var vertex in vertices)
            {
                comboBoxStartVertex.Items.Add(vertex.ID);
                comboBoxDestinationVertex.Items.Add(vertex.ID);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Get the selected vertex IDs from the ComboBoxes
            int startVertexID = Convert.ToInt32(comboBoxStartVertex.SelectedItem);
            int destinationVertexID = Convert.ToInt32(comboBoxDestinationVertex.SelectedItem);

            // Find the corresponding vertices
            StartVertex = vertices.Find(v => v.ID == startVertexID);
            DestinationVertex = vertices.Find(v => v.ID == destinationVertexID);

            // Close the form
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Close the form
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
