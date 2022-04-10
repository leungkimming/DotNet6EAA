
namespace Common {
    public static class Enums {
        public readonly record struct Department(int Id, string Name);

        public static Department[] Departments = new Department[] {
            new Department(1,"IT"),
            new Department(2,"HR")
        };
    }
}
