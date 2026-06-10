using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudMicroservicios.Data;
using CrudMicroservicios.Models;

namespace CrudMicroservicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductosController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
            => await _context.Productos.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var p = await _context.Productos.FindAsync(id);
            return p == null ? NotFound(new { mensaje = "No encontrado." }) : p;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            if (producto.Estado != "Activo" && producto.Estado != "Inactivo")
                return BadRequest(new { mensaje = "El estado solo puede ser 'Activo' o 'Inactivo'." });

            producto.Id = 0;  // ← fuerza autoincremento
            producto.FechaCreacion = DateTime.Now;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest(new { mensaje = "El ID no coincide." });

            if (producto.Estado != "Activo" && producto.Estado != "Inactivo")
                return BadRequest(new { mensaje = "El estado solo puede ser 'Activo' o 'Inactivo'." });

            _context.Entry(producto).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var p = await _context.Productos.FindAsync(id);
            if (p == null) return NotFound(new { mensaje = "No encontrado." });
            _context.Productos.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}