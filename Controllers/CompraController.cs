using Ecommerce.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class CompraController : ControllerBase
{
    [HttpPost]
    public ActionResult Post(Product product)
    {
        
        return CreatedAtAction("", new { id = product.Id }, product);
    }
}