using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoApp.Models {
    public class Todo {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        [Display(Name="待辦事項")]
        public string Task { get; set; }

        public bool IsDone { get; set; }

        public bool IsArchive { get; set; }
    }
}