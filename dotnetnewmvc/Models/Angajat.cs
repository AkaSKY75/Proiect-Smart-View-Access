using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetnewmvc.Models
{
    public class Angajat
    {
        public string email { get; set; }
        public string parola { get; set; }

        public Angajat(string email, string parola)
        {
            this.email = email;
            this.parola = parola;
        }
    }
}
