using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings
                ["WpfZooManager.Properties.Settings.Database01ConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);
            Zoos();
            ShowAnnimals();
        }

        private void Zoos()
        {
            try
            {
                string query = "select * from Zoo";

                //use a sqlAdapter to connect and run the query
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    //filling the sqlAdapter onto our zooTable
                    sqlDataAdapter.Fill(zooTable);

                    //which information of the Table from DataTable should be shown on ListBox?
                    MyLB.DisplayMemberPath = "LOCATION";
                    //Which Value should be delivered, when an iten is selected from our listBox?
                    MyLB.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate 
                    MyLB.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                //MessageBox.Show(e.ToString());
            }

         
        }

        private void ShowAssociatedAnimls()
        {
            try
            {
                //joining Animal table with ZooAnimal table
                string query = "select * from Animal a join ZooAnimal za " +
                    "on a.Id = za.AnimalId where za.ZooId = @ZooId ";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //use a sqlAdapter to connect and run the query
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", MyLB.SelectedValue);

                    DataTable animalTable = new DataTable();

                    //filling the sqlAdapter onto our zooTable
                    sqlDataAdapter.Fill(animalTable);

                    //which information of the Table from DataTable should be shown on ListBox?
                    AssociatedAnimalLB.DisplayMemberPath = "Name";
                    //Which Value should be delivered, when an iten is selected from our listBox?
                    AssociatedAnimalLB.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate 
                    AssociatedAnimalLB.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                //MessageBox.Show(e.ToString());
            }


        }

        private void MyLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showZooInTextBox();
            ShowAssociatedAnimls();
        }

        private void ShowAnnimals()
        {
            try
            {
                string query = "select * from Animal";

                //use a sqlAdapter to connect and run the query
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable AnimalTable = new DataTable();

                    //filling the sqlAdapter onto our zooTable
                    sqlDataAdapter.Fill(AnimalTable);

                    //which information of the Table from DataTable should be shown on ListBox?
                    AnimalsLB.DisplayMemberPath = "Name";
                    //Which Value should be delivered, when an iten is selected from our listBox?
                    AnimalsLB.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate 
                    AnimalsLB.ItemsSource = AnimalTable.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }


        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete Zoo button was clicked");

            try
            {
                string query = "delete from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", MyLB.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }finally
            {
                sqlConnection.Close();
                Zoos();
            }       
            
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Zoo button was clicked");

            try
            {
                string query = "insert into Zoo values (@LOCATION)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@LOCATION", MyTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                Zoos();
            }
        }

        private void addAnimalToZoo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Animal to Zoo button was clicked");

            try
            {
                string query = "insert into ZooAnimal values (@ZooId,@AnimalId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", MyLB.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsLB.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimls();
            }
        }

         private void removeAnimal_Click(object sender, RoutedEventArgs e)
          {
              MessageBox.Show("Remove Animal button was clicked");

              try
              {
                string query = " delete a from Animal a join ZooAnimal za " +
                    "on a.Id = za.AnimalId where a.Id = @ZooId ";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                  sqlConnection.Open();
                  sqlCommand.Parameters.AddWithValue("@ZooId", AssociatedAnimalLB.SelectedValue);
                  sqlCommand.ExecuteScalar();
              }
              catch (Exception ex)
              {

                  MessageBox.Show(ex.ToString());
              }
              finally
              {
                  sqlConnection.Close();
                  ShowAssociatedAnimls();
              }

          }

        private void deleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete Animal button was clicked");

            try
            {
                string query = "delete from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsLB.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnnimals();
                
            }
        }

        private void addAnimal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Animal button was clicked");

            try
            {
                string query = "insert into Animal values (@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", MyTextBox2.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnnimals();
            }
        }
        

        private void showZooInTextBox()
        {
            try
            {
                string query = "select LOCATION from Zoo where Id = @ZooId ";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //use a sqlAdapter to connect and run the query
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", MyLB.SelectedValue);

                    DataTable zooTable = new DataTable();

                    //filling the sqlAdapter onto our zooTable
                    sqlDataAdapter.Fill(zooTable);

                    //getting the first entry and locationfrom the table
                    MyTextBox.Text = zooTable.Rows[0]["LOCATION"].ToString();
                }
            }
            catch (Exception e)
            {

                //MessageBox.Show(e.ToString());
            }


        }

        private void showAnimalInTextBox()
        {
            try
            {
                string query = "select Name from Animal where Id = @AnimalId ";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //use a sqlAdapter to connect and run the query
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsLB.SelectedValue);

                    DataTable animalTable = new DataTable();

                    //filling the sqlAdapter onto our zooTable
                    sqlDataAdapter.Fill(animalTable);

                    //getting the first entry and locationfrom the table
                    MyTextBox2.Text = animalTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception e)
            {

                //MessageBox.Show(e.ToString());
            }


        }

        private void AnimalsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showAnimalInTextBox();
        }

        private void updateZoo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Update Zoo button was clicked");

            try
            {
                string query = "update Zoo Set LOCATION = @LOCATION where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", MyLB.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@LOCATION", MyTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                Zoos();
            }
        }

        private void updateAnimal_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Update Animal button was clicked");

            try
            {
                string query = "update Animal Set Name = @Name where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsLB.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", MyTextBox2.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnnimals();
            }
        }
    }
}
