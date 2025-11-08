using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;



namespace CMS_25_A
{
    public partial class CustomerForm : Form
    {
        // Use connection string from App.config instead of hardcoded value
        private MySqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CMS_25_A.Properties.Settings.CMS_25_AConnectionString"].ConnectionString;
            return new MySqlConnection(connectionString);
        }

        public CustomerForm()
        {
            InitializeComponent();
        }

        private void tblCustomerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tblCustomerBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cMS_25_ADataSet);

        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cMS_25_ADataSet.tblCustomer' table. You can move, or remove it, as needed.
            this.tblCustomerTableAdapter.Fill(this.cMS_25_ADataSet.tblCustomer);

        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            cMS_25_ADataSet.Clear();
            tblCustomerTableAdapter.Fill(cMS_25_ADataSet.tblCustomer);
        }

        private void Addnew_Click(object sender, EventArgs e)
        {
            tblCustomerBindingSource.AddNew();
            MessageBox.Show(" The Customer table is updated.");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Fixed: Use parameterized query to prevent SQL injection
            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO tblCustomer (CarNo, Name, Address, Model) VALUES (@CarNo, @Name, @Address, @Model)", 
                    con);
                
                cmd.Parameters.AddWithValue("@CarNo", carNoTextBox.Text);
                cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                cmd.Parameters.AddWithValue("@Model", modelTextBox.Text);
                
                cmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Good!");

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            tblCustomerBindingSource.RemoveCurrent();
            Save_Click(sender, e);
            MessageBox.Show("The Customer table is deleted.");
        }

        private void currentPosition()
        {
            int CurrentPosition, rowNumber;

            rowNumber = tblCustomerBindingSource.Count;
            CurrentPosition = tblCustomerBindingSource.Position + 1;
            textBox1.Text = CurrentPosition.ToString() + " of " + rowNumber.ToString();

        }


        private void Next_Click(object sender, EventArgs e)
        {
            this.tblCustomerBindingSource.MoveNext();
            currentPosition();
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            this.tblCustomerBindingSource.MovePrevious();
            currentPosition();
        }

        public void searchData(string searchValue)
        {

            string query = "select * from tblCustomer where carNo like '%" + searchValue + "%'";


            SqlCommand cmd = new SqlCommand(query, con);
            System.Data.DataTable dataTbl = new System.Data.DataTable();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            dataAdapter.Fill(dataTbl);
            dataGridView1.DataSource = dataTbl;

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = textBox2.Text.ToString();
            searchData(searchValue);
        }

        public void Export_Data_To_Word(DataGridView DGV, string filename)
        {
            if (DGV.Rows.Count != 0)
            {
                int RowCount = DGV.Rows.Count;
                int ColumnCount = DGV.Columns.Count;
                Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];

                //add rows
                int r = 0;
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    for (r = 0; r <= RowCount - 1; r++)
                    {
                        DataArray[r, c] = DGV.Rows[r].Cells[c].Value;
                    } //end row loop
                } //end column loop

                Word.Document oDoc = new Word.Document();
                oDoc.Application.Visible = true;

                //page orintation
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;


                dynamic oRange = oDoc.Content.Application.Selection.Range;
                string oTemp = "";
                for (r = 0; r <= RowCount - 1; r++)
                {
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        oTemp = oTemp + DataArray[r, c] + "\t";

                    }
                }

                //table format
                oRange.Text = oTemp;

                object Separator = Microsoft.Office.Interop.Word.WdTableFieldSeparator.wdSeparateByTabs;
                object ApplyBorders = true;
                object AutoFit = true;
                object AutoFitBehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitContent;

                oRange.ConvertToTable(ref Separator, ref RowCount, ref ColumnCount,
                                      Type.Missing, Type.Missing, ref ApplyBorders,
                                      Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, ref AutoFit, ref AutoFitBehavior, Type.Missing);

                oRange.Select();

                oDoc.Application.Selection.Tables[1].Select();
                oDoc.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
                oDoc.Application.Selection.Tables[1].Rows.Alignment = 0;
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                oDoc.Application.Selection.InsertRowsAbove(1);
                oDoc.Application.Selection.Tables[1].Rows[1].Select();

                //header row style
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Bold = 1;
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Name = "Tahoma";
                oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Size = 14;

                //add header row manually
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = DGV.Columns[c].HeaderText;
                }

                //table style 
                oDoc.Application.Selection.Tables[1].set_Style("Grid Table 4 - Accent 5");
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                oDoc.Application.Selection.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                //header text
                foreach (Word.Section section in oDoc.Application.ActiveDocument.Sections)
                {
                    Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.Text = "Информация за автомобили-собственици";
                    headerRange.Font.Size = 16;
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                }

                //save the file
                oDoc.SaveAs2(filename);
            }
        }


        private void Word_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Word Documents (*.docx)|*.docx";

            sfd.FileName = "export.docx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {

                Export_Data_To_Word(dataGridView1, sfd.FileName);
            }

        }

        private void Excel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfs = new SaveFileDialog();

            sfs.Filter = "Excel Documents (*.xlsx)|*.xlsx";

            sfs.FileName = "export.xlsx";

            if (sfs.ShowDialog() == DialogResult.OK)
            {

                Export_Data_To_Excel(dataGridView1, sfs.FileName);
            }
        }

        private void Export_Data_To_Excel(DataGridView DGV2, string filename)
        {
            if (DGV2.Rows.Count != 0)
            {
                int RowCount = DGV2.Rows.Count;
                int ColumnCount = DGV2.Columns.Count;
                Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];

                //add rows
                int r = 0;
                for (int c = 0; c <= ColumnCount - 1; c++)
                {
                    for (r = 0; r <= RowCount - 1; r++)
                    {
                        DataArray[r, c] = DGV2.Rows[r].Cells[c].Value;
                    } //end row loop
                } //end column loop

                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = Excel.Workbooks.Add(XlSheetType.xlWorksheet);
                Worksheet ws = (Worksheet)Excel.ActiveSheet;
                Excel.Visible = true;

                ws.Cells[1, 1] = "CarNo";
                ws.Cells[1, 2] = "Name";
                ws.Cells[1, 3] = "Address";

                for (int j = 2; j <= dataGridView1.Rows.Count; j++)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        ws.Cells[j, i] = dataGridView1.Rows[j - 2].Cells[i - 1].Value;
                    }
                }


            }
        }
    }
}
