using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost_Assignment_0512
{
    [Serializable]
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }

        public string CitizenID { get; set; }
        public string Phone { get; set; }

        public string Mail { get; set; }
        public string Adress { get; set; }

        public string CompanyName { get; set; }

        public string TaxNo { get; set; }

        public string CompanyAdress { get; set; }
        public string BusinessArea { get; set; }


     
        public Customer(string name, string surname, string birthday, string citizenid, string phone, string mail, string adress, string companyname , string taxno, string companyadress, string businessarea   )
        {
            Name = name;
            Surname = surname;
            Birthday = birthday;
            CitizenID = citizenid;
            Phone = phone;
            Mail = mail;
            Adress = adress;
            CompanyName = companyname;
            TaxNo = taxno;
            CompanyAdress = companyadress;
            BusinessArea = businessarea;
        }


    }
}
