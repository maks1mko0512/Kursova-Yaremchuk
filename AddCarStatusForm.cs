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
    public partial class AddCarStatusForm : Form
    {
        public AddCarStatusForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Отримуємо значення з TextBox
            string statusName = textBox1.Text.Trim();

            // Перевіряємо, чи введено значення
            if (string.IsNullOrEmpty(statusName))
            {
                MessageBox.Show("Будь ласка, введіть назву статусу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // SQL-запит для додавання
                string query = "INSERT INTO CarStatuses (StatusName) VALUES (@StatusName)";

                // Параметри запиту
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@StatusName", statusName)
                };

                // Виконання запиту
                DatabaseHelper db = new DatabaseHelper();
                db.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Статус успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Закриваємо форму після успішного додавання
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час додавання статусу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
