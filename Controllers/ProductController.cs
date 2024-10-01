using eCommerce.Domain.Entity;
using Ecommerce.Entity;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private static List<Produto> Products = new List<Produto>
    {
        new Produto (1, "Livro - Programação C#", "Livro sobre C#", 99.90m, 3.22m, TipoProduto.LIVRO ),
        new Produto (2, "Fone de Ouvido Bluetooth", "Fone sem fio com tecnologia Bluetooth", 199.99m, 0.22m, TipoProduto.ELETRONICO)
    };

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAll()
    {
        return Ok(Products);
    }

    [HttpGet("{id}")]
    public ActionResult<Produto> GetById(int id)
    {
        var product = Products.Find(p => p.Id == id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public ActionResult Add(Produto product)
    {
        Products.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
