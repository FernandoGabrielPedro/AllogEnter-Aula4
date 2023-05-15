using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AllogAula4
{
    class Controller {
        public Controller() {}

        //Retorna uma lista de clientes a partir de um arquiv csv
        List<Cliente> getListaClientes(string filepath) {
            //Gera leitores/gerenciadores de arquivos e vetores auxiliares
            StreamReader leitorArquivo = new StreamReader(filepath);
            List<Cliente> listaClientes = new List<Cliente>();
            string linha;
            string[] linhaSeparada;

            leitorArquivo.ReadLine(); //Leitor de arquivo pula o cabeçalho

            //Para cada linha do arquivo, a separa em diferentes strings (separador = ','), a converte em um novo Cliente, e o insere na lista
            while(true) {
                linha = leitorArquivo.ReadLine();
                if(linha == null)
                    break; //Caso não houverem mais linhas, interrompe o loop
                
                linhaSeparada = linha.Split(",");

                listaClientes.Add(new Cliente(Convert.ToInt32(linhaSeparada[0]), linhaSeparada[1], linhaSeparada[2], linhaSeparada[3], linhaSeparada[4]));
            }

            //Fecha o arquivo e retorna a lista
            leitorArquivo.Close();
            return listaClientes;
        }

        //Atualiza as informações do arquivo csv a partir de uma lista de clientes
        void updateArquivoClientes(string filepath, List<Cliente> listaClientes) {
            //Usa um gerenciador de arquivos para apagar os conteúos do arquivo
            FileStream fileStream = new FileStream(filepath, FileMode.Truncate);
            fileStream.Close();

            StreamWriter escritorArquivo = new StreamWriter(filepath);

            listaClientes.OrderBy(cliente => cliente.getId()); //Ordena a lista por id

            escritorArquivo.WriteLine("ID,NOME,EMAIL,ENDERECO,TELEFONE"); //Escreve o cabeçalho novamente
            //Para cada elemento na lista, escreve uma nova linha
            foreach(Cliente cliente in listaClientes) {
                escritorArquivo.WriteLine(cliente.getId() + "," + cliente.getNome() + "," + cliente.getEmail() + "," + cliente.getEndereco() + "," + cliente.getTelefone());
            }

            escritorArquivo.Close();
        }

        // O e-mail será válido caso contenha um apenas um '@' e pelo menos um '.', seu primeiro caractere não seja '@' e '.', não contenha caracteres especiais, e possua tamanho maior ou igual a 10
        bool verificarEmail(string email) {
            /*
            string caracteresInvalidos = "!#$%¨&*()-=+{}[]/?ºª§¹²³£¢¬^~:;,<>ç";

            if(email.StartsWith('@'))
                return false;
            int arrobaContador = 0;
            foreach(char c in email)
                if(c.Equals('@'))
                    arrobaContador += 1;
            if(arrobaContador != 1)
                return false;
            if(!email.Contains('.'))
                return false;
            if(email.StartsWith('.'))
                return false;
            if(email.Count() < 10)
                return false;
            foreach(char c in caracteresInvalidos)
                if(email.Contains(Convert.ToString(c)))
                    return false;
            */
            Regex verificacaoEmail = new Regex("^(\w+)([\.\-\_]\w+)*@(\w+)(\.\w+)+");

            if(verificacaoEmail.Match(email).Success())
                return true;
            return false;
        }

        //Insere um novo Cliente ao final da lista de clientes.
        void inserirCliente(List<Cliente> listaClientes) {
            string[] vetorAtributos = {"NOME", "EMAIL", "ENDERECO", "TELEFONE"};
            string[] vetorInputs = new string[4];
            int id;

            Console.Clear();

            //Recebe um input do usuário para cada um dos atributos, ou seja, para NOME, EMAIL, ENDERECO e TELEFONE, e os armazena em um vetor
            for(int i = 0;i < 4;i++) {
                Console.WriteLine("Insira o " + vetorAtributos[i] + " do cliente:");

                vetorInputs[i] = Console.ReadLine();

                if(vetorInputs[i].Equals("") || vetorInputs[i].Equals(null))
                    throw new Exception(); //Caso o input seja vazio ou nulo, gera um erro
                if(i == 1)
                    if(!verificarEmail(vetorInputs[i]))
                        throw new Exception(); //Caso seja EMAIL, verifica se ele é válido. Caso contráio, gera um erro.
            }

            //Se não houver clientes na lista, o id do novo cliente será 0. Caso contrário, será o id do último cliente da lista + 1.
            if(listaClientes.Count() < 1)
                id = 0;
            else
                id = listaClientes.ElementAt(listaClientes.Count() - 1).getId() + 1;

            //Gera um novo cliente com os atributos inseridos, e o insere ao final da lista
            Cliente newCliente = new Cliente(id, vetorInputs[0], vetorInputs[1], vetorInputs[2], vetorInputs[3]);
            listaClientes.Add(newCliente);
        }

        //Edita um atributo de um elemento da lista de clientes.
        void editarCliente(List<Cliente> listaClientes) {
            int id;
            int selecaoAtributo;
            string input;

            //Recebe o id por input
            Console.WriteLine("Insira o ID do usuário:");
            id = Convert.ToInt32(Console.ReadLine());

            //Itera por todos os clientes da lista, e verifica se existe um cliente que possua esse id
            bool idExistente = false;
            foreach(Cliente cliente in listaClientes) {
                if(cliente.getId().Equals(id)) {
                    idExistente = true;
                    break;
                }
            }
            if(!idExistente)
                throw new Exception(); //Caso contrário, emite um erro

            Console.WriteLine(
                "Insira o atributo a ser editado:\n" +
                "1- NOME\n" +
                "2- EMAIL\n" +
                "3- ENDERECO\n" +
                "4- TELEFONE\n"
            );
            //Recebe um input para selecionar o atributo desejado
            selecaoAtributo = Convert.ToInt32(Console.ReadLine());

            while(true) {
                Console.WriteLine("Insira uma nova informação.");
                input = Console.ReadLine(); //Recebe um input

                if(!input.Equals("") & !input.Equals(null)) { //Caso o email não seja vazio ou nulo, prossegue
                    if(selecaoAtributo != 2) //Caso não for uma edição de email, prossegue
                        break;
                    if(verificarEmail(input)) //Caso o email seja verificado, prossegue
                        break;
                }

                Console.WriteLine("--INFORMAÇÃO INVÁLIDA--");
            }

            switch(selecaoAtributo) { //Edita o atributo relevante
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

        //Deleta o cliente que possua o id inserido da lista de clientes
        void deletarCliente(List<Cliente> listaClientes) {
            int id;
            Cliente clienteDeletar;

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

                    default: //INSERÇÃO INVÁLIDA
                        view.inputInvalido();
                        Console.ReadLine();
                        break;
                }
            }

            return;
        }
    }
}