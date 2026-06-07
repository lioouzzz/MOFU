namespace MOFU.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public DateTime CreateAt { get; set; }
    }
}
