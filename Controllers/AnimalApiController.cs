using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AnimalApiController : ControllerBase
{
    private readonly LaReserveContext _context;

    public AnimalApiController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: api/AnimalApi
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimaux()
    {
        return await _context.Animaux.OrderBy(e => e.Nom).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
        var animal = await _context.Animaux.Where(s => s.Id == id)
            .Include(s => s.Lieu)
            .SingleOrDefaultAsync();
        if (animal == null)
            return NotFound();
        return animal;
    }

    [HttpGet("parlieu/{lieuId}")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimauxParLieu(int lieuId)
    {
        var animauxparlieu = await _context.Animaux.Where(s => s.LieuId == lieuId)
            .Include(s => s.Lieu)
            .ToListAsync();
        if (animauxparlieu == null)
            return NotFound();
        return animauxparlieu;
    }

    [HttpGet("{id}/PrisenchargePar")]
    //Affiche tout le personnel pouvant s'occuper de l'animal identifié par son id
    public async Task<ActionResult<IEnumerable<Personnel>>> GetPersonnelParAnimal(int id)
    {
        var animal = await _context.Animaux.FindAsync(id);
        if (animal == null)
        {
            return NotFound();
        }
        var personnels = await _context.Personnels.Where(m => m.Specialisation == animal.Type).ToListAsync();
        return personnels;
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> PostAnimal(AnimalDTO animalDTO)
    {
        Animal animal = new Animal(animalDTO);


        var lieu = _context.Lieux.Find(animal.LieuId);
        animal.Lieu = lieu!;
        // Create new animal in DB
        _context.Animaux.Add(animal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostAnimal), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnimal(int id, AnimalDTO animalDTO)
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
            if (!_context.Animaux.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var animal = await _context.Animaux.FindAsync(id);
        if (animal == null)
            return NotFound();

        _context.Animaux.Remove(animal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

