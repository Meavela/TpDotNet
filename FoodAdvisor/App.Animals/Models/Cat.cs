namespace App.Animals.Models
{
    public class Cat : Animal
    {
        public override string Type => "Cat";

        public string DoMiaou { get; set; }
    }
}
