using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.CustomerData
{
    // Asiakasluokka
    public class Customer
    {
        private int _id;

        public int Id { get { return _id; } set { _id = value; } }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsInActive { get; set; }       


        // Alustetaan luokan ominaisuudet tyhjäksi merkkijonoksi
        public Customer()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            CompanyName = string.Empty;
            StreetAddress = string.Empty;
            City = string.Empty;
            PostalCode = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            IsInActive = false;

        }

        // Tuo asiakkaan IDn, etunimen ja sukunimen merkkijonona
        public override string ToString()
        {
            return Id.ToString() + " " + FirstName + " " + LastName;
        }

    }
}
