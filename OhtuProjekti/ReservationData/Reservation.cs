using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vt_systems.CustomerData;

namespace vt_systems.ReservationData
{
    // Varaus luokka
    public class Reservation
    {
        private int _id;

        public int ReservationId { get { return _id; } set { _id = value; } }
        public DateTime ReservationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int CustomerID { get; set; }
        public int OfficeID { get; set; }
        public string ReservationInfo { get; set; }
        public bool IsBilled { get; set; }
        public Customer Customer { get; set; }

        public Reservation()
        {
            IsBilled= false;
        }

        public ObservableCollection<ReservationObject> ReservedObjects { get; set; } = new ObservableCollection<ReservationObject>(); // en tiedä onko edes tarpeen, mutta alustavasti tuli mieleen ratkaisuna

        // lisää olio kokoelmaan
        public void AddObjectToCollection(ReservationObject reservationObject)
        {
            ReservedObjects.Add(reservationObject);
        }

        // palauttaa IDarvon string muuttujana
        public override string ToString()
        {
            return ReservationId.ToString();
        }

    }
}
