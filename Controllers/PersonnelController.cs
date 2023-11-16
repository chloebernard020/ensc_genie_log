using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PersonnelController : Controller
{
    private readonly LaReserveContext _context;

    public PersonnelController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: Personnel
    public async Task<IActionResult> Index()
    {
        var personnel = await _context.Personnels.OrderBy(p => p.Nom).ToListAsync();
        return View(personnel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personnel = await _context.Personnels
            .FirstOrDefaultAsync(m => m.Id == id);
        if (personnel == null)
        {
            return NotFound();
        }
        var animaux = await _context.Animaux.Where(a => a.Type == personnel.Specialisation).ToListAsync();
        ViewBag.ListeAnimaux = animaux;
        return View(personnel);
    }

    public IActionResult Create()
    {
        //Permet de récupérer la liste de tous les types existants
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);
        var Metiers = Enum.GetValues(typeof(Metier));
        ViewBag.ListeMetiers = new SelectList(Metiers);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Metier,Specialisation")] Personnel personnel)
    {

        _context.Add(personnel);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Personnel", new RouteValueDictionary { { "Id", personnel.Id } });

    }

    public async Task<IActionResult> ConfirmDeletePersonnel(int id)
    {
        var personnel = await _context.Personnels.FirstOrDefaultAsync(p => p.Id == id);

        return View(personnel);
    }


    public async Task<IActionResult> DeletePersonnel(int id)
    {

        var personnel = await _context.Personnels.FirstOrDefaultAsync(p => p.Id == id);
        if (personnel == null)
            return NotFound();

        _context.Personnels.Remove(personnel);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Modif(int? id)
    {
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);
        var Metiers = Enum.GetValues(typeof(Metier));
        ViewBag.ListeMetiers = new SelectList(Metiers);

        if (id == null)
        {
            return NotFound();
        }
        var personnel = await _context.Personnels.FindAsync(id);
        if (personnel == null)
        {
            return NotFound();
        }
        return View(personnel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Modif(int id, [Bind("Id,Nom,Prenom,Metier,Specialisation")] Personnel personnel)
    {
        if (id != personnel.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(personnel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonnelExist(personnel.Id))
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
        return View(personnel);
    }
    private bool PersonnelExist(int id)
    {
        return _context.Personnels.Any(p => p.Id == id);
    }
}
