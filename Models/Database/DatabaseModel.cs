using System.ComponentModel.DataAnnotations;

namespace BackEndNotes.Models.Database
{
    public class DatabaseModel
    {
        [Required]
        public string? Connection { get; set; }
        [Required]
        public string? Database { get; set; }
    }
}