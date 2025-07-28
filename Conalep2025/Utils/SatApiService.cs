using HtmlAgilityPack;
using Conalep2025.Dto;
using Conalep2025.Models;


namespace Conalep2025.Utils
{
    public class SatApiServices()
    {
  

        private async Task<SatIdData> FindDataOnSatApiService(SatIdCif cif)
        {
            var htmlDoc = new HtmlDocument();
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://siat.sat.gob.mx/app/qr/faces/pages/mobile/")
            };

            try
            {
                var data = await client.GetStringAsync("validadorqr.jsf?D1=10&D2=1&D3=" + cif.IdCif + "_" + cif.Rfc);
                htmlDoc.LoadHtml(data);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
            }

            Guid guid = Guid.NewGuid();
            SatIdData satData = new SatIdData();

            return satData;
            /*

                    var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body//table");
                    var text = htmlBody.InnerText.ToString();
                    string[] datos = text.Split("\r\n");
                    if (datos[7] == "    0003")
                    {
                        return (null, 1);
                    }
                    var s2 = datos[17].Trim();

                    var ap2 = Encoding.GetEncoding("iso-8859-1").GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(s2));

                    renapo = new InformacionCurp
                    {
                        InformacionCurpId = guid,
                        AnioReg = datos[8].Trim(),
                        Apellido1 = datos[13].Trim(),
                        Apellido2 = ap2,
                        Crip = datos[21].Trim(),
                        Curp = datos[25].Trim(),
                        CveEntidadEmisora = datos[29].Trim(),
                        CveEntidadNac = datos[33].Trim(),
                        CveMunicipioReg = datos[37].Trim(),
                        DocProbatorio = datos[41].Trim(),
                        FechNac = datos[45].Trim(),
                        Foja = datos[49].Trim(),
                        FolioCarta = datos[53].Trim(),
                        Libro = datos[57].Trim(),
                        Nacionalidad = datos[61].Trim(),
                        Nombres = datos[65].Trim(),
                        NumActa = datos[69].Trim(),
                        NumEntidadReg = datos[73].Trim(),
                        NumRegExtranjeros = datos[77].Trim(),
                        Sexo = datos[81].Trim(),
                        StatusCurp = datos[85].Trim(),
                        Tomo = datos[89].Trim()
                    };

                */

        }
    }
}