using System.ComponentModel.DataAnnotations;

namespace MOFU.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = "";
        public List<ProjectMember> ProjectMember { get; set; } = new();

        [MaxLength(20)]
        public string ProjectKey { get; set; } = "";
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteAt { get; set; }
    }
}
