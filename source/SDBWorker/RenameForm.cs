using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SDB
{
    public partial class RenameForm : Form
    {

        private string[] mItems;

        public RenameForm(string[] items)
        {
            InitializeComponent();
            mItems = items;
            this.chkNumbered.CheckedChanged += new EventHandler(chkNumbered_CheckedChanged);
            this.radioButtonPrefix.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            this.radioButtonSuffix.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
        }

        void chkNumbered_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFix.Enabled = !chkNumbered.Checked;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = radioButtonPrefix.Checked ? "Prefix:" : "Suffix:";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 1;
                foreach (var it in mItems)
                {
                    string text = chkNumbered.Checked ? index.ToString() : textBoxFix.Text;
                    if (Directory.Exists(it))
                        Directory.Move(it, GetRenamePath(it, text, radioButtonSuffix.Checked, false));
                    else
                        File.Move(it, GetRenamePath(it, text, radioButtonSuffix.Checked, true));
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void textBoxFix_KeyPress(object sender, KeyPressEventArgs e)
        {
            toolTip1.Hide(textBoxFix);
            if (e.KeyChar != '\b' && Path.GetInvalidFileNameChars().Contains(e.KeyChar))
            {
                System.Media.SystemSounds.Beep.Play();
                toolTip1.Show(e.KeyChar + " is invalid file name character.", textBoxFix, new Point(1, 20), 5000);
                e.Handled = true;
            }
        }

        private string GetRenamePath(string fPath, string text, bool suffix, bool isFile)
        {
            string fileName = Path.GetFileName(fPath);
            text = suffix ? " " + text : text + " ";
            if (!isFile)
            {
                fileName = suffix ? fileName + text : text + fileName;
            }
            else
            {
                int dotIndex = fileName.LastIndexOf('.');
                if (dotIndex >= 0)
                {
                    fileName = suffix ? fileName.Insert(dotIndex, text) : text + fileName;
                }
                else
                {
                    fileName = suffix ? fileName + text : text + fileName;
                }
            }
            return Path.GetDirectoryName(fPath) + "\\" + fileName;
        }

    }
}
