using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumApp.Controllers
{
    public class ComentariosController : Controller
    {
        private static List<Comentario> _comentarios = new List<Comentario>();
        private static int _nextId = 1;

        // GET: Comentarios
        public ActionResult Index()
        {
            var comentariosPrincipais = _comentarios.Where(c => c.ComentarioId == null).ToList();
            return View(comentariosPrincipais);
        }

        // GET: Comentarios/Details
        public ActionResult Details(int id)
        {
            var comentario = _comentarios.FirstOrDefault(c => c.Id == id);
            if (comentario == null)
                return HttpNotFound();

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
                return RedirectToAction("Index");
            }
            return View(comentario);
        }

        // POST: Comentarios/CreateReply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReply(int parentId, [Bind(Include = "Texto,Autor")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                comentario.Id = _nextId++;
                comentario.DataCriacao = DateTime.Now;
                comentario.ComentarioId = parentId;
                _comentarios.Add(comentario);
                return RedirectToAction("Details", new { id = parentId });
            }
            return RedirectToAction("Details", new { id = parentId });
        }



    }
}