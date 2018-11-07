using System;

namespace Unisinos.Spotify.WebApi.Models
{
    public class Musica
    {
        public string Nome { get; set; }
        public double Duracao { get; set; }
        public Artista Artista { get; set; }

        public void Atualizar(Musica musica)
        {
            this.Nome = musica.Nome;
            this.Duracao = musica.Duracao;
            this.Artista = musica.Artista;
        }
    }
}