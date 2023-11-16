public class EvenementDTO
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;

    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public int LieuId { get; set; }
    public int AnimalId { get; set; }
    public int PersonnelId { get; set; }

}