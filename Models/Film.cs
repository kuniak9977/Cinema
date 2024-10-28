using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class Film
    {
        private string name;
        private string description;
        private string type;

        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Type { get { return type; } set { type = value; } }

        public Film(string _name, string _desc, string _type)
        {
            this.name = _name;
            this.description = _desc;
            this.type = _type;
        }

        public Film() { }

        public override string ToString()
        {
            return $"Nazwa: {name}, Opis: {description}, Gatunek: {type}";
        }
    }
}
