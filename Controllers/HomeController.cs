using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminFolios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace AdminFolios.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
             
        public IActionResult Folios(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var model = _context.CodigosLicencias.OrderByDescending(x => x.Folio)
                                                .Skip((pagina -1) * cantidadRegistrosPorPagina)
                                                .Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = _context.CodigosLicencias.Count();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();

            return View(modelo);
        }

        public ActionResult Activar(int id)
        {
            var folioActualizado =  ActualizaFolios(id, 1);

            if (folioActualizado == null)
            {
                return NotFound();
            }

            return View("Folios", folioActualizado);
        }

        public ActionResult Editar(int id)
        {
            var folioAActualizar = _context.CodigosLicencias.Where(x => x.Folio == id)
                                                            .First();

            if (folioAActualizar == null)
            {
                return NotFound();
            }

            return View("Editar", folioAActualizar);
        }

        public ActionResult Agregar(CodigosLicencias FolioAInsertar)
        {
            return View("Agregar");
        }

        public ActionResult Actualiza(CodigosLicencias updatedFolio)
        {
            var folioAActualizar = _context.CodigosLicencias.Where(x => x.Folio == updatedFolio.Folio).FirstOrDefault();

            folioAActualizar.FechaIni = updatedFolio.FechaIni;
            folioAActualizar.FechaFin = updatedFolio.FechaFin;
            folioAActualizar.CantCfdis = updatedFolio.CantCfdis;
            folioAActualizar.CfdisRestantes = updatedFolio.CfdisRestantes;

            _context.SaveChanges();

            var model = (from f in _context.CodigosLicencias
                         where f.Folio == updatedFolio.Folio
                         select f).ToList();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = 1;
            modelo.TotalRegistros = 1;
            modelo.RegistrosPorPagina = 1;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();

            return View("Folios", modelo);
        }

        public ActionResult Agrega(CodigosLicencias FolioAInsertar)
        {
            FolioAInsertar.Licencia = "df499fa8-404b-4d4f-9110-b8462b8cd943";
            FolioAInsertar.TipoPlan = 1;
            FolioAInsertar.CaducidadMeses = 12;
            FolioAInsertar.Gratuitos = false;
            _context.CodigosLicencias.Add(FolioAInsertar);
            _context.SaveChanges();

            var model = _context.CodigosLicencias.Where(x => x.Folio == FolioAInsertar.Folio).ToList();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = 1;
            modelo.TotalRegistros = 1;
            modelo.RegistrosPorPagina = 1;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            return View("Folios", modelo);
        }

        public ActionResult Desactivar(int id)
        {
            var folioActualizado =  ActualizaFolios(id, 0);

            if (folioActualizado == null)
            {
                return NotFound();
            }

            return View("Folios", folioActualizado);
        }

        public ActionResult Pendiente(int id)
        {
            var folioActualizado =  ActualizaFolios(id, 2);

            if (folioActualizado == null)
            {
                return NotFound();
            }

            return View("Folios", folioActualizado);
        }

        public ActionResult Pendientes(int pagina = 1)
        {
            var foliosAMostrar =  FoliosPorEstatus(2, pagina);

            if (foliosAMostrar == null)
            {
                return NotFound();
            }

            return View("Folios", foliosAMostrar);
        }

        public ActionResult Cancelados(int pagina = 1)
        {
            var foliosAMostrar =  FoliosPorEstatus(0, pagina);

            if (foliosAMostrar == null)
            {
                return NotFound();
            }

            return View("Folios", foliosAMostrar);
        }

        public ActionResult Activos(int pagina = 1)
        {
            var foliosAMostrar =  FoliosPorEstatus(1, pagina);

            if (foliosAMostrar == null)
            {
                return NotFound();
            }

            return View("Folios", foliosAMostrar);
        }

        public ActionResult PorCaducar(int pagina = 1)
        {
            var foliosAMostrar = FoliosPorCaducar(pagina);

            if (foliosAMostrar == null)
            {
                return NotFound();
            }

            return View("Folios", foliosAMostrar);
        }

        //public IActionResult Todos()
        //{
        //    var model = _context.CodigosLicencias.ToList();
        //    return View("Folios", model);
        //}

        public ActionResult Rfc(string rfc, int pagina = 1)
        {
            //if (ModelState.IsValid)
            //{

            //}
            var foliosAMostrar =  FoliosPorRFCStatus(rfc, -1, pagina);

            if (foliosAMostrar == null)
            {
                return NotFound();
            }

            return View("Folios", foliosAMostrar);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IndexViewModel ActualizaFolios(int id, int status)
        {
            var folioAModificar = (from f in _context.CodigosLicencias
                                   where f.Folio == id
                                   select f).First();

            folioAModificar.Activa = status;
            _context.SaveChanges();

            var folio = (from f in _context.CodigosLicencias
                         where f.Folio == id
                         select f).ToList();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = folio;
            modelo.PaginaActual = 1;
            modelo.TotalRegistros = 1;
            modelo.RegistrosPorPagina = 1;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            //modelo.ValoresQueryString["accion"] = parametro;

            return modelo;
        }

        private IndexViewModel FoliosPorRFCStatus(string rfc, int status, int pagina)
        {
            var cantidadRegistrosPorPagina = 10;
            int totalDeRegistros = 1;
            List<CodigosLicencias> model;
            //var folios;
            if (status == -1)
            {
                model = _context.CodigosLicencias.Where(l => l.RfcCliente == rfc)
                                                .OrderByDescending(x => x.Folio)
                                                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                .Take(cantidadRegistrosPorPagina).ToList();

                 totalDeRegistros = _context.CodigosLicencias.Where(l => l.RfcCliente == rfc)
                                                   .ToList().Count();
            }
            else
            {
                model = _context.CodigosLicencias.Where(l => l.RfcCliente == rfc && l.Activa == status)
                                                .OrderByDescending(x => x.Folio)
                                                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                .Take(cantidadRegistrosPorPagina).ToList();

                totalDeRegistros = _context.CodigosLicencias.Where(l => l.RfcCliente == rfc && l.Activa == status)
                                                  .ToList().Count();
            }

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            modelo.ValoresQueryString["rfc"] = rfc;

            return modelo;
        }

        private IndexViewModel FoliosPorEstatus(int status, int pagina)
        {
            string parametro = "Todos";
            switch(status){
                case 1: parametro = "Activos";
                    break;
                case 0:
                    parametro = "Cancelados";
                    break;
                case 2:
                    parametro = "Pendientes";
                    break;
            }
            var cantidadRegistrosPorPagina = 10;
            var model = _context.CodigosLicencias.Where(l => l.Activa == status)
                                                .OrderByDescending(x => x.Folio)
                                                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                .Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = _context.CodigosLicencias.Where(l => l.Activa == status)
                                                .ToList().Count();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            //modelo.ValoresQueryString["accion"] = parametro;

            return modelo;
                        
        }

        private IndexViewModel FoliosPorCaducar(int pagina)
        {
            DateTime hoy = DateTime.Now;
            var cantidadRegistrosPorPagina = 10;
            var model = _context.CodigosLicencias.Where(l => l.FechaFin > hoy && l.FechaFin <= hoy.AddDays(30) && l.CfdisRestantes > 0)
                                                .OrderByDescending(x => x.Folio)
                                                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                .Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = _context.CodigosLicencias.Where(l => l.FechaFin > hoy && l.FechaFin <= hoy.AddDays(30) && l.CfdisRestantes > 0)
                                                .ToList().Count();

            var modelo = new IndexViewModel();
            modelo.CodigosLicencias = model;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            //modelo.ValoresQueryString["accion"] = parametro;

            return modelo;

        }

        public ActionResult BuscarRFC(string term)
        {
            var resultado = _context.CodigosLicencias.Where(x => x.RfcCliente.Contains(term))
                                                        .Select(x => x.RfcCliente)
                                                        .Distinct()
                                                        .Take(5)                                                        
                                                        .ToList();
            return Json(resultado);
        }
    }
}
