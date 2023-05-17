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

        // Verifica o e-mail a partir de uma expressão Regex
        bool verificarEmail(string email) {
            Regex regex = new Regex(@"^\w+([\.\-_]\w+)*@\w+(\.\w+)+$");

            if(regex.Match(email).Success)
                return true;
            return false;
        }

        bool verificarTelefone(string telefone) {
            Regex regex = new Regex(@"^(\+[0-9]+ )?(\([0-9]+\))*[0-9-]+$");

            if(regex.Match(telefone).Success)
                return true;
            return false;
        }

        //Itera por todos os clientes da lista, e retorna true caso exista um cliente que possua esse id
        bool verificarId(int id, List<Cliente> listaClientes) {
            foreach(Cliente cliente in listaClientes) {
                if(cliente.getId().Equals(id)) {
                    return true;
                }
            }
            return false;
        }

        //Insere um novo Cliente ao final da lista de clientes.
        bool inserirCliente(List<Cliente> listaClientes) {
            string[] vetorAtributos = {"NOME", "EMAIL", "ENDERECO", "TELEFONE"};
            string[] vetorInputs = new string[4];
            int id;

            Console.Clear();
            Console.WriteLine("--INSIRA '/' PARA INTERROMPER O PROCESSO\n");

            //Recebe um input do usuário para cada um dos atributos, ou seja, para NOME, EMAIL, ENDERECO e TELEFONE, e os armazena em um vetor
            for(int i = 0;i < 4;i++) {
                Console.WriteLine("Insira o " + vetorAtributos[i] + " do cliente:");

                vetorInputs[i] = Console.ReadLine();

                //Caso o input seja '/', interrompe a operação
                if("/".Equals(vetorInputs[i]))
                    return false;

                //Caso o input seja vazio ou nulo, seja um e-mail inválido, ou seja um telefone inválido, repete a inserção daquele valor
                if(vetorInputs[i].Equals("") || vetorInputs[i].Equals(null)) {
                    Console.WriteLine("--INPUT INVÁLIDO--");
                    i--;
                    continue;
                }
                if((i == 1 && !verificarEmail(vetorInputs[i]))) {
                    Console.WriteLine("--INPUT INVÁLIDO--");
                    i--;
                    continue;
                }
                if((i == 3 && !verificarTelefone(vetorInputs[i]))) {
                    Console.WriteLine("--INPUT INVÁLIDO--");
                    i--;
                    continue;
                }
            }

            //Se não houver clientes na lista, o id do novo cliente será 0. Caso contrário, será o id do último cliente da lista + 1.
            if(listaClientes.Count() < 1)
                id = 0;
            else
                id = listaClientes.ElementAt(listaClientes.Count() - 1).getId() + 1;

            //Gera um novo cliente com os atributos inseridos, e o insere ao final da lista
            Cliente newCliente = new Cliente(id, vetorInputs[0], vetorInputs[1], vetorInputs[2], vetorInputs[3]);
            listaClientes.Add(newCliente);
            return true;
        }

        //Edita um atributo de um elemento da lista de clientes.
        bool editarCliente(List<Cliente> listaClientes) {
            int id = 0;
            int selecaoAtributo = 0;
            bool insercaoEmAndamento;
            string input = "";

            Console.Clear();
            Console.WriteLine("--INSIRA '/' PARA INTERROMPER O PROCESSO\n");

            //Loop para receber um id, encerra ao receber um id válido
            insercaoEmAndamento = true;
            while(insercaoEmAndamento) {
                Console.WriteLine("Insira o ID do usuário:");
                input = Console.ReadLine();

                //Caso o input seja '/', interrompe a operação
                if("/".Equals(input))
                    return false;
                
                //Tenta converter o input em um id válido
                try {
                    id = Convert.ToInt32(input);
                    if(verificarId(id, listaClientes))
                        insercaoEmAndamento = false; //Caso seja válido e não ocorram erros, continua a operação
                } catch(Exception e) {
                } finally {
                    if(insercaoEmAndamento)
                        Console.WriteLine("--INSERÇÃO INVÁLIDA--");
                }
            }

            //Loop para receber um atributo, encerra ao receber um atributo válido
            insercaoEmAndamento = true;
            while(insercaoEmAndamento) {
                Console.WriteLine(
                    "Insira o atributo a ser editado:\n" +
                    "1- NOME\n" +
                    "2- EMAIL\n" +
                    "3- ENDERECO\n" +
                    "4- TELEFONE\n"
                );
                input = Console.ReadLine();

                //Caso o input seja '/', interrompe a operação
                if("/".Equals(input))
                    return false;
                
                //Tenta converter o input em um atributo válido
                try {
                    selecaoAtributo = Convert.ToInt32(input);
                    if(selecaoAtributo > 0 && selecaoAtributo < 5)
                        insercaoEmAndamento = false; //Caso seja válido e não ocorram erros, continua a operação
                } catch(Exception e) {
                }
                finally {
                    if(insercaoEmAndamento)
                        Console.WriteLine("--INSERÇÃO INVÁLIDA--");
                }
            }

            //Loop para receber a nova edição do atributo, encerra ao receber um input válido
            insercaoEmAndamento = true;
            while(insercaoEmAndamento) {
                Console.WriteLine("Insira a nova informação.");
                input = Console.ReadLine(); //Recebe um input

                //Caso o input seja '/', interrompe a operação
                if("/".Equals(input))
                    return false;

                if(!input.Equals("") & !input.Equals(null)) { //Caso a nova informação não seja vazia ou null, prossegue
                    switch(selecaoAtributo) {
                        case 2:
                            if(verificarEmail(input)) //Caso o email seja verificado, prossegue
                                insercaoEmAndamento = false;
                            break;
                        case 4:
                            if(verificarTelefone(input)) //Caso o email seja verificado, prossegue
                                insercaoEmAndamento = false;
                            break;
                        default:
                            insercaoEmAndamento = false; //Caso não necessite de verificação (nome, endereço), prossegue
                            break;
                    }
                    
                }

                if(insercaoEmAndamento)
                    Console.WriteLine("--INFORMAÇÃO INVÁLIDA--");
            }

            //Edita o atributo relevante
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

            return true;
        }

        //Deleta o cliente que possua o id inserido da lista de clientes
        bool deletarCliente(List<Cliente> listaClientes) {
            string input = "";
            int id = -1;
            bool insercaoEmAndamento;
            Cliente clienteDeletar = new Cliente();

            Console.Clear();
            Console.WriteLine("--INSIRA '/' PARA INTERROMPER O PROCESSO\n");

            //Loop para receber um id, encerra ao receber um id válido
            insercaoEmAndamento = true;
            while(insercaoEmAndamento) {
                Console.WriteLine("Insira o ID do usuário:");
                input = Console.ReadLine();

                //Caso o input seja '/', interrompe a operação
                if("/".Equals(input))
                    return false;
                
                //Tenta converter o input em um id válido
                try {
                    id = Convert.ToInt32(input);
                    if(verificarId(id, listaClientes))
                        insercaoEmAndamento = false; //Caso seja válido e não ocorram erros, continua a operação
                } catch(Exception e) {
                } finally {
                    if(insercaoEmAndamento)
                        Console.WriteLine("--INSERÇÃO INVÁLIDA--");
                }
            }

            clienteDeletar = listaClientes.Find(cliente => cliente.getId().Equals(id));
            listaClientes.Remove(clienteDeletar);
            return true;
        }


        public void Run(string filepath) {
            View view = new View();
            FileStream fileStream;
            StreamWriter escritorArquivo;
            List<Cliente> listaClientes;
            int inputMenu;
            bool operacaoResult = false;

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
                    inputMenu = -1;
                }

                Console.Clear();
                switch(inputMenu) {
                    case 0: //ENCERRAR APLICAÇÃO
                        mainLoopAtivo = false;
                        break;
                    
                    case 1: //CADASTRAR CLIENTE
                        operacaoResult = inserirCliente(listaClientes);
                        break;

                    case 2: //EDITAR CLIENTE
                        operacaoResult = editarCliente(listaClientes);
                        break;

                    case 3: //DELETAR CLIENTE
                        operacaoResult = deletarCliente(listaClientes);
                        break;

                    case 4: //VISUALIZAR CLIENTES
                        Console.Clear();
                        view.visualizarClientes(listaClientes);
                        Console.ReadLine();
                        continue;

                    default: //INSERÇÃO INVÁLIDA
                        Console.WriteLine("--INSERÇÃO INVÁLIDA--");
                        Console.ReadLine();
                        continue;
                }

                if(operacaoResult) {
                    updateArquivoClientes(filepath, listaClientes);
                    view.processoConcluido();
                    Console.ReadLine();
                } else {
                    view.operacaoInterrompida();
                    Console.ReadLine();
                }
            }

            return;
        }
    }
}