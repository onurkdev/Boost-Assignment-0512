using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boost_Assignment_0512
{
    using Newtonsoft;
    using System.IO;
    
    public partial class Form1 : Form
    {
        Customer selectedCustomer;
        string[] BAList = {"Tekstil", "IT", "Otomobil", "Perakende", "Bankacılık", "İhracat" };
        string DbPath = @"C:\Users\Onur.Kurtulmus\source\repos\Boost-Assignment-0512\Boost-Assignment-0512\database.json";
        public Form1()
        {
            InitializeComponent();
        }

        private void FormClearer()
        {
            txtName.Clear();
            txtAdress.Clear();
            txtCitizenID.Clear();
            txtCompanyAdress.Clear();
            txtCompanyName.Clear();
            txtMail.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtSurname.Clear();
            txtTaxNo.Clear();
            cmbBusinessArea.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            selectedCustomer = null;
            
        }

        private void BAFiller()
        {
            cmbBusinessArea.Items.Clear();
            cmbBusinessArea.Items.AddRange(BAList);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BAFiller();
            DatabaseParseAndUpdateGrid();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = true;
        }

        public Customer CustomerFormParse()
        {

            Customer customer = new Customer(txtName.Text,txtSurname.Text,dtpBirthday.Value.ToString(),txtCitizenID.Text,txtPhone.Text,txtMail.Text,txtAdress.Text,txtCompanyName.Text,txtTaxNo.Text,txtCompanyAdress.Text,cmbBusinessArea.SelectedItem.ToString());
            
            string response = ReadAllData();

            if (Newtonsoft.Json.JsonConvert.DeserializeObject(response) == null)
                customer.Id = 0;
            else
            {
                List<Customer> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(response);
                customer.Id = list.Count;
            }
            return customer;
        }

        public string ReadAllData()
        {
            string response;
            using (StreamReader stream = new StreamReader(DbPath))
            {
                response = stream.ReadToEnd();
            }
            return response;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<Customer> oldcustomerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(ReadAllData());

            Customer currentcustomer = CustomerFormParse();
            if (oldcustomerList != null)
            {
                currentcustomer.Id = oldcustomerList.Count;
                oldcustomerList.Add(currentcustomer);
            }
            else
            {
                currentcustomer.Id = 0;
                List<Customer> newcustomerList = new List<Customer>();
                oldcustomerList = newcustomerList;
                oldcustomerList.Add(currentcustomer);
            }

            AddListToJson(oldcustomerList);
            DatabaseParseAndUpdateGrid();
            FormClearer();
        }

        private void AddListToJson(List<Customer> list)
        {
            string request = Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            using (StreamWriter stream = new StreamWriter(DbPath))
            {
                stream.Write(request);
            }
            
        }

        private List<Customer> GetAsList()
        {
            string databaseResponse = ReadAllData();
            List<Customer> customerList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(databaseResponse);
            return customerList;
        }
        private void DatabaseParseAndUpdateGrid()
        {
            DataGridRefresh(GetAsList());
        }

        private void DataGridRefresh (List<Customer> list)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            List<Customer> customerList = (List<Customer>)dataGridView1.DataSource;
            int selectedindex = dataGridView1.SelectedRows[0].Index; 
            Customer customer = customerList.ElementAt(selectedindex);
            FormFiller(customer);
            selectedCustomer=customer;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }
        public void FormFiller(Customer customer)
        {
            txtAdress.Text = customer.Adress;
            txtCitizenID.Text = customer.CitizenID;
            txtCompanyAdress.Text = customer.CompanyAdress;
            txtCompanyName.Text = customer.CompanyName;
            txtMail.Text=customer.Mail;
            txtPhone.Text=customer.Phone;
            txtName.Text = customer.Name;
            txtSurname.Text = customer.Surname;
            txtTaxNo.Text = customer.TaxNo;
            dtpBirthday.Value = DateTime.Parse(customer.Birthday);
            cmbBusinessArea.Text = customer.BusinessArea;

        }
        private List<Customer> DeleteAtSelected(Customer customer)
        {
            List<Customer> customers = GetAsList();
            customers.RemoveAt(customer.Id);
            return customers;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<Customer> customers  = DeleteAtSelected(selectedCustomer);
            Customer updatedCustomer = CustomerFormParse();
            updatedCustomer.Id = selectedCustomer.Id;
            customers.Insert(updatedCustomer.Id, updatedCustomer);
            AddListToJson(customers);
            FormClearer();
            DatabaseParseAndUpdateGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult response = MessageBox.Show("Silmek istediğinize Emin misiniz?");
            if (response == DialogResult.OK)
            {
                AddListToJson(DeleteAtSelected(selectedCustomer));
                DatabaseParseAndUpdateGrid();
                MessageBox.Show("Silme işlemi Başarılı");
                FormClearer();
            }
            else
            {
                return;
            }
            

            
        }
    }
    
}
