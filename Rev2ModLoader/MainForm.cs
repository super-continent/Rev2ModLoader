using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rev2ModLoader
{
    public partial class MainForm : Form
    {
        ModLoader ML;
        public MainForm()
        {
            InitializeComponent();
            comboScriptSelected.SelectedIndex = 0;
            ML = new ModLoader();

            buttonExtractScript.Enabled = false;

            if (!Directory.Exists("./rev2_mods"))
            {
                Directory.CreateDirectory("rev2_mods");
            }
        }

        private void buttonInject_Click(object sender, EventArgs e)
        {
            if (ML.Inject())
            {
                buttonExtractScript.Enabled = true;
                buttonInject.Enabled = false;
            }
        }

        private void buttonExtractScript_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] script = ML.TryExtractScript(comboScriptSelected.SelectedIndex);

                var sfd = new SaveFileDialog();
                sfd.Filter = "bbscript files (*.bbscript)|*.bbscript";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var file = sfd.OpenFile();
                    file.Write(script, 0, script.Length);
                    file.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Couldn't extract script!\nException: {exception.ToString()}", "Error!");
            }
        }
    }
}
