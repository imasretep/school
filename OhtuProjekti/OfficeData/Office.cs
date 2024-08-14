using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.OfficeData
{
    // Toimipiste luokka
    public class Office
    {
        private int _id;

        public int OfficeID { get { return _id; } set { _id = value; } }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; } 
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsInActive { get; set; }


        // Alustetaan ominaisuudet tyhiksi merkkijonoiksi
        public Office()
        {
            StreetAddress = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            IsInActive = false;
        }
    }
}
