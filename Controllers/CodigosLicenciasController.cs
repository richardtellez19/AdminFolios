using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminFolios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AdminFolios.Controllers
{
    //[Route("api/[controller]")]
    //[Route("api/[controller]")]
    [Route("mobile/folios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CodigosLicenciasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CodigosLicenciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Regresa todo el listado de folios
        // POST: mobile/folios/todos
        [HttpPost("todos")]
        public async Task<ActionResult<IEnumerable<CodigosLicencias>>> GetCodigosLicencias()
        {
            return await _context.CodigosLicencias.ToListAsync();
        }

        //Regresa un folio en base a su id
        // GET: mobile/folios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodigosLicencias>> GetCodigosLicencias(long id)
        {
            var codigosLicencias = await _context.CodigosLicencias.FindAsync(id);

            if (codigosLicencias == null)
            {
                return NotFound();
            }

            return codigosLicencias;
        }

        //Regresa boolean si es que tiene folios disponibles
        [HttpPost("foliosActivos")]
        public Boolean PostFoliosActivos(string rfc)
        {
            return RevisaFoliosActivos(rfc);
        }

        //Regresa folios en base a RFC y status
        // POST: mobile/folios/Restantes?rfc=MOB100617FNA&status=1
        [HttpPost("statusRFC")]
        public async Task<ActionResult<IEnumerable<CodigosLicencias>>> PostFoliosStatus(string rfc, int status)
        {
            //var codigosLicencias = await _context.CodigosLicencias.Where(l => l.RfcCliente == rfc).FirstAsync();
            //var codigosLicencias = await (from l in _context.CodigosLicencias
            //where l.RfcCliente == rfc && l.Activa == 1
            //select l).FirstAsync();
            var codigosLicencias = await FoliosPorRFCStatus(rfc, status);

            if (codigosLicencias == null)
            {
                return NotFound();
            }

            return codigosLicencias;
        }


        //Regresa precio de los folios
        //
        [HttpPost("precioFolios")]
        public async Task<ActionResult<IEnumerable<PreciosFolios>>> PrecioFolios()
        {            
            var precioFolios = await (from p in _context.PreciosFolios select p).ToListAsync() ;

            if (precioFolios == null)
            {
                return NotFound();
            }

            return precioFolios;
        }

        //Agrega un nuevo registro a la Base de Datos

        [HttpPost("nuevaSolicitud")]
        public async Task<ActionResult<long>> NuevaSolicitud(string rfc, int cantidad)
        {
            DateTime hoy = DateTime.Now;
            CodigosLicencias nuevo = new CodigosLicencias();
            nuevo.Licencia = "df499fa8-404b-4d4f-9110-b8462b8cd943";
            nuevo.TipoPlan = 1;
            nuevo.CaducidadMeses = 12;
            nuevo.Gratuitos = false;
            nuevo.RfcCliente = rfc;
            nuevo.CantCfdis = cantidad;
            nuevo.CfdisRestantes = cantidad;
            nuevo.FechaIni = hoy;
            nuevo.FechaFin = hoy.AddDays(365);
            nuevo.Activa = 2;
            _context.CodigosLicencias.Add(nuevo);
            await _context.SaveChangesAsync();

            return nuevo.Folio;
        }

        //Regresa folios en base a un status
        // POST: mobile/folios/Restantes?status=1
        [HttpPost("status")]
        public async Task<ActionResult<IEnumerable<CodigosLicencias>>> PostFoliostatus(int status)
        {
            //var codigosLicencias = await _context.CodigosLicencias.Where(l => l.RfcCliente == rfc).FirstAsync();
            //var codigosLicencias = await (from l in _context.CodigosLicencias
            //where l.RfcCliente == rfc && l.Activa == 1
            //select l).FirstAsync();
            var codigosLicencias = await FoliosPorEstatus(status);

            if (codigosLicencias == null)
            {
                return NotFound();
            }

            return codigosLicencias;
        }

        //Cambia el status de un folio
        //Cambiar status folios
        [HttpPut("folio")]
        public async Task<ActionResult<CodigosLicencias>> PostFoliosUpdate(int id, int status)
        {
            //var codigosLicencias = await _context.CodigosLicencias.Where(l => l.RfcCliente == rfc).FirstAsync();
            //var codigosLicencias = await (from l in _context.CodigosLicencias
            //where l.RfcCliente == rfc && l.Activa == 1
            //select l).FirstAsync();
            var codigosLicencias = await ActualizaFolios(id, status);

            if (codigosLicencias == null)
            {
                return NotFound();
            }

            return codigosLicencias;
        }
        
        //Disminuye en 1 los folios de dado RFC
        [HttpPut("consumo")]
        public async Task<ActionResult<CodigosLicencias>> PutConsumo(string rfc)
        {
            var folioAModificar = (from f in _context.CodigosLicencias
                                   where f.RfcCliente == rfc && f.Activa == 1 && f.CfdisRestantes > 0
                                   orderby f.Folio
                                   select f).FirstOrDefault();

            if (folioAModificar == null)
                return NotFound();

            var codigosLicencias = await DisminuyeFolio(rfc);

            if (codigosLicencias == null)
            {
                return NotFound();
            }

            return codigosLicencias;
        }


        //// DELETE: api/CodigosLicencias/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<CodigosLicencias>> DeleteCodigosLicencias(long id)
        //{
        //    var codigosLicencias = await _context.CodigosLicencias.FindAsync(id);
        //    if (codigosLicencias == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CodigosLicencias.Remove(codigosLicencias);
        //    await _context.SaveChangesAsync();

        //    return codigosLicencias;
        //}

        private bool CodigosLicenciasExists(long id)
        {
            return _context.CodigosLicencias.Any(e => e.Folio == id);
        }

        //Regresa los folios en base a un estado específico 
        private Task<List<CodigosLicencias>> FoliosPorRFCStatus(string rfc, int status)
        {
            //var folios;
            if (status == -1)
            {
                return (from l in _context.CodigosLicencias
                        where l.RfcCliente == rfc
                        select l).ToListAsync();
            }
            else
            {
                return (from l in _context.CodigosLicencias
                        where l.RfcCliente == rfc && l.Activa == status
                        select l).ToListAsync();
            }
        }

        //Regresa los folios en base a un estado específico 
        private Task<List<CodigosLicencias>> FoliosPorEstatus(int status)
        {
            var folios = (from l in _context.CodigosLicencias
                          where l.Activa == status
                          select l).ToListAsync();
            return folios;
        }

        private Task<CodigosLicencias> ActualizaFolios(int id, int status)
        {
            var folioAModificar = (from f in _context.CodigosLicencias
                                   where f.Folio == id
                                   select f).First();

            folioAModificar.Activa = status;
            _context.SaveChanges();

            var folio = (from f in _context.CodigosLicencias
                         where f.Folio == id
                         select f).FirstAsync();


            return folio;
        }

        private Task<CodigosLicencias> DisminuyeFolio(string rfc)
        {
            var folioAModificar = (from f in _context.CodigosLicencias
                                   where f.RfcCliente == rfc && f.Activa == 1 && f.CfdisRestantes > 0
                                   orderby f.Folio
                                   select f).FirstOrDefault();

            if (folioAModificar == null)
                return null;

            folioAModificar.CfdisRestantes -= 1;
            _context.SaveChanges();

            var folio = (from f in _context.CodigosLicencias
                         where f.Folio == folioAModificar.Folio
                         select f).FirstAsync();


            return folio;
        }

        private Boolean RevisaFoliosActivos(string rfc)
        {
            var folios = _context.CodigosLicencias.Where(x => x.RfcCliente == rfc && x.Activa == 1 && x.CfdisRestantes > 0).FirstOrDefault();
            if (folios == null)
                return false;
            else
                return true;
        }
    }
}

