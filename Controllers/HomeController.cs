using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projet_bernard_degorre_gr3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;



public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LaReserveContext _context;

    public HomeController(ILogger<HomeController> logger, LaReserveContext context)
    {
        _logger = logger;
        _context = context;
    }




    public IActionResult Index()
    {
        //Permet de récupérer la liste de tous les Evenements existants
        var EvenementsList = _context.Evenements.OrderBy(e => e.Date).ToList();
        ViewBag.ListeEvenement = EvenementsList;

        var LieuEvenementsList = _context.Lieux.ToList();
        ViewBag.ListeEvenementLieux = LieuEvenementsList;



        return View();
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
