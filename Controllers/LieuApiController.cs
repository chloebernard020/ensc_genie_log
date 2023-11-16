using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class LieuApiController : ControllerBase
{
    private readonly LaReserveContext _context;

    public LieuApiController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: api/LieuApi
    public async Task<ActionResult<IEnumerable<Lieu>>> GetLieux()
    {
        return await _context.Lieux.OrderBy(e => e.Nom).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lieu>> GetLieu(int id)
    {
        var lieu = await _context.Lieux.FindAsync(id);
        if (lieu == null)
            return NotFound();
        return lieu;
    }

    [HttpGet("avecanimaux")]
    public async Task<ActionResult<IEnumerable<Lieu>>> GetLieuAvecAnimaux()
    {
        var lieux = await _context.Lieux.OrderBy(e => e.Nom).ToListAsync();
        var lieuxavecanimaux = lieux;
        var nbanimaux = 0;
        int i = 1;
        while (i < lieux.Count)
        {
            var lieuavecanimal = await _context.Animaux.Where(s => s.LieuId == lieux[i].Id)
            .ToListAsync();
            nbanimaux = lieuavecanimal.Count;
            if (nbanimaux == 0)
            {
                lieuxavecanimaux.Remove(lieux[i]);

            }
            else
            {
                i = i + 1;
            }
        }
        if (lieuxavecanimaux.Count == 0)
            return NotFound();
        return lieuxavecanimaux;
    }

    [HttpPost]
    public async Task<ActionResult<Lieu>> PostLieu(Lieu lieu)
    {
        _context.Lieux.Add(lieu);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLieu), new { id = lieu.Id }, lieu);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLieu(int id, Lieu lieu)
    {
        if (id != lieu.Id)
            return BadRequest();

        _context.Entry(lieu).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LieuExists(id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    private bool LieuExists(int id)
    {
        return _context.Lieux.Any(m => m.Id == id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLieu(int id)
    {
        var lieu = await _context.Lieux.FindAsync(id);
        if (lieu == null)
            return NotFound();

        _context.Lieux.Remove(lieu);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
