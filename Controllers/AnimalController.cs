using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AnimalController : Controller
{
    private readonly LaReserveContext _context;

    public AnimalController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: Animaux
    public async Task<IActionResult> Index()
    {
        var animaux = await _context.Animaux.Include(a => a.Lieu).OrderBy(a => a.Nom).ToListAsync();

        var LieuxList = _context.Lieux.ToList();
        ViewBag.ListeLieux = LieuxList;


        return View(animaux);
    }

    //Afiche le détail d'un animal
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var animal = await _context.Animaux.Include(m => m.Lieu).OrderBy(m => m.Nom)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (animal == null)
        {
            return NotFound();
        }
        var personnels = await _context.Personnels.Where(a => a.Specialisation == animal.Type).ToListAsync();
        ViewBag.ListePersonnels = personnels;
        return View(animal);
    }


    public IActionResult Create()
    {
        //Permet de récupérer la liste de tous les types existants
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);

        //Permet de récupérer la liste de tous les lieux existants
        var LieuxList = _context.Lieux.ToList();
        ViewBag.ListeLieux = new SelectList(LieuxList, "Id", "Nom");
        return View();


    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nom,Age,Espece,Type,LieuId")] Animal animal)
    {
        var Lieu = _context.Lieux.Find(animal.LieuId);
        animal.Lieu = Lieu!;

        _context.Add(animal);
        await _context.SaveChangesAsync();


        return RedirectToAction("Details", "Animal", new RouteValueDictionary { { "Id", animal.Id } });
    }


    public async Task<IActionResult> ConfirmDeleteAnimal(int id)
    {
        var animal = await _context.Animaux.FirstOrDefaultAsync(a => a.Id == id);

        return View(animal);
    }

    public async Task<IActionResult> DeleteAnimal(int id)
    {

        var animal = await _context.Animaux.FirstOrDefaultAsync(a => a.Id == id);
        if (animal == null)
            return NotFound();

        _context.Animaux.Remove(animal);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Modif(int? id)
    {
        //Permet de récupérer la liste de tous les types existants
        var Types = Enum.GetValues(typeof(Type));
        ViewBag.ListeTypes = new SelectList(Types);

        //Permet de récupérer la liste de tous les lieux existants
        var LieuxList = _context.Lieux.ToList();
        ViewBag.ListeLieux = new SelectList(LieuxList, "Id", "Nom");

        if (id == null)
        {
            return NotFound();
        }
        var animal = await _context.Animaux.FindAsync(id);
        if (animal == null)
        {
            return NotFound();
        }
        return View(animal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Modif(int id, [Bind("Id,Nom,Age,Espece,Type,LieuId")] AnimalDTO animalDTO)
    {
        if (id != animalDTO.Id)
            return BadRequest();

        Animal animal = new Animal(animalDTO);

        var lieu = _context.Lieux.Find(animal.LieuId);


        animal.Lieu = lieu!;


        _context.Entry(animal).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Animaux.Any(a => a.Id == id))
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


    //Permet d'afficher les animaux d'un lieu
    [HttpGet]

    public IActionResult FiltrerLieux(int choix)
    {

        var Animaux = _context.Animaux.Include(a => a.Lieu).Where(a => a.LieuId == choix).ToList();
        ViewBag.ListeAnimaux = Animaux;

        ViewBag.Lieu = _context.Lieux.Where(l => l.Id == choix).SingleOrDefault();

        return View();
    }


}