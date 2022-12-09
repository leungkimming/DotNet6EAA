using Blazored.LocalStorage;
using Common;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client {
    public sealed class MenuTarget : ConstantBase<MenuTarget> {
        public static readonly MenuTarget Blank = new MenuTarget("_blank", "Blank");
        public static readonly MenuTarget Self = new MenuTarget("_self", "Self");
        public static readonly MenuTarget Popup = new MenuTarget("_popup", "Popup");
        private MenuTarget(string code, string description)
            : base(code, description) {
        }
    }
    public class MenuComponent {
        private IUserRoleAuthorization _userRoleAuthorization;
        private IList<DrawerItem> drawerItems;
        private IConfiguration _config;
        public MenuComponent(IUserRoleAuthorization userRoleAuthorization, IConfiguration config) {
            _userRoleAuthorization = userRoleAuthorization;
            drawerItems = new List<DrawerItem>();
            _config = config;
        }
        public IList<DrawerItem> GetMenuItems() {
            AddItem("*", "Home", "home", "/dotnet6EAA/", "home", MenuTarget.Self.Code);
            AddItem("SP01/SP02/SP03/SP04/", "System Parameters", "tell-a-friend", "/dotnet6EAA/systemparameters/searchdatas", "app", MenuTarget.Self.Code);
            AddItem("AA01", "Document Processing", "tell-a-friend", "/dotnet6EAA/documentprocessing", "app", MenuTarget.Self.Code);
            AddItem("AA01/AC01", "Search User", "dollar", "/dotnet6EAA/searchuser", "app", MenuTarget.Self.Code);
            AddItem("AB01", "Add User", "dollar", "/dotnet6EAA/adduser", "app", MenuTarget.Self.Code);
            AddItem("RA01", "Report", "dollar", "/dotnet6EAA/Report", "app", MenuTarget.Self.Code);
            AddItem("*", "Swagger UI", "gear", "/dotnet6EAA/swagger/index.html", "settings", MenuTarget.Blank.Code);
            return drawerItems;
        }

        private void AddItem(string accesscode, string description, string icon, string url, string group, string target) {
            if (drawerItems.Any(x => x.Text == description)) return;
            if(!_userRoleAuthorization.IsAuthroize(accesscode)) return;
            drawerItems.Add(new DrawerItem {
                Text = description,
                Group = group,
                Icon = icon,
                Url = url,
                Target = target
            });
        }
    }
}
