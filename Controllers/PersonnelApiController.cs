using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class PersonnelApiController : ControllerBase
{
    private readonly LaReserveContext _context;

    public PersonnelApiController(LaReserveContext context)
    {
        _context = context;
    }

    // GET: api/PersonnelApi
    public async Task<ActionResult<IEnumerable<Personnel>>> GetPersonnels()
    {
        return await _context.Personnels.OrderBy(e => e.Nom).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Personnel>> GetPersonnel(int id)
    {
        var personnel = await _context.Personnels.FindAsync(id);
        if (personnel == null)
            return NotFound();
        return personnel;
    }

    [HttpGet("{id}/PeutSOccuperDe")]
    //Affiche tous les animaux dont peut s'occuper un personnel identifié par son id
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimauxParPersonnel(int id)
    {
        var personnel = await _context.Personnels.FindAsync(id);
        if (personnel == null)
        {
            return NotFound();
        }
        var animaux = await _context.Animaux.Where(m => m.Type == personnel.Specialisation).ToListAsync();
        return animaux;
    }

    [HttpGet("{id}/PeutTravailler")]
    //Affiche tous les lieux où peut travailler un personnel identifié par son id
    public async Task<ActionResult<IEnumerable<Lieu>>> GetLieuParPersonnel(int id)
    {
        var personnel = await _context.Personnels.FindAsync(id);
        if (personnel == null)
        {
            return NotFound();
        }
        var lieux = await _context.Lieux.Where(m => m.Type == personnel.Specialisation).ToListAsync();
        return lieux;
    }

    [HttpPost]
    public async Task<ActionResult<Personnel>> PostPersonnel(Personnel personnel)
    {
        _context.Personnels.Add(personnel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersonnel), new { id = personnel.Id }, personnel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersonnel(int id, Personnel personnel)
    {
        if (id != personnel.Id)
            return BadRequest();

        _context.Entry(personnel).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonnelExists(id))
                return NotFound();
            else
                throw;
        }
        return NoContent();
    }

    private bool PersonnelExists(int id)
    {
        return _context.Personnels.Any(m => m.Id == id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonnel(int id)
    {
        var personnel = await _context.Personnels.FindAsync(id);
        if (personnel == null)
            return NotFound();

        _context.Personnels.Remove(personnel);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}

