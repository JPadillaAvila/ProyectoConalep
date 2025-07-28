using System;
using System.ComponentModel.DataAnnotations;

namespace Conalep2025.Models
{


    public class Expediente
    {
        [Key]
        public string ExpedienteID { get; set; }

        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres.")]
        public string CURP { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "El RFC debe tener exactamente 13 caracteres.")]
        public string RFC { get; set; }

        public DateTime FechaValidacionRFC { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public int Edad { get; set; }

        // Cambiado a tipo Sexo
        public string Sexo { get; set; }

        public string? Estado { get; set; }

        public string? Municipio { get; set; }

        public string? Localidad { get; set; }

        public string? Calle { get; set; }

        public string? NumeroExterior { get; set; }

        public string? NumeroInterior { get; set; }

        public string? CodigoPostal { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }
        
        [EmailAddress(ErrorMessage = "El campo de correo electrónico no tiene un formato válido.")]
        public string Correo { get; set; }

        public string NSS { get; set; }

        //ENUM
        public string GrupoSanguineo { get; set; }

        //ENUM
        public string GradoEstudios { get; set; }

        public string OtroEmpleo { get; set; }
    }
}