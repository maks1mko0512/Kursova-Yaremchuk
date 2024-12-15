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
    public partial class AddCarForm : Form
    {
        public AddCarForm()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Отримуємо значення з полів
                int brandID = Convert.ToInt32(comboBoxBrand.SelectedValue);
                string model = txtModel.Text;
                int year = int.Parse(txtYear.Text);
                int bodyTypeID = Convert.ToInt32(comboBoxBodyType.SelectedValue);
                decimal engineCapacity = decimal.Parse(txtEngineCapacity.Text);
                string fuelType = txtFuelType.Text;
                string transmission = txtTransmission.Text;
                int mileage = string.IsNullOrEmpty(txtMileage.Text) ? 0 : int.Parse(txtMileage.Text);
                decimal price = decimal.Parse(txtPrice.Text);
                int statusID = Convert.ToInt32(comboBoxStatus.SelectedValue);
                string description = txtDescription.Text;

                // SQL-запит для додавання
                string query = "INSERT INTO Cars (BrandID, Model, Year, BodyTypeID, EngineCapacity, FuelType, Transmission, Mileage, Price, StatusID, Description) " +
                               "VALUES (@BrandID, @Model, @Year, @BodyTypeID, @EngineCapacity, @FuelType, @Transmission, @Mileage, @Price, @StatusID, @Description)";

                // Параметри запиту
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BrandID", brandID),
                    new SqlParameter("@Model", model),
                    new SqlParameter("@Year", year),
                    new SqlParameter("@BodyTypeID", bodyTypeID),
                    new SqlParameter("@EngineCapacity", engineCapacity),
                    new SqlParameter("@FuelType", fuelType),
                    new SqlParameter("@Transmission", transmission),
                    new SqlParameter("@Mileage", mileage),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@StatusID", statusID),
                    new SqlParameter("@Description", description)
                };

                // Виконання запиту
                DatabaseHelper db = new DatabaseHelper();
                db.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Автомобіль успішно додано!");
                this.Close(); // Закриває форму після додавання
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання: {ex.Message}");
            }

        }

        private void AddCarForm_Load(object sender, EventArgs e)
        {
            DatabaseHelper db = new DatabaseHelper();

            // Завантаження Brand
            string brandQuery = "SELECT BrandID, BrandName FROM Brands";
            DataTable brands = db.ExecuteQuery(brandQuery);
            comboBoxBrand.DataSource = brands;
            comboBoxBrand.DisplayMember = "BrandName";
            comboBoxBrand.ValueMember = "BrandID";

            // Завантаження BodyType
            string bodyTypeQuery = "SELECT BodyTypeID, BodyTypeName FROM BodyTypes";
            DataTable bodyTypes = db.ExecuteQuery(bodyTypeQuery);
            comboBoxBodyType.DataSource = bodyTypes;
            comboBoxBodyType.DisplayMember = "BodyTypeName";
            comboBoxBodyType.ValueMember = "BodyTypeID";

            // Завантаження Status
            string statusQuery = "SELECT StatusID, StatusName FROM CarStatuses";
            DataTable statuses = db.ExecuteQuery(statusQuery);
            comboBoxStatus.DataSource = statuses;
            comboBoxStatus.DisplayMember = "StatusName";
            comboBoxStatus.ValueMember = "StatusID";
        }
    }
}
