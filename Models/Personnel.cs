public enum Metier
{
    Vétérinaire,
    Soigneur,
    Animateur,
    Directeur
}
public class Personnel
{
    public int Id { get; set; }
    public string Nom { get; set; }=null! ;
    public string Prenom { get; set; }=null! ;
    public Metier Metier { get; set; }
    public Type Specialisation { get; set; } 
}