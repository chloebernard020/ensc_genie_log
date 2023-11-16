using System.ComponentModel.DataAnnotations;
public class Lieu
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;

    [Display(Name = "Type des animaux présents dans le parc : ")]
    public Type Type { get; set; }

}
