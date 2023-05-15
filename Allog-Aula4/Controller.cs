using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllogAula4
{
    class Controller {
        public Controller() {}

        List<Cliente> getListaClientes(string filepath) {
            StreamReader leitorArquivo = new StreamReader(filepath);
            List<Cliente> listaClientes = new List<Cliente>();
            string linha;
            string[] linhaSeparada;

            leitorArquivo.ReadLine();
            while(true) {
                linha = leitorArquivo.ReadLine();
                if(linha == null)
                    break;
                
                linhaSeparada = linha.Split(",");

                listaClientes.Add(new Cliente(Convert.ToInt32(linhaSeparada[0]), linhaSeparada[1], linhaSeparada[2], linhaSeparada[3], linhaSeparada[4]));
            }

            leitorArquivo.Close();
            return listaClientes;
        }

        void updateArquivoClientes(string filepath, List<Cliente> listaClientes) {
            FileStream fileStream = new FileStream(filepath, FileMode.Truncate);
            fileStream.Close();

            StreamWriter escritorArquivo = new StreamWriter(filepath);

            listaClientes.OrderBy(cliente => cliente.getId());

            escritorArquivo.WriteLine("ID,NOME,EMAIL,ENDERECO,TELEFONE");
            foreach(Cliente cliente in listaClientes) {
                escritorArquivo.WriteLine(cliente.getId() + "," + cliente.getNome() + "," + cliente.getEmail() + "," + cliente.getEndereco() + "," + cliente.getTelefone());
            }

            escritorArquivo.Close();
        }

        // O e-mail será válido caso contenha um apenas um '@' e pelo menos um '.', seu primeiro caractere não seja '@' e '.', não contenha caracteres especiais, e possua tamanho maior ou igual a 10
        bool verificarEmail(string email) {
            string caracteresInvalidos = "!#$%¨&*()-=+{}[]/?ºª§¹²³£¢¬^~:;,<>ç";

            if(email.StartsWith('@'))
                return false;
            int arrobaContador = 0;
            foreach(char c in email)
                if(c.Equals('@'))
                    arrobaContador += 1;
            if(arrobaContador != 1)
                return false;
            if(!email.Contains("."))
                return false;
            if(email.StartsWith('.'))
                return false;
            if(email.Count() < 10)
                return false;
            foreach(char c in caracteresInvalidos)
                if(email.Contains(c))
                    return false;
            
            return true;
        }

        void inserirCliente(List<Cliente> listaClientes) {
            string[] vetorAtributos = {"NOME", "EMAIL", "ENDERECO", "TELEFONE"};
            string[] vetorInputs = new string[4];
            int id;

            Console.Clear();

            for(int i = 0;i < 4;i++) {
                Console.WriteLine("Insira o " + vetorAtributos[i] + " do cliente:");
                vetorInputs[i] = Console.ReadLine();
                if(vetorInputs[i].Equals("") || vetorInputs[i].Equals(null))
                    throw new Exception();
                if(i == 1)
                    if(!verificarEmail(vetorInputs[i]))
                        throw new Exception();
            }

            if(listaClientes.Count() < 1)
                id = 0;
            else
                id = listaClientes.ElementAt(listaClientes.Count() - 1).getId() + 1;

            Cliente newCliente = new Cliente(id, vetorInputs[0], vetorInputs[1], vetorInputs[2], vetorInputs[3]);
            listaClientes.Add(newCliente);
        }

        void editarCliente(List<Cliente> listaClientes) {
            int id;
            int selecaoAtributo;
            string input;

            Console.WriteLine("Insira o ID do usuário:");
            id = Convert.ToInt32(Console.ReadLine());

            bool idExistente = false;
            foreach(Cliente cliente in listaClientes)
                if(cliente.getId().Equals(id))
                    idExistente = true;
            if(!idExistente)
                throw new Exception();

            Console.WriteLine(
                "Insira o atributo a ser editado:\n" +
                "1- NOME\n" +
                "2- EMAIL\n" +
                "3- ENDERECO\n" +
                "4- TELEFONE\n"
            );
            selecaoAtributo = Convert.ToInt32(Console.ReadLine());

            while(true) {
                Console.WriteLine("Insira uma nova informação.");
                input = Console.ReadLine();

                if(selecaoAtributo != 2)
                    break;
                if(!verificarEmail(input))
                    break;
                if(!input.Equals(""))
                    break;

                Console.WriteLine("--INFORMAÇÃO INVÁLIDA--");
            }

            switch(selecaoAtributo) {
                case 1:
                    listaClientes.ElementAt(id).setNome(input);
                    break;
                case 2:
                    listaClientes.ElementAt(id).setEmail(input);
                    break;
                case 3:
                    listaClientes.ElementAt(id).setEndereco(input);
                    break;
                case 4:
                    listaClientes.ElementAt(id).setTelefone(input);
                    break;
            }
        }

        void deletarCliente(List<Cliente> listaClientes) {
            int id;
            Cliente clienteDeletar = new Cliente(-1,"","","","");

            Console.WriteLine("Insira o ID do usuário:");
            id = Convert.ToInt32(Console.ReadLine());

            bool idExistente = false;
            foreach(Cliente cliente in listaClientes) {
                if(cliente.getId().Equals(id)) {
                    idExistente = true;
                    clienteDeletar = cliente;
                }
            }
            if(!idExistente)
                throw new Exception();
            
            listaClientes.Remove(clienteDeletar);
        }


        public void Run(string filepath) {
            View view = new View();
            FileStream fileStream;
            StreamWriter escritorArquivo;
            List<Cliente> listaClientes;
            int inputMenu;

            //Verifica a existência do arquivo. Caso o arquivo não exista, ele é criado.
            if(!File.Exists(filepath)) {
                fileStream = new FileStream(filepath, FileMode.OpenOrCreate);
                fileStream.Close();
                escritorArquivo = new StreamWriter(filepath);
                escritorArquivo.WriteLine("ID,NOME,EMAIL,ENDERECO,TELEFONE");
                escritorArquivo.Close();
            }
            
            try {
                listaClientes = getListaClientes(filepath);
            } catch(Exception e) {
                Console.WriteLine("Não foi possível abrir o arquivo. Finalizando aplicação.");
                Console.ReadLine();
                return;
            }

            bool mainLoopAtivo = true;
            while(mainLoopAtivo) {
                Console.Clear();
                view.menuPrincipal();
                
                try {
                    inputMenu = Convert.ToInt32(Console.ReadLine());
                } catch(Exception e) {
                    view.inputInvalido();
                    Console.ReadLine();
                    continue;
                }

                switch(inputMenu) {
                    case 0: //ENCERRAR APLICAÇÃO
                        mainLoopAtivo = false;
                        break;
                    
                    case 1: //CADASTRAR CLIENTE
                        Console.Clear();
                        try {
                            inserirCliente(listaClientes);
                            updateArquivoClientes(filepath, listaClientes);
                            view.processoConcluido();
                            Console.ReadLine();
                        } catch(Exception e) {
                            view.inputInvalido();
                            Console.ReadLine();
                            continue;
                        }
                        break;

                    case 2: //EDITAR CLIENTE
                        try {
                            editarCliente(listaClientes);
                            updateArquivoClientes(filepath, listaClientes);
                            view.processoConcluido();
                            Console.ReadLine();
                        } catch(Exception e) {
                            view.inputInvalido();
                            Console.ReadLine();
                            continue;
                        }
                        break;

                    case 3: //DELETAR CLIENTE
                        try {
                            deletarCliente(listaClientes);
                            updateArquivoClientes(filepath, listaClientes);
                            view.processoConcluido();
                            Console.ReadLine();
                        } catch(Exception e) {
                            view.inputInvalido();
                            Console.ReadLine();
                            continue;
                        }
                        break;

                    case 4: //VISUALIZAR CLIENTES
                        Console.Clear();
                        view.visualizarClientes(listaClientes);
                        Console.ReadLine();
                        break;
                }
            }

            return;
        }
    }
}