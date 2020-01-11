using System;

namespace App.SousTypes.Models
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
