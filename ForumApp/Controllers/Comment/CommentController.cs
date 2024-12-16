using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ForumApp.Controllers
{
    public class ComentariosController : Controller
    {
        private static List<Comentario> _comentarios = new List<Comentario>();
        private static int _nextId = 1;

        // Lista estática com dados
        public ComentariosController()
        {
            if (!_comentarios.Any())
            {
                // Comentário 1 com respostas
                var comentario1 = new Comentario
                {
                    Id = _nextId++,
                    Autor = "Giovane Moreira",
                    Texto = "Estou gostando da aplicação!",
                    DataCriacao = DateTime.Now.AddDays(-5)
                };
                _comentarios.Add(comentario1);

                _comentarios.Add(new Comentario
                {
                    Id = _nextId++,
                    Autor = "Stefanie Medeiros",
                    Texto = "Também estou gostando muito!",
                    DataCriacao = DateTime.Now.AddDays(-4),
                    ComentarioId = comentario1.Id
                });

                // Comentário 2 com resposta
                var comentario2 = new Comentario
                {
                    Id = _nextId++,
                    Autor = "Ana Catarina",
                    Texto = "Alguém poderia tirar dúvidas de como funciona o sistema da Zencheck?",
                    DataCriacao = DateTime.Now.AddDays(-2)
                };
                _comentarios.Add(comentario2);

                _comentarios.Add(new Comentario
                {
                    Id = _nextId++,
                    Autor = "Jorge Pedrosa",
                    Texto = "Claro! Tem alguma dúvida específica?",
                    DataCriacao = DateTime.Now.AddDays(-1),
                    ComentarioId = comentario2.Id
                });
            }
        }

        // GET: Comentarios
        public ActionResult Index()
        {
            var comentariosPrincipais = _comentarios
                .Where(c => c.ComentarioId == null)
                .OrderByDescending(c => c.DataCriacao)
                .ToList();
            return View(comentariosPrincipais);
        }

        // GET: Comentarios/Details
        public ActionResult Details(int id)
        {
            var comentario = _comentarios.FirstOrDefault(c => c.Id == id);
            if (comentario == null)
                return HttpNotFound();

            // Busca as respostas deste comentário
            var respostas = _comentarios.Where(c => c.ComentarioId == id).ToList();
            comentario.Respostas = respostas;

            return View(comentario);
        }

        // GET: Comentarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Texto,Autor")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                comentario.Id = _nextId++;
                comentario.DataCriacao = DateTime.Now;
                _comentarios.Add(comentario);
                TempData["Message"] = "Comentário criado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(comentario);
        }

        // POST: Comentarios/CreateReply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReply(int comentarioId, string texto, string autor)
        {
            if (!string.IsNullOrEmpty(texto) && !string.IsNullOrEmpty(autor))
            {
                var resposta = new Comentario
                {
                    Id = _nextId++,
                    Texto = texto,
                    Autor = autor,
                    DataCriacao = DateTime.Now,
                    ComentarioId = comentarioId
                };
                _comentarios.Add(resposta);
                TempData["Message"] = "Resposta adicionada com sucesso!";
            }
            return RedirectToAction("Details", new { id = comentarioId });
        }

        // GET: Comentarios/Edit
        public ActionResult Edit(int id)
        {
            var comentario = _comentarios.FirstOrDefault(c => c.Id == id);
            if (comentario == null)
                return HttpNotFound();
            return View(comentario);
        }

        // POST: Comentarios/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Texto")] Comentario comentarioAtualizado)
        {
            var comentario = _comentarios.FirstOrDefault(c => c.Id == id);
            if (comentario == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                comentario.Texto = comentarioAtualizado.Texto;
                TempData["Message"] = "Comentário atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(comentario);
        }

        // POST: Comentarios/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var comentario = _comentarios.FirstOrDefault(c => c.Id == id);
            if (comentario != null)
            {
                var todasRespostas = _comentarios.Where(c => c.ComentarioId == id).ToList();
                foreach (var resposta in todasRespostas)
                {
                    _comentarios.Remove(resposta);
                }
                _comentarios.Remove(comentario);
                TempData["Message"] = "Comentário excluído com sucesso!";
            }
            return RedirectToAction("Index");
        }
    }
}