using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.WorkspaceData
{
    // Toimitila luokka
    public class Workspace
    {
        private int _id;

        public int WorkspaceID { get { return _id; } set { _id = value; } }
        public string? WSDescription { get; set; }
        public string? WSName { get; set; }
        public double PriceByHour { get; set; }
        public double PriceByDay { get; set; }
        public double PriceByWeek { get; set; }
        public double PriceByMonth { get; set; }
        public double UnitPrice { get; set; }
        public bool IsInActive { get; set; }
        public int OfficeID { get; set; } // tilaan liitetty OfficeID
        public string OfficeCity { get; set; }

        // Oletusarvot
        public Workspace()
        {
            WSDescription = string.Empty;
            WSName = string.Empty;
            PriceByHour = 0;
            PriceByDay = 0;
            PriceByWeek = 0;
            PriceByMonth = 0;
            IsInActive = false;
        }

        public static implicit operator ObservableCollection<object>(Workspace v)
        {
            throw new NotImplementedException();
        }
    }
}
