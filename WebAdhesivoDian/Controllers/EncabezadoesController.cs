using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;
using WebAdhesivoDian.ModelsViews;
using WebAdhesivoDian.Lib;
using WebAdhesivoDian.Filters;
using Microsoft.AspNetCore.Authorization;

namespace WebAdhesivoDian.Controllers
{
    [Authorize]
    public class EncabezadoesController : Controller
    {
        private readonly WebAdhesivoDianContext _context;
        private string pathSubido;
        public EncabezadoesController(WebAdhesivoDianContext context)
        {
            _context = context;
        }

        //[AuthorizeUsers(Policy = "CONTROLLER")]
        // GET: Encabezadoes
        public async Task<IActionResult> Index()
        {
            //var encabezado = await _context.Encabezado.ToListAsync();
            var encabezado = await _context.Encabezado.OrderByDescending(e => e.FechaCreacion).ToListAsync();

            foreach (var item in encabezado) {
                item.Cliente = _context.Cliente.Find(item.IdCliente);
                item.EstadoPedido = _context.EstadoPedido.Find(item.IdEstado);

            }
                
               



            return View(encabezado);
        }



        // GET: Encabezadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encabezado = await _context.Encabezado.FirstOrDefaultAsync(m => m.IdEncabezado == id);
            if (encabezado == null)
            {
                return NotFound();
            }

            List<DetalleEncabezado> detalleEncabezados = _context.DetalleEncabezado.Where(m => m.IdEncabezado == encabezado.IdEncabezado).ToList();

            foreach (var item in detalleEncabezados)
                item.Cajero = _context.Cajero.Find(item.IdCajero);

            return View(detalleEncabezados);
        }
        [AuthorizeUsers]
        // GET: Encabezadoes/Create
        public IActionResult Create()
        {
            return View(new ModelsViews.CargarArchivo());
        }

        // POST: Encabezadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CargarArchivo cargarArchivo)
        {
            Encabezado encabezado = new Encabezado();
            DateTime dateTime = DateTime.Now;
            string strFechaHoraTransmision = $"{dateTime.Year:0000}{dateTime.Month:00}{dateTime.Day:00}{dateTime.Hour:00}{dateTime.Minute:00}{dateTime.Second:00}";
            string strRutaArchivoImpresion = string.Empty;
            string strRutaArchivoReporte = string.Empty;

            pathSubido = await SubirArchivo(cargarArchivo, strFechaHoraTransmision);            

            Dictionary<string, List<EstructuraArchivoSalidad>> dcOficinaInsumos = new Dictionary<string, List<EstructuraArchivoSalidad>>();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    List<EstructuraArchivoCargado> ltsEstructuraArchivoCargados = new List<EstructuraArchivoCargado>();

                    ltsEstructuraArchivoCargados = ValidarYCargar(strFechaHoraTransmision);

                    if (ltsEstructuraArchivoCargados.Count < 1)
                        throw new Exception("El archivo cargado esta vacío por favor validar.");

                    dcOficinaInsumos = CargarEstructuraSalida(ltsEstructuraArchivoCargados);

                    Cliente cliente = _context.Cliente.FirstOrDefault(t => t.Codigo == ltsEstructuraArchivoCargados[0].CodigoCliente);
                    EstadoPedido estado = _context.EstadoPedido.FirstOrDefault(t => t.IdEstado == 2);
                    if (cliente == null)
                        throw new Exception($"No se pudo obtener el cliente con código '{ltsEstructuraArchivoCargados[0].CodigoCliente}'");

                    int numeroPedidoSiguiente = cliente.UltimoPedido + 1;
                    encabezado.NombreArchivoCargado = Path.GetFileName(pathSubido);
                    encabezado.NombrePedido = $"A{cliente.Codigo}{numeroPedidoSiguiente:000000}";
                    encabezado.IdCliente = cliente.IdCliente;
                    encabezado.FechaCreacion = DateTime.Now;
                    encabezado.EstadoPedido = estado;
                    
                    _context.Add(encabezado);
                    await _context.SaveChangesAsync();

                    strRutaArchivoImpresion = Path.Combine(Path.GetDirectoryName(pathSubido).Replace(@"\Transmision", "").Replace(@$"\{strFechaHoraTransmision}",""), "Impresion", encabezado.NombrePedido, encabezado.NombrePedido + ".txt");
                    strRutaArchivoReporte = Path.Combine(Path.GetDirectoryName(pathSubido).Replace(@"\Transmision", "").Replace(@$"\{strFechaHoraTransmision}", ""), "Reporte", encabezado.NombrePedido, encabezado.NombrePedido + "_Reportes" + ".csv");
                    
                    if (!Directory.Exists(Path.GetDirectoryName(strRutaArchivoImpresion)))
                        Directory.CreateDirectory(Path.GetDirectoryName(strRutaArchivoImpresion));
                    if (!Directory.Exists(Path.GetDirectoryName(strRutaArchivoReporte)))
                        Directory.CreateDirectory(Path.GetDirectoryName(strRutaArchivoReporte));

                   using StreamWriter swReporte = new StreamWriter(strRutaArchivoReporte);
                   using StreamWriter swImpresion = new StreamWriter(strRutaArchivoImpresion);

                    bool encabezadoReporte = true;
                    bool encabezadoImpresion = true;
                    int ultimaNumeracionCadena = 0;
                    foreach (var item in dcOficinaInsumos)
                    {
                        int contador = 0;
                        List<EstructuraArchivoSalidad> ordenado = item.Value.OrderByDescending(t => t.consecutivo).ThenBy(t => t.OriginalCopia).ToList();


                        if (encabezadoImpresion)
                        {
                            swImpresion.WriteLine("numAdhesivo;codBarras;copia");
                            encabezadoImpresion = false;
                        }
                        if (encabezadoReporte)
                        {
                            swReporte.WriteLine($"Sucursal	Cajero\tCantidadAdhesivos\tCantidadHojas\tCantJuegos\tDesde\tHasta\tConsecutivoInicalCadena\tConsecutivoFinalCadena\tCantidadAnulados");
                            encabezadoReporte = false;
                        }

                        List<EstructuraArchivoSalidad> ltsAnulado = AgregarAnulados(item.Value);
                        foreach (var itemAnulado in ltsAnulado)
                            swImpresion.WriteLine($"{itemAnulado.CodigoCliente}{itemAnulado.NumeracionCadena:000000};{itemAnulado.FormateoCodigoFijo}{itemAnulado.CodigoCliente}{itemAnulado.CodigoOficina}{itemAnulado.CodigoCajero}{itemAnulado.consecutivo:000000}{itemAnulado.DigitoControl};{itemAnulado.OriginalCopia}");

                        foreach (EstructuraArchivoSalidad itemSalida in ordenado)
                        {
                            if (contador == 0)
                            {

                                DetalleEncabezado detalleEncabezado = new DetalleEncabezado();
                                detalleEncabezado.Cantidad = itemSalida.CantidadAdhesivos;
                                detalleEncabezado.CantidadInicial = Convert.ToInt32(itemSalida.Desded);
                                detalleEncabezado.CantidadFinal = Convert.ToInt32(itemSalida.Hastad);
                                detalleEncabezado.ConsectivoinicialCadena = itemSalida.NumeracionInicialCadena;
                                detalleEncabezado.ConsectivoFinalCadena = itemSalida.NumeracionFinalCadena;
                                detalleEncabezado.IdCajero = Convert.ToInt32(item.Key);
                                detalleEncabezado.IdEncabezado = encabezado.IdEncabezado;

                                _context.Add(detalleEncabezado);
                                await _context.SaveChangesAsync();

                                Cajero cajero = _context.Cajero.Find(detalleEncabezado.IdCajero);
                                cajero.ultimaNumeracion = detalleEncabezado.CantidadFinal;

                                _context.Update(cajero);
                                await _context.SaveChangesAsync();

                                itemSalida.CantidadAdhesivos = ordenado.Count;
                                itemSalida.CantJuegos = ordenado.Count / 2;
                                itemSalida.CantidadHojas = (ordenado.Count + ltsAnulado.Count) / 24;

                                swReporte.WriteLine($"{itemSalida.NombreOficina}\t{itemSalida.NombreCajero}\t{itemSalida.CantidadAdhesivos}\t{itemSalida.CantidadHojas}\t{itemSalida.CantJuegos}\t{itemSalida.Desded}\t{itemSalida.Hastad}\t{itemSalida.NumeracionInicialCadena}\t{itemSalida.NumeracionFinalCadena}\t{ltsAnulado.Count}");

                            }

                            swImpresion.WriteLine($"{itemSalida.CodigoCliente}{itemSalida.NumeracionCadena:000000};{itemSalida.FormateoCodigoFijo}{itemSalida.CodigoCliente}{itemSalida.CodigoOficina}{itemSalida.CodigoCajero}{itemSalida.consecutivo:000000}{itemSalida.DigitoControl};{itemSalida.OriginalCopia}");

                            contador++;
                        }

                        if (item.Value.Max(t => t.NumeracionCadena) > ultimaNumeracionCadena)                        
                            ultimaNumeracionCadena = item.Value.Max(t => t.NumeracionCadena);
                        
                    }

                    cliente.UltimoPedido = numeroPedidoSiguiente;
                    cliente.ConsecutivoCadena = ultimaNumeracionCadena;
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al Generar Pedido: {0}", ex.Message));
                    return View(cargarArchivo);
                }
                transaccion.Commit();
            }
            ViewBag.ProcesoTerminado = $"Pedido Generado Correctamente<br><br>" +
                                        $"Numero Pedido: {encabezado.NombrePedido}<br>" +
                                        $"Ruta Insumos para produccion:<br><ul>" +
                                        $"<li>Impresión: {strRutaArchivoImpresion}<br></li>" +
                                        $"<li>Reporte: {strRutaArchivoReporte}</li><ul>";
            return View(new CargarArchivo());
        }

        private List<EstructuraArchivoSalidad> AgregarAnulados(List<EstructuraArchivoSalidad> ordenado)
        {

            List<EstructuraArchivoSalidad> estructuraArchivoSalidads = new List<EstructuraArchivoSalidad>();
            try
            {
                int cantidadAnulado = ((((ordenado[0].CantidadAdhesivos / 12) + 1) * 12) - ordenado[0].CantidadAdhesivos);

                if (cantidadAnulado != 12)
                {
                    for (int i = 0; i < cantidadAnulado; i++)
                    {
                        for (int iOrginalCopia = 1; iOrginalCopia <= 2; iOrginalCopia++)
                        {
                            estructuraArchivoSalidads.Add(new EstructuraArchivoSalidad
                            {
                                CodigoCliente = ordenado[0].CodigoCliente,
                                CodigoOficina = ordenado[0].CodigoOficina,
                                CodigoCajero = ordenado[0].CodigoCajero,
                                NombreOficina = $"Sucursal {ordenado[0].CodigoOficina}",
                                NombreCajero = $"Cajero {ordenado[0].CodigoCajero}",
                                consecutivo = 0,
                                CantidadAdhesivos = 0,
                                CantidadHojas = 0,
                                CantJuegos = 0,
                                Desded = $"{0}",
                                DigitoControl = $"{0}",
                                FormateoCodigoFijo = ordenado[0].FormateoCodigoFijo,
                                Hastad = $"{0}",
                                NumeracionCadena = 0,
                                NumeracionInicialCadena = 0,
                                NumeracionFinalCadena = 0,
                                OriginalCopia = iOrginalCopia
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return estructuraArchivoSalidads;
        }

        private Dictionary<string, List<EstructuraArchivoSalidad>> CargarEstructuraSalida(List<EstructuraArchivoCargado> ltsEstructuraArchivoCargados)
        {            
            Dictionary<string, List<EstructuraArchivoSalidad>> dcOficinaInsumos = new Dictionary<string, List<EstructuraArchivoSalidad>>();
            try
            {
                int numeracionCadena = 0;
                foreach (var item in ltsEstructuraArchivoCargados)
                {
                    Cliente cliente = _context.Cliente.FirstOrDefault(t => t.Codigo == item.CodigoCliente);
                    Oficina oficina = _context.Oficina.FirstOrDefault(t => t.Codigo == item.CodigoOficina && t.idCliente == cliente.IdCliente);
                    Cajero cajero = _context.Cajero.FirstOrDefault(t => t.Codigo == item.CodigoCajero && t.IdOficina == oficina.IdOficina);  

                    string strCodigoOficinaCajero = $"{cajero.IdCajero}";

                    if (!dcOficinaInsumos.ContainsKey(strCodigoOficinaCajero))
                        dcOficinaInsumos.Add(strCodigoOficinaCajero, new List<EstructuraArchivoSalidad>());

                    if (numeracionCadena == 0)                    
                        numeracionCadena = cliente.ConsecutivoCadena + 1;
                    

                    //numeracionCadena = cliente.ConsecutivoCadena + numeracionCadena;
                    int FinalNumeracion = (item.InicioNumeracion + item.CantidadNumeracion);
                    for (int i = item.InicioNumeracion; i < FinalNumeracion; i++)
                    {
                        for (int iOrginalCopia = 2; iOrginalCopia >= 1; iOrginalCopia--)
                        {
                            dcOficinaInsumos[strCodigoOficinaCajero].Add(new EstructuraArchivoSalidad
                            {
                                CodigoCliente = item.CodigoCliente,
                                CodigoOficina = item.CodigoOficina,
                                CodigoCajero = item.CodigoCajero,
                                NombreOficina = $"Sucursal {item.CodigoOficina}",
                                NombreCajero = $"Cajero {item.CodigoCajero}",
                                consecutivo = i,
                                CantidadAdhesivos = item.CantidadNumeracion,
                                CantidadHojas = 0,
                                CantJuegos = 0,
                                Desded = item.InicioNumeracion.ToString(),
                                DigitoControl = new SecurityCodeClass($"{item.CodigoCliente}{item.CodigoOficina}{item.CodigoCajero}{i:000000}").GetSecurityCode().ToString(),
                                FormateoCodigoFijo = cliente.FormateoCodigoFijo,
                                Hastad = (FinalNumeracion - 1).ToString(),
                                NumeracionCadena = numeracionCadena,
                                NumeracionInicialCadena = 0,
                                NumeracionFinalCadena = 0,
                                OriginalCopia = iOrginalCopia
                            });

                            numeracionCadena++;
                        }           
                    }
                }

                foreach (var item in dcOficinaInsumos)
                {
                    int inicioConsecutivoCadena = item.Value.Where(t => t.NumeracionCadena > 0).Min(t => t.NumeracionCadena);
                    int finalConsecutivoCadena = item.Value.Where(t => t.NumeracionCadena > 0).Max(t => t.NumeracionCadena);                    

                    item.Value.ForEach(t => t.NumeracionInicialCadena = inicioConsecutivoCadena);
                    item.Value.ForEach(t => t.NumeracionFinalCadena = finalConsecutivoCadena);
                }

                return dcOficinaInsumos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<EstructuraArchivoCargado> ValidarYCargar(string fechaHoraPedido)
        {
            List<EstructuraArchivoCargado> ltsEstructuraArchivoCargados = new List<EstructuraArchivoCargado>();
            List<string> ltsMensajesConsistensias = new List<string>();
            Dictionary<string, string> dckeyValuePairs = new Dictionary<string, string>();

            try
            {
                Cliente cliente = null;
                string strCodigoCliente = String.Empty;
                using (StreamReader reader = new StreamReader(pathSubido, Encoding.Default))
                {
                    String linea;
                    int posicionLinea = 1;
                    while ((linea = reader.ReadLine()) != null)
                    {
                        List<string> lines = linea.Split(new[] { ',' }).ToList();



                        if (lines.Count() < 7)
                        {
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} la estructura esta errada, se esperan 7 columnas y contiene '{lines.Count()}'");
                            continue;
                        }

                        strCodigoCliente = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoCliente];
                        string strTipoPorducto = lines[(int)CargarArchivo.MyEnumCargarArchivo.TipoPorducto];
                        string strCodigoOficina = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoOficina];
                        string strCodigoCajero = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero];
                        string strInicioNumeracion = lines[(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion];
                        string strCantidadNumeracion = lines[(int)CargarArchivo.MyEnumCargarArchivo.CantidadNumeracion];
                        string strInfoCliente = lines[(int)CargarArchivo.MyEnumCargarArchivo.InfoCliente];
                        Oficina oficina = null;
                        Cajero cajero = null;

                        if (string.IsNullOrEmpty(strCodigoCliente) || string.IsNullOrWhiteSpace(strCodigoCliente))
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCliente + 1} el codigo de cliente se encuentra vacío.");
                        else
                        {
                            cliente = _context.Cliente.FirstOrDefault(t => t.Codigo == strCodigoCliente);
                            if (cliente == null)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCliente + 1} el codigo de cliente '{strCodigoCliente}' no existe.");
                            else if (!cliente.Estado)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCliente + 1} El cliente '{strCodigoCliente}' se encuentra inactiva.");

                        }


                        if (string.IsNullOrEmpty(strCodigoOficina) || string.IsNullOrWhiteSpace(strCodigoOficina))                         
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoOficina + 1} el codigo de Oficina se encuentra vacío.");                                                          
                        else if (!string.IsNullOrEmpty(strCodigoCliente))
                        {
                            List<Oficina> oficinas = new List<Oficina>();

                            if (cliente != null) 
                            {
                                oficinas = _context.Oficina.Where(t => t.idCliente == cliente.IdCliente && t.Codigo == strCodigoOficina).ToList();

                                if (oficinas.Count > 0)
                                    oficina = oficinas.FirstOrDefault();

                                if (oficina == null)
                                    ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoOficina + 1} el codigo de Oficina '{strCodigoOficina}' no existe.");

                                else if (!oficina.Estado)
                                    ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoOficina + 1} la Oficina '{strCodigoOficina}' se encuentra inactiva.");

                            }
                        } 

                        if (string.IsNullOrEmpty(strCodigoCajero) || string.IsNullOrWhiteSpace(strCodigoCajero))
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero + 1} el codigo del Cajero se encuentra vacío.");                        
                        else 
                        {
                            List<Cajero> cajeros = new List<Cajero>();

                            if (oficina != null) 
                            {
                                cajeros = _context.Cajero.Where(t => t.IdOficina == oficina.IdOficina && t.Codigo == strCodigoCajero).ToList();

                                if (cajeros.Count > 0)
                                    cajero = cajeros.FirstOrDefault();

                                if (cajero == null)
                                    ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero + 1} el codigo del Cajero '{strCodigoCajero}' no existe.");

                                else if (!cajero.Estado)
                                    ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero + 1} el cajero '{strCodigoCajero}' se encuentra inactivo.");

                            }
                        }                         
                       

                        if (string.IsNullOrEmpty(strInicioNumeracion) || string.IsNullOrWhiteSpace(strInicioNumeracion))
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero + 1} la numeración se encuentra vacío.");
                        else if (cajero != null)
                        {
                            int InicioNumeracion = 0;
                            int.TryParse(strInicioNumeracion,out InicioNumeracion);

                            if (InicioNumeracion == 0)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strInicioNumeracion}' contiene letras o viene en cero.");
                            else if (InicioNumeracion < 0)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strInicioNumeracion}' contiene un valor menor a cero");
                            else if (strInicioNumeracion.Length > 6)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strInicioNumeracion}' contiene '{strInicioNumeracion.Length}' caracteres y se esperan '6'.");
                            else if (InicioNumeracion != (cajero.ultimaNumeracion + 1))                             
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strInicioNumeracion}' enviada por el cliente no corresponde a la ultima numeracion '{(cajero.ultimaNumeracion + 1):000000}' controlada al código de cajero '{cajero.Codigo}'.");                            
                        }


                        if (string.IsNullOrEmpty(strCantidadNumeracion) || string.IsNullOrWhiteSpace(strCantidadNumeracion))
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.CantidadNumeracion + 1} la cantidad se encuentra vacío.");
                        else
                        {
                            int CantidadNumeracion = 0;
                            int.TryParse(strCantidadNumeracion, out CantidadNumeracion);

                            if (CantidadNumeracion == 0)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strCantidadNumeracion}' contiene letras o viene en cero.");
                            else if (CantidadNumeracion < 0)
                                ltsMensajesConsistensias.Add($"En la linea {posicionLinea} y columan {(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion + 1} la numeración '{strCantidadNumeracion}' contiene un valor menor a cero");
                        }

                        string strOficinaCajero = $"{strCodigoOficina}{strCodigoCajero}";

                        if (dckeyValuePairs.ContainsKey(strOficinaCajero))                        
                            ltsMensajesConsistensias.Add($"En la linea {posicionLinea} el codigo de oficina '{strCodigoOficina}' y el cajero '{strCodigoCajero}' ya existe, por favor validar.");                        
                        else                        
                            dckeyValuePairs.Add(strOficinaCajero, strInicioNumeracion);
                        


                        if (ltsMensajesConsistensias.Count > 0)
                        {
                            posicionLinea++;
                            continue;
                        }

                        ltsEstructuraArchivoCargados.Add(new EstructuraArchivoCargado
                        {
                            CodigoCliente = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoCliente],
                            TipoPorducto = lines[(int)CargarArchivo.MyEnumCargarArchivo.TipoPorducto],
                            CodigoOficina = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoOficina],
                            CodigoCajero = lines[(int)CargarArchivo.MyEnumCargarArchivo.CodigoCajero],
                            InicioNumeracion = Convert.ToInt32(lines[(int)CargarArchivo.MyEnumCargarArchivo.InicioNumeracion]),
                            CantidadNumeracion = Convert.ToInt32(lines[(int)CargarArchivo.MyEnumCargarArchivo.CantidadNumeracion]),
                            InfoCliente = lines[(int)CargarArchivo.MyEnumCargarArchivo.InfoCliente]
                        });
                        posicionLinea++;
                    }

                    reader.Close();

                    if (posicionLinea > 1)
                    {
                        if (cliente == null)
                            throw new Exception($"El cliente con código '{strCodigoCliente}' no existe, por favor crear el cliente con dicho código.");



                        string RutaTransmisiones = Path.Combine(cliente.RutaBaseSalida, "Transmision", fechaHoraPedido, Path.GetFileName(pathSubido));

                        if (!Directory.Exists(Path.GetDirectoryName(RutaTransmisiones)))
                            Directory.CreateDirectory(Path.GetDirectoryName(RutaTransmisiones));

                        if (System.IO.File.Exists(RutaTransmisiones))
                            System.IO.File.Delete(RutaTransmisiones);

                        System.IO.File.Move(pathSubido, RutaTransmisiones);

                        pathSubido = RutaTransmisiones;

                        if (ltsMensajesConsistensias.Count > 0)
                        {
                            string strRutaArchivoInconsistente = Path.Combine(Path.GetDirectoryName(pathSubido), Path.GetFileNameWithoutExtension(pathSubido) + "_Inconsistencia" + Path.GetExtension(pathSubido));

                            using (StreamWriter sw = new StreamWriter(strRutaArchivoInconsistente))
                                foreach (string row in ltsMensajesConsistensias)
                                    sw.WriteLine(row);

                            //Functions.FileHelper.LisToFile(strRutaArchivoInconsistente, ltsMensajesConsistensias, Encoding.Default, false, false);
                            throw new Exception($"Se encontraron '{ltsMensajesConsistensias.Count}' inconsistencia en el archivo '{Path.GetFileName(pathSubido)}'" +
                                $"{Environment.NewLine}{Environment.NewLine}Por favor validar el archivo '{Path.GetFileName(strRutaArchivoInconsistente)}' que se encuentra en la siguiente ruta '{Path.GetDirectoryName(strRutaArchivoInconsistente)}'");
                        }
                    }

                }

                return ltsEstructuraArchivoCargados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> SubirArchivo(CargarArchivo cargarArchivo, string fechaPedido)
        {
            string path = string.Empty;
            try
            {
                string rutaBase = $@"D:\TempAdhesivoDian\{fechaPedido}";
                if (!Directory.Exists(rutaBase))                
                    Directory.CreateDirectory(rutaBase);                

                path = Path.Combine(rutaBase, cargarArchivo.RutaArchivo.FileName);

                using var strem = new FileStream(path, FileMode.Create);
                await cargarArchivo.RutaArchivo.CopyToAsync(strem); 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return path;
        }

        // GET: Encabezadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encabezado = await _context.Encabezado.FindAsync(id);
            if (encabezado == null)
            {
                return NotFound();
            }
            return View(encabezado);
        }

        // POST: Encabezadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEncabezado,NombrePedido,FechaCreacion,NombreArchivoCargado")] Encabezado encabezado)
        {
            if (id != encabezado.IdEncabezado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encabezado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncabezadoExists(encabezado.IdEncabezado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(encabezado);
        }

        // GET: Encabezadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encabezado = await _context.Encabezado
                .FirstOrDefaultAsync(m => m.IdEncabezado == id);
            if (encabezado == null)
            {
                return NotFound();
            }

            return View(encabezado);
        }

        // POST: Encabezadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encabezado = await _context.Encabezado.FindAsync(id);
            _context.Encabezado.Remove(encabezado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncabezadoExists(int id)
        {
            return _context.Encabezado.Any(e => e.IdEncabezado == id);
        }
    }
}
