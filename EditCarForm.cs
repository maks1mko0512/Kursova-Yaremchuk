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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace CarCatologMain
{
    public partial class EditCarForm : Form
    {
        private int _carId;

        public EditCarForm(int carId)
        {
            InitializeComponent();

            _carId = carId;
        }

        private void EditCarForm_Load(object sender, EventArgs e)
        {
            DatabaseHelper db = new DatabaseHelper();

            // Завантаження даних автомобіля
            string query = @"SELECT * FROM Cars WHERE CarID = @CarID";
            SqlParameter[] parameters = { new SqlParameter("@CarID", _carId) };
            DataTable carData = db.ExecuteQuery(query, parameters);

            if (carData.Rows.Count > 0)
            {
                DataRow row = carData.Rows[0];

                // Заповнення полів
                txtModel.Text = row["Model"].ToString();
                txtYear.Text = row["Year"].ToString();
                txtEngineCapacity.Text = row["EngineCapacity"].ToString();
                txtFuelType.Text = row["FuelType"].ToString();
                txtTransmission.Text = row["Transmission"].ToString();
                txtMileage.Text = row["Mileage"].ToString();
                txtPrice.Text = row["Price"].ToString();
                txtDescription.Text = row["Description"].ToString();

                // Заповнення ComboBox для зовнішніх ключів
                LoadComboBoxes(row);
            }
        }

        private void LoadComboBoxes(DataRow carData)
        {
            DatabaseHelper db = new DatabaseHelper();

            // Brands
            string brandQuery = "SELECT BrandID, BrandName FROM Brands";
            DataTable brands = db.ExecuteQuery(brandQuery);
            comboBoxBrand.DataSource = brands;
            comboBoxBrand.DisplayMember = "BrandName";
            comboBoxBrand.ValueMember = "BrandID";
            comboBoxBrand.SelectedValue = carData["BrandID"];

            // BodyTypes
            string bodyTypeQuery = "SELECT BodyTypeID, BodyTypeName FROM BodyTypes";
            DataTable bodyTypes = db.ExecuteQuery(bodyTypeQuery);
            comboBoxBodyType.DataSource = bodyTypes;
            comboBoxBodyType.DisplayMember = "BodyTypeName";
            comboBoxBodyType.ValueMember = "BodyTypeID";
            comboBoxBodyType.SelectedValue = carData["BodyTypeID"];

            // Statuses
            string statusQuery = "SELECT StatusID, StatusName FROM CarStatuses";
            DataTable statuses = db.ExecuteQuery(statusQuery);
            comboBoxStatus.DataSource = statuses;
            comboBoxStatus.DisplayMember = "StatusName";
            comboBoxStatus.ValueMember = "StatusID";
            comboBoxStatus.SelectedValue = carData["StatusID"];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Отримання значень із форми
                string model = txtModel.Text;
                int year = int.Parse(txtYear.Text);
                int brandId = Convert.ToInt32(comboBoxBrand.SelectedValue);
                int bodyTypeId = Convert.ToInt32(comboBoxBodyType.SelectedValue);
                decimal engineCapacity = decimal.Parse(txtEngineCapacity.Text);
                string fuelType = txtFuelType.Text;
                string transmission = txtTransmission.Text;
                int mileage = string.IsNullOrEmpty(txtMileage.Text) ? 0 : int.Parse(txtMileage.Text);
                decimal price = decimal.Parse(txtPrice.Text);
                int statusId = Convert.ToInt32(comboBoxStatus.SelectedValue);
                string description = txtDescription.Text;

                // SQL-запит для оновлення даних
                string query = @"UPDATE Cars
                         SET BrandID = @BrandID, Model = @Model, Year = @Year,
                             BodyTypeID = @BodyTypeID, EngineCapacity = @EngineCapacity,
                             FuelType = @FuelType, Transmission = @Transmission, 
                             Mileage = @Mileage, Price = @Price, StatusID = @StatusID, 
                             Description = @Description
                         WHERE CarID = @CarID";

                // Параметри запиту
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CarID", _carId),
                    new SqlParameter("@BrandID", brandId),
                    new SqlParameter("@Model", model),
                    new SqlParameter("@Year", year),
                    new SqlParameter("@BodyTypeID", bodyTypeId),
                    new SqlParameter("@EngineCapacity", engineCapacity),
                    new SqlParameter("@FuelType", fuelType),
                    new SqlParameter("@Transmission", transmission),
                    new SqlParameter("@Mileage", mileage),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@StatusID", statusId),
                    new SqlParameter("@Description", description)
                };

                DatabaseHelper db = new DatabaseHelper();
                db.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Дані успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Закрити форму після збереження
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
