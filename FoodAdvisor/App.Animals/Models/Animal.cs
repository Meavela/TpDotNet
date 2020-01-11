namespace App.Animals.Models
{
    [JsonSousTypes(typeof(Dog), "Dog")]
    [JsonSousTypes(typeof(Cat), "Cat")]
    public class Animal
    {
        public virtual string Type => "Animal";

        public string Name { get; set; }
    }
}
