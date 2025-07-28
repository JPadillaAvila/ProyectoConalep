namespace Conalep2025.Models
{
    public class reg_nominas
    {
        public int Id { get; set; } // El campo 'id' en MySQL es 'int(10) unsigned'
        public int Qna { get; set; } // El campo 'qna' en MySQL es 'int(11)'
        public int Year { get; set; } // El campo 'year' en MySQL es 'int(11)'
        public string? Descripcion { get; set; } // El campo 'descripcion' en MySQL es 'varchar(50)'
        public DateTime? FecPago { get; set; } // El campo 'fec_pago' en MySQL es 'date'
        public long Status { get; set; } // El campo 'status' en MySQL es 'bigint(20)'
        public string? DbTable { get; set; } // El campo 'db_table' en MySQL es 'varchar(120)'
        //public string info { get; set; }
    }
}
