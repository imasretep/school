using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vt_systems.DeviceData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.ReservationData
{
    // Varaukselle tulevat (laitteet, palvelut, toimitila)
    public class ReservationObject
    {
        private int _id;

        public int ObjectID { get { return _id; } set { _id = value; } }
        public int ReservationID { get; set; }
        public int SeriveID { get; set; }
        public int DeviceID { get; set; }
        public int WorkspaceID { get; set; }
        public int Qty { get; set; }

        /// <summary>
        /// Ei käytössä?
        /// </summary>
        public double Price { get; set; }

        public Device Device { get; set; }
        public Service Service { get; set; }
        public Workspace Workspace { get; set; }


        // Default values for new object
        public ReservationObject()
        {
            Qty = 1;
            Price = 0;
        }
    }
}
