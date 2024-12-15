using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarCatologMain
{
    public partial class AddBrandForm : Form
    {
        public AddBrandForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string brandName = textBox1.Text;

            if (string.IsNullOrEmpty(brandName))
            {
                MessageBox.Show("Будь ласка, введіть назву бренду!");
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            string query = "INSERT INTO Brands (BrandName) VALUES (@BrandName)";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@BrandName", brandName)
            };

            db.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Бренд додано успішно!");
            this.Close();
        }
    }
}
