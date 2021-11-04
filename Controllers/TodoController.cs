using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginacaoProject.Data;
using PaginacaoProject.Models;

namespace PaginacaoProject.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route("load")]
        public async Task<IActionResult> LoadAsync([FromServices] AppDbContext context)
        {
            for (var i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = $"Tarefa {i}"
                };
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet]
        [Route("skip/{skip:int}/take/{take:int}")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context, 
            [FromRoute] int skip = 0, 
            [FromRoute] int take = 25 )
        {
            var totalCount = await context.Todos.CountAsync();
            var todos = await context
                .Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                skip,
                take,
                //daria pra fazer o calculo de numero de paginas e informar o front a pagina atual a quantidade de pagina
                data = todos
            });
        }
    }
}