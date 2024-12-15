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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCarsData();
            comboBoxTableSelection.Items.AddRange(new string[] { "BodyTypes", "Brands", "CarStatuses", "Cars" });
        }
        private void LoadCarsData()
        {
            DatabaseHelper db = new DatabaseHelper();
            string query = "SELECT Cars.CarID, Brands.BrandName, Cars.Model, Cars.Year, BodyTypes.BodyTypeName, \r\n       Cars.EngineCapacity, Cars.FuelType, Cars.Transmission, Cars.Mileage, Cars.Price, \r\n       CarStatuses.StatusName, Cars.Description\r\nFROM Cars\r\nJOIN Brands ON Cars.BrandID = Brands.BrandID\r\nJOIN BodyTypes ON Cars.BodyTypeID = BodyTypes.BodyTypeID\r\nJOIN CarStatuses ON Cars.StatusID = CarStatuses.StatusID;\r\n";
            DataTable carsTable = db.ExecuteQuery(query);
            dataGridView1.DataSource = carsTable;
        }

        private void btnOpenAddForm_Click(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTableSelection.SelectedItem.ToString();
            Form addForm = null;

            switch (selectedTable)
            {
                case "BodyTypes":
                    addForm = new AddBodyTypeForm();
                    break;
                case "Brands":
                    addForm = new AddBrandForm();
                    break;
                case "CarStatuses":
                    addForm = new AddCarStatusForm();
                    break;
                case "Cars":
                    addForm = new AddCarForm();
                    break;
                default:
                    MessageBox.Show("Будь ласка, виберіть таблицю для додавання!");
                    return;
            }

            if (addForm != null)
            {
                addForm.ShowDialog();
                LoadCarsData(); // Перезавантаження даних після закриття форми
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть рядок для редагування!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Отримання ID вибраного запису
            int carId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CarID"].Value);

            // Відкриття форми редагування
            EditCarForm editForm = new EditCarForm(carId);
            editForm.ShowDialog();

            // Оновлення даних у DataGridView після редагування
            LoadCarsData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть рядок для видалення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Отримання ID вибраного запису
            int carId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CarID"].Value);

            // Підтвердження видалення
            var confirmResult = MessageBox.Show("Ви впевнені, що хочете видалити цей запис?",
                                                "Підтвердження видалення",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                DeleteCar(carId); // Видалення запису
                LoadCarsData(); // Оновлення даних
            }
        }

        private void DeleteCar(int carId)
        {
            try
            {
                // SQL-запит для видалення
                string query = "DELETE FROM Cars WHERE CarID = @CarID";

                // Параметри запиту
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@CarID", carId)
                };

                // Виконання запиту
                DatabaseHelper db = new DatabaseHelper();
                db.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Запис успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Будь ласка, введіть текст для пошуку!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchCars(searchTerm);
        }

        private void SearchCars(string searchTerm)
        {
            try
            {
                // SQL-запит для пошуку
                string query = @"SELECT Cars.CarID, Brands.BrandName, Cars.Model, Cars.Year, 
                                BodyTypes.BodyTypeName, Cars.EngineCapacity, Cars.FuelType, 
                                Cars.Transmission, Cars.Mileage, Cars.Price, 
                                CarStatuses.StatusName, Cars.Description
                         FROM Cars
                         JOIN Brands ON Cars.BrandID = Brands.BrandID
                         JOIN BodyTypes ON Cars.BodyTypeID = BodyTypes.BodyTypeID
                         JOIN CarStatuses ON Cars.StatusID = CarStatuses.StatusID
                         WHERE Brands.BrandName LIKE @SearchTerm
                            OR Cars.Model LIKE @SearchTerm
                            OR Cars.FuelType LIKE @SearchTerm
                            OR CarStatuses.StatusName LIKE @SearchTerm";

                // Параметри запиту
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@SearchTerm", "%" + searchTerm + "%")
                };

                // Виконання запиту
                DatabaseHelper db = new DatabaseHelper();
                DataTable searchResults = db.ExecuteQuery(query, parameters);

                // Відображення результатів у DataGridView
                dataGridView1.DataSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Clear(); // Очищення поля пошуку
            LoadCarsData();    // Завантаження всіх даних
        }
    }
}
