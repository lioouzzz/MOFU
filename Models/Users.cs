using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;

namespace MOFU.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; }
        public DateTime? DeleteAt { get; set; }
        public List<ProjectMember> ProjectMember { get; set; } = new();
    }
}
