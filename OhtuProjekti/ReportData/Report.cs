using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vt_systems.CustomerData;
using vt_systems.DeviceData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.ReportData
{
    // Raportti luokka - tämä sisältää kaikki jotta voidaan luoda raportti
    public class Report
    {
        public Customer Customer { get; set; }
        public Workspace Workspace { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Sum { get; set; }
        public int DaysRented { get; set; }
        public Device Device { get; set; }
        public Service Service { get; set; }     
    }
}
