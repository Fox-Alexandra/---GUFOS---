using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interface;
using GUFOS.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositorio
{
    public class EventoRepositorio : IEventoRepositorio
    {
        GufosContext context = new GufosContext();
        
        public async Task<Evento> Delete(Evento eventoRetornado) //O Task serve para deixar assincrono
        {
            context.Evento.Remove(eventoRetornado); //remove eventoRetornado da tabela Evento
            await context.SaveChangesAsync(); //Salva as alterações
            return eventoRetornado;
        }

        public async Task<List<Evento>> Get()
        {
            return await context.Evento.Include(l => l.Localizacao).Include(c => c.Categoria).ToListAsync(); // expressão lambida/ usado para puxar tabelas para otras tabelas
        }

        public async Task<Evento> Get(int id)
        {
            return await context.Evento.Include(l => l.Localizacao).Include(c => c.Categoria).FirstOrDefaultAsync(e => e.EventoId == id);// usar assim quando usar o de cima
        }

        public async Task<Evento> Post(Evento evento)
        {
            await context.Evento.AddAsync(evento);
            await context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento> Put(Evento evento)
        {
            context.Entry(evento).State = EntityState.Modified; //usa esse pq não necessita chamar um por um para as modificações
            await context.SaveChangesAsync();
            return evento;
        }
    }
}