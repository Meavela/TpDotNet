namespace App.SousTypes.Models.Others
{
    [JsonSousTypes(typeof(Fourchette), "Fourchette")]
    [JsonSousTypes(typeof(Couteau), "Couteau")]
    [JsonSousTypes(typeof(Cuillere), "Cuillere")]
    public class Couvert
    {
        public virtual string Type => "Couvert";

        public string Name { get; set; }
    }
}
