using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphEditor
{
    public class InputDialog
    {
        public static string ShowDialog(string title, string prompt, string defaultValue = "")
        {
            Form form = new Form();
            form.Width = 300;
            form.Height = 150;
            form.Text = title;

            Label label = new Label() { Left = 20, Top = 20, Text = prompt };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250, Text = defaultValue };
            Button buttonOk = new Button() { Text = "OK", Left = 20, Top = 80, Width = 100 };
            Button buttonCancel = new Button() { Text = "Cancel", Left = 150, Top = 80, Width = 100 };

            buttonOk.Click += (sender, e) => { form.DialogResult = DialogResult.OK; };
            buttonCancel.Click += (sender, e) => { form.DialogResult = DialogResult.Cancel; };
            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(buttonOk);
            form.Controls.Add(buttonCancel);
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            return form.ShowDialog() == DialogResult.OK ? textBox.Text : defaultValue;
        }
    }
}
