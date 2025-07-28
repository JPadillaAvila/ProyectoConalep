using Conalep2025.Models;
using System.ComponentModel;
using Conalep2025.Models.SiriModels;
using Conalep2025.Models.ProyeccionModels;

namespace Conalep2025.Repository
{
    public interface IrepositoryMySQL
    {

        //EXPEDIENTES
        Task<Expediente> GetExpedienteByCURP(string curp);
        public Task<List<Expediente>> GetExpediente();
        public Task<Expediente> GetExpedienteId(string ExpedienteId);
        public Task<Expediente> CrearExpediente(Expediente crearExpediente);
        public Task<Expediente> ActualizarExpediente(string ExpedienteId, Expediente actualizarExpediente);
        public Task EliminarExpediente(string ExpedienteId);

        //------------------TERMINA EXPEDIENTES-------------------------------------
        //void DoSomething();
        //------------------EMPIEZA REPORTE A TERCEROS------------------------------
        public Task<List<RegNominasInfo>> GetRegNominasInfoAsync();
        public Task<List<reg_nominas>> GetRegNominasDBTableAsync();
        public Task<List<reg_nominas>> GetInfoNominasAsync();
        public Task<List<SericaHeaderModel>> GetHeaderSericaPrestamoAsync(string tabla);
        public Task<List<SericaDetalleReporteModel>> GetSericaPrestamoAsync(string tabla);
        public Task<List<SericaHeaderModel>> GetHeaderSericaSinPrestamo(string tabla);
        public Task<List<SericaDetalleReporteModel>> GetSericaSinPrestamo(string tabla);
        public Task<List<SericaHeaderModel>> GetHeaderSericaIncremento(string tabla);
        public Task<List<SericaDetalleReporteModel>> GetSericaIncremento(string tabla);
        public Task<List<ResumenConceptos>> GetDetalleTerceros(string tabla);
        public Task<List<ResumenConceptos>> GetResumenMetlife(string tabla);
        public Task<List<ResumenConceptos>> GetResumenKondinero(string tabla);
        public Task<List<ResumenConceptos>> GetResumenCredifiel(string tabla);
        public Task<List<ResumenConceptos>> GetResumenFonacot(string tabla);
        public Task<List<ResumenConceptos>> GetResumenSutConalep(string tabla);
        public Task<List<ResumenConceptos>> GetResumenSITACONQROO(string tabla);
        public Task<List<ResumenConceptos>> GetResumenSITEM(string tabla);
        public Task<List<FovissteReportModel>> GetFovissteReporte(string tabla);
        public Task<List<FonacotReportModel>> GetFonacotReportModel(string tabla);
        public Task<List<LineaModel>> GetSipeSic(string fec_termi, string fec_pago2, string tabla);
        public Task<List<CrediFomReportModel>> GetCrediFonReportModel(string tabla);
        //------------------TERMINA REPORTE A TERCEROS-------------------------------
        //------------------EMPIEZA REPORTES TRIMESTRALES----------------------------
        //Reportes trimestrales
        public Task<List<Reg_trimestral>> GetReg_Trimestrals();
        public Task<List<PeriodoModel>> GetPeriodoModels(string tabla);
        public Task<List<TrimestralCY1Model>> GetTrimestralCY1Models(string tabla);
        public Task<List<TrimestralD2Model>> GetTrimestralD2Models(string tablaActual, string tablaAnterior);
        public Task<List<TrimestralHModel>> GetTrimestralHModels(string tablaActual, string tablaAnterior);
        public Task<List<TrimestralBY1Model>> GetTrimestralBY1Models(string tabla);
        public Task<List<TrimestralD6Model>> GetTrimestralD6Models(string tabla);
        public Task<List<TrimestralDocentesModel>> GetTrimestralDocentes(string tabla);
        public Task<List<TrimestralADMModel>> GetTrimestralADM(string tabla);
        //------------------TERMINA REPORTES TRIMESTRALES----------------------------
        //------------------EMPIEZA PROYECCION PRESUPUESTAL--------------------------
        public Task<List<PartidasProyeccionModel>> GetPartidas();
        public Task<List<URModel>> GetURModels();
        public Task<List<ProyeccionFinalModel>> GetProyeccionFinalModels();
        public Task<List<ProyeccionFinalModel>> GetProyeccionFinalUR(int ClaveUR, string fuenteF);
        public Task<List<ProyeccionFinalModel>> GetProyeccionFinalUR2(int claverUR);
       
        public Task<List<ProyeccionFinalModel>> GetPartidasReporte();
        public Task<List<ProyeccionFinalModel>> GetClavesPartidasPorFuente(string fuenteF);
        public Task InsertConceptosXPartidasAsync(int partida, string concepto, int claveUr, string fuenteFinanciamiento);
        
       
        //pruebas para los conceptos para proyeccion 
        public Task<List<PartidasModel>> GetPartidas(string FuenteF);
        // SIRI

        public Task<List<SiriAltaModel>> GetSiriAltaModels(string tablaNomina,string TablaSiri);
        public Task<List<SiriModificacionModel>> GetSiriModificacionModels(string tablaNomina, string TablaSiri);
        public Task<List<SiriBajaModel>> GetSiriBajaModels(string tablaNomina, string TablaSiri);
    //confirmacion
        public Task<List<NominaSiriModel>> GetNominaSiriModels(string tablaNomina);
        public Task<List<LineaModel>> GetConfirmacionModels(string tablaNomina, string TablaSiri);

    }
}
