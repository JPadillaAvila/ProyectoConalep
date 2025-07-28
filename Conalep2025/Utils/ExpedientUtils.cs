namespace Conalep2025.Utils
{
    public class ExpedientUtils
    {
        public static int UtilCalcularEdad(DateTime fechaNacimiento)
        {
            var today = DateTime.Today;
            var age = today.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}