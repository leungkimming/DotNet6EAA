namespace Common {
    public class AddUserResponse : DTObase {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string? DepartmentName { get; set; }
    }
}