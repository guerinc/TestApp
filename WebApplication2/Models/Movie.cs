using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string MovieName { get; set; }

        public string MovieDescription { get; set; }

        public int Score { get; set; }
    }
}
