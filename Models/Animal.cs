public class Animal
{
    public int Id { get; set; }
    public string Nom { get; set; } =null! ;
    public int Age { get; set; } 
    public string Espece { get; set; } =null! ;
    public Type Type { get; set; } 
    public int LieuId {get;set;}
    public Lieu Lieu { get; set; }=null! ;
    public Animal() { }

    // Copy constructor
    public Animal(AnimalDTO dto)
    {
        // Copy DTO field values
        Id = dto.Id;
        Nom = dto.Nom;
        Age = dto.Age;
        Espece = dto.Espece;
        Type = dto.Type;
        LieuId = dto.LieuId;
    }
}
