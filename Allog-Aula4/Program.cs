using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllogAula4
{
    class Program {

        static void Main(string[] args) {
            string filepath = "clientes.csv";
            
            Controller programa = new Controller();
            programa.Run(filepath);
        }
    }
}