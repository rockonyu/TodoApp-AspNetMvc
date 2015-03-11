using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TodoApp.Migrations;

namespace TodoApp.Models {
    public class TodoEntities : DbContext{
        public TodoEntities()
            : base("DefaultConnection") {
        }

        static TodoEntities() {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<TodoEntities>(new MigrateDatabaseToLatestVersion<TodoEntities, Configuration>());
        }

        public DbSet<Todo> Todos { get; set; }
    }
}