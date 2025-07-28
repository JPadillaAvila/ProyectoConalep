
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Conalep2025.Data;
using Conalep2025.Models;
using Conalep2025.Pages.Expedientes;
using System.Data;
using System.IO.Pipelines;
using Conalep2025.Models.SiriModels;
using Conalep2025.Models.ProyeccionModels;
namespace Conalep2025.Repository

{
    public class RepositoryMySQL : IrepositoryMySQL
    {
        private readonly ApplicationDbContextMySQL _contexto2;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public RepositoryMySQL(ApplicationDbContextMySQL context, IConfiguration configuration)
        {
            _contexto2 = context;
            _connectionString = configuration.GetConnectionString("MysqlConnection");
        }


        public async Task<Expediente> GetExpedienteByCURP(string curp)
        {
            return await _contexto2.Expedientes
                .FirstOrDefaultAsync(e => e.CURP == curp);
        }

        public async Task<Expediente> ActualizarExpediente(string ExpedienteId, Expediente actualizarExpediente)
        {

            var ExpedienteDesdeDb = await _contexto2.Expedientes.FindAsync(ExpedienteId);
            ExpedienteDesdeDb.ExpedienteID = actualizarExpediente.ExpedienteID;
            ExpedienteDesdeDb.CURP = actualizarExpediente.CURP;
            ExpedienteDesdeDb.RFC = actualizarExpediente.RFC;
            ExpedienteDesdeDb.FechaValidacionRFC = actualizarExpediente.FechaValidacionRFC;
            ExpedienteDesdeDb.PrimerApellido = actualizarExpediente.PrimerApellido;
            ExpedienteDesdeDb.SegundoApellido = actualizarExpediente.SegundoApellido;
            ExpedienteDesdeDb.Edad = actualizarExpediente.Edad;
            ExpedienteDesdeDb.Sexo = actualizarExpediente.Sexo;
            ExpedienteDesdeDb.Estado = actualizarExpediente.Estado;
            ExpedienteDesdeDb.Municipio = actualizarExpediente.Municipio;
            ExpedienteDesdeDb.Localidad = actualizarExpediente.Localidad;
            ExpedienteDesdeDb.Calle = actualizarExpediente.Calle;
            ExpedienteDesdeDb.NumeroExterior = actualizarExpediente.NumeroExterior;
            ExpedienteDesdeDb.NumeroInterior = actualizarExpediente.NumeroInterior;
            ExpedienteDesdeDb.CodigoPostal = actualizarExpediente.CodigoPostal;
            ExpedienteDesdeDb.Telefono = actualizarExpediente.Telefono;
            ExpedienteDesdeDb.Celular = actualizarExpediente.Celular;
            ExpedienteDesdeDb.Correo = actualizarExpediente.Correo;
            ExpedienteDesdeDb.NSS = actualizarExpediente.NSS;
            ExpedienteDesdeDb.GrupoSanguineo = actualizarExpediente.GrupoSanguineo;
            ExpedienteDesdeDb.GradoEstudios = actualizarExpediente.GradoEstudios;
            ExpedienteDesdeDb.OtroEmpleo = actualizarExpediente.OtroEmpleo;

            await _contexto2.SaveChangesAsync();
            return ExpedienteDesdeDb;

        }

        public async Task<Expediente> CrearExpediente(Expediente crearExpediente)
        {
            if (CrearExpediente != null)
            {

                await _contexto2.Expedientes.AddAsync(crearExpediente);
                await _contexto2.SaveChangesAsync();
                return crearExpediente;

            }
            else
            {

                return new Expediente();

            }

        }

        public async Task EliminarExpediente(string ExpedienteId)
        {

            var ExpedienteDesdeDb = await _contexto2.Expedientes.FindAsync(ExpedienteId);
            _contexto2.Remove(ExpedienteDesdeDb);
            await _contexto2.SaveChangesAsync();
        }

        public Task<List<Expediente>> GetExpediente()
        {
            return _contexto2.Expedientes.ToListAsync();
        }

        public async Task<Expediente> GetExpedienteId(string ExpedienteId)
        {
            var ExpedienteDesdeDb = await _contexto2.Expedientes.FindAsync(ExpedienteId);
            if (ExpedienteDesdeDb == null)
            {

                return new Expediente();

            }
            return ExpedienteDesdeDb;
        }
        //---------------------FIN EXPEDIENTES--------------------

        public async Task<List<RegNominasInfo>> GetRegNominasInfoAsync()
        {
            var result = new List<RegNominasInfo>();
            var connectionString = _connectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetRegNominasInfo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new RegNominasInfo
                            {
                                Info = reader.GetString("Info")
                            });
                        }
                    }
                }
            }

            return result;
        }
        public async Task<List<reg_nominas>> GetRegNominasDBTableAsync()
        {
            var connectionString = _connectionString;
            var result = new List<reg_nominas>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetRegNominasDBTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new reg_nominas
                            {
                                DbTable = reader.GetString("DbTable"),
                                FecPago = reader.GetDateTime("fecpago"),
                                Year = reader.GetInt32("Year"),
                                Qna = reader.GetInt32("Qna"),
                                Descripcion = reader.GetString("Descripcion")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<SericaHeaderModel>> GetHeaderSericaPrestamoAsync(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<SericaHeaderModel>();

            using (var connection = new MySqlConnection(connectionString))

            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetHeaderSericaPrestamo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new SericaHeaderModel
                            {
                                ENC0 = reader.GetString("ENC0"),
                                FechaPago = reader.GetString("FechaPago"),
                                ENC1 = reader.GetString("ENC1"),
                                ENC2 = reader.GetInt32("ENC2"),
                                IMPORTE_11301 = reader.GetDecimal("IMPORTE_11301"),
                                IM12201 = reader.GetInt32("IM12201"),
                                IM12301 = reader.GetInt32("IM12301"),
                                IM13101 = reader.GetInt32("IM13101"),
                                IMPORTE_13102 = reader.GetDecimal("IMPORTE_13102"),
                                IM13401 = reader.GetInt32("IM13401"),
                                IM13402 = reader.GetInt32("IM13402"),
                                IM13407 = reader.GetInt32("IM13407"),
                                IM13408 = reader.GetDecimal("IM13408"),
                                IM13411 = reader.GetDecimal("IM13411"),
                                IMPORTE_15403 = reader.GetDecimal("IMPORTE_15403"),
                                IMPORTE_15402 = reader.GetDecimal("IMPORTE_15402"),
                                despensa = reader.GetDecimal("despensa"),
                                prestamos = reader.GetDecimal("prestamos"),
                                superissste = reader.GetDecimal("superissste"),
                                ade_medico = reader.GetDecimal("ade_medico"),
                                CHC = reader.GetDecimal("CHC"),
                                pension = reader.GetDecimal("pension"),
                                faltas = reader.GetDecimal("faltas"),
                                retardos = reader.GetDecimal("retardos"),
                                TOT_PERC = reader.GetDecimal("TOT_PERC"),
                                tot_dedu = reader.GetDecimal("tot_dedu"),
                                tot_neto = reader.GetDecimal("tot_neto")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<SericaDetalleReporteModel>> GetSericaPrestamoAsync(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<SericaDetalleReporteModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();


                using (var command = new MySqlCommand("GetSericaPrestamos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@sueldo", "sueldo");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SericaDetalleReporteModel
                                {
                                    TI = reader.GetString("TI"),
                                    NSS = reader.IsDBNull("NSS") ? string.Empty : reader.GetString("NSS"),
                                    NOMBRE = reader.IsDBNull("NOMBRE") ? string.Empty : reader.GetString("NOMBRE"),
                                    APE_PAT = reader.IsDBNull("APE_PAT") ? string.Empty : reader.GetString("APE_PAT"),
                                    APE_MAT = reader.IsDBNull("APE_MAT") ? string.Empty : reader.GetString("APE_MAT"),
                                    RFC = reader.IsDBNull("RFC") ? string.Empty : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? string.Empty : reader.GetString("CURP"),
                                    SEXO = reader.IsDBNull("SEXO") ? string.Empty : reader.GetString("SEXO"),
                                    PAGADURIA = reader.IsDBNull("PAGADURIA") ? string.Empty : reader.GetString("PAGADURIA"),
                                    NO_EMPLE = reader.IsDBNull("NO_EMPLE") ? string.Empty : reader.GetString("NO_EMPLE"),
                                    NUM_CHEQ = reader.IsDBNull("NUM_CHEQ") ? string.Empty : reader.GetString("NUM_CHEQ"),
                                    REGIMEN_ISSSTE = reader.IsDBNull("REGIMEN_ISSSTE") ? (int?)null : reader.GetInt32("REGIMEN_ISSSTE"),
                                    TIPO_CONTRATO = reader.IsDBNull("TIPO_CONTRATO") ? (int?)null : reader.GetInt32("TIPO_CONTRATO"),
                                    TOT_PERC = (decimal)(reader.IsDBNull("TOT_PERC") ? (decimal?)null : reader.GetDecimal("TOT_PERC")),
                                    TOT_DEDU = (decimal)(reader.IsDBNull("TOT_DEDU") ? (decimal?)null : reader.GetDecimal("TOT_DEDU")),
                                    PARTIDA_11301 = reader.IsDBNull("PARTIDA_11301") ? string.Empty : reader.GetString("PARTIDA_11301"),
                                    CONCEPTO_11301 = reader.IsDBNull("CONCEPTO_11301") ? string.Empty : reader.GetString("CONCEPTO_11301"),
                                    IMPORTE_11301 = (decimal)(reader.IsDBNull("IMPORTE_11301") ? (decimal?)null : reader.GetDecimal("IMPORTE_11301")),
                                    PT12201 = reader.IsDBNull("PT12201") ? string.Empty : reader.GetString("PT12201"),
                                    CP12201 = reader.IsDBNull("CP12201") ? string.Empty : reader.GetString("CP12201"),
                                    IM12201 = reader.IsDBNull("IM12201") ? string.Empty : reader.GetString("IM12201"),
                                    PT12301 = reader.IsDBNull("PT12301") ? string.Empty : reader.GetString("PT12301"),
                                    CP12301 = reader.IsDBNull("CP12301") ? string.Empty : reader.GetString("CP12301"),
                                    IM12301 = reader.IsDBNull("IM12301") ? string.Empty : reader.GetString("IM12301"),
                                    PT13101 = reader.IsDBNull("PT13101") ? string.Empty : reader.GetString("PT13101"),
                                    CP13101 = reader.IsDBNull("CP13101") ? string.Empty : reader.GetString("CP13101"),
                                    IM13101 = reader.IsDBNull("IM13101") ? string.Empty : reader.GetString("IM13101"),
                                    PARTIDA_13102 = reader.IsDBNull("PARTIDA_13102") ? string.Empty : reader.GetString("PARTIDA_13102"),
                                    CONCEPTO_13102 = reader.IsDBNull("CONCEPTO_13102") ? string.Empty : reader.GetString("CONCEPTO_13102"),
                                    IMPORTE_13102 = (decimal)(reader.IsDBNull("IMPORTE_13102") ? (decimal?)null : reader.GetDecimal("IMPORTE_13102")),
                                    IPT13401 = reader.IsDBNull("IPT13401") ? string.Empty : reader.GetString("IPT13401"),
                                    ICP13401 = reader.IsDBNull("ICP13401") ? string.Empty : reader.GetString("ICP13401"),
                                    IM13401 = reader.IsDBNull("IM13401") ? string.Empty : reader.GetString("IM13401"),
                                    IPT13402 = reader.IsDBNull("IPT13402") ? string.Empty : reader.GetString("IPT13402"),
                                    ICP13402 = reader.IsDBNull("ICP13402") ? string.Empty : reader.GetString("ICP13402"),
                                    IM13402 = reader.IsDBNull("IM13402") ? string.Empty : reader.GetString("IM13402"),
                                    IPT13407 = reader.IsDBNull("IPT13407") ? string.Empty : reader.GetString("IPT13407"),
                                    ICP13407 = reader.IsDBNull("ICP13407") ? string.Empty : reader.GetString("ICP13407"),
                                    IM13407 = reader.IsDBNull("IM13407") ? string.Empty : reader.GetString("IM13407"),
                                    IPT13408 = reader.IsDBNull("IPT13408") ? string.Empty : reader.GetString("IPT13408"),
                                    ICP13408 = reader.IsDBNull("ICP13408") ? string.Empty : reader.GetString("ICP13408"),
                                    IM13408 = reader.IsDBNull("IM13408") ? string.Empty : reader.GetString("IM13408"),
                                    IPT13411 = reader.IsDBNull("IPT13411") ? string.Empty : reader.GetString("IPT13411"),
                                    ICP13411 = reader.IsDBNull("ICP13411") ? string.Empty : reader.GetString("ICP13411"),
                                    IM13411 = reader.IsDBNull("IM13411") ? string.Empty : reader.GetString("IM13411"),
                                    PARTIDA_15403 = reader.IsDBNull("PARTIDA_15403") ? string.Empty : reader.GetString("PARTIDA_15403"),
                                    CONCEPTO_15403 = reader.IsDBNull("CONCEPTO_15403") ? string.Empty : reader.GetString("CONCEPTO_15403"),
                                    IMPORTE_15403 = (decimal)(reader.IsDBNull("IMPORTE_15403") ? (decimal?)null : reader.GetDecimal("IMPORTE_15403")),
                                    PARTIDA_15402 = reader.IsDBNull("PARTIDA_15402") ? string.Empty : reader.GetString("PARTIDA_15402"),
                                    CONCEPTO_15402 = reader.IsDBNull("CONCEPTO_15402") ? string.Empty : reader.GetString("CONCEPTO_15402"),
                                    IMPORTE_15402 = (decimal)(reader.IsDBNull("IMPORTE_15402") ? (decimal?)null : reader.GetDecimal("IMPORTE_15402")),
                                    PARTIDA_10001 = reader.IsDBNull("PARTIDA_10001") ? string.Empty : reader.GetString("PARTIDA_10001"),
                                    CONCEPTO_10001 = reader.IsDBNull("CONCEPTO_10001") ? string.Empty : reader.GetString("CONCEPTO_10001"),
                                    IMPORTE_10001 = (decimal)(reader.IsDBNull("IMPORTE_10001") ? (decimal?)null : reader.GetDecimal("IMPORTE_10001")),
                                    PARTIDA_10002 = reader.IsDBNull("PARTIDA_10002") ? string.Empty : reader.GetString("PARTIDA_10002"),
                                    CONCEPTO_10002 = reader.IsDBNull("CONCEPTO_10002") ? string.Empty : reader.GetString("CONCEPTO_10002"),
                                    IMPORTE_10002 = (decimal)(reader.IsDBNull("IMPORTE_10002") ? (decimal?)null : reader.GetDecimal("IMPORTE_10002")),
                                    PARTIDA_20001 = reader.IsDBNull("PARTIDA_20001") ? string.Empty : reader.GetString("PARTIDA_20001"),
                                    CONCEPTO_20001 = reader.IsDBNull("CONCEPTO_20001") ? string.Empty : reader.GetString("CONCEPTO_20001"),
                                    IMPORTE_20001 = (decimal)(reader.IsDBNull("IMPORTE_20001") ? (decimal?)null : reader.GetDecimal("IMPORTE_20001")),
                                    PARTIDA_20002 = reader.IsDBNull("PARTIDA_20002") ? string.Empty : reader.GetString("PARTIDA_20002"),
                                    CONCEPTO_20002 = reader.IsDBNull("CONCEPTO_20002") ? string.Empty : reader.GetString("CONCEPTO_20002"),
                                    IMPORTE_20002 = (decimal)(reader.IsDBNull("IMPORTE_20002") ? (decimal?)null : reader.GetDecimal("IMPORTE_20002")),
                                    PT20003 = reader.IsDBNull("PT20003") ? string.Empty : reader.GetString("PT20003"),
                                    CP20003 = reader.IsDBNull("CP20003") ? string.Empty : reader.GetString("CP20003"),
                                    IM20003 = reader.IsDBNull("IM20003") ? string.Empty : reader.GetString("IM20003"),
                                    PT20004 = reader.IsDBNull("PT20004") ? string.Empty : reader.GetString("PT20004"),
                                    CP20004 = reader.IsDBNull("CP20004") ? string.Empty : reader.GetString("CP20004"),
                                    IM20004 = reader.IsDBNull("IM20004") ? string.Empty : reader.GetString("IM20004"),
                                    PARTIDA_20005 = reader.IsDBNull("PARTIDA_20005") ? string.Empty : reader.GetString("PARTIDA_20005"),
                                    CONCEPTO_20005 = reader.IsDBNull("CONCEPTO_20005") ? string.Empty : reader.GetString("CONCEPTO_20005"),
                                    IMPORTE_20005 = (decimal)(reader.IsDBNull("IMPORTE_20005") ? (decimal?)null : reader.GetDecimal("IMPORTE_20005")),
                                    PARTIDA_20006 = reader.IsDBNull("PARTIDA_20006") ? string.Empty : reader.GetString("PARTIDA_20006"),
                                    CONCEPTO_20006 = reader.IsDBNull("CONCEPTO_20006") ? string.Empty : reader.GetString("CONCEPTO_20006"),
                                    IMPORTE_20006 = (decimal)(reader.IsDBNull("IMPORTE_20006") ? (decimal?)null : reader.GetDecimal("IMPORTE_20006")),
                                    PARTIDA_20007 = reader.IsDBNull("PARTIDA_20007") ? string.Empty : reader.GetString("PARTIDA_20007"),
                                    CONCEPTO_20007 = reader.IsDBNull("CONCEPTO_20007") ? string.Empty : reader.GetString("CONCEPTO_20007"),
                                    IMPORTE_20007 = (decimal)(reader.IsDBNull("IMPORTE_20007") ? (decimal?)null : reader.GetDecimal("IMPORTE_20007")),
                                    PT20008 = reader.IsDBNull("PT20008") ? string.Empty : reader.GetString("PT20008"),
                                    CP20008 = reader.IsDBNull("CP20008") ? string.Empty : reader.GetString("CP20008"),
                                    IM20008 = reader.IsDBNull("IM20008") ? string.Empty : reader.GetString("IM20008"),
                                    TOT_NETO = (decimal)(reader.IsDBNull("TOT_NETO") ? (decimal?)null : reader.GetDecimal("TOT_NETO")),
                                    SDO_ISS = (decimal)(reader.IsDBNull("SDO_ISS") ? (decimal?)null : reader.GetDecimal("SDO_ISS")),
                                    DESPENSA = (decimal)(reader.IsDBNull("DESPENSA") ? (decimal?)null : reader.GetDecimal("DESPENSA")),
                                    PRESTAMOS = (decimal)(reader.IsDBNull("PRESTAMOS") ? (decimal?)null : reader.GetDecimal("PRESTAMOS")),
                                    CHC = (decimal)(reader.IsDBNull("CHC") ? (decimal?)null : reader.GetDecimal("CHC")),
                                    PENSION = (decimal)(reader.IsDBNull("PENSION") ? (decimal?)null : reader.GetDecimal("PENSION")),
                                    Tipo = reader.GetString("Tipo"),
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SericaHeaderModel>> GetHeaderSericaSinPrestamo(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<SericaHeaderModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetHeaderSericaSinPrestamos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@sueldo", "sueldo");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SericaHeaderModel
                                {

                                    ENC0 = reader.GetString("ENC0"),
                                    FechaPago = reader.GetString("FechaPago"),
                                    ENC1 = reader.GetString("ENC1"),
                                    ENC2 = reader.GetInt32("ENC2"),
                                    IMPORTE_11301 = reader.GetDecimal("IMPORTE_11301"),
                                    IM12201 = reader.GetInt32("IM12201"),
                                    IM12301 = reader.GetInt32("IM12301"),
                                    IM13101 = reader.GetInt32("IM13101"),
                                    IMPORTE_13102 = reader.GetDecimal("IMPORTE_13102"),
                                    IM13401 = reader.GetInt32("IM13401"),
                                    IM13402 = reader.GetInt32("IM13402"),
                                    IM13407 = reader.GetInt32("IM13407"),
                                    IM13408 = reader.GetDecimal("IM13408"),
                                    IM13411 = reader.GetDecimal("IM13411"),
                                    IMPORTE_15403 = reader.GetDecimal("IMPORTE_15403"),
                                    IMPORTE_15402 = reader.GetDecimal("IMPORTE_15402"),
                                    despensa = reader.GetDecimal("despensa"),
                                    prestamos = reader.GetDecimal("prestamos"),
                                    superissste = reader.GetDecimal("superissste"),
                                    ade_medico = reader.GetDecimal("ade_medico"),
                                    CHC = reader.GetDecimal("CHC"),
                                    pension = reader.GetDecimal("pension"),
                                    faltas = reader.GetDecimal("faltas"),
                                    retardos = reader.GetDecimal("retardos"),
                                    TOT_PERC = reader.GetDecimal("TOT_PERC"),
                                    tot_dedu = reader.GetDecimal("tot_dedu"),
                                    tot_neto = reader.GetDecimal("tot_neto")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SericaDetalleReporteModel>> GetSericaSinPrestamo(string tabla)
        {
            var connectionString = _connectionString;

            var result = new List<SericaDetalleReporteModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetSericaSinPrestamos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@sueldo", "sueldo");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SericaDetalleReporteModel
                                {
                                    TI = reader.GetString("TI"),
                                    NSS = reader.IsDBNull("NSS") ? string.Empty : reader.GetString("NSS"),
                                    NOMBRE = reader.IsDBNull("NOMBRE") ? string.Empty : reader.GetString("NOMBRE"),
                                    APE_PAT = reader.IsDBNull("APE_PAT") ? string.Empty : reader.GetString("APE_PAT"),
                                    APE_MAT = reader.IsDBNull("APE_MAT") ? string.Empty : reader.GetString("APE_MAT"),
                                    RFC = reader.IsDBNull("RFC") ? string.Empty : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? string.Empty : reader.GetString("CURP"),
                                    SEXO = reader.IsDBNull("SEXO") ? string.Empty : reader.GetString("SEXO"),
                                    PAGADURIA = reader.IsDBNull("PAGADURIA") ? string.Empty : reader.GetString("PAGADURIA"),
                                    NO_EMPLE = reader.IsDBNull("NO_EMPLE") ? string.Empty : reader.GetString("NO_EMPLE"),
                                    NUM_CHEQ = reader.IsDBNull("NUM_CHEQ") ? string.Empty : reader.GetString("NUM_CHEQ"),
                                    REGIMEN_ISSSTE = reader.IsDBNull("REGIMEN_ISSSTE") ? (int?)null : reader.GetInt32("REGIMEN_ISSSTE"),
                                    TIPO_CONTRATO = reader.IsDBNull("TIPO_CONTRATO") ? (int?)null : reader.GetInt32("TIPO_CONTRATO"),
                                    TOT_PERC = (decimal)(reader.IsDBNull("TOT_PERC") ? (decimal?)null : reader.GetDecimal("TOT_PERC")),
                                    TOT_DEDU = (decimal)(reader.IsDBNull("TOT_DEDU") ? (decimal?)null : reader.GetDecimal("TOT_DEDU")),
                                    PARTIDA_11301 = reader.IsDBNull("PARTIDA_11301") ? string.Empty : reader.GetString("PARTIDA_11301"),
                                    CONCEPTO_11301 = reader.IsDBNull("CONCEPTO_11301") ? string.Empty : reader.GetString("CONCEPTO_11301"),
                                    IMPORTE_11301 = (decimal)(reader.IsDBNull("IMPORTE_11301") ? (decimal?)null : reader.GetDecimal("IMPORTE_11301")),
                                    PT12201 = reader.IsDBNull("PT12201") ? string.Empty : reader.GetString("PT12201"),
                                    CP12201 = reader.IsDBNull("CP12201") ? string.Empty : reader.GetString("CP12201"),
                                    IM12201 = reader.IsDBNull("IM12201") ? string.Empty : reader.GetString("IM12201"),
                                    PT12301 = reader.IsDBNull("PT12301") ? string.Empty : reader.GetString("PT12301"),
                                    CP12301 = reader.IsDBNull("CP12301") ? string.Empty : reader.GetString("CP12301"),
                                    IM12301 = reader.IsDBNull("IM12301") ? string.Empty : reader.GetString("IM12301"),
                                    PT13101 = reader.IsDBNull("PT13101") ? string.Empty : reader.GetString("PT13101"),
                                    CP13101 = reader.IsDBNull("CP13101") ? string.Empty : reader.GetString("CP13101"),
                                    IM13101 = reader.IsDBNull("IM13101") ? string.Empty : reader.GetString("IM13101"),
                                    PARTIDA_13102 = reader.IsDBNull("PARTIDA_13102") ? string.Empty : reader.GetString("PARTIDA_13102"),
                                    CONCEPTO_13102 = reader.IsDBNull("CONCEPTO_13102") ? string.Empty : reader.GetString("CONCEPTO_13102"),
                                    IMPORTE_13102 = (decimal)(reader.IsDBNull("IMPORTE_13102") ? (decimal?)null : reader.GetDecimal("IMPORTE_13102")),
                                    IPT13401 = reader.IsDBNull("IPT13401") ? string.Empty : reader.GetString("IPT13401"),
                                    ICP13401 = reader.IsDBNull("ICP13401") ? string.Empty : reader.GetString("ICP13401"),
                                    IM13401 = reader.IsDBNull("IM13401") ? string.Empty : reader.GetString("IM13401"),
                                    IPT13402 = reader.IsDBNull("IPT13402") ? string.Empty : reader.GetString("IPT13402"),
                                    ICP13402 = reader.IsDBNull("ICP13402") ? string.Empty : reader.GetString("ICP13402"),
                                    IM13402 = reader.IsDBNull("IM13402") ? string.Empty : reader.GetString("IM13402"),
                                    IPT13407 = reader.IsDBNull("IPT13407") ? string.Empty : reader.GetString("IPT13407"),
                                    ICP13407 = reader.IsDBNull("ICP13407") ? string.Empty : reader.GetString("ICP13407"),
                                    IM13407 = reader.IsDBNull("IM13407") ? string.Empty : reader.GetString("IM13407"),
                                    IPT13408 = reader.IsDBNull("IPT13408") ? string.Empty : reader.GetString("IPT13408"),
                                    ICP13408 = reader.IsDBNull("ICP13408") ? string.Empty : reader.GetString("ICP13408"),
                                    IM13408 = reader.IsDBNull("IM13408") ? string.Empty : reader.GetString("IM13408"),
                                    IPT13411 = reader.IsDBNull("IPT13411") ? string.Empty : reader.GetString("IPT13411"),
                                    ICP13411 = reader.IsDBNull("ICP13411") ? string.Empty : reader.GetString("ICP13411"),
                                    IM13411 = reader.IsDBNull("IM13411") ? string.Empty : reader.GetString("IM13411"),
                                    PARTIDA_15403 = reader.IsDBNull("PARTIDA_15403") ? string.Empty : reader.GetString("PARTIDA_15403"),
                                    CONCEPTO_15403 = reader.IsDBNull("CONCEPTO_15403") ? string.Empty : reader.GetString("CONCEPTO_15403"),
                                    IMPORTE_15403 = (decimal)(reader.IsDBNull("IMPORTE_15403") ? (decimal?)null : reader.GetDecimal("IMPORTE_15403")),
                                    PARTIDA_15402 = reader.IsDBNull("PARTIDA_15402") ? string.Empty : reader.GetString("PARTIDA_15402"),
                                    CONCEPTO_15402 = reader.IsDBNull("CONCEPTO_15402") ? string.Empty : reader.GetString("CONCEPTO_15402"),
                                    IMPORTE_15402 = (decimal)(reader.IsDBNull("IMPORTE_15402") ? (decimal?)null : reader.GetDecimal("IMPORTE_15402")),
                                    PARTIDA_10001 = reader.IsDBNull("PARTIDA_10001") ? string.Empty : reader.GetString("PARTIDA_10001"),
                                    CONCEPTO_10001 = reader.IsDBNull("CONCEPTO_10001") ? string.Empty : reader.GetString("CONCEPTO_10001"),
                                    IMPORTE_10001 = (decimal)(reader.IsDBNull("IMPORTE_10001") ? (decimal?)null : reader.GetDecimal("IMPORTE_10001")),
                                    //(decimal)(reader.IsDBNull("IMPORTE_10002") ? (decimal?)null : reader.GetDecimal("IMPORTE_10002")),
                                    //IMPORTE_10001 = (decimal)(reader.IsDBNull("IMPORTE_10001") ? (decimal?)null : reader.GetDecimal("IMPORTE_10001")),
                                    PARTIDA_10002 = reader.IsDBNull("PARTIDA_10002") ? string.Empty : reader.GetString("PARTIDA_10002"),
                                    CONCEPTO_10002 = reader.IsDBNull("CONCEPTO_10002") ? string.Empty : reader.GetString("CONCEPTO_10002"),
                                    IMPORTE_10002 = (decimal)(reader.IsDBNull("IMPORTE_10002") ? (decimal?)null : reader.GetDecimal("IMPORTE_10002")),
                                    PARTIDA_20001 = reader.IsDBNull("PARTIDA_20001") ? string.Empty : reader.GetString("PARTIDA_20001"),
                                    CONCEPTO_20001 = reader.IsDBNull("CONCEPTO_20001") ? string.Empty : reader.GetString("CONCEPTO_20001"),
                                    IMPORTE_20001 = (decimal)(reader.IsDBNull("IMPORTE_20001") ? (decimal?)null : reader.GetDecimal("IMPORTE_20001")),
                                    PARTIDA_20002 = reader.IsDBNull("PARTIDA_20002") ? string.Empty : reader.GetString("PARTIDA_20002"),
                                    CONCEPTO_20002 = reader.IsDBNull("CONCEPTO_20002") ? string.Empty : reader.GetString("CONCEPTO_20002"),
                                    IMPORTE_20002 = (decimal)(reader.IsDBNull("IMPORTE_20002") ? (decimal?)null : reader.GetDecimal("IMPORTE_20002")),
                                    PT20003 = reader.IsDBNull("PT20003") ? string.Empty : reader.GetString("PT20003"),
                                    CP20003 = reader.IsDBNull("CP20003") ? string.Empty : reader.GetString("CP20003"),
                                    IM20003 = reader.IsDBNull("IM20003") ? string.Empty : reader.GetString("IM20003"),
                                    PT20004 = reader.IsDBNull("PT20004") ? string.Empty : reader.GetString("PT20004"),
                                    CP20004 = reader.IsDBNull("CP20004") ? string.Empty : reader.GetString("CP20004"),
                                    IM20004 = reader.IsDBNull("IM20004") ? string.Empty : reader.GetString("IM20004"),
                                    PARTIDA_20005 = reader.IsDBNull("PARTIDA_20005") ? string.Empty : reader.GetString("PARTIDA_20005"),
                                    CONCEPTO_20005 = reader.IsDBNull("CONCEPTO_20005") ? string.Empty : reader.GetString("CONCEPTO_20005"),
                                    IMPORTE_20005 = (decimal)(reader.IsDBNull("IMPORTE_20005") ? (decimal?)null : reader.GetDecimal("IMPORTE_20005")),
                                    PARTIDA_20006 = reader.IsDBNull("PARTIDA_20006") ? string.Empty : reader.GetString("PARTIDA_20006"),
                                    CONCEPTO_20006 = reader.IsDBNull("CONCEPTO_20006") ? string.Empty : reader.GetString("CONCEPTO_20006"),
                                    IMPORTE_20006 = (decimal)(reader.IsDBNull("IMPORTE_20006") ? (decimal?)null : reader.GetDecimal("IMPORTE_20006")),
                                    PARTIDA_20007 = reader.IsDBNull("PARTIDA_20007") ? string.Empty : reader.GetString("PARTIDA_20007"),
                                    CONCEPTO_20007 = reader.IsDBNull("CONCEPTO_20007") ? string.Empty : reader.GetString("CONCEPTO_20007"),
                                    IMPORTE_20007 = (decimal)(reader.IsDBNull("IMPORTE_20007") ? (decimal?)null : reader.GetDecimal("IMPORTE_20007")),
                                    PT20008 = reader.IsDBNull("PT20008") ? string.Empty : reader.GetString("PT20008"),
                                    CP20008 = reader.IsDBNull("CP20008") ? string.Empty : reader.GetString("CP20008"),
                                    IM20008 = reader.IsDBNull("IM20008") ? string.Empty : reader.GetString("IM20008"),
                                    TOT_NETO = (decimal)(reader.IsDBNull("TOT_NETO") ? (decimal?)null : reader.GetDecimal("TOT_NETO")),
                                    SDO_ISS = (decimal)(reader.IsDBNull("SDO_ISS") ? (decimal?)null : reader.GetDecimal("SDO_ISS")),
                                    DESPENSA = (decimal)(reader.IsDBNull("DESPENSA") ? (decimal?)null : reader.GetDecimal("DESPENSA")),
                                    PRESTAMOS = (decimal)(reader.IsDBNull("PRESTAMOS") ? (decimal?)null : reader.GetDecimal("PRESTAMOS")),
                                    CHC = (decimal)(reader.IsDBNull("CHC") ? (decimal?)null : reader.GetDecimal("CHC")),
                                    PENSION = (decimal)(reader.IsDBNull("PENSION") ? (decimal?)null : reader.GetDecimal("PENSION")),
                                    Tipo = reader.GetString("Tipo"),
                                    TOT_DEDU1 = reader.GetDecimal("TOT_DEDU")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            Console.Write(result);
            return result;

        }

        public async Task<List<SericaHeaderModel>> GetHeaderSericaIncremento(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<SericaHeaderModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetHeaderSericaIncremento", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@sueldo", "sueldo");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SericaHeaderModel
                                {

                                    ENC0 = reader.GetString("ENC0"),
                                    FechaPago = reader.GetString("FechaPago"),
                                    ENC1 = reader.GetString("ENC1"),
                                    ENC2 = reader.GetInt32("ENC2"),
                                    IMPORTE_11301 = reader.GetDecimal("IMPORTE_11301"),
                                    IM12201 = reader.GetInt32("IM12201"),
                                    IM12301 = reader.GetInt32("IM12301"),
                                    IM13101 = reader.GetInt32("IM13101"),
                                    IMPORTE_13102 = reader.GetDecimal("IMPORTE_13102"),
                                    IM13401 = reader.GetInt32("IM13401"),
                                    IM13402 = reader.GetInt32("IM13402"),
                                    IM13407 = reader.GetInt32("IM13407"),
                                    IM13408 = reader.GetDecimal("IM13408"),
                                    IM13411 = reader.GetDecimal("IM13411"),
                                    IMPORTE_15403 = reader.GetDecimal("IMPORTE_15403"),
                                    IMPORTE_15402 = reader.GetDecimal("IMPORTE_15402"),
                                    despensa = reader.GetDecimal("despensa"),
                                    prestamos = reader.GetDecimal("prestamos"),
                                    superissste = reader.GetDecimal("superissste"),
                                    ade_medico = reader.GetDecimal("ade_medico"),
                                    CHC = reader.GetDecimal("CHC"),
                                    pension = reader.GetDecimal("pension"),
                                    faltas = reader.GetDecimal("faltas"),
                                    retardos = reader.GetDecimal("retardos"),
                                    TOT_PERC = reader.GetDecimal("TOT_PERC"),
                                    tot_dedu = reader.GetDecimal("tot_dedu"),
                                    tot_neto = reader.GetDecimal("tot_neto")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SericaDetalleReporteModel>> GetSericaIncremento(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<SericaDetalleReporteModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetSericaIncremento", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@sueldo", "sueldo");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SericaDetalleReporteModel
                                {
                                    TI = reader.GetString("TI"),
                                    NSS = reader.IsDBNull("NSS") ? string.Empty : reader.GetString("NSS"),
                                    NOMBRE = reader.IsDBNull("NOMBRE") ? string.Empty : reader.GetString("NOMBRE"),
                                    APE_PAT = reader.IsDBNull("APE_PAT") ? string.Empty : reader.GetString("APE_PAT"),
                                    APE_MAT = reader.IsDBNull("APE_MAT") ? string.Empty : reader.GetString("APE_MAT"),
                                    RFC = reader.IsDBNull("RFC") ? string.Empty : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? string.Empty : reader.GetString("CURP"),
                                    SEXO = reader.IsDBNull("SEXO") ? string.Empty : reader.GetString("SEXO"),
                                    PAGADURIA = reader.IsDBNull("PAGADURIA") ? string.Empty : reader.GetString("PAGADURIA"),
                                    NO_EMPLE = reader.IsDBNull("NO_EMPLE") ? string.Empty : reader.GetString("NO_EMPLE"),
                                    NUM_CHEQ = reader.IsDBNull("NUM_CHEQ") ? string.Empty : reader.GetString("NUM_CHEQ"),
                                    REGIMEN_ISSSTE = reader.IsDBNull("REGIMEN_ISSSTE") ? (int?)null : reader.GetInt32("REGIMEN_ISSSTE"),
                                    TIPO_CONTRATO = reader.IsDBNull("TIPO_CONTRATO") ? (int?)null : reader.GetInt32("TIPO_CONTRATO"),
                                    TOT_PERC = (decimal)(reader.IsDBNull("TOT_PERC") ? (decimal?)null : reader.GetDecimal("TOT_PERC")),
                                    TOT_DEDU = (decimal)(reader.IsDBNull("TOT_DEDU") ? (decimal?)null : reader.GetDecimal("TOT_DEDU")),
                                    PARTIDA_11301 = reader.IsDBNull("PARTIDA_11301") ? string.Empty : reader.GetString("PARTIDA_11301"),
                                    CONCEPTO_11301 = reader.IsDBNull("CONCEPTO_11301") ? string.Empty : reader.GetString("CONCEPTO_11301"),
                                    IMPORTE_11301 = (decimal)(reader.IsDBNull("IMPORTE_11301") ? (decimal?)null : reader.GetDecimal("IMPORTE_11301")),
                                    PT12201 = reader.IsDBNull("PT12201") ? string.Empty : reader.GetString("PT12201"),
                                    CP12201 = reader.IsDBNull("CP12201") ? string.Empty : reader.GetString("CP12201"),
                                    IM12201 = reader.IsDBNull("IM12201") ? string.Empty : reader.GetString("IM12201"),
                                    PT12301 = reader.IsDBNull("PT12301") ? string.Empty : reader.GetString("PT12301"),
                                    CP12301 = reader.IsDBNull("CP12301") ? string.Empty : reader.GetString("CP12301"),
                                    IM12301 = reader.IsDBNull("IM12301") ? string.Empty : reader.GetString("IM12301"),
                                    PT13101 = reader.IsDBNull("PT13101") ? string.Empty : reader.GetString("PT13101"),
                                    CP13101 = reader.IsDBNull("CP13101") ? string.Empty : reader.GetString("CP13101"),
                                    IM13101 = reader.IsDBNull("IM13101") ? string.Empty : reader.GetString("IM13101"),
                                    PARTIDA_13102 = reader.IsDBNull("PARTIDA_13102") ? string.Empty : reader.GetString("PARTIDA_13102"),
                                    CONCEPTO_13102 = reader.IsDBNull("CONCEPTO_13102") ? string.Empty : reader.GetString("CONCEPTO_13102"),
                                    IMPORTE_13102 = (decimal)(reader.IsDBNull("IMPORTE_13102") ? (decimal?)null : reader.GetDecimal("IMPORTE_13102")),
                                    IPT13401 = reader.IsDBNull("IPT13401") ? string.Empty : reader.GetString("IPT13401"),
                                    ICP13401 = reader.IsDBNull("ICP13401") ? string.Empty : reader.GetString("ICP13401"),
                                    IM13401 = reader.IsDBNull("IM13401") ? string.Empty : reader.GetString("IM13401"),
                                    IPT13402 = reader.IsDBNull("IPT13402") ? string.Empty : reader.GetString("IPT13402"),
                                    ICP13402 = reader.IsDBNull("ICP13402") ? string.Empty : reader.GetString("ICP13402"),
                                    IM13402 = reader.IsDBNull("IM13402") ? string.Empty : reader.GetString("IM13402"),
                                    IPT13407 = reader.IsDBNull("IPT13407") ? string.Empty : reader.GetString("IPT13407"),
                                    ICP13407 = reader.IsDBNull("ICP13407") ? string.Empty : reader.GetString("ICP13407"),
                                    IM13407 = reader.IsDBNull("IM13407") ? string.Empty : reader.GetString("IM13407"),
                                    IPT13408 = reader.IsDBNull("IPT13408") ? string.Empty : reader.GetString("IPT13408"),
                                    ICP13408 = reader.IsDBNull("ICP13408") ? string.Empty : reader.GetString("ICP13408"),
                                    IM13408 = reader.IsDBNull("IM13408") ? string.Empty : reader.GetString("IM13408"),
                                    IPT13411 = reader.IsDBNull("IPT13411") ? string.Empty : reader.GetString("IPT13411"),
                                    ICP13411 = reader.IsDBNull("ICP13411") ? string.Empty : reader.GetString("ICP13411"),
                                    IM13411 = reader.IsDBNull("IM13411") ? string.Empty : reader.GetString("IM13411"),
                                    PARTIDA_15403 = reader.IsDBNull("PARTIDA_15403") ? string.Empty : reader.GetString("PARTIDA_15403"),
                                    CONCEPTO_15403 = reader.IsDBNull("CONCEPTO_15403") ? string.Empty : reader.GetString("CONCEPTO_15403"),
                                    IMPORTE_15403 = (decimal)(reader.IsDBNull("IMPORTE_15403") ? (decimal?)null : reader.GetDecimal("IMPORTE_15403")),
                                    PARTIDA_15402 = reader.IsDBNull("PARTIDA_15402") ? string.Empty : reader.GetString("PARTIDA_15402"),
                                    CONCEPTO_15402 = reader.IsDBNull("CONCEPTO_15402") ? string.Empty : reader.GetString("CONCEPTO_15402"),
                                    IMPORTE_15402 = (decimal)(reader.IsDBNull("IMPORTE_15402") ? (decimal?)null : reader.GetDecimal("IMPORTE_15402")),
                                    PARTIDA_10001 = reader.IsDBNull("PARTIDA_10001") ? string.Empty : reader.GetString("PARTIDA_10001"),
                                    CONCEPTO_10001 = reader.IsDBNull("CONCEPTO_10001") ? string.Empty : reader.GetString("CONCEPTO_10001"),
                                    IMPORTE_10001 = reader.IsDBNull("IMPORTE_10001") ? 0 : reader.GetDecimal("IMPORTE_10001"),
                                    PARTIDA_10002 = reader.IsDBNull("PARTIDA_10002") ? string.Empty : reader.GetString("PARTIDA_10002"),
                                    CONCEPTO_10002 = reader.IsDBNull("CONCEPTO_10002") ? string.Empty : reader.GetString("CONCEPTO_10002"),
                                    IMPORTE_10002 = (decimal)(reader.IsDBNull("IMPORTE_10002") ? (decimal?)null : reader.GetDecimal("IMPORTE_10002")),
                                    PARTIDA_20001 = reader.IsDBNull("PARTIDA_20001") ? string.Empty : reader.GetString("PARTIDA_20001"),
                                    CONCEPTO_20001 = reader.IsDBNull("CONCEPTO_20001") ? string.Empty : reader.GetString("CONCEPTO_20001"),
                                    IMPORTE_20001 = (decimal)(reader.IsDBNull("IMPORTE_20001") ? (decimal?)null : reader.GetDecimal("IMPORTE_20001")),
                                    PARTIDA_20002 = reader.IsDBNull("PARTIDA_20002") ? string.Empty : reader.GetString("PARTIDA_20002"),
                                    CONCEPTO_20002 = reader.IsDBNull("CONCEPTO_20002") ? string.Empty : reader.GetString("CONCEPTO_20002"),
                                    IMPORTE_20002 = (decimal)(reader.IsDBNull("IMPORTE_20002") ? (decimal?)null : reader.GetDecimal("IMPORTE_20002")),
                                    PT20003 = reader.IsDBNull("PT20003") ? string.Empty : reader.GetString("PT20003"),
                                    CP20003 = reader.IsDBNull("CP20003") ? string.Empty : reader.GetString("CP20003"),
                                    IM20003 = reader.IsDBNull("IM20003") ? string.Empty : reader.GetString("IM20003"),
                                    PT20004 = reader.IsDBNull("PT20004") ? string.Empty : reader.GetString("PT20004"),
                                    CP20004 = reader.IsDBNull("CP20004") ? string.Empty : reader.GetString("CP20004"),
                                    IM20004 = reader.IsDBNull("IM20004") ? string.Empty : reader.GetString("IM20004"),
                                    PARTIDA_20005 = reader.IsDBNull("PARTIDA_20005") ? string.Empty : reader.GetString("PARTIDA_20005"),
                                    CONCEPTO_20005 = reader.IsDBNull("CONCEPTO_20005") ? string.Empty : reader.GetString("CONCEPTO_20005"),
                                    IMPORTE_20005 = (decimal)(reader.IsDBNull("IMPORTE_20005") ? (decimal?)null : reader.GetDecimal("IMPORTE_20005")),
                                    PARTIDA_20006 = reader.IsDBNull("PARTIDA_20006") ? string.Empty : reader.GetString("PARTIDA_20006"),
                                    CONCEPTO_20006 = reader.IsDBNull("CONCEPTO_20006") ? string.Empty : reader.GetString("CONCEPTO_20006"),
                                    IMPORTE_20006 = (decimal)(reader.IsDBNull("IMPORTE_20006") ? (decimal?)null : reader.GetDecimal("IMPORTE_20006")),
                                    PARTIDA_20007 = reader.IsDBNull("PARTIDA_20007") ? string.Empty : reader.GetString("PARTIDA_20007"),
                                    CONCEPTO_20007 = reader.IsDBNull("CONCEPTO_20007") ? string.Empty : reader.GetString("CONCEPTO_20007"),
                                    IMPORTE_20007 = (decimal)(reader.IsDBNull("IMPORTE_20007") ? (decimal?)null : reader.GetDecimal("IMPORTE_20007")),
                                    PT20008 = reader.IsDBNull("PT20008") ? string.Empty : reader.GetString("PT20008"),
                                    CP20008 = reader.IsDBNull("CP20008") ? string.Empty : reader.GetString("CP20008"),
                                    IM20008 = reader.IsDBNull("IM20008") ? string.Empty : reader.GetString("IM20008"),
                                    TOT_NETO = (decimal)(reader.IsDBNull("TOT_NETO") ? (decimal?)null : reader.GetDecimal("TOT_NETO")),
                                    SDO_ISS = (decimal)(reader.IsDBNull("SDO_ISS") ? (decimal?)null : reader.GetDecimal("SDO_ISS")),
                                    DESPENSA = (decimal)(reader.IsDBNull("DESPENSA") ? (decimal?)null : reader.GetDecimal("DESPENSA")),
                                    PRESTAMOS = (decimal)(reader.IsDBNull("PRESTAMOS") ? (decimal?)null : reader.GetDecimal("PRESTAMOS")),
                                    CHC = (decimal)(reader.IsDBNull("CHC") ? (decimal?)null : reader.GetDecimal("CHC")),
                                    PENSION = (decimal)(reader.IsDBNull("PENSION") ? (decimal?)null : reader.GetDecimal("PENSION")),
                                    Tipo = reader.GetString("Tipo"),
                                    //TOT_DEDU1 = reader.GetDecimal("TOT_DEDU1")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetDetalleTerceros(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenConceptos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    METLIFE = reader.GetDecimal("METLIFE"),
                                    KONDINERO = reader.GetDecimal("KONDINERO"),
                                    CREDIFIEL = reader.GetDecimal("CREDIFIEL"),
                                    FONACOT = reader.GetDecimal("FONACOT"),
                                    SUTCONALEP = reader.GetDecimal("SUTCONALEP"),
                                    SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    SITEM = reader.GetDecimal("SITEM"),
                                    Credifom = reader.GetDecimal("CREDIFOM")


                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<PartidasModel>> GetPartidas(string FuenteF)
        {
            var connectionString = _connectionString;
            var result = new List<PartidasModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetSumaConceptos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_fuente_financiamiento", FuenteF);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new PartidasModel
                            {
                                Partida11301 = reader.GetDecimal(reader.GetOrdinal("11301")),
                                Partida11302 = reader.GetDecimal(reader.GetOrdinal("11302")),
                                Partida12101 = reader.GetDecimal(reader.GetOrdinal("12101")),
                                Partida13101 = reader.GetDecimal(reader.GetOrdinal("13101")),
                                Partida13102 = reader.GetDecimal(reader.GetOrdinal("13102")),
                                Partida13103 = reader.GetDecimal(reader.GetOrdinal("13103")),
                                Partida13201 = reader.GetDecimal(reader.GetOrdinal("13201")),
                                Partida13202 = reader.GetDecimal(reader.GetOrdinal("13202")),
                                Partida13204 = reader.GetDecimal(reader.GetOrdinal("13204")),
                                Partida1401 = reader.GetDecimal(reader.GetOrdinal("1401")),
                                Partida14201 = reader.GetDecimal(reader.GetOrdinal("14201")),
                                Partida14301 = reader.GetDecimal(reader.GetOrdinal("14301")),
                                Partida14401 = reader.GetDecimal(reader.GetOrdinal("14401")),
                                Partida15301 = reader.GetDecimal(reader.GetOrdinal("15301")),
                                Partida15401 = reader.GetDecimal(reader.GetOrdinal("15401")),
                                Partida15402 = reader.GetDecimal(reader.GetOrdinal("15402")),
                                Partida15404 = reader.GetDecimal(reader.GetOrdinal("15404")),
                                Partida15405 = reader.GetDecimal(reader.GetOrdinal("15405")),
                                Partida15406 = reader.GetDecimal(reader.GetOrdinal("15406")),
                                Partida15408 = reader.GetDecimal(reader.GetOrdinal("15408")),
                                Partida15409 = reader.GetDecimal(reader.GetOrdinal("15409")),
                                Partida15501 = reader.GetDecimal(reader.GetOrdinal("15501")),
                                Partida15502 = reader.GetDecimal(reader.GetOrdinal("15502")),
                                Partida15901 = reader.GetDecimal(reader.GetOrdinal("15901")),
                                Partida15902 = reader.GetDecimal(reader.GetOrdinal("15902")),
                                Partida15903 = reader.GetDecimal(reader.GetOrdinal("15903")),
                                Partida17101 = reader.GetDecimal(reader.GetOrdinal("17101")),
                                Partida17102 = reader.GetDecimal(reader.GetOrdinal("17102")),
                                Partida1711 = reader.GetDecimal(reader.GetOrdinal("1711"))

                            });
                        }
                    }
                }
            }

            return result;

        }

        public async Task<List<reg_nominas>> GetInfoNominasAsync()
        {
            var connectionString = _connectionString;
            var result = new List<reg_nominas>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetRegNominasDBTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new reg_nominas
                            {
                                DbTable = reader.GetString("DbTable"),
                                FecPago = reader.GetDateTime("fecpago")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<FovissteReportModel>> GetFovissteReporte(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<FovissteReportModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("ReporteFovissste", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new FovissteReportModel
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    NOMBRE = reader.GetString("NOMBRE"),
                                    RFC = reader.GetString("RFC"),
                                    SDO_BASE = reader.GetDecimal("SDO_BASE"),
                                    FOVISSSTE = reader.GetDecimal("FOVISSSTE"),
                                    SEGURO_DE_DANOS = reader.GetDecimal("SEGURO_DE_DANOS"),
                                    FOVISSSTE_CF = reader.GetDecimal("FOVISSSTE_CF"),
                                    SDO_HON = reader.GetDecimal("SDO_HON")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;

        }

        public async Task<List<FonacotReportModel>> GetFonacotReportModel(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<FonacotReportModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetReporteFonacot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new FonacotReportModel
                                {

                                    TIPO = reader.GetString("TIPO"),
                                    RFC = reader.GetString("RFC"),
                                    NOMBRE = reader.GetString("NOMBRE"),
                                    SDO_BASE = reader.GetDecimal("BASE"),
                                    IMPORTE = reader.GetDecimal("FONACOT"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenMetlife(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenMetlife", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    METLIFE = reader.GetDecimal("METLIFE")
                                    //KONDINERO = reader.GetDecimal("KONDINERO")
                                    //CREDIFIEL = reader.GetDecimal("CREDIFIEL"),
                                    //FONACOT = reader.GetDecimal("FONACOT"),
                                    //SUTCONALEP = reader.GetDecimal("SUTCONALEP"),
                                    //SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenKondinero(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenKondinero", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    KONDINERO = reader.GetDecimal("KONDINERO")
                                    //CREDIFIEL = reader.GetDecimal("CREDIFIEL"),
                                    //FONACOT = reader.GetDecimal("FONACOT"),
                                    //SUTCONALEP = reader.GetDecimal("SUTCONALEP"),
                                    //SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenCredifiel(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenCredifiel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),

                                    CREDIFIEL = reader.GetDecimal("CREDIFIEL")
                                    //FONACOT = reader.GetDecimal("FONACOT"),
                                    //SUTCONALEP = reader.GetDecimal("SUTCONALEP"),
                                    //SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenFonacot(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenFonacot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    FONACOT = reader.GetDecimal("FONACOT")
                                    //SUTCONALEP = reader.GetDecimal("SUTCONALEP"),
                                    //SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenSutConalep(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenSutconalep", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    SUTCONALEP = reader.GetDecimal("SUTCONALEP")
                                    //SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenSITACONQROO(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenSITACONQROO", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),

                                    SITACONQROO = reader.GetDecimal("SITACONQROO"),
                                    //SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ResumenConceptos>> GetResumenSITEM(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<ResumenConceptos>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetResumenSitem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ResumenConceptos
                                {
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetString("CT"),
                                    RFC = reader.GetString("RFC"),
                                    Nombre = reader.GetString("Nombre"),
                                    SITEM = reader.GetDecimal("SITEM")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<CrediFomReportModel>> GetCrediFonReportModel(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<CrediFomReportModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("GetCredifomReporte", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new CrediFomReportModel
                                {

                                    RFC = reader.GetString("RFC"),
                                    NOMBRE = reader.GetString("Nombre"),
                                    CT = reader.GetString("CT"),
                                    TIPO = reader.GetString("TIPO"),
                                    Credifom = reader.GetDecimal("Credifom"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<LineaModel>> GetSipeSic(string fec_termi, string fec_pago2, string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<LineaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetSipeSic", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);
                    command.Parameters.AddWithValue("@fec_termi", fec_termi);
                    command.Parameters.AddWithValue("@fec_pago2", fec_pago2);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new LineaModel
                                {
                                    Linea = reader.GetString("linea")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }
        //empieza reportes trimestrales
        public async Task<List<Reg_trimestral>> GetReg_Trimestrals()
        {
            var connectionString = _connectionString;
            var result = new List<Reg_trimestral>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("sp_select_reg_trimestral", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new Reg_trimestral
                                {
                                    DBTable = reader.GetString("DBTable"),
                                    info = reader.GetString("info"),
                                    CAMPANIA = reader.GetString("CAMPANIA"),
                                    PeriodoInicio = reader.GetString("PeriodoIni"),
                                    PeriodoFin = reader.GetString("PeriodoFin"),
                                    TablaAnterior = reader.IsDBNull("TablaAnterior") ? "" : reader.GetString("TablaAnterior"),
                                    Trimestre = reader.GetString("Trimestre")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;

        }

        public async Task<List<TrimestralDocentesModel>> GetTrimestralDocentes(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<TrimestralDocentesModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("sp_trimestralDoc", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralDocentesModel
                                {
                                    QnaPago = reader.IsDBNull("Qna_Pago") ? 0 : reader.GetInt32("Qna_Pago"),
                                    NombreCCT = reader.IsDBNull("NombreCCT") ? string.Empty : reader.GetString("NombreCCT"),
                                    RFC = reader.IsDBNull("RFC") ? string.Empty : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? string.Empty : reader.GetString("CURP"),
                                    NombreCompleto = reader.IsDBNull("NombreCompleto") ? string.Empty : reader.GetString("NombreCompleto"),
                                    Nomb = reader.IsDBNull("Nomb") ? string.Empty : reader.GetString("Nomb"),
                                    Ap1 = reader.IsDBNull("Ap1") ? string.Empty : reader.GetString("Ap1"),
                                    Ap2 = reader.IsDBNull("Ap2") ? string.Empty : reader.GetString("Ap2"),
                                    CATEG = reader.IsDBNull("CATEG") ? string.Empty : reader.GetString("CATEG"),
                                    SumFederalTotal = reader.IsDBNull("SUM_FEDERAL_TOTAL") ? 0 : reader.GetDecimal("SUM_FEDERAL_TOTAL"),
                                    Titu = reader.IsDBNull("Titu") ? string.Empty : reader.GetString("Titu"),
                                    //PERCEPCIONES
                                    P001A = reader.IsDBNull("P001A") ? 0 : reader.GetDecimal("P001A"),
                                    P001B = reader.IsDBNull("P001B") ? 0 : reader.GetDecimal("P001B"),
                                    P001C = reader.IsDBNull("P001C") ? 0 : reader.GetDecimal("P001C"),
                                    P002A = reader.IsDBNull("P002A") ? 0 : reader.GetDecimal("P002A"),
                                    P009A = reader.IsDBNull("P009A") ? 0 : reader.GetDecimal("P009A"),
                                    P016L = reader.IsDBNull("P016L") ? 0 : reader.GetDecimal("P016L"),
                                    // P016L2 = reader.IsDBNull("P016L2") ? 0 : reader.GetDecimal("P016L2"),
                                    P016M = reader.IsDBNull("P016M") ? 0 : reader.GetDecimal("P016M"),
                                    //P016M2 = reader.IsDBNull("P016M2") ? 0 : reader.GetDecimal("P016M2"),
                                    A003E = reader.IsDBNull("A003E") ? 0 : reader.GetDecimal("A003E"),
                                    A0500 = reader.IsDBNull("A0500") ? 0 : reader.GetDecimal("A0500"),
                                    A0625 = reader.IsDBNull("A0625") ? 0 : reader.GetDecimal("A0625"),
                                    A0750 = reader.IsDBNull("A0750") ? 0 : reader.GetDecimal("A0750"),
                                    A5000 = reader.IsDBNull("A5000") ? 0 : reader.GetDecimal("A5000"),
                                    A5175 = reader.IsDBNull("A5175") ? 0 : reader.GetDecimal("A5175"),
                                    A8095 = reader.IsDBNull("A8095") ? 0 : reader.GetDecimal("A8095"),
                                    Pipe = reader.IsDBNull("Pipe") ? string.Empty : reader.GetString("Pipe")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");

                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TrimestralADMModel>> GetTrimestralADM(string tabla)
        {

            var connectionString = _connectionString;
            var result = new List<TrimestralADMModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralADM", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralADMModel
                                {

                                    Qna_pago = reader.GetInt32("QNA_PAGO"),
                                    NombreCCT = reader.GetString("NombreCCT"),
                                    RFC = reader.GetString("RFC"),
                                    CURP = reader.GetString("CURP"),
                                    Nombre = reader.GetString("Nombre"),
                                    Concatenated = reader.GetString("Concatenated"),
                                    CT = reader.GetInt32("CT"),
                                    CATEG = reader.GetString("CATEG"),
                                    PLAZA = reader.GetInt32("PLAZA"),
                                    SUM_FEDERAL_TOTAL = reader.GetDecimal("SUM_FEDERAL_TOTAL"),

                                    //PERCEPCIONES
                                    P001A = reader.GetDecimal("P001A"),

                                    P002A = reader.IsDBNull("P002A") ? 0 : reader.GetDecimal("P002A"),

                                    P009A = reader.IsDBNull("P009A") ? 0 : reader.GetDecimal("P009A"),

                                    P010A = reader.GetDecimal("P010A"),

                                    P016A = reader.IsDBNull("P016A") ? 0 : reader.GetDecimal("P016A"),

                                    P016B = reader.IsDBNull("P016B") ? 0 : reader.GetDecimal("P016B"),

                                    P016D = reader.IsDBNull("P016D") ? 0 : reader.GetDecimal("P016D"),

                                    P016E = reader.IsDBNull("P016E") ? 0 : reader.GetDecimal("P016E"),

                                    P016I = reader.IsDBNull("P016I") ? 0 : reader.GetDecimal("P016I"),

                                    P016J = reader.IsDBNull("P016J") ? 0 : reader.GetDecimal("P016J"),

                                    P016K = reader.IsDBNull("P016K") ? 0 : reader.GetDecimal("P016K"),

                                    P016L = reader.IsDBNull("P016L") ? 0 : reader.GetDecimal("P016L"),

                                    //P016L2 = reader.IsDBNull("P016L2") ? 0 : reader.GetDecimal("P016L2"),

                                    // P016M2 = reader.IsDBNull("P016M2") ? 0 : reader.GetDecimal("P016M2"),

                                    P016R = reader.IsDBNull("P016R") ? 0 : reader.GetDecimal("P016R"),

                                    P021A = reader.IsDBNull("P021A") ? 0 : reader.GetDecimal("P021A"),

                                    P022A = reader.IsDBNull("P022A") ? 0 : reader.GetDecimal("P022A"),

                                    P038A = reader.IsDBNull("P038A") ? 0 : reader.GetDecimal("P038A"),

                                    P038B = reader.IsDBNull("P038B") ? 0 : reader.GetDecimal("P038B"),

                                    P038C = reader.IsDBNull("P038C") ? 0 : reader.GetDecimal("P038C"),

                                    P038K = reader.IsDBNull("P038K") ? 0 : reader.GetDecimal("P038K"),

                                    P039A = reader.IsDBNull("P039A") ? 0 : reader.GetDecimal("P039A"),

                                    //DEDUCCIONES
                                    D001A = reader.IsDBNull("D001A") ? 0 : reader.GetDecimal("D001A"),

                                    D001B = reader.IsDBNull("D001B") ? 0 : reader.GetDecimal("D001B"),

                                    D001C = reader.IsDBNull("D001C") ? 0 : reader.GetDecimal("D001C"),

                                    D001D = reader.IsDBNull("D001D") ? 0 : reader.GetDecimal("D001D"),

                                    D001E = reader.IsDBNull("D001E") ? 0 : reader.GetDecimal("D001E"),

                                    D002A = reader.IsDBNull("D002A") ? 0 : reader.GetDecimal("D002A"),

                                    D003E = reader.IsDBNull("D003E") ? 0 : reader.GetDecimal("D003E"),

                                    D004A = reader.IsDBNull("D004A") ? 0 : reader.GetDecimal("D004A"),

                                    D004B = reader.IsDBNull("D004B") ? 0 : reader.GetDecimal("D004B"),

                                    D004C = reader.IsDBNull("D004C") ? 0 : reader.GetDecimal("D004C"),

                                    D004E = reader.IsDBNull("D004E") ? 0 : reader.GetDecimal("D004E"),

                                    D004K = reader.IsDBNull("D004K") ? 0 : reader.GetDecimal("D004K"),

                                    D007A = reader.IsDBNull("D007A") ? 0 : reader.GetDecimal("D007A"),

                                    D009A = reader.IsDBNull("D009A") ? 0 : reader.GetDecimal("D009A"),

                                    D009B = reader.IsDBNull("D009B") ? 0 : reader.GetDecimal("D009B"),

                                    D009C = reader.IsDBNull("D009C") ? 0 : reader.GetDecimal("D009C"),

                                    D011A = reader.IsDBNull("D011A") ? 0 : reader.GetDecimal("D011A"),

                                    D019A = reader.IsDBNull("D019A") ? 0 : reader.GetDecimal("D019A"),

                                    D019C = reader.IsDBNull("D019C") ? 0 : reader.GetDecimal("D019C"),

                                    D099A = reader.IsDBNull("D099A") ? 0 : reader.GetDecimal("D099A"),

                                    //APORTACIONES
                                    A003E = reader.IsDBNull("A003E") ? 0 : reader.GetDecimal("A003E"),

                                    A0454 = reader.IsDBNull("A0454") ? 0 : reader.GetDecimal("A0454"),

                                    A0500 = reader.IsDBNull("A0500") ? 0 : reader.GetDecimal("A0500"),

                                    A0625 = reader.IsDBNull("A0625") ? 0 : reader.GetDecimal("A0625"),

                                    A0750 = reader.IsDBNull("A0750") ? 0 : reader.GetDecimal("A0750"),

                                    A5000 = reader.IsDBNull("A5000") ? 0 : reader.GetDecimal("A5000"),

                                    Pipe = reader.GetString("Pipe")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TrimestralBY1Model>> GetTrimestralBY1Models(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<TrimestralBY1Model>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralBY1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralBY1Model
                                {

                                    RFC = reader.GetString("RFC"),
                                    CURP = reader.GetString("CURP"),
                                    nombre = reader.GetString("nombre"),
                                    cct = reader.GetString("cct"),
                                    TipADM = reader.GetInt32("TipADM"),
                                    TipDOC = reader.GetInt32("TipDOC"),
                                    TipADMM = reader.GetInt32("TipADMM"),
                                    TipDOCC = reader.GetInt32("TipDOCC"),
                                    SUMA = reader.GetDecimal("SUMA")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Excepcion en by1: {ex.Message} ");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TrimestralCY1Model>> GetTrimestralCY1Models(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<TrimestralCY1Model>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralCY1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralCY1Model
                                {
                                    Cve = reader.IsDBNull("cve") ? "" : reader.GetString("cve"),
                                    RFC = reader.IsDBNull("RFC") ? "" : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? "" : reader.GetString("CURP"),
                                    Nombre = reader.IsDBNull("Nombre") ? "" : reader.GetString("Nombre"),
                                    CATEG = reader.IsDBNull("CATEG") ? "" : reader.GetString("CATEG"),
                                    partpres = reader.IsDBNull("partpres") ? "" : reader.GetString("partpres"),
                                    cveUnidad = reader.IsDBNull("cveUnidad") ? "" : reader.GetString("cveUnidad"),
                                    plaza = reader.IsDBNull("plaza") ? 0 : reader.GetInt32("plaza"),
                                    TOTALPERC = reader.IsDBNull("TOTALPERC") ? 0 : reader.GetDecimal("TOTALPERC")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Excepcion en cy1: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<TrimestralD2Model>> GetTrimestralD2Models(string tablaActual, string tablaAnterior)
        {
            var connectionString = _connectionString;
            var result = new List<TrimestralD2Model>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralD2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tablaActual", tablaActual);
                    command.Parameters.AddWithValue("@tablaAnterior", tablaAnterior);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralD2Model
                                {

                                    RFC = reader.GetString("RFC"),
                                    CURP = reader.GetString("CURP"),
                                    Nombre = reader.GetString("nombre"),
                                    partPres = reader.GetString("partPres"),
                                    CT = reader.GetString("CT"),
                                    Plaza = reader.GetString("plaza"),
                                    CATEG = reader.GetString("CATEG"),
                                    tipoMov = reader.GetInt32("tipoMov")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Excepcion en d2: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<PeriodoModel>> GetPeriodoModels(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<PeriodoModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetPeriodos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new PeriodoModel
                                {

                                    periodoini = reader.GetString("periodoini"),
                                    periodofin = reader.GetString("periodofin")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TrimestralD6Model>> GetTrimestralD6Models(string tabla)
        {
            var connectionString = _connectionString;
            var result = new List<TrimestralD6Model>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralD6", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tabla", tabla);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralD6Model
                                {

                                    CCT = reader.GetString("CCT"),
                                    RFC = reader.GetString("RFC"),
                                    CURP = reader.GetString("CURP"),
                                    Nombre = reader.GetString("Nombre"),
                                    Categ = reader.GetString("CATEG"),
                                    Funcion = reader.GetString("FUNCION"),
                                    TotalPerc = reader.GetDecimal("TOTALPERC")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<TrimestralHModel>> GetTrimestralHModels(string tablaActual, string tablaAnterior)
        {
            var connectionString = _connectionString;
            var result = new List<TrimestralHModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetTrimestralH", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tablaActual", tablaActual);
                    command.Parameters.AddWithValue("@tablaAnterior", tablaAnterior);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new TrimestralHModel
                                {

                                    TipoNom = reader.IsDBNull("TipoNom") ? string.Empty : reader.GetString("TipoNom"),
                                    PartPres = reader.IsDBNull("partPres") ? string.Empty : reader.GetString("partPres"),
                                    Plaza = reader.IsDBNull("CATEG") ? string.Empty : reader.GetString("CATEG"),
                                    RFC = reader.IsDBNull("RFC") ? string.Empty : reader.GetString("RFC"),
                                    CURP = reader.IsDBNull("CURP") ? string.Empty : reader.GetString("CURP"),
                                    Nombre = reader.IsDBNull("nombre") ? string.Empty : reader.GetString("nombre"),
                                    TipoMov = reader.IsDBNull("tipoMov") ? 0 : reader.GetInt32("tipoMov")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"EXCEPCION EN H: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }
        // TERMINA TRIMESTRAL
        // EMPIEZA PROYECCION 
        public async Task<List<ProyeccionFinalModel>> GetProyeccionFinalModels()
        {

            var connectionString = _connectionString;
            var result = new List<ProyeccionFinalModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("CalculoFinalProyeccion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ProyeccionFinalModel
                                {
                                    Partida = reader.GetInt32("Partida"),
                                    Descrip = reader.GetString("Descrip"),
                                    D_CLAVE_UR = reader.GetInt32("D_CLAVE_UR"),
                                    F_FOLIO = reader.GetString("F_FOLIO"),
                                    Sostenimiento = reader.IsDBNull("Sostenimiento") ? string.Empty : reader.GetString("Sostenimiento"),
                                    Enero = reader.GetInt32("ENERO"),
                                    febrero = reader.GetInt32("FEBRERO"),
                                    marzo = reader.GetInt32("MARZO"),
                                    abril = reader.GetInt32("ABRIL"),
                                    mayo = reader.GetInt32("MAYO"),
                                    junio = reader.GetInt32("JUNIO"),
                                    julio = reader.GetInt32("JULIO"),
                                    agosto = reader.GetInt32("AGOSTO"),
                                    septiembre = reader.GetInt32("SEPTIEMBRE"),
                                    octubre = reader.GetInt32("OCTUBRE"),
                                    noviembre = reader.GetInt32("NOVIEMBRE"),
                                    diciembre = reader.GetInt32("DICIEMBRE"),
                                    total = reader.GetInt32("TOTAL")

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;


        }

        public async Task<List<PartidasProyeccionModel>> GetPartidas()
        {
            var connectionString = _connectionString;
            var result = new List<PartidasProyeccionModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetPartidas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new PartidasProyeccionModel
                                {

                                    partida = reader.GetInt32("partida"),
                                    concepto = reader.GetString("Concepto"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<URModel>> GetURModels()
        {

            var connectionString = _connectionString;
            var result = new List<URModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetUR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new URModel
                                {
                                    ClaveUR = reader.GetInt32("ClaveUR"),
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;

        }

        public async Task InsertConceptosXPartidasAsync(int partida, string concepto, int claveUr, string fuenteFinanciamiento)
        {
            var connectionString = _connectionString;
            int contador = 0;  // Variable para contar las inserciones
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetConceptosXPartidas", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros explícitamente con tipo de dato
                        command.Parameters.Add("@p_partida", MySqlDbType.Int32).Value = partida;
                        command.Parameters.Add("@p_concepto", MySqlDbType.VarChar, 10).Value = concepto ?? (object)DBNull.Value;
                        command.Parameters.Add("@p_clave_ur", MySqlDbType.Int32).Value = claveUr;
                        command.Parameters.Add("@p_fuente_financiamiento", MySqlDbType.VarChar, 10).Value = fuenteFinanciamiento ?? (object)DBNull.Value;

                        // Mostrar los parámetros para depuración
                        //Console.WriteLine($"Partida: {partida}, Concepto: {concepto}, ClaveUR: {claveUr}, FuenteFinanciamiento: {fuenteFinanciamiento}");

                        // Ejecutar el comando
                        var result = await command.ExecuteNonQueryAsync();

                        // Si se insertaron filas, aumentar el contador por cada una
                        if (result > 0)
                        {
                            contador += result;  // Sumar el número de filas afectadas
                        }

                        Console.WriteLine($"Filas afectadas por esta ejecución: {result}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al ejecutar el procedimiento: {ex.Message}");
                        Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    }
                }
            }

            // Mostrar el número total de inserciones realizadas
            Console.WriteLine($"Total de inserciones realizadas: {contador}");
        }

        public async Task<List<ProyeccionFinalModel>> GetPartidasReporte()
        {

            var connectionString = _connectionString;
            var result = new List<ProyeccionFinalModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetPartidasReporte", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ProyeccionFinalModel
                                {
                                    Partida = reader.GetInt32("partida"),
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;


        }

        public async Task<List<ProyeccionFinalModel>> GetClavesPartidasPorFuente(string fuenteF)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProyeccionFinalModel>> GetProyeccionFinalUR(int ClaveUR, string fuenteF)
        {
            var connectionString = _connectionString;
            var result = new List<ProyeccionFinalModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetClavesPartidasPorFuente", connection))

                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@claveUr", ClaveUR);
                    command.Parameters.AddWithValue("@fuenteF", fuenteF);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ProyeccionFinalModel
                                {
                                    Partida = reader.GetInt32("Partida"),
                                    Descrip = reader.GetString("Descrip"),
                                    D_CLAVE_UR = reader.GetInt32("D_CLAVE_UR"),
                                    F_FOLIO = reader.GetString("F_FOLIO"),
                                    NombreUR = reader.GetString("NombreUR"),
                                    Unidad = reader.GetString("Unidad"),
                                    Sostenimiento = reader.IsDBNull("Sostenimiento") ? string.Empty : reader.GetString("Sostenimiento"),
                                    Enero = reader.GetInt32("ENERO"),
                                    febrero = reader.GetInt32("FEBRERO"),
                                    marzo = reader.GetInt32("MARZO"),
                                    abril = reader.GetInt32("ABRIL"),
                                    mayo = reader.GetInt32("MAYO"),
                                    junio = reader.GetInt32("JUNIO"),
                                    julio = reader.GetInt32("JULIO"),
                                    agosto = reader.GetInt32("AGOSTO"),
                                    septiembre = reader.GetInt32("SEPTIEMBRE"),
                                    octubre = reader.GetInt32("OCTUBRE"),
                                    noviembre = reader.GetInt32("NOVIEMBRE"),
                                    diciembre = reader.GetInt32("DICIEMBRE"),
                                    total = reader.GetInt32("TOTAL")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ProyeccionFinalModel>> GetProyeccionFinalUR2(int ClaveUR)
        {

            var connectionString = _connectionString;
            var result = new List<ProyeccionFinalModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetClavesPartidasPorFuente1", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@claveUr", ClaveUR);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new ProyeccionFinalModel
                                {
                                    Partida = reader.GetInt32("Partida"),
                                    Descrip = reader.GetString("Descrip"),
                                    D_CLAVE_UR = reader.GetInt32("D_CLAVE_UR"),
                                    F_FOLIO = reader.GetString("F_FOLIO"),
                                    NombreUR = reader.GetString("NombreUR"),
                                    Unidad = reader.GetString("Unidad"),
                                    Sostenimiento = reader.IsDBNull("Sostenimiento") ? string.Empty : reader.GetString("Sostenimiento"),
                                    Enero = reader.GetInt32("ENERO"),
                                    febrero = reader.GetInt32("FEBRERO"),
                                    marzo = reader.GetInt32("MARZO"),
                                    abril = reader.GetInt32("ABRIL"),
                                    mayo = reader.GetInt32("MAYO"),
                                    junio = reader.GetInt32("JUNIO"),
                                    julio = reader.GetInt32("JULIO"),
                                    agosto = reader.GetInt32("AGOSTO"),
                                    septiembre = reader.GetInt32("SEPTIEMBRE"),
                                    octubre = reader.GetInt32("OCTUBRE"),
                                    noviembre = reader.GetInt32("NOVIEMBRE"),
                                    diciembre = reader.GetInt32("DICIEMBRE"),
                                    total = reader.GetInt32("total")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        // EMPIEZA SIRI

        //public async Task<List<LineaSiriAltaModel>> GetSiriLineaAlta()
        //{
        //    var connectionString = _connectionString;
        //    var result = new List<LineaSiriAltaModel>();

        //    using (var connection = new MySqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (var command = new MySqlCommand("GetDetalleAltaBim", connection))

        //        {

        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@tablaDetalleAltaBim", "detallealta_bim4");


        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                try
        //                {
        //                    while (await reader.ReadAsync())
        //                    {

        //                        result.Add(new LineaSiriAltaModel
        //                        {
        //                            Linea = reader.GetString("Linea")
        //                        });

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error: {ex.Message}");
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        //public async Task<List<LineaSiriBajaModel>> GetSiriLineaBaja()
        //{
        //    var connectionString = _connectionString;
        //    var result = new List<LineaSiriBajaModel>();

        //    using (var connection = new MySqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (var command = new MySqlCommand("GetDetalleAltaBim", connection))

        //        {

        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@tablaDetalleAltaBim", "detallealta_bim4");


        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                try
        //                {
        //                    while (await reader.ReadAsync())
        //                    {

        //                        result.Add(new LineaSiriBajaModel
        //                        {
        //                            Linea = reader.GetString("Linea")
        //                        });

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error: {ex.Message}");
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}


        public async Task<List<SiriAltaModel>> GetSiriAltaModels()
        {
            var connectionString = _connectionString;
            var result = new List<SiriAltaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetDetalleAltaBim", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tablaDetalleAltaBim", "detallealta_bim4");


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SiriAltaModel
                                {
                                    curp = reader.GetString("curp"),
                                    SDOBAS_SAR = reader.GetDecimal("SDOBAS_SAR"),
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SiriAltaModel>> GetSiriAltaModels(string tablaNomina, string TablaSiri)
        {
            var connectionString = _connectionString;
            var result = new List<SiriAltaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetSiriAltas", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TablaNomina", tablaNomina);
                    command.Parameters.AddWithValue("@TablaSiri", TablaSiri);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SiriAltaModel
                                {
                                    curp = reader.GetString("curp"),
                                    SDOBAS_SAR = reader.GetDecimal("CV"),
                                    DIAS_LABORADOS = reader.GetInt32("DIAS_LABORADOS")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SiriModificacionModel>> GetSiriModificacionModels(string tablaNomina, string TablaSiri)
        {
            var connectionString = _connectionString;
            var result = new List<SiriModificacionModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetSiriModificaciones", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TablaNomina", tablaNomina);
                    command.Parameters.AddWithValue("@TablaSiri", TablaSiri);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SiriModificacionModel
                                {
                                    curp = reader.GetString("curp"),
                                    SDOBAS_SAR = reader.GetDecimal("CV"),
                                    DIAS_LABORADOS = reader.GetInt32("DIAS_LABORADOS")
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<SiriBajaModel>> GetSiriBajaModels(string tablaNomina, string TablaSiri)
        {
            var connectionString = _connectionString;
            var result = new List<SiriBajaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetSiriBajas", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TablaNomina", tablaNomina);
                    command.Parameters.AddWithValue("@TablaSiri", TablaSiri);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new SiriBajaModel
                                {
                                    curp = reader.GetString("curp"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        //para confirmacion de siri prueba
        public async Task<List<LineaModel>> GetSiriConfirmacionModels(string tablaNomina, string TablaSiri)
        {


            var connectionString = _connectionString;
            var result = new List<LineaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetSiriBajas", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TablaNomina", tablaNomina);
                    command.Parameters.AddWithValue("@TablaSiri", TablaSiri);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new LineaModel
                                {
                                    Linea = reader.GetString("Linea"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;

        }

        public async Task<List<LineaModel>> GetConfirmacionModels(string tablaNomina, string TablaSiri)
        {
            var connectionString = _connectionString;
            var result = new List<LineaModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetConfirmacionSiri", connection))

                {

                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@TablaNomina", tablaNomina);
                    //command.Parameters.AddWithValue("@TablaSiri", TablaSiri);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {

                                result.Add(new LineaModel
                                {
                                    Linea = reader.GetString("LINEA"),

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<NominaSiriModel>> GetNominaSiriModels(string tablaNomina)
        {
            var connectionString = _connectionString;
            var result = new List<NominaSiriModel>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("GetNominaSiri", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tablaNominaSiri", tablaNomina);
                    //command.Parameters.AddWithValue("@TablaSiri", TablaSiri);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                result.Add(new NominaSiriModel
                                {
                                    RFC = reader.GetString("RFC"),
                                    CURP = reader.GetString("CURP"),
                                    NOMBRE = reader.GetString("Nombre"),
                                    APE_PAT = reader.IsDBNull("APE_PAT") ? string.Empty : reader.GetString("APE_PAT"),
                                    APE_MAT = reader.IsDBNull("APE_MAT") ? string.Empty : reader.GetString("APE_MAT"),
                                    TIPO = reader.GetString("TIPO"),
                                    CT = reader.GetInt32("CT"),
                                    SDO_SAR = reader.GetDecimal("SDO_SAR"),
                                    REGISTROS = reader.GetInt32("REGISTROS"),
                                    VIVIENDA = reader.GetDecimal("VIVIENDA"),
                                    RETIROYCESANTIA = reader.GetDecimal("RETIROYCESANTIA"),
                                    TRAB_CESANTIA = reader.GetDecimal("TRAB_CESANTIA"),
                                    TRAB_AHORRO = reader.GetDecimal("TRAB_AHORRO"),
                                    APO_AHORRO = reader.GetDecimal("APO_AHORRO"),
                                    FEC_ING = reader.IsDBNull("FEC_ING") ? DateTime.MinValue : reader.GetDateTime("FEC_ING")
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
            }
            return result;
        }
    }

}
