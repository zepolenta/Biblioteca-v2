using System;
using System.Diagnostics;

public class Livro
{
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoPublicacao { get; set; }

    public Livro(int id, string titulo, string autor, int anoPublicacao)
    {
        ID = id;
        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
    }
}

public class No
{
    public Livro Livro { get; set; }
    public No Esquerda { get; set; }
    public No Direita { get; set; }
    public int Altura { get; set; }

    public No(Livro livro)
    {
        Livro = livro;
        Esquerda = null;
        Direita = null;
        Altura = 1;
    }
}

public class ArvoreAVL
{
    public No raiz;
    public int NumeroRotacoes { get; private set; }

    public ArvoreAVL()
    {
        raiz = null;
        NumeroRotacoes = 0;
    }

    private int Altura(No no)
    {
        if (no == null)
            return 0;
        return no.Altura;
    }

    private int FatorBalanceamento(No no)
    {
        if (no == null)
            return 0;
        return Altura(no.Esquerda) - Altura(no.Direita);
    }

    private No RotacaoDireita(No z)
    {
        No y = z.Esquerda;
        No T3 = y.Direita;

        y.Direita = z;
        z.Esquerda = T3;

        z.Altura = 1 + Math.Max(Altura(z.Esquerda), Altura(z.Direita));
        y.Altura = 1 + Math.Max(Altura(y.Esquerda), Altura(y.Direita));

        NumeroRotacoes++;
        return y;
    }

    private No RotacaoEsquerda(No z)
    {
        No y = z.Direita;
        No T2 = y.Esquerda;

        y.Esquerda = z;
        z.Direita = T2;

        z.Altura = 1 + Math.Max(Altura(z.Esquerda), Altura(z.Direita));
        y.Altura = 1 + Math.Max(Altura(y.Esquerda), Altura(y.Direita));

        NumeroRotacoes++;
        return y;
    }

    public No Inserir(No no, Livro livro)
    {
        if (no == null)
            return new No(livro);

        if (livro.ID < no.Livro.ID)
            no.Esquerda = Inserir(no.Esquerda, livro);
        else
            no.Direita = Inserir(no.Direita, livro);

        no.Altura = 1 + Math.Max(Altura(no.Esquerda), Altura(no.Direita));

        int balanceamento = FatorBalanceamento(no);

        if (balanceamento > 1)
        {
            if (livro.ID < no.Esquerda.Livro.ID)
                return RotacaoDireita(no);
            else
            {
                no.Esquerda = RotacaoEsquerda(no.Esquerda);
                return RotacaoDireita(no);
            }
        }
        if (balanceamento < -1)
        {
            if (livro.ID > no.Direita.Livro.ID)
                return RotacaoEsquerda(no);
            else
            {
                no.Direita = RotacaoDireita(no.Direita);
                return RotacaoEsquerda(no);
            }
        }

        return no;
    }

    public Livro BuscarPorID(No no, int id)
    {
        if (no == null)
            return null;
        if (no.Livro.ID == id)
            return no.Livro;
        else if (id < no.Livro.ID)
            return BuscarPorID(no.Esquerda, id);
        else
            return BuscarPorID(no.Direita, id);
    }

    public void ListarEmOrdem(No no)
    {
        if (no != null)
        {
            ListarEmOrdem(no.Esquerda);
            Console.WriteLine($"ID: {no.Livro.ID}, Título: {no.Livro.Titulo}, Autor: {no.Livro.Autor}, Ano: {no.Livro.AnoPublicacao}");
            ListarEmOrdem(no.Direita);
        }
    }

    public No Remover(No no, int id)
    {
        if (no == null)
            return no;

        if (id < no.Livro.ID)
            no.Esquerda = Remover(no.Esquerda, id);
        else if (id > no.Livro.ID)
            no.Direita = Remover(no.Direita, id);
        else
        {
            if (no.Esquerda == null || no.Direita == null)
            {
                No temp = null;
                if (temp == no.Esquerda)
                    temp = no.Direita;
                else
                    temp = no.Esquerda;

                if (temp == null)
                {
                    temp = no;
                    no = null;
                }
                else
                    no = temp;
            }
            else
            {
                No temp = MinimoValorNo(no.Direita);
                no.Livro = temp.Livro;
                no.Direita = Remover(no.Direita, temp.Livro.ID);
            }
        }

        if (no == null)
            return no;

        no.Altura = 1 + Math.Max(Altura(no.Esquerda), Altura(no.Direita));

        int balanceamento = FatorBalanceamento(no);

        if (balanceamento > 1)
        {
            if (FatorBalanceamento(no.Esquerda) >= 0)
                return RotacaoDireita(no);
            else
            {
                no.Esquerda = RotacaoEsquerda(no.Esquerda);
                return RotacaoDireita(no);
            }
        }
        if (balanceamento < -1)
        {
            if (FatorBalanceamento(no.Direita) <= 0)
                return RotacaoEsquerda(no);
            else
            {
                no.Direita = RotacaoDireita(no.Direita);
                return RotacaoEsquerda(no);
            }
        }

        return no;
    }

    private No MinimoValorNo(No no)
    {
        No atual = no;
        while (atual.Esquerda != null)
            atual = atual.Esquerda;
        return atual;
    }
}

public class SistemaBiblioteca
{
    private ArvoreAVL arvoreAVL;
    private Stopwatch stopwatch;

    public SistemaBiblioteca()
    {
        arvoreAVL = new ArvoreAVL();
        stopwatch = new Stopwatch();
    }

    public void Menu()
    {
        Console.WriteLine("Sistema de Gerenciamento de Biblioteca");
        Console.WriteLine("1. Inserir novo livro");
        Console.WriteLine("2. Buscar livro por ID");
        Console.WriteLine("3. Listar todos os livros");
        Console.WriteLine("4. Busca avançada");
        Console.WriteLine("5. Remover livro");
        Console.WriteLine("6. Mostrar estatísticas");
        Console.WriteLine("7. Sair");
    }

    public void InserirLivro()
    {
        Console.Write("Digite o ID do livro: ");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.Write("Digite o título do livro: ");
        string titulo = Console.ReadLine();
        Console.Write("Digite o autor do livro: ");
        string autor = Console.ReadLine();
        Console.Write("Digite o ano de publicação do livro: ");
        int ano = Convert.ToInt32(Console.ReadLine());
        Livro novoLivro = new Livro(id, titulo, autor, ano);

        stopwatch.Restart();
        arvoreAVL.raiz = arvoreAVL.Inserir(arvoreAVL.raiz, novoLivro);
        stopwatch.Stop();

        Console.WriteLine("Livro inserido com sucesso.");
    }

    public void BuscarPorID()
    {
        Console.Write("Digite o ID do livro: ");
        int id = Convert.ToInt32(Console.ReadLine());

        stopwatch.Restart();
        Livro livro = arvoreAVL.BuscarPorID(arvoreAVL.raiz, id);
        stopwatch.Stop();

        if (livro != null)
            Console.WriteLine($"ID: {livro.ID}, Título: {livro.Titulo}, Autor: {livro.Autor}, Ano: {livro.AnoPublicacao}");
        else
            Console.WriteLine("Livro não encontrado.");
    }

    public void ListarTodosOsLivros()
    {
        arvoreAVL.ListarEmOrdem(arvoreAVL.raiz);
    }

    public void BuscaAvancada()
    {
        Console.WriteLine("Opções de busca avançada:");
        Console.WriteLine("1. Filtrar por ano de publicação");
        Console.WriteLine("2. Filtrar por autor");
        Console.Write("Escolha uma opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                Console.Write("Digite o ano de publicação desejado: ");
                int ano = Convert.ToInt32(Console.ReadLine());
                FiltrarPorAnoDePublicacao(ano);
                break;
            case "2":
                Console.Write("Digite o nome do autor: ");
                string autor = Console.ReadLine();
                FiltrarPorAutor(autor);
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    public void FiltrarPorAnoDePublicacao(int ano)
    {
        // Implemente a lógica para filtrar os livros por ano de publicação
    }

    public void FiltrarPorAutor(string autor)
    {
        // Implemente a lógica para filtrar os livros por autor
    }

    public void RemoverLivro()
    {
        Console.Write("Digite o ID do livro a ser removido: ");
        int id = Convert.ToInt32(Console.ReadLine());
        arvoreAVL.raiz = arvoreAVL.Remover(arvoreAVL.raiz, id);
        Console.WriteLine("Livro removido com sucesso.");
    }

    public void MostrarEstatisticas()
    {
        Console.WriteLine($"Número de rotações realizadas: {arvoreAVL.NumeroRotacoes}");
        Console.WriteLine($"Tempo médio de busca: {stopwatch.ElapsedMilliseconds} milissegundos");
    }

    public void Executar()
    {
        while (true)
        {
            Menu();
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    InserirLivro();
                    break;
                case "2":
                    BuscarPorID();
                    break;
                case "3":
                    ListarTodosOsLivros();
                    break;
                case "4":
                    BuscaAvancada();
                    break;
                case "5":
                    RemoverLivro();
                    break;
                case "6":
                    MostrarEstatisticas();
                    break;
                case "7":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                    break;
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        SistemaBiblioteca sistema = new SistemaBiblioteca();
        sistema.Executar();
    }
}
