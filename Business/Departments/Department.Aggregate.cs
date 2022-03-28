using Business.Users;

namespace Business.Departments
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public Department(string name
            , string description, string manager) : this()
        {
            this.Update(name, description, manager);
        }

        public void Update(string name
            , string description, string manager)
        {
            Name = name;
            Description = description;
            Manager = manager;
        }
    }
}