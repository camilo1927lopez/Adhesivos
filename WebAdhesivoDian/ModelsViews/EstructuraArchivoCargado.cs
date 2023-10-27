using System.ComponentModel.DataAnnotations;

namespace WebAdhesivoDian.ModelsViews
{
    public class EstructuraArchivoCargado
    {        
        public string CodigoCliente { get; set; }
        public string TipoPorducto { get; set; }
        public string CodigoOficina { get; set; }
        public string CodigoCajero { get; set; }
        public int InicioNumeracion { get; set; }
        public int CantidadNumeracion { get; set; }
        public string InfoCliente { get; set; }
    }
}
