using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class EvenementApiController : ControllerBase
{
    private readonly LaReserveContext _context;

    public EvenementApiController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: api/EvenementApi
    public async Task<ActionResult<IEnumerable<Evenement>>> GetEvenements()
    {
        return await _context.Evenements.OrderBy(e => e.Nom).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Evenement>> GetEvenement(int id)
    {
        var evenement = await _context.Evenements.FindAsync(id);
        if (evenement == null)
            return NotFound();
        return evenement;
    }

    [HttpGet("parlieu/{lieuId}")]
    public async Task<ActionResult<IEnumerable<Evenement>>> GetEvenementParLieu(int lieuId)
    {
        var evenementparlieu = await _context.Evenements.Where(s => s.LieuId == lieuId)
            .ToListAsync();
        if (evenementparlieu == null)
            return NotFound();
        return evenementparlieu;
    }

    [HttpPost]
    public async Task<ActionResult<Evenement>> PostEvenement(EvenementDTO evenementDTO)
    {
        Evenement evenement = new Evenement(evenementDTO);

        // On cherche le lieu, l'animal et le personnel impliqués dans l'événement
        var lieu = _context.Lieux.Find(evenement.LieuId);
        evenement.Lieu = lieu!;
        var animal = _context.Animaux.Find(evenement.AnimalId);
        evenement.Animal = animal!;
        var personnel = _context.Personnels.Find(evenement.PersonnelId);
        evenement.Personnel = personnel!;

        // On créé l'événement dans la BD
        _context.Evenements.Add(evenement);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostEvenement), new { id = evenement.Id }, evenement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvenement(int id, EvenementDTO evenementDTO)
    {
        if (id != evenementDTO.Id)
            return BadRequest();

        Evenement evenement = new Evenement(evenementDTO);

        // On cherche le lieu, l'animal et le personnel impliqués dans l'événement
        var lieu = _context.Lieux.Find(evenement.LieuId);
        evenement.Lieu = lieu!;
        var animal = _context.Animaux.Find(evenement.AnimalId);
        evenement.Animal = animal!;
        var personnel = _context.Personnels.Find(evenement.PersonnelId);
        evenement.Personnel = personnel!;

        _context.Entry(evenement).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Evenements.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvenements(int id)
    {
        var evenement = await _context.Evenements.FindAsync(id);
        if (evenement == null)
            return NotFound();

        _context.Evenements.Remove(evenement);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}