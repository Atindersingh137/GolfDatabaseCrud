using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Golf
{
    public partial class Form1 : Form
    {

        // connection string
        private string connectionString = @"Data Source=LAPTOP-52CB3J4K\SQLEXPRESS;Initial Catalog=Golf;Integrated Security=True;";
        //Data Source=LAPTOP-52CB3J4K\SQLEXPRESS;Initial Catalog=Golf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        //Data Source=LAPTOP-52CB3J4K\SQLEXPRESS;Initial Catalog=Golf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        
        SqlConnection Con = new SqlConnection();
        DataTable GolfTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
            
            Con.ConnectionString = connectionString;
           

        }
        private void loaddb()
        {

            // load database colums
            datatablecolumns();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string QueryString = @"SELECT * FROM Golf order by ID";
                //open your connection 
                connection.Open();

                SqlCommand command = new SqlCommand(QueryString, connection);

                //start your db reader 


                SqlDataReader reader = command.ExecuteReader();
                 while(reader.Read())/// looping through data 

                {
                    GolfTable.Rows.Add(
                        reader["ID"],
                        reader["Title"],
                        reader["Firstname"],
                        reader["Surname"],
                        reader["Gender"],
                        reader["DOB"],
                        reader["Street"],
                        reader["Suburb"],
                        reader["City"],
                        reader["Available week days"],
                        reader["HAndicap"]);


                }
                reader.Close();  // close reader
                connection.Close(); // close connection
                // add the database into your data grid view 


                dgvGolf.DataSource = GolfTable;



            }
        }
        private void datatablecolumns()
        {
            // clear the old data 
            GolfTable.Clear();

            // Add in the column title to the database 
            try
            {

                GolfTable.Columns.Add("ID");
                GolfTable.Columns.Add("Title");
                GolfTable.Columns.Add("FirstName");
                GolfTable.Columns.Add("SurName");
                GolfTable.Columns.Add("Gender");
                GolfTable.Columns.Add("DOB");
                GolfTable.Columns.Add("Street");
                GolfTable.Columns.Add("Subrub");
                GolfTable.Columns.Add("City");
                GolfTable.Columns.Add("Available week days");
                GolfTable.Columns.Add("Handicap");
                
            }
            catch(Exception e)
            {
               // MessageBox.Show(e.Message);
            }

        }
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            loaddb();
            
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvGolf_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            //get the value of clicked cell
            string newvalue = dgvGolf.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            //show the value on the header 
            this.Text= "Row : " + e.RowIndex.ToString() + " Col: " + e.ColumnIndex.ToString() + " Value = " + newvalue;

            //fill text boxes
            txtID.Text = dgvGolf.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvGolf.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtFirstName.Text = dgvGolf.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSurName.Text = dgvGolf.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtGender.Text = dgvGolf.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtDOB.Text = dgvGolf.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtStreet.Text = dgvGolf.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtSuburb.Text = dgvGolf.Rows[e.RowIndex].Cells[7].Value.ToString();
            txtCity.Text = dgvGolf.Rows[e.RowIndex].Cells[8].Value.ToString();
            txtAWD.Text = dgvGolf.Rows[e.RowIndex].Cells[9].Value.ToString();
            txtHandicap.Text = dgvGolf.Rows[e.RowIndex].Cells[10].Value.ToString();
        } catch {
}

        }

        private void btnCountGolfers_Click(object sender, EventArgs e)
        {
            // a simple Scalar query just returning one value.
            string queryString = "SELECT COUNT(ID) FROM Golf";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand Command = new SqlCommand(queryString, connection);
                connection.Open();
                txtGolfers.Text = Command.ExecuteScalar().ToString();
                connection.Close();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            // this puts the parameters into the code so that the data in the text boxes is added to the database
            string NewEntry = "INSERT INTO Golf (Title, Firstname, Surname, Gender, DOB, Street, Suburb, City, [Available week days], Handicap) VALUES ( @Title, @Firstname, @Surname, @Gender, @DOB, @Street, @Suburb, @City, @Available, @Handicap)";
            using (SqlCommand newdata = new SqlCommand(NewEntry, Con))
            {
                newdata.Parameters.AddWithValue("@Title", txtTitle.Text);
                newdata.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                newdata.Parameters.AddWithValue("@SurName", txtSurName.Text);
                newdata.Parameters.AddWithValue("@Gender", txtGender.Text);
                newdata.Parameters.AddWithValue("@DOB", txtDOB.Text);
                newdata.Parameters.AddWithValue("@City", txtCity.Text);
                newdata.Parameters.AddWithValue("@Street", txtStreet.Text);
                newdata.Parameters.AddWithValue("@Suburb", txtSuburb.Text);
                newdata.Parameters.AddWithValue("@Available", txtAWD.Text);
                newdata.Parameters.AddWithValue("@Handicap", txtHandicap.Text);

                Con.Open(); // open connection to tyhe database 
                newdata.ExecuteNonQuery(); // Run the query 
                Con.Close(); // clos ethe connection of the database 

                MessageBox.Show("Data Has Been Inserted");
                loaddb();
                 

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //this updates existing data in the database where the ID of the data equals the ID in the text box
            string updatestatement = "UPDATE Golf set Title=@Title, Firstname=@Firstname, Surname=@Surname, Gender=@Gender, DOB=@DOB, Street=@Street, Suburb=@Suburb, City=@City, [Available week days]=@Available, Handicap=@Handicap where ID = @ID";
            SqlCommand update = new SqlCommand(updatestatement, Con);
            //create the parameters and pass the data from the textboxes
            update.Parameters.AddWithValue("@ID", txtID.Text);
            update.Parameters.AddWithValue("@Title", txtTitle.Text);
            update.Parameters.AddWithValue("@Firstname", txtFirstName.Text);
            update.Parameters.AddWithValue("@Surname", txtSurName.Text);
            update.Parameters.AddWithValue("@Street", txtStreet.Text);
            update.Parameters.AddWithValue("@Suburb", txtSuburb.Text);
            update.Parameters.AddWithValue("@City", txtCity.Text);
            update.Parameters.AddWithValue("@Gender", txtGender.Text);
            update.Parameters.AddWithValue("@DOB", txtDOB.Text);
            update.Parameters.AddWithValue("@Handicap", txtHandicap.Text);
            update.Parameters.AddWithValue("@Available", txtAWD.Text);
            Con.Open();
            //its NONQuery as data is only going up
            update.ExecuteNonQuery();
            Con.Close();
            loaddb();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string DeleteCommand = "Delete Golf where ID = @ID";
            SqlCommand DeleteData = new SqlCommand(DeleteCommand, Con);
            DeleteData.Parameters.AddWithValue("@ID", txtID.Text);
            Con.Open();
            DeleteData.ExecuteNonQuery();
            Con.Close();
            loaddb();
            MessageBox.Show("Data is Deleted");
        }
    }
}
