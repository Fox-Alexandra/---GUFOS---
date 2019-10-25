using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS.Models;

namespace Gufos.Interface
{
    public interface ICategoriaRepositorio 
    {
        //Definindo todos os metodos que vai ter no repositorio
         Task<List<Categoria>> Get();
         Task<Categoria> Get(int id);
         Task<Categoria> Post(Categoria categoria);
         Task<Categoria> Put(Categoria categoria);
         Task<Categoria> Delete(Categoria categoriaRetornada);

    }
}