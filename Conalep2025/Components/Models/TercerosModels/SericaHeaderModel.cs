using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Conalep2025.Models
{
    [Keyless]
    public class SericaHeaderModel
    {
        public string TI { get; set; }
        public string ENC0 { get; set; }
        public string FechaPago { get; set; }
        public string ENC1 { get; set; }
        public int ENC2 { get; set; }
        public decimal IMPORTE_11301 { get; set; }
        public decimal IM12201 { get; set; }
        public decimal IM12301 { get; set; }
        public decimal IM13101 { get; set; }
        public decimal IMPORTE_13102 { get; set; }
        public decimal IM13401 { get; set; }
        public decimal IM13402 { get; set; }
        public decimal IM13407 { get; set; }
        public decimal IM13408 { get; set; }
        public decimal IM13411 { get; set; }
        public decimal IMPORTE_15403 { get; set; }
        public decimal IMPORTE_15402 { get; set; }
        public decimal despensa { get; set; }
        public decimal prestamos { get; set; }
        public decimal superissste { get; set; }
        public decimal ade_medico { get; set; }
        public decimal CHC { get; set; }
        public decimal pension { get; set; }
        public decimal faltas { get; set; }
        public decimal retardos { get; set; }
        public decimal TOT_PERC { get; set; }
        public decimal tot_dedu { get; set; }
        public decimal tot_neto { get; set; }

    }
}
