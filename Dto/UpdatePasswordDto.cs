namespace MOFU.Dto
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
