using System;
using System.Windows.Forms;

namespace GraphEditor
{
    public partial class GraphTypeSelectionForm : Form
    {
        public bool IsDirectedGraph { get; private set; }

        public GraphTypeSelectionForm()
        {
            InitializeComponent();
        }

        private void directedButton_Click(object sender, EventArgs e)
        {
            IsDirectedGraph = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void undirectedButton_Click(object sender, EventArgs e)
        {
            IsDirectedGraph = false;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
