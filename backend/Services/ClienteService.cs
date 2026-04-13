using BoschPizza.Models;

namespace BoschPizza.Services;

public static class ClienteService
{
    static List<Cliente> Clientes { get; }
    static int nextId = 3;

    static ClienteService()
    {
        Clientes = new List<Cliente>
        {
            new Cliente { Id= 1, Name = "Ana Julya", Address = "Rua Cidade de Carapicuiba", Phone = "47991767324" },  
            new Cliente { Id= 2, Name = "Marcelinho", Address = "Rua da Cidade", Phone = "47123456789" },  
        };
    }

    public static List<Cliente> GetAll() => Clientes;


    public static Cliente? Get(int id) => Clientes.FirstOrDefault(c  => c.Id == id);

    public static void Add(Cliente cliente)
    {
        cliente.Id = nextId++;
        Clientes.Add(cliente);
    }

    public static void Delete(int id)
    {
        var cliente = Get(id);
        if (cliente is null) return;
        Clientes.Remove(cliente);
    }

    public static void Update(Cliente cliente)
    {
        var index = Clientes.FindIndex(c => c.Id == cliente.Id);
        if (index == -1) return;
        Clientes[index] = cliente;
    }
    
}