using System.ComponentModel.DataAnnotations;

namespace MOFU.Models
{
    public class ProjectMember
    {
        [Key]
        public int ProjectMemberId { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; } = "Member";
        public Users Users { get; set; } = null;
        public Project Project { get; set; } = null;

    }
}
