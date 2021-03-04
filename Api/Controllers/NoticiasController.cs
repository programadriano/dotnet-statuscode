using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoticiasController : ControllerBase
    {
        private static readonly ICollection<Noticia> noticias = new Collection<Noticia>
        {
            new Noticia
            {
                Id = 1,
                Titulo = "Titulo 1"
            },
            new Noticia
            {
                Id = 2,
                Titulo = "Titulo 2"
            },
            new Noticia
            {
                Id = 3,
                Titulo = "Titulo 3"
            }
        };

        [HttpGet]
        public IActionResult Get()
        {
            var response = noticias.ToList();

            if (response == null)
                return NoContent();

            return Ok(new
            {
                success = true,
                data = response
            });
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var response = noticias.FirstOrDefault(x => x.Id == id);

            if (response == null)
                return NoContent();

            return Ok(new
            {
                success = true,
                data = response
            });
        }


        [HttpPost]
        public IActionResult Post([FromBody] Noticia request)
        {
            try
            {
                noticias.Add(new Noticia
                {
                    Id = noticias.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1,
                    Titulo = request.Titulo
                });

                return Ok("Notícia criada com sucesso!");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Noticia request)
        {
            try
            {
                var result = noticias.FirstOrDefault(x => x.Id == request.Id);

                if (result == null)
                    return NoContent();

                result.Titulo = request.Titulo;

                return CreatedAtAction("Get", new { id },
                new
                {
                    success = true,
                    data = result ?? new object()
                });

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = noticias.FirstOrDefault(x => x.Id == id);

            if (result == null)
                return NoContent();

            noticias.Remove(result);

            return Ok("Notícia deletada com sucesso!");
        }
    }
}
