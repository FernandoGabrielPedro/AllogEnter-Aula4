using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllogAula4
{
    class View {

        public void inputInvalido() {
            Console.WriteLine(
                "-----\n" +
                "ERRO: INSERÇÃO INVÁLIDA\n" +
                "Pressione ENTER para prosseguir.\n" +
                "-----\n"
            );
        }
        public void menuPrincipal() {
            Console.WriteLine(
                "--SISTEMA DE DADOS - CLIENTES--\n" +
                "Escolha uma ação:\n" +
                "\t0- ENCERRAR APLICAÇÃO\n" +
                "\t1- CADASTRAR CLIENTE\n" +
                "\t2- EDITAR CLIENTE\n" +
                "\t3- EXCLUIR CLIENTE\n" +
                "\t4- VISUALIZAR CLIENTES\n"
            );
        }
        public void visualizarClientes(List<Cliente> listaClientes) {
            Console.WriteLine(
                "--VISUALIZAÇÃO DE CLIENTES--\n"
            );

            foreach(Cliente cliente in listaClientes) {
                Console.WriteLine(
                    "----------\n" +
                    "ID:\t\t" + cliente.getId() + "\n" +
                    "NOME:\t\t" + cliente.getNome() + "\n" +
                    "E-MAIL:\t\t" + cliente.getEmail() + "\n" +
                    "ENDEREÇO:\t" + cliente.getEndereco() + "\n" +
                    "TELEFONE:\t" + cliente.getTelefone() + "\n" +
                    "----------\n"
                );
            }

            if(listaClientes.Count() < 1)
                Console.WriteLine(
                    "--NÃO HÁ CLIENTES PARA EXIBIR--\n"
                );

            Console.WriteLine(
                "--PRESSIONE ENTER PARA CONTINUAR--"
            );
        }
    }
}