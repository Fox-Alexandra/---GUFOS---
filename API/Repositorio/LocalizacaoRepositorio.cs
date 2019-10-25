using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interface;
using GUFOS.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositorio
{
    public class LocalizacaoRepositorio : ILocalizacaoRepositorio
    {
        GufosContext context = new GufosContext();
        public async Task<Localizacao> Delete(Localizacao localizacaoRetornada)
        {
            
            context.Localizacao.Remove(localizacaoRetornada);
            await context.SaveChangesAsync();

            return localizacaoRetornada;
        }

        public async Task<List<Localizacao>> Get()
        {
            List<Localizacao> listaDeLocalizacao = await context.Localizacao.ToListAsync();
            return listaDeLocalizacao;
        }

        public Task<Localizacao> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Localizacao> Post(Localizacao localizacao)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Localizacao> Put(Localizacao localizacao)
        {
            context.Entry(localizacao).State = EntityState.Modified; //usa esse pq não necessita chamar um por um para as modificações
            await context.SaveChangesAsync();
            return localizacao;
        }
    }
}