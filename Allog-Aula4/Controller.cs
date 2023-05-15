using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllogAula4
{
    class Controller {
        public Controller() {}

        List<Cliente> getListaClientes(StreamReader leitorArquivo) {
            List<Cliente> listaCliente = new List<Cliente>();
            string linha;
            string[] linhaSeparada;

            leitorArquivo.ReadLine();
            while(true) {
                linha = leitorArquivo.ReadLine();
                if(linha == null)
                    break;
                
                linhaSeparada = linha.Split(",");

                try {
                    listaCliente.Add(new Cliente(Convert.ToInt32(linhaSeparada[0]), linhaSeparada[1], linhaSeparada[2], linhaSeparada[3], linhaSeparada[4]));
                } catch(Exception e) {
                    
                }
            }

            return listaCliente;
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
            }

            if(listaClientes.Count() < 1)
                id = 0;
            else
                id = listaClientes.ElementAt(listaClientes.Count() - 1).getId() + 1;

            Cliente newCliente = new Cliente(id, vetorInputs[0], vetorInputs[1], vetorInputs[2], vetorInputs[3]);
            listaClientes.Add(newCliente);
        }

        void editarCliente(List<Cliente> listaClientes, int id) {

        }


        public void Run(string filepath) {
            View view = new View();
            FileStream fileStream;
            StreamReader leitorArquivo;
            StreamWriter escritorArquivo;
            int inputMenu;

            //Verifica a existência do arquivo. Caso o arquivo não exista, ele é criado.
            if(!File.Exists(filepath)) {
                fileStream = new FileStream(filepath, FileMode.OpenOrCreate);
                fileStream.Close();
                Thread.Sleep(1000);
                escritorArquivo = new StreamWriter(filepath);
                escritorArquivo.WriteLine("ID,NOME,EMAIL,ENDERECO,TELEFONE");
                escritorArquivo.Close();
            }
            
            //
            try {
                leitorArquivo = new StreamReader(filepath);
            } catch(Exception e) {
                Console.WriteLine("Não foi possível abrir o arquivo. Finalizando aplicação.");
                return;
            }

            List<Cliente> listaClientes = getListaClientes(leitorArquivo);
            leitorArquivo.Close();

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
                        } catch(Exception e) {
                            view.inputInvalido();
                            Console.ReadLine();
                            continue;
                        }
                        break;

                    case 2: //EDITAR CLIENTE
                        inputMenu = 1;
                        break;

                    case 3: //DELETAR CLIENTE
                        inputMenu = 1;
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