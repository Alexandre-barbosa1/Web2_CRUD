using Web2_CRUD.Data;
using Web2_CRUD.Models;

namespace Web2_CRUD.Services;

// Regras de negócio de Pedido, incluindo a validação do relacionamento com Cliente.
public class PedidoService
{
    private readonly PedidoRepository _pedidoRepo;
    private readonly ClienteRepository _clienteRepo;

    public PedidoService(PedidoRepository pedidoRepo, ClienteRepository clienteRepo)
    {
        _pedidoRepo = pedidoRepo;
        _clienteRepo = clienteRepo;
    }

    public void Criar(int clienteId, string produto, decimal valor)
    {
        if (_clienteRepo.ReadById(clienteId) == null)
            throw new ArgumentException($"Cliente com Id {clienteId} não encontrado.");
        if (string.IsNullOrWhiteSpace(produto)) throw new ArgumentException("Produto é obrigatório.");
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.");

        _pedidoRepo.Create(new Pedido { Produto = produto.Trim(), Valor = valor, ClienteId = clienteId });
    }

    public List<Pedido> ListarTodos() => _pedidoRepo.ReadAll();

    public Pedido BuscarPorId(int id)
        => _pedidoRepo.ReadById(id) ?? throw new InvalidOperationException("Pedido não encontrado.");

    public List<Pedido> BuscarPorClienteId(int clienteId)
    {
        if (_clienteRepo.ReadById(clienteId) == null)
            throw new ArgumentException($"Cliente com Id {clienteId} não encontrado.");
        return _pedidoRepo.ReadByClienteId(clienteId);
    }
    public void Atualizar(int id, string? novoProduto, decimal? novoValor, int? novoClienteId)
    {
        var pedido = BuscarPorId(id);

        if (!string.IsNullOrWhiteSpace(novoProduto)) pedido.Produto = novoProduto.Trim();
        if (novoValor.HasValue)
        {
            if (novoValor.Value <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
            pedido.Valor = novoValor.Value;
        }
        if (novoClienteId.HasValue)
        {
            if (_clienteRepo.ReadById(novoClienteId.Value) == null)
                throw new ArgumentException($"Cliente com Id {novoClienteId.Value} não encontrado.");
            pedido.ClienteId = novoClienteId.Value;
        }

        _pedidoRepo.Update(pedido);
    }

    public void Excluir(int id)
    {
        BuscarPorId(id); // garante que existe
        _pedidoRepo.Delete(id);
    }
}
