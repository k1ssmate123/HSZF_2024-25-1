using IOQ9ET_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{
    public class AppDbContext
    {
        public List<Actor> Actors { get; set; }

        public AppDbContext()
        {
            Actors = new List<Actor>();
            Actors.Add(new Actor("SEGG", 25, NationalityEnum.American, new List<Character>()));
            Actors.Add(new Actor("SEGG", 25, NationalityEnum.American, new List<Character>()));
            Actors.Add(new Actor("SEGG", 25, NationalityEnum.American, new List<Character>()));

        }
    }
}
