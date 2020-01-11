namespace App.SousTypes.Models.Others
{
    public class Fourchette : Couvert
    {
        public override string Type => "Fourchette";

        public int NombreDeDents { get; set; }
    }
}
