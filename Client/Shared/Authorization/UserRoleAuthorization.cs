using Blazored.LocalStorage;
using Common;

namespace Client {
    public class UserRoleAuthorization : IUserRoleAuthorization {
        private string accesscodes;
        private ILocalStorageService _localStorage;
        public UserRoleAuthorization(ILocalStorageService localStorage) {
            accesscodes = "";
            _localStorage = localStorage;
        }
        public async Task SetAccessCodesAsync() {
            var hasKey = await _localStorage.ContainKeyAsync("RuntimeInfo");
            if (hasKey) {
                RuntimeInfo _runtimeInfo = await _localStorage.GetItemAsync<RuntimeInfo>("RuntimeInfo");
                accesscodes = _runtimeInfo.AccessRights;
            }
        }
        public bool IsAuthroize(string accessCode) {
            if (string.IsNullOrEmpty(accesscodes)) return false;
            if (accessCode == "*") return true;
            string[]  accsessCodeArray=accesscodes.Split('/');
            string[] menuAccessCodeArray=accessCode.Split('/');
            if (!accsessCodeArray.Intersect(menuAccessCodeArray).Any()) {
                return false;
            }
            return true;
        }
    }
}
