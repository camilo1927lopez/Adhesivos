using System;

namespace WebAdhesivoDian.Utilidades
{
    public class Utilidades
    {
        /// <summary>
        /// Método que calcula el dgito de control de la banda magéntica, exigido por la Asobancaria
        ///
        /// ------------------------------------
        /// Created by: Andrés Ayala.
        /// Date: 19/06/2013.
        /// ------------------------------------
        /// MODIFICATION HISTORY
        /// Author:
        /// Date:
        /// Description:
        /// </summary>
        /// <param name="cuenta">La cuenta bancaria</param>
        /// <param name="numeroCheque">El nmero del cheque</param>
        /// <returns>Un entero con el dgito calculado.</returns>
        public static string CalcularDigitoAsobancaria(String cuenta, String numeroCheque)
        {
            int digitoAsobancaria = 0;



            String cadenaPonderadora = "137137137137137137137137";
            if (Int64.Parse(cuenta) == 0)
            {
                return "0";
            }
            else
            {
                String cadenaBase = cuenta + numeroCheque;
                int longitudBase = cadenaBase.Length;
                int longitudPonderador = cadenaPonderadora.Length;
                int resultado = 0;
                while (longitudBase > 0)
                {
                    int resultadoParcial = Int32.Parse(cadenaBase.Substring(longitudBase - 1, 1)) * Int32.Parse(cadenaPonderadora.Substring(longitudPonderador - 1, 1));
                    resultado += resultadoParcial;
                    longitudBase--;
                    longitudPonderador--;
                }
                digitoAsobancaria = resultado % 10;
            }
            return digitoAsobancaria.ToString();
        }
    }
}
