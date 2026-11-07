using Web2_CRUD.Services;

namespace Web2_CRUD.Controllers;

// Faz a ponte entre o Console (menu) e o PedidoService. Só I/O + tratamento de erro.
public class PedidoController
{
    private readonly PedidoService _service;

    public PedidoController(PedidoService service)
    {
        _service = service;
    }

    public void MenuLoop()
    {
        bool voltar = false;
        while (!voltar)
        {
            Console.WriteLine();
            Console.WriteLine("--- PEDIDOS ---");
            Console.WriteLine("1. Criar");
            Console.WriteLine("2. Listar");
            Console.WriteLine("3. Buscar Pedido");
            Console.WriteLine("4. Atualizar");
            Console.WriteLine("5. Excluir");
            Console.WriteLine("0. Voltar");
            Console.Write("Escolha: ");
            switch (Console.ReadLine())
            {
                case "1": Criar(); break;
                case "2": Listar(); break;
                case "3": BuscarPorId(); break;
                case "4": Atualizar(); break;
                case "5": Excluir(); break;
                case "0": voltar = true; break;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }

    private void Criar()
    {
        try
        {
            Console.Write("Id do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int clienteId)) { Console.WriteLine("Id inválido."); return; }
            Console.Write("Produto: ");
            var produto = Console.ReadLine() ?? "";
            Console.Write("Valor: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valor)) { Console.WriteLine("Valor inválido."); return; }

            _service.Criar(clienteId, produto, valor);
            Console.WriteLine("Pedido criado com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }

    private void Listar()
    {
        var pedidos = _service.ListarTodos();
        if (pedidos.Count == 0) { Console.WriteLine("Nenhum pedido cadastrado."); return; }
        foreach (var p in pedidos)
            Console.WriteLine($"[{p.Id}] {p.Produto} - R$ {p.Valor:F2} - Cliente: {p.ClienteNome} (Id {p.ClienteId})");
    }
    private void BuscarPorId()
    {
        try
        {
            Console.Write("Id do pedido: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var pedidos =_service.BuscarPorId(id);
            Console.WriteLine($"[{pedidos.Id}] {pedidos.Produto} - R$ {pedidos.Valor:F2} - Cliente: {pedidos.ClienteNome} (Id {pedidos.ClienteId})");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }
  
    private void Atualizar()
    {
        try
        {
            Console.Write("Id do pedido: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            var pedido = _service.BuscarPorId(id);

            Console.Write($"Novo produto ({pedido.Produto}): ");
            var produto = Console.ReadLine();
            Console.Write($"Novo valor ({pedido.Valor}): ");
            var valorStr = Console.ReadLine();
            Console.Write($"Novo Id de cliente ({pedido.ClienteId}): ");
            var clienteIdStr = Console.ReadLine();

            decimal? novoValor = decimal.TryParse(valorStr, out var v) ? v : null;
            int? novoClienteId = int.TryParse(clienteIdStr, out var cid) ? cid : null;

            _service.Atualizar(id, produto, novoValor, novoClienteId);
            Console.WriteLine("Pedido atualizado com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }

    private void Excluir()
    {
        try
        {
            Console.Write("Id do pedido: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Id inválido."); return; }
            _service.Excluir(id);
            Console.WriteLine("Pedido excluído com sucesso.");
        }
        catch (Exception ex) { Console.WriteLine($"Erro: {ex.Message}"); }
    }
}
