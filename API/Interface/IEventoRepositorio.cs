using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS.Models;

namespace API.Interface
{
    public interface IEventoRepositorio
    {
        Task<List<Evento>> Get();
        Task<Evento> Get(int id);
        Task<Evento> Post(Evento evento);
        Task<Evento> Put(Evento evento);
        Task<Evento> Delete (Evento eventoRetornado);

    }
}