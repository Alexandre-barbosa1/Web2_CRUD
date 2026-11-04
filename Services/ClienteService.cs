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

    public void Excluir(int id)
    {
        BuscarPorId(id); // garante que existe
        _clienteRepo.Delete(id);
    }
}
