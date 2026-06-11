using System.ComponentModel.DataAnnotations;

namespace MOFU.Dto
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName { get; set; } = "";

        [Required]
        [MinLength(8)]

        public string UserPassword { get; set; } = "";

        [Required]
        [EmailAddress]

        public string UserEmail { get; set; } = "";
    }
}
