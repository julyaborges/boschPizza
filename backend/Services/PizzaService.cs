using BoschPizza.Models;

namespace BoschPizza.Services;

public static class PizzaService
{
    static List<Pizza> Pizzas { get; }
    static int nextId = 3;

    //Método Construtor
    static PizzaService()
    {
        Pizzas = new List<Pizza>
        {
            new Pizza { Id= 1, Name = "Calabresa", IsGlutenFree = false },  
            new Pizza { Id= 2, Name = "Vegerariana", IsGlutenFree = true },  
        };
    }

    //Busca todos os itens da lista
    public static List<Pizza> GetAll() => Pizzas;


    //Busca pizza por ID
    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p  => p.Id == id);

    //Adicionar nova pizza
    public static void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    //Delete
    public static void Delete(int id)
    {
        var pizza = Get(id);
        if (pizza is null) return;
        Pizzas.Remove(pizza);
    }

    //Update
    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if (index == -1) return;
        Pizzas[index] = pizza;
    }
    
}