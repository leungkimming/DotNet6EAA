namespace Client {
    public static class DrawerMenu {
        public static IEnumerable<DrawerItem> MenuItems =>
        new List<DrawerItem>
            {
                new DrawerItem{ Text = "Home", Icon = "home", Url="/dotnet6EAA", Group = "home"},
                new DrawerItem{ Text = "User", Icon = "dollar", Url="/dotnet6EAA/adduser", Group = "app"},
                new DrawerItem{ Text = "System Parameters", Icon = "tell-a-friend", Url="/dotnet6EAA/systemparameters/searchdatas", Group = "app"},
           };
    }
}
