using Common;
using System.Net.Http.Json;
using System.Text.Json;
using Telerik.Blazor.Components;

namespace Client {
    public class GeneralSearchUtil<T, L> where T : DTObaseRequest where L : DTObaseResponse {

        private readonly HttpUtil _httpUtil;

        public bool Notspinning = true;

        public string Message;
        public TelerikNotification NotificationComponent { get; set; }
        public TelerikAnimationContainer AnimationContainerRef { get; set; }
        public bool Expanded { get; set; } = false;
        
        public GeneralSearchUtil(HttpUtil httpUtil) {
            _httpUtil = httpUtil;
        }
        public async Task RefreshAsync(TelerikGrid<L> telerikGrid) {
            var state=telerikGrid.GetState();
            await telerikGrid.SetState(state);
        }
        public async Task<GetAllDatasResponse<L>> SearchAsync(T searchRequest, string apiUrl) {
            Message = "";
            Notspinning = false;
            HttpContent jsonContent = JsonContent.Create(searchRequest);
            var response = await _httpUtil.PostAsync($"{apiUrl}",jsonContent);
            Notspinning = true;
            if (response == null) {
                Message = "Response data is null";
                return new GetAllDatasResponse<L>();
            }
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) {
                GetAllDatasResponse<L> responseDatas=JsonSerializer.Deserialize<GetAllDatasResponse<L>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return responseDatas ?? new GetAllDatasResponse<L>();
            } else {
                Message = content ?? "Content is null";
            }
            return new GetAllDatasResponse<L>();
        }
        public string ExpandIcon {
            get {
                return Expanded ? "arrow-chevron-up" : "arrow-chevron-down";
            }
        }
        public void ToggleAsync() {
            if (Expanded) {
                AnimationContainerRef?.HideAsync();
            } else {
                AnimationContainerRef?.ShowAsync();
            }
            Expanded = !Expanded;
        }
        public async Task CreateAsync(HttpContent jsonContent, string apiUrl) {
            await PostData(jsonContent, apiUrl);
        }

        private async Task PostData(HttpContent jsonContent, string apiUrl) {
            Message = "";
            Notspinning = false;
            var response = await _httpUtil.PostAsync($"{apiUrl}",jsonContent);
            Notspinning = true;
            if (response == null) {
                Message = "Response data is null";
            }
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) {
                AddDataResponse responseDatas=JsonSerializer.Deserialize<AddDataResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                //Message = responseDatas;
            } else {
                Message = content ?? "Content is null";
            }
        }

        public async Task UpdateAsync(HttpContent jsonContent, string apiUrl) {
            await PostData(jsonContent, apiUrl);
        }
        public void ShowSuccessNotifications(string message) {
            NotificationComponent.Show(new NotificationModel {
                Text = message,
                ThemeColor = "success",
                CloseAfter = 0
            });
        }
    }
}
