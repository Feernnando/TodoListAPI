using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController (AppDbContext context)
        {
            _context = context;
        }

        // GET: Lista todos os produtos

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Produto>>> GetTarefas()
        {
            return await _context.Produtos.ToListAsync();
        }

        // GET: Filtra Pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var tarefa = await _context.Produtos.FindAsync(id);

            if (tarefa == null)
                return NotFound("Produto não encontrado.");

            return tarefa;
        }

        // GET : Filtra por Categoria

        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPorCategoria(string categoria)
        {
            var produtos = await _context.Produtos
                .Where(p => p.Categoria.ToLower() == categoria.ToLower())
                .ToListAsync();
            if (!produtos.Any())
                return NotFound("Nenhum produto encontrado nessa categoria.");
            
            return produtos;
        }

        //POST: Cadastra um novo produto
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        //PUT : Atualiza um produto existente

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest("ID do produto não confere.");

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete : Deleta um produto

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var tarefa = await _context.Produtos.FindAsync(id);

            if (tarefa == null)
                return NotFound("Produto não encontrado.");

            _context.Produtos.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
