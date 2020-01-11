using System;

namespace App.Animals.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class JsonSousTypes : Attribute
    {
        public Type Type;
        public string Name;

        public JsonSousTypes(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
