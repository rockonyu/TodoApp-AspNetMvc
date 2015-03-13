using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoApp.Models;
using TodoApp.ViewModels;

namespace TodoApp.Controllers {
    public class TodoController : Controller {
        private TodoEntities db = new TodoEntities();

        // GET: Todo
        public ActionResult Index() {
            return View();
        }

        public ActionResult Archive() {
            return View(db.Todos.Where(m => m.IsArchive).ToList());
        }

        [HttpPost]
        public void ArchiveDone() {
            var todos = db.Todos.ToList();
            foreach (var item in todos) {
                if (item.IsDone) { item.IsArchive = true; }
            }
            db.SaveChanges();
        }

        // POST: Todo/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        public JsonResult Create(Todo todo) {
            if (ModelState.IsValid) {
                todo.Order = Count() + 1;
                db.Todos.Add(todo);
                db.SaveChanges();
            }
            return Json(todo.Id);
        }

        [HttpPost]
        public void Delete(int id) {
            Todo todo = db.Todos.Find(id);
            db.Todos.Remove(todo);
            db.SaveChanges();
        }

        [HttpPost]
        public void Edit(TodoEditModel model) {
            var todo = db.Todos.Find(model.Id);
            todo.Task = model.Task;
            db.Entry(todo).State = EntityState.Modified;
            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult Toggle(int id) {
            var todo = db.Todos.Find(id);
            todo.IsDone = !todo.IsDone;
            db.SaveChanges();
            return Json(todo.IsDone);
        }

        [HttpPost]
        public JsonResult ChangeOrder(List<int> ids) {
            var activeTodos = GetActiveTodos().AsEnumerable();
            for (var i = 0; i < ids.Count; i++) {
                var id = ids[i];
                var todo = activeTodos.FirstOrDefault(m => m.Id == id);
                if (todo != null) {
                    todo.Order = i + 1;
                }
            }
            db.SaveChanges();
            return Json(activeTodos.OrderBy(m => m.Order).ToList());
        }

        public JsonResult GetTodos() {
            return Json(GetActiveTodos().OrderBy(m => m.Order).ToList(), JsonRequestBehavior.AllowGet);
        }

        public int Count() {
            return GetActiveTodos().Count();
        }

        private IQueryable<Todo> GetActiveTodos() {
            return db.Todos.Where(m => !m.IsArchive);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
