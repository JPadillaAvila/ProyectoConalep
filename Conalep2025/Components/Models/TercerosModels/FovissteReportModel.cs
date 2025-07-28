using Microsoft.EntityFrameworkCore;

namespace Conalep2025.Models
{
    [Keyless]
    public class FovissteReportModel
    {

        public string TIPO { get; set; }
        public string NOMBRE { get; set; }
        public string RFC { get; set; }
        public decimal SDO_BASE { get; set; }
        public decimal FOVISSSTE { get; set; }
        public decimal SEGURO_DE_DANOS { get; set; }
        public decimal FOVISSSTE_CF { get; set; }
        public decimal SDO_HON { get; set; }
    }
}
