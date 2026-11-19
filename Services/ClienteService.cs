using Web2_CRUD.Data;
using Web2_CRUD.Models;

namespace Web2_CRUD.Services;

// Regras de negócio de Cliente. Não sabe nada de Console — só valida e delega ao repository.
public class ClienteService
{
    private readonly ClienteRepository _clienteRepo;
    private readonly PedidoRepository _pedidoRepo;

    public ClienteService(ClienteRepository clienteRepo, PedidoRepository pedidoRepo)
    {
        _clienteRepo = clienteRepo;
        _pedidoRepo = pedidoRepo;
    }

    public void Criar(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new ArgumentException("Email inválido.");
        _clienteRepo.Create(new Cliente { Nome = nome.Trim(), Email = email.Trim() });
    }

    public List<Cliente> ListarTodos() => _clienteRepo.ReadAll();

    public Cliente BuscarPorId(int id)
        => _clienteRepo.ReadById(id) ?? throw new InvalidOperationException("Cliente não encontrado.");

    public void Atualizar(int id, string? novoNome, string? novoEmail)
    {
        var cliente = BuscarPorId(id);
        if (!string.IsNullOrWhiteSpace(novoNome)) cliente.Nome = novoNome.Trim();
        if (!string.IsNullOrWhiteSpace(novoEmail))
        {
            if (!novoEmail.Contains('@')) throw new ArgumentException("Email inválido.");
            cliente.Email = novoEmail.Trim();
        }
        _clienteRepo.Update(cliente);
    }

    // Quantos pedidos este cliente tem — regra de negócio usada antes de excluir (cascade).
    public int ContarPedidos(int clienteId) => _pedidoRepo.ReadByClienteId(clienteId).Count;
    public decimal SomarPedidos(int clienteId) => _pedidoRepo.ReadByClienteId(clienteId).Sum(p => p.Valor);
    public void Excluir(int id)
    {
        BuscarPorId(id); // garante que existe
        //regra de negócio antes de excluir
        var valorTotalPedidos = SomarPedidos(id);
        var lim = decimal.Parse(Environment.GetEnvironmentVariable("VALOR_NAO_EXCLUSAO") ?? "1000000");
        if (valorTotalPedidos > lim)
        {
            throw new InvalidOperationException($"Não é possível excluir o cliente, pois o valor total dos pedidos ({valorTotalPedidos:C}) excede o limite permitido.");
        }

        var qtdPedidos = ContarPedidos(id);
            
            if (qtdPedidos > 0)
            {
                do{
                    Console.WriteLine($"Atenção: este cliente possui {qtdPedidos} pedido(s). Ao excluir, os pedidos também serão excluídos (cascade).");
                    Console.Write("Confirmar exclusão? (s/n): ");
                    var confirmacao = Console.ReadLine()?.Trim().ToLower();
                    if (confirmacao == "s") break;
                    if (confirmacao == "n") { Console.WriteLine("Operação cancelada."); return; }
                    Console.WriteLine("Opção inválida. Digite 's' para sim ou 'n' para não.");
                } while (true);
                
            }
        _clienteRepo.Delete(id);
    }
}
