# .NET Core StatusCode
 Esse repositório contem um projeto .NET 5, com alguns exemplos práticos demonstrando como retornar o StatusCode de uma Controller com os verbos RESTful.

## Motivação

Os códigos de status das respostas HTTP indicam se uma requisição *HTTP* foi corretamente concluída. 
A seguir vocês tem um lista contendo as respostas agrupadas em cinco classes:

* Respostas de informação (100-199),
* Respostas de sucesso (200-299),
* Redirecionamentos (300-399)
* Erros do cliente (400-499)
* Erros do servidor (500-599).

### Implementando os retornos 

Para validar os retornos dos verbos de uma API *RESTful* eu criei uma controller chamada: `NoticiasController` com os verbos mais utilizados no nosso dia a dia Get, GetById, Post, PUT e Delete. A seguir você tem um exemplo de cada uma delas.

```Csharp
/*
 Get verificando se tem algum registro no banco, caso não ele retorna o Status 204 No Content caso sim um 200 com os resultados 
 */

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
```

```Csharp
/*
 GetById segui o mesmo fluxo do Get verificando se tem algum registro no banco, caso não ele retorna o Status 204 No Content caso sim um 200 com os resultados 
 */

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
```

```Csharp
/*
No post nos verificamos se ocorreu algum erro, caso sim, retornamos o Status 500 com uma exception do que aconteceu, caso não, retornamos o Status 200 e uma string informando o sucesso.
 */

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
```


```Csharp
/*
No put nós temos uma verificação a mais do que nos outros fluxos, não verificamos se nada foi atualizado, se os registros são identicos, caso sim ele retorna o Status 304.
 */

 [HttpPut]
        public IActionResult Put(int id, [FromBody] Noticia request)
        {
            try
            {
                var result = noticias.FirstOrDefault(x => x.Id == request.Id);

                if (result == null)
                    return NoContent();


               if (result.Equals(request))
                    return StatusCode(304, "O conteudo esta com o mesmo valor!");


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
```


```Csharp
/*
 No Delete nós trabalhamos com os tatus 204 verificando se não existe aquele registro e 200 caso o registro exista e podemos deletar ele ou o 500 em caso de erro
 */


 [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = noticias.FirstOrDefault(x => x.Id == id);

            if (result == null)
                return NoContent();

            try
            {
                noticias.Remove(result);
                return Ok("Notícia deletada com sucesso!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
 
```
