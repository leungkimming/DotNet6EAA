using Common;

namespace Client {
    public class StateContainer {
        private UserInfoDTO? userInfoDTO;

        public UserInfoDTO UserInfoDTO
        {
            get => userInfoDTO ?? new UserInfoDTO();
            set
            {
                userInfoDTO = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
