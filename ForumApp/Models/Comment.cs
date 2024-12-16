using System;
using System.Collections.Generic;

namespace ForumApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string Autor { get; set; }
        public DateTime DataCriacao { get; set; }
        public int? ComentarioId { get; set; }
        public virtual Comment ComentarioPrincipal { get; set; }
        public virtual ICollection<Comment> Respostas { get; set; }

        public Comment()
        {
            Respostas = new List<Comment>();
            DataCriacao = DateTime.Now;
        }
    }
}