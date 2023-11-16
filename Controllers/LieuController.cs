using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class LieuController : Controller
{
    private readonly LaReserveContext _context;

    public LieuController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: Lieu
    public async Task<IActionResult> Index()
    {
        var lieu = await _context.Lieux.OrderBy(p => p.Nom).ToListAsync();
        return View(lieu);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lieu = await _context.Lieux
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lieu == null)
        {
            return NotFound();
        }
        var animaux = await _context.Animaux.Include(a => a.Lieu).Where(a => a.Lieu.Id == id).ToListAsync();
        ViewBag.ListeAnimaux = animaux;
        return View(lieu);
    }

    public IActionResult Create()
    {
        //Permet de récupérer la liste de tous les types existants
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nom,Type")] Lieu lieu)
    {

        _context.Add(lieu);
        await _context.SaveChangesAsync();

        // Redirect to student details
        return RedirectToAction("Details", "Lieu", new RouteValueDictionary { { "Id", lieu.Id } });

    }

    public async Task<IActionResult> ConfirmDeleteLieu(int id)
    {
        var lieu = await _context.Lieux.FirstOrDefaultAsync(l => l.Id == id);

        return View(lieu);
    }


    public async Task<IActionResult> DeleteLieu(int id)
    {

        var lieu = await _context.Lieux.FirstOrDefaultAsync(l => l.Id == id);
        if (lieu == null)
            return NotFound();

        //On retire de la réserve tous les animaux qui appartiennent au Lieu qui veut être supprimé
        var AnimauxLieu = _context.Animaux.Include(a => a.Lieu).Where(a => a.LieuId == id).ToList();


        foreach (var a in AnimauxLieu)
        {
            _context.Animaux.Remove(a);

        }

        _context.Lieux.Remove(lieu);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Modif(int? id)
    {
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);

        if (id == null)
        {
            return NotFound();
        }
        var lieu = await _context.Lieux.FindAsync(id);
        if (lieu == null)
        {
            return NotFound();
        }
        return View(lieu);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Modif(int id, [Bind("Id,Nom,Type")] Lieu lieu)
    {
        if (id != lieu.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lieu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LieuExist(lieu.Id))
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
        return View(lieu);
    }
    private bool LieuExist(int id)
    {
        return _context.Personnels.Any(l => l.Id == id);
    }



}