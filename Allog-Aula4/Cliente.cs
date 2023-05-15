using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllogAula4
{
    class Cliente {
        private int id;
        public int getId() {return this.id;}
        public void setId(int id){this.id = id;}

        private string nome;
        public string getNome() {return this.nome;}
        public void setNome(string nome){this.nome = nome;}

        private string email;
        public string getEmail() {return this.email;}
        public void setEmail(string email){this.email = email;}

        private string endereco;
        public string getEndereco() {return this.endereco;}
        public void setEndereco(string endereco){this.endereco = endereco;}

        private string telefone;
        public string getTelefone() {return this.telefone;}
        public void setTelefone(string telefone){this.telefone = telefone;}

        public Cliente() {}
        public Cliente(int id, string nome, string email, string endereco, string telefone) {
            this.id = id;
            this.nome = nome;
            this.email = email;
            this.endereco = endereco;
            this.telefone = telefone;
        }
    }
}