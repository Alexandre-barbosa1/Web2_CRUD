using Web2_CRUD.Services;

namespace Web2_CRUD.Controllers;

// Faz a ponte entre o Console (menu) e o ClienteService. Só I/O + tratamento de erro.
public class ClienteController
{
    private readonly ClienteService _service;
    private readonly PedidoService _pedidoService;

    public ClienteController(ClienteService service, PedidoService pedidoService)
    {
        _service = service;
        _pedidoService = pedidoService;
    }

    public void MenuLoop()
    {
        bool voltar = false;
        while (!voltar)
        {
            Console.WriteLine();
            Console.WriteLine("--- CLIENTES ---");
            Console.WriteLine("1. Criar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Buscar Cliente");
            Console.WriteLine("4. Buscar Pedidos do Cliente");
            Console.WriteLine("5. Atualizar");
            Console.WriteLine("6. Excluir");
            Console.WriteLine("0. Voltar");
            Console.Write("Escolha: ");
            switch (Console.ReadLine())
            {
                case "1": Criar(); break;
                case "2": Listar(); break;
                case "3": BuscarPorId(); break;
                case "4": BuscarPedidos(); break;
                case "5": Atualizar(); break;
                case "6": Excluir(); break;
                case "0": voltar = true; break;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }

    private void Criar()
    {
        try
        {
            Console.Write("Nome: ");
            var nome = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";
            _service.Criar(nome, email);
            Console.WriteLine("Cliente criado com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }

    private void Listar()
    {
        var clientes = _service.ListarTodos();
        if (clientes.Count == 0) { Console.WriteLine("Nenhum cliente cadastrado."); return; }
        foreach (var c in clientes)
            Console.WriteLine($"[{c.Id}] {c.Nome} - {c.Email}");
    }

    private void BuscarPorId()
    {
        try
        {
            Console.Write("Id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var cliente = _service.BuscarPorId(id);
            Console.WriteLine($"[{cliente.Id}] {cliente.Nome} - {cliente.Email}");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }
    private void BuscarPedidos()
    {
        try
        {
            Console.Write("Id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var pedidos = _pedidoService.BuscarPorClienteId(id);
            if (pedidos.Count == 0) { Console.WriteLine("Nenhum pedido encontrado para este cliente."); return; }
            foreach (var p in pedidos)
                Console.WriteLine($"[{p.Id}] {p.Produto} - R$ {p.Valor:F2} - Cliente: {p.ClienteNome} (Id {p.ClienteId})");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }

    private void Atualizar()
    {
        try
        {
            Console.Write("Id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var cliente = _service.BuscarPorId(id);

            Console.Write($"Novo nome ({cliente.Nome}): ");
            var nome = Console.ReadLine();
            Console.Write($"Novo email ({cliente.Email}): ");
            var email = Console.ReadLine();

            _service.Atualizar(id, nome, email);
            Console.WriteLine("Cliente atualizado com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }

    private void Excluir()
    {
        try
        {
            Console.Write("Id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }

            var qtdPedidos = _pedidoService.ListarTodos().Count(p => p.ClienteId == id);
            if (qtdPedidos > 0)
            {
                Console.WriteLine($"Atenção: este cliente possui {qtdPedidos} pedido(s). Ao excluir, os pedidos também serão excluídos (cascade).");
                Console.Write("Confirmar exclusão? (s/n): ");
                if (Console.ReadLine()?.Trim().ToLower() != "s") { Console.WriteLine("Operação cancelada."); return; }
            }

            _service.Excluir(id);
            Console.WriteLine("Cliente excluído com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }
}
