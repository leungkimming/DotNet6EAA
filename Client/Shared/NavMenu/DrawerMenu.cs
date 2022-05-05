using Common;

namespace Client {
    public sealed class MenuTarget : ConstantBase<MenuTarget> {
        public static readonly MenuTarget Blank = new MenuTarget("_blank", "Blank");
        public static readonly MenuTarget Self = new MenuTarget("_self", "Self");
        public static readonly MenuTarget Popup = new MenuTarget("_popup", "Popup");
        private MenuTarget(string code, string description)
            : base(code, description) {
        }
    }
    public static class DrawerMenu {
        public static IEnumerable<DrawerItem> MenuItems =>
        new List<DrawerItem>
            {
                new DrawerItem{ Text = "Home", Icon = "home", Url="/dotnet6EAA", Group = "home"},
                new DrawerItem{ Text = "Add User", Icon = "dollar", Url="/dotnet6EAA/adduser", Group = "app", Target="_popup"},
                new DrawerItem{ Text = "Search User", Icon = "dollar", Url="/dotnet6EAA/searchuser", Group = "app"},
                new DrawerItem{ Text = "System Parameters", Icon = "tell-a-friend", Url="/dotnet6EAA/systemparameters/searchdatas", Group = "app"},
                 new DrawerItem{ Text = "Document Processing", Icon = "tell-a-friend", Url="/dotnet6EAA/documentprocessing", Group = "app"},
                new DrawerItem{ Text = "Swagger UI", Icon="gear", Url="/dotnet6EAA/swagger/index.html",Group="settings",Target= MenuTarget.Blank.Code},

           };
    }
}
