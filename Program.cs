using Web2_CRUD.Controllers;
using Web2_CRUD.Data;
using Web2_CRUD.Services;

Database.Inicializar();

var clienteRepo = new ClienteRepository();
var pedidoRepo = new PedidoRepository();

var clienteService = new ClienteService(clienteRepo, pedidoRepo);
var pedidoService = new PedidoService(pedidoRepo, clienteRepo);

var clienteController = new ClienteController(clienteService, pedidoService);
var pedidoController = new PedidoController(pedidoService);

bool sair = false;
while (!sair)
{
    Console.WriteLine();
    Console.WriteLine("===== MENU PRINCIPAL =====");
    Console.WriteLine("1. Gerenciar Clientes");
    Console.WriteLine("2. Gerenciar Pedidos");
    Console.WriteLine("0. Sair");
    Console.Write("Escolha: ");
    switch (Console.ReadLine())
    {
        case "1": clienteController.MenuLoop(); break;
        case "2": pedidoController.MenuLoop(); break;
        case "0": sair = true; break;
        default: Console.WriteLine("Opção inválida."); break;
    }
}
