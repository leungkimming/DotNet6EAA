namespace Client
{
    public static class DrawerMenu
    {
        public static IEnumerable<DrawerItem> MenuItems =>
        new List<DrawerItem>
            {
                new DrawerItem{ Text = "Home", Icon = "home", Url="/", Group = "home"},
                new DrawerItem{ Text = "User", Icon = "dollar", Url="/adduser", Group = "app"},
                new DrawerItem{ Text = "System Parameters", Icon = "tell-a-friend", Url="/systemparameters/searchdatas", Group = "app"},
           };
    }
}
