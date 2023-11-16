using System.ComponentModel.DataAnnotations;

public class Evenement
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;

    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public int LieuId { get; set; }
    [Display(Name = "Lieu de la représentation :")]
    public Lieu Lieu { get; set; } = null!;
    public int AnimalId { get; set; }
    [Display(Name = "Animal concerné :")]
    public Animal Animal { get; set; } = null!;

    public int PersonnelId { get; set; }
    [Display(Name = "Personnel en charge :")]
    public Personnel Personnel { get; set; } = null!;
    public Evenement() { }

    // Copy constructor
    public Evenement(EvenementDTO dto)
    {
        // Copy DTO field values
        Id = dto.Id;
        Nom = dto.Nom;
        Description = dto.Description;
        Date = dto.Date;
        LieuId = dto.LieuId;
        AnimalId = dto.AnimalId;
        PersonnelId = dto.PersonnelId;
    }
}