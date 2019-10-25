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
    public class PresencaController : ControllerBase
    {
        GufosContext context = new GufosContext();

        [HttpGet]

        public async Task<ActionResult<List<Presenca>>> Get()
        {
            List<Presenca> listaDePresenca = await context.Presenca.Include(e => e.Evento).ToListAsync(); // expressão lambida/ usado para puxar tabelas para otras tabelas

            if(listaDePresenca == null)
            {
                return NotFound();
            }
            return listaDePresenca;
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<Presenca>> Get(int id)
        {
            Presenca presencaRetornada = await context.Presenca.Include(e => e.Evento).FirstOrDefaultAsync(p => p.PresencaId ==id);
            if (presencaRetornada == null)
            {
                return NotFound();
            }
            return presencaRetornada;
        }
        [HttpPost]

        public async Task<ActionResult<Presenca>> Post(Presenca presenca)
        {
            try //usado para tentar algo/retornar erros
            {
                await context.Presenca.AddAsync(presenca);
                await context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
            return presenca;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Presenca>> Put(int id, Presenca presenca)
        {
            if (id != presenca.PresencaId)
            {
                return BadRequest();
            }
            context.Entry(presenca).State = EntityState.Modified; //usaf esse/ pq não necessita chamar um por um para as modificações 

            try
            {
            await context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                var presencaValida = context.Presenca.FindAsync(id);
                if (presencaValida == null)
                {
                   return NotFound(); 
                }else{
                    throw;
                }
            }

            return presenca;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Presenca>> Delete(int id)
        {
            Presenca presencaRetornada = await context.Presenca.FindAsync(id);
            if (presencaRetornada == null)
            {
                return NotFound();
            }
            context.Presenca.Remove(presencaRetornada);
            await context.SaveChangesAsync();

            return presencaRetornada;
        }
    }
}