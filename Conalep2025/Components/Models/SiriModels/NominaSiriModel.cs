using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Conalep2025.Models.SiriModels
{
    public class NominaSiriModel
    {
        public string RFC { get; set; }
        public string CURP { get; set; }
        public string NOMBRE { get; set; }
        public string APE_PAT { get; set; }
        public string APE_MAT { get; set; }
        public string TIPO { get; set; }
        public int CT { get; set; }
        public decimal SDO_SAR { get; set; }
        public int REGISTROS { get; set; }
        public decimal VIVIENDA { get; set; }
        public decimal RETIROYCESANTIA { get; set; }
        public decimal TRAB_CESANTIA { get; set; }
        public decimal TRAB_AHORRO { get; set; }
        public decimal APO_AHORRO { get; set; }
        public DateTime FEC_ING { get; set; }


    }
}
