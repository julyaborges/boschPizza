using BoschPizza.Data;
using BoschPizza.Models;
using BoschPizza.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BoschPizza.Controllers;

//indica que essa classe é um controller de API
[ApiController]

//Definir a rota base do controller
//[controller] será substituido por "pizza" porque o nome base é PizzaController 
[Route("[controller]")]
[Authorize]
public class PizzaController : ControllerBase
{

    private readonly AppDbContext _context;

    public PizzaController(AppDbContext context)
    {
        _context = context;
    }

    // Mapeamento requisições HTTP GET para /pizza
    [HttpGet]
    //Retorna a lista completa de pizzas usando o serviço
    public async Task<ActionResult<List<Pizza>>> GetAll()
    {
         return await _context.Pizzas.ToListAsync();    
    }
    

    // Mapeamento requisições HTTP GET para /pizza/{id} com Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> Get(int id)
    {
        //Busca a pizza pelo id
        var pizza = await _context.Pizzas.FindAsync(id);

        //Se nao encontrar, retorna um erro (404 Not Found)
        if (pizza is null) return NotFound();

        //se encontrar, retorna a pizza
        return pizza;
    }

    //Mapeia requisicoes HTTP POST para /pizza
    [HttpPost]
    public async Task<IActionResult> Create(Pizza pizza)
    {
        //Adiciona a nova pizza usando o serviço
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();

        //Retorna status 201 Created
        // Também informa qual ação pode recuperar o item criado
        return CreatedAtAction(nameof(Get), new {id=pizza.Id}, pizza);
    }

    //Mapeia as requisicoes HTTP PUT para /pizza/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Pizza pizza)
    {
        //Verifica se o id da URL é diferente do Id enviado no corpo
        if (id != pizza.Id) return BadRequest();

        //verificar se a pizza existe na lista
        var existe = await _context.Pizzas.FindAsync(id);

        //Se nao existir, retorna 404 Not Found
        if (existe is null) return NotFound();

        //Atualiza a pizza
        existe.Name = pizza.Name;
        existe.IsGlutenFree = pizza.IsGlutenFree;

        await _context.SaveChangesAsync();

        //Retorna 204 NoContent, indicando qe a atualizacao foi realizada
        return NoContent();
    }

    //Mapeia requisicoes HTTP Delete para /pizza/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        //Buscar a pizza antes de excluir
        var pizza = await _context.Pizzas.FindAsync(id);

        //Se não existir, retorna 404 Not Found
        if (pizza is null) return NotFound();

        //Remove a pizza
        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
        
        //retorna 204 No Content
        return NoContent();
    }

}


