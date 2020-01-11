namespace App.Animals.Models.Others
{
    public class Couteau : Couvert
    {
        public override string Type => "Couteau";

        public bool AvecDents { get; set; }
    }
}
