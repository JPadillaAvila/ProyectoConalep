using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Conalep2025.Models
{
    [Keyless]
    public class SericaDetalleReporteModel
    {
        public string? TI { get; set; }
        public string? NSS { get; set; }
        public string? NOMBRE { get; set; }
        public string? APE_PAT { get; set; }
        public string? APE_MAT { get; set; }
        public string? RFC { get; set; }
        public string? CURP { get; set; }
        public string? SEXO { get; set; }
        public string? PAGADURIA { get; set; }
        public string? NO_EMPLE { get; set; }
        public string? NUM_CHEQ { get; set; }
        public int? REGIMEN_ISSSTE { get; set; }
        public int? TIPO_CONTRATO { get; set; }
        public decimal TOT_PERC { get; set; }
        public decimal TOT_DEDU { get; set; }
        public string? PARTIDA_11301 { get; set; }
        public string? CONCEPTO_11301 { get; set; }
        public decimal IMPORTE_11301 { get; set; }
        public string? PT12201 { get; set; }
        public string? CP12201 { get; set; }
        public string? IM12201 { get; set; }
        public string? PT12301 { get; set; }
        public string? CP12301 { get; set; }
        public string? IM12301 { get; set; }
        public string? PT13101 { get; set; }
        public string? CP13101 { get; set; }
        public string? IM13101 { get; set; }
        public string? PARTIDA_13102 { get; set; }
        public string? CONCEPTO_13102 { get; set; }
        public decimal IMPORTE_13102 { get; set; }
        public string? IPT13401 { get; set; }
        public string? ICP13401 { get; set; }
        public string? IM13401 { get; set; }
        public string? IPT13402 { get; set; }
        public string? ICP13402 { get; set; }
        public string? IM13402 { get; set; }
        public string? IPT13407 { get; set; }
        public string? ICP13407 { get; set; }
        public string? IM13407 { get; set; }
        public string? IPT13408 { get; set; }
        public string? ICP13408 { get; set; }
        public string? IM13408 { get; set; }
        public string? IPT13411 { get; set; }
        public string? ICP13411 { get; set; }
        public string? IM13411 { get; set; }
        public string? PARTIDA_15403 { get; set; }
        public string? CONCEPTO_15403 { get; set; }
        public decimal IMPORTE_15403 { get; set; }
        public string? PARTIDA_15402 { get; set; }
        public string? CONCEPTO_15402 { get; set; }
        public decimal IMPORTE_15402 { get; set; }
        public string? PARTIDA_10001 { get; set; }
        public string? CONCEPTO_10001 { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
      
        public decimal IMPORTE_10001 { get; set; }
        public string? PARTIDA_10002 { get; set; }
        public string? CONCEPTO_10002 { get; set; }
        public decimal IMPORTE_10002 { get; set; }
        public string? PARTIDA_20001 { get; set; }
        public string? CONCEPTO_20001 { get; set; }
        public decimal IMPORTE_20001 { get; set; }
        public string? PARTIDA_20002 { get; set; }
        public string? CONCEPTO_20002 { get; set; }
        public decimal IMPORTE_20002 { get; set; }
        public string? PT20003 { get; set; }
        public string? CP20003 { get; set; }
        public string? IM20003 { get; set; }
        public string? PT20004 { get; set; }
        public string? CP20004 { get; set; }
        public string? IM20004 { get; set; }
        public string? PARTIDA_20005 { get; set; }
        public string? CONCEPTO_20005 { get; set; }
        public decimal IMPORTE_20005 { get; set; }
        public string? PARTIDA_20006 { get; set; }
        public string? CONCEPTO_20006 { get; set; }
        public decimal IMPORTE_20006 { get; set; }
        public string? PARTIDA_20007 { get; set; }
        public string? CONCEPTO_20007 { get; set; }
        public decimal IMPORTE_20007 { get; set; }
        public string? PT20008 { get; set; }
        public string? CP20008 { get; set; }
        public string? IM20008 { get; set; }
        public decimal TOT_NETO { get; set; }
        public decimal SDO_ISS { get; set; }
        public decimal DESPENSA { get; set; }
        public decimal PRESTAMOS { get; set; }
        public decimal CHC { get; set; }
        public decimal PENSION { get; set; }
        public string? Tipo { get; set; }
        public decimal TOT_DEDU1 { get; set; }

    }
}
