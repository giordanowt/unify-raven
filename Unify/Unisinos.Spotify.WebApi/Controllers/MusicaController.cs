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

            return Ok("Música salva! id = " + id);
        }

        // GET api/values/5
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
    }
}
