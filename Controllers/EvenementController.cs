using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EvenementController : Controller
{
    private readonly LaReserveContext _context;

    public EvenementController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: Lieu
    public async Task<IActionResult> Index()
    {
        var evenement = await _context.Evenements.Include(e => e.Animal).Include(e => e.Personnel).Include(e => e.Lieu).ToListAsync();
        return View(evenement);


    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var evenement = await _context.Evenements.Include(e => e.Lieu).Include(e => e.Animal).Include(e => e.Personnel)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (evenement == null)
        {
            return NotFound();
        }
        return View(evenement);
    }


    public IActionResult Create()
    {
        //Permet de récupérer la liste de tous les Animaux existants
        var AnimauxList = _context.Animaux.ToList();
        ViewBag.ListeAnimaux = new SelectList(AnimauxList, "Id", "Nom");

        //Permet de récupérer la liste de tous les Personnels existants
        var PersonnelsList = _context.Personnels.ToList();
        ViewBag.ListePersonnels = new SelectList(PersonnelsList, "Id", "Nom");

        //Permet de récupérer la liste de tous les lieux existants
        var LieuxList = _context.Lieux.ToList();
        ViewBag.ListeLieux = new SelectList(LieuxList, "Id", "Nom");
        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nom,Description,Date,LieuId,AnimalId,PersonnelId")] EvenementDTO evenementDTO)
    {
        Evenement evenement = new Evenement(evenementDTO);

        var lieu = _context.Lieux.Find(evenement.LieuId);

        evenement.Lieu = lieu!;

        if (ModelState.IsValid)
        {
            // Create new enrollment in DB
            _context.Add(evenement);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Details", "Evenement", new RouteValueDictionary { { "Id", evenement.Id } });

    }

    public async Task<IActionResult> ConfirmDeleteEvenement(int id)
    {
        var evenement = await _context.Evenements.FirstOrDefaultAsync(a => a.Id == id);

        return View(evenement);
    }

    public async Task<IActionResult> DeleteEvenement(int id)
    {

        var evenement = await _context.Evenements.FirstOrDefaultAsync(a => a.Id == id);
        if (evenement == null)
            return NotFound();

        _context.Evenements.Remove(evenement);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Modif(int? id)
    {
        var LieuxList = _context.Lieux.ToList();
        ViewBag.ListeLieux = new SelectList(LieuxList, "Id", "Nom");
        var AnimauxList = _context.Animaux.ToList();
        ViewBag.ListeAnimaux = new SelectList(AnimauxList, "Id", "Nom");

        //Permet de récupérer la liste de tous les Personnels existants
        var PersonnelsList = _context.Personnels.ToList();
        ViewBag.ListePersonnels = new SelectList(PersonnelsList, "Id", "Nom");

        if (id == null)
        {
            return NotFound();
        }
        var evenement = await _context.Evenements.FindAsync(id);
        if (evenement == null)
        {
            return NotFound();
        }
        return View(evenement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Modif(int id, [Bind("Id,Nom,Description,Date,LieuId,AnimalId,PersonnelId")] EvenementDTO evenementDTO)
    {
        if (id != evenementDTO.Id)
            return BadRequest();

        Evenement evenement = new Evenement(evenementDTO);

        var lieu = _context.Lieux.Find(evenement.LieuId);

        evenement.Lieu = lieu!;


        _context.Entry(evenement).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Evenements.Any(a => a.Id == id))
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
}