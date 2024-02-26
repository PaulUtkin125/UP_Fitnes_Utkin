using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_Fitnes_Utkin.Model
{
    class Tovar_Sklad
    {
        public int Id { get; set; }
        public string Name_tovar { get; set; }
        public KategorTovara Category { get; set; }
        public int CategID { get; set; }
        public int Count_tekyshee { get; set; }
        public double Price_sht { get; set; }

    }
}
