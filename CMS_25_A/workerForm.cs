using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMS_25_A
{
    public partial class workerForm : Form
    {
        public workerForm()
        {
            InitializeComponent();
        }

        private void tblWorkerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tblWorkerBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cMS_25_ADataSet);

        }

        private void workerForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cMS_25_ADataSet.tblWorker' table. You can move, or remove it, as needed.
            this.tblWorkerTableAdapter.Fill(this.cMS_25_ADataSet.tblWorker);

        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.Validate();
            tblWorkerBindingSource.EndEdit();
            tblWorkerTableAdapter.Update(cMS_25_ADataSet.tblWorker);
            MessageBox.Show("The Worker table is saved!");
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            cMS_25_ADataSet.Clear();
            tblWorkerTableAdapter.Fill(cMS_25_ADataSet.tblWorker);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            tblWorkerBindingSource.RemoveCurrent();
            Save_Click(sender, e);
            MessageBox.Show("The Worker table is deleted!");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //Form1 newForm = new Form1();
            //newForm.Show();
            //this.Hide();
            this.Close();
        }
    }
}
