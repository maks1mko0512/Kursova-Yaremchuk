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
    public partial class AddBodyTypeForm : Form
    {
        public AddBodyTypeForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string bodyTypeName = textBox1.Text;

            if (string.IsNullOrEmpty(bodyTypeName))
            {
                MessageBox.Show("Будь ласка, введіть назву типу кузова!");
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            string query = "INSERT INTO BodyTypes (BodyTypeName) VALUES (@BodyTypeName)";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@BodyTypeName", bodyTypeName)
            };

            db.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Тип кузова додано успішно!");
            this.Close(); // Закриває форму після додавання
        }
    }
}
