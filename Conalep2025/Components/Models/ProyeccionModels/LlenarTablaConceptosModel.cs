using System.Security.Cryptography.Pkcs;

namespace Conalep2025.Models.ProyeccionModels
{
    public class LlenarTablaConceptosModel
    {
        public int partida { get; set; }
        public string concepto { get; set; }
        public int UR { get; set; }
        public string FuenteF { get; set; }
    }
}
