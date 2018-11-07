using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Unisinos.Spotify.WebApi.Models;
using Raven.Client.Documents;
using System.Text;

namespace Unisinos.Spotify.WebApi.Controllers
{
    [Route("api/musicas")]
    public class MusicaController : Controller
    {
        [HttpPost]
        public ActionResult Post([FromBody] Musica musica)
        {
            if (musica == null)
            {
                return BadRequest("Música não informada.");
            }
            string id = "";
            using (var ds = new DocumentStore { Urls = new string[] { "http://localhost:9081/" } }.Initialize())
            {
                using (var session = ds.OpenSession("db_unify"))
                {

                    session.Store(musica);
                    session.SaveChanges();
                    id = session.Advanced.GetDocumentId(musica);
                }
            }

            return Ok("Música salva! " + id);
        }

        // GET api/musicas/5
        [HttpGet("{id}")]
        public Musica Get(string id)
        {
            using (var ds = new DocumentStore { Urls = new string[] { "http://localhost:9081/" } }.Initialize())
            {
                using (var session = ds.OpenSession("db_unify"))
                {
                    return session.Load<Musica>("musicas/" + id);
                }
            }
        }

        // GET api/musicas
        [HttpGet]
        public IList Get()
        {
            IList musicas = new List<Musica>();

            using (var ds = new DocumentStore { Urls = new string[] { "http://localhost:9081/" } }.Initialize())
            {
                using (var session = ds.OpenSession("db_unify"))
                {
                    musicas = (from musica in session.Query<Musica>()
                             select musica).ToList();
                }
            }
            return musicas;
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]Musica musica)
        {
            using (var ds = new DocumentStore { Urls = new string[] { "http://localhost:9081/" } }.Initialize())
            {
                using (var session = ds.OpenSession("db_unify"))
                {
                    var musicaCarregada = session.Load<Musica>("musicas/" + id);
                    if(musicaCarregada == null)
                        return BadRequest("Música não existe!");

                    musicaCarregada.Atualizar(musica);
                    session.Store(musicaCarregada);
                    session.SaveChanges();
                }
            }
            return Ok(musica);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            using (var ds = new DocumentStore { Urls = new string[] { "http://localhost:9081/" } }.Initialize())
            {
                using (var session = ds.OpenSession("db_unify"))
                {
                    var musica = session.Load<Musica>("musicas/" + id);

                    if (musica != null)
                    {
                        session.Delete(musica);
                        session.SaveChanges();
                    }
                }
            }
        }
    }
}
