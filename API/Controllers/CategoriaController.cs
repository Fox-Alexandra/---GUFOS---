using System.Collections.Generic;
using System.Threading.Tasks;
using Gufos.Repositorio;
using GUFOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GUFOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class CategoriaController : ControllerBase
    {
        CategoriaRepositorio repositorio = new CategoriaRepositorio(); //Instancia a classe CategoriaRepositorio

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            try{
                return await repositorio.Get();
            }catch(System.Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            Categoria categoriaRetornada = await repositorio.Get(id);
            if (categoriaRetornada == null)
            {
                return NotFound();
            }
            return categoriaRetornada;
        }


        /// <summary>
        /// Insere uma categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns>Retorna a categoria cadastrada</returns>
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {
            try //usado para tentar algo/retornar erros
            //Tente o seguinte:
            {
                return await repositorio.Post(categoria); //Retorne o repositorio do tipo post que tenha a variavel cateri
            }
            catch (System.Exception) //Caso de erro, apareça a seguinte mensagem
            {
                
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Categoria>> Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId){

                return BadRequest();
            }
            try
            {
                await repositorio.Put(categoria);
            }
            catch (DbUpdateConcurrencyException) //Caso haja uma excessao e o update nao ocorra faça:
            {
                var categoriaValida = await repositorio.Get(id);
                if(categoriaValida == null){
                    return NotFound();
                }else{

                    throw;
                }
            }
            return categoria;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            Categoria categoriaRetornada = await repositorio.Get(id);
            if (categoriaRetornada == null)
            {
                return NotFound();
            }

            await repositorio.Delete(categoriaRetornada);
            return categoriaRetornada;
        }
    }
}