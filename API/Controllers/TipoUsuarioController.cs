using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GUFOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TipoUsuarioController : ControllerBase
    {
        GufosContext context = new GufosContext();

        [HttpGet]

        public async Task<ActionResult<List<TipoUsuario>>> Get()
        {
            List<TipoUsuario> listaDeTipoUsuario = await context.TipoUsuario.ToListAsync();

            if(listaDeTipoUsuario == null)
            {
                return NotFound();
            }
            return listaDeTipoUsuario;
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<TipoUsuario>> Get(int id)
        {
            TipoUsuario tipoUsuarioRetornado = await context.TipoUsuario.FindAsync(id);
            if (tipoUsuarioRetornado == null)
            {
                return NotFound();
            }
            return tipoUsuarioRetornado;
        }
        [HttpPost]

        public async Task<ActionResult<TipoUsuario>> Post(TipoUsuario tipoUsuario)
        {
            try //usado para tentar algo/retornar erros
            {
                await context.TipoUsuario.AddAsync(tipoUsuario);
                await context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return tipoUsuario;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TipoUsuario>> Put(int id, TipoUsuario tipoUsuario)
        {
            if (id != tipoUsuario.TipoUsuarioId)
            {
                return BadRequest();
            }
            context.Entry(tipoUsuario).State = EntityState.Modified; //usaf esse/ pq não necessita chamar um por um para as modificações 

            try
            {
            await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                var tipoUsuarioValido = context.TipoUsuario.FindAsync(id);
                if (tipoUsuarioValido == null)
                {
                   return NotFound(); 
                }else{
                    throw;
                }
            }

            return tipoUsuario;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoUsuario>> Delete(int id)
        {
            TipoUsuario tipoUsuarioRetornado = await context.TipoUsuario.FindAsync(id);
            if (tipoUsuarioRetornado == null)
            {
                return NotFound();
            }
            context.TipoUsuario.Remove(tipoUsuarioRetornado);
            await context.SaveChangesAsync();

            return tipoUsuarioRetornado;
        }
    }
}