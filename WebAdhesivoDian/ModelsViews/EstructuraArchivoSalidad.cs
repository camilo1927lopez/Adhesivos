namespace WebAdhesivoDian.ModelsViews
{
    public class EstructuraArchivoSalidad
    {
        public string CodigoCliente { get; set; }
        public int NumeracionCadena { get; set; }
        public int NumeracionInicialCadena { get; set; }
        public int NumeracionFinalCadena { get; set; }
        public string FormateoCodigoFijo { get; set; }
        public string CodigoOficina { get; set; }
        public string NombreOficina { get; set; }
        public string CodigoCajero { get; set; }
        public string NombreCajero { get; set; }
        public int consecutivo { get; set; }
        public string DigitoControl { get; set; }
        public int OriginalCopia { get; set; }
        public int CantidadAdhesivos { get; set; }
        public int CantidadHojas { get; set; }
        public int CantJuegos { get; set; }
        public string Desded { get; set; }
        public string Hastad { get; set; }
    }
}
