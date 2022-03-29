using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminFolios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminFolios.Controllers
{
    [Authorize]
    public class ConteoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ConteoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Conteo(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var model = _context.EdiCount.OrderByDescending(x => x.Año)
                                         .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                         .Take(cantidadRegistrosPorPagina).ToList();

            var suma = _context.EdiCount.Sum(x => x.Conteo);

            var totalDeRegistros = _context.EdiCount.Count();

            var modelo = new IndexConteoViewModelcs();
            modelo.EdiCount = model;
            modelo.total = suma;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();

            return View(modelo);
        }
        public IActionResult conteoF(string rfc = "-1", int year = -1, int mes = -1, int pagina = 1)
        {
            int suma = 0;
            var cantidadRegistrosPorPagina = 10;
            int totalDeRegistros = 0;
            List<EdiCount> model;
            if (rfc == "-1" || string.IsNullOrEmpty(rfc))
            {
                if (year == -1)
                {
                    if (mes == -1)
                    {
                        model = _context.EdiCount
                                        .OrderByDescending(x => x.Año)
                                        .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                        .Take(cantidadRegistrosPorPagina).ToList();
                        totalDeRegistros = _context.EdiCount.Count();
                        suma = _context.EdiCount.Sum(x => x.Conteo);
                    }
                    else
                    {
                        model = _context.EdiCount.Where(x => x.Mes == mes)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();
                        totalDeRegistros = _context.EdiCount.Where(x => x.Mes == mes).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Mes == mes).Sum(x => x.Conteo);
                    }
                }
                else
                {
                    if (mes == -1)
                    {
                        model = _context.EdiCount.Where(x => x.Año == year)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Año == year).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Año == year).Sum(x => x.Conteo);
                    }
                    else
                    {
                        model = _context.EdiCount.Where(x => x.Mes == mes && x.Año == year)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Mes == mes && x.Año == year).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Mes == mes && x.Año == year).Sum(x => x.Conteo);
                    }
                }
            }
            else
            {
                if (year == -1)
                {
                    if (mes == -1)
                    {
                        model = _context.EdiCount.Where(x => x.Rfc == rfc)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Rfc == rfc).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Rfc == rfc).Sum(x => x.Conteo);
                    }
                    else
                    {
                        model = _context.EdiCount.Where(x => x.Mes == mes && x.Rfc == rfc)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Mes == mes && x.Rfc == rfc).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Mes == mes && x.Rfc == rfc).Sum(x => x.Conteo);
                    }
                }
                else
                {
                    if (mes == -1)
                    {
                        model = _context.EdiCount.Where(x => x.Año == year && x.Rfc == rfc)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Año == year && x.Rfc == rfc).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Año == year && x.Rfc == rfc).Sum(x => x.Conteo);
                    }
                    else
                    {
                        model = _context.EdiCount.Where(x => x.Mes == mes && x.Año == year && x.Rfc == rfc)
                                                 .OrderByDescending(x => x.Año)
                                                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                                 .Take(cantidadRegistrosPorPagina).ToList();

                        totalDeRegistros = _context.EdiCount.Where(x => x.Año == year && x.Rfc == rfc).ToList().Count();
                        suma = _context.EdiCount.Where(x => x.Mes == mes && x.Año == year && x.Rfc == rfc).Sum(x => x.Conteo);
                    }
                }
            }

            var modelo = new IndexConteoViewModelcs();
            modelo.EdiCount = model;
            modelo.total = suma;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            modelo.ValoresQueryString["rfc"] = rfc;
            modelo.ValoresQueryString["mes"] = mes;
            modelo.ValoresQueryString["year"] = year;

            return View("Conteo", modelo);
        }

        public IActionResult conteoS(int mes, int year, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            int totalDeRegistros = 0;
            int inicio = 1;
            int final = 6;
            if (mes >= 7)
            {
                inicio = 7;
                final = 12;
            }
            List<EdiCount> model;
            model = _context.EdiCount.Where(x => x.Mes >= inicio && x.Mes <= final && x.Año == year)
                                     .OrderByDescending(x => x.Año)
                                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                                     .Take(cantidadRegistrosPorPagina).ToList();

            totalDeRegistros = _context.EdiCount.Where(x => x.Mes >= inicio && x.Mes <= final && x.Año == year).ToList().Count();

            var suma = _context.EdiCount.Where(x => x.Mes >= inicio && x.Mes <= final && x.Año == year).Sum(x => x.Conteo);

            var modelo = new IndexConteoViewModelcs();
            modelo.EdiCount = model;
            modelo.total = suma;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new Microsoft.AspNetCore.Routing.RouteValueDictionary();
            modelo.ValoresQueryString["mes"] = mes;
            modelo.ValoresQueryString["year"] = year;

            return View("Conteo", modelo);
        }

        public IActionResult conteoAnual(int yearInicial, int yearFinal)
        {
            if (yearInicial == 0)
                yearInicial = Convert.ToInt32(DateTime.Now.AddYears(-1).ToString("yyyy"));
            if (yearFinal == 0)
                yearFinal = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            List<ConteoGraficaModel> conteoGraficaModelList = new List<ConteoGraficaModel>();
            int index = yearInicial;
            while (index <= yearFinal)
            {
                ConteoGraficaModel conteoGraficaModel = new ConteoGraficaModel();
                conteoGraficaModel.ConteoAnual = new string[13];
                conteoGraficaModel.ConteoAnual[0] = index.ToString();
                for (int i = 1; i <= 12; i++)
                {
                    conteoGraficaModel.ConteoAnual[i] = _context.EdiCount.Where(x => x.Año == index && x.Mes == i).Sum(x => x.Conteo).ToString();
                }
                conteoGraficaModelList.Add(conteoGraficaModel);
                index++;
            }

            return View("Grafica", conteoGraficaModelList);
        }
        //public IActionResult Index()
        //{
        //    List<EdiCount> model;
        //    model = _context.EdiCount.ToList();
        //    return View(model);
        //}
    }
}
