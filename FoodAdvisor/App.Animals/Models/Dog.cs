namespace App.SousTypes.Models
{
    public class Dog : Animal
    {
        public override string Type => "Dog";

        public string Breed { get; set; }
    }
}
