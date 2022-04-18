using Common;
using System.Net.Http.Json;
using System.Text.Json;
using Telerik.Blazor.Components;

namespace Client {
    public class GeneralSearchUtil<T, L> where T : DTObaseRequest where L : DTObaseResponse {
        private readonly Util _util;
        private readonly HttpClient _http;
        public bool Notspinning = true;

        public string Message;
        public TelerikAnimationContainer AnimationContainerRef { get; set; }
        public bool Expanded { get; set; } = false;
        
        public GeneralSearchUtil(Util util, HttpClient http) {
            _util = util;
            _http = http;
        }
        public async Task RefreshAsync(TelerikGrid<L> telerikGrid) {
            var state=telerikGrid.GetState();
            await telerikGrid.SetState(state);
        }
        public async Task<GetAllDatasResponse<L>> SearchAsync(T searchRequest, string apiUrl) {
            Message = "";
            Notspinning = false;
            await _util.RefreshToken();
            HttpContent jsonContent = JsonContent.Create(searchRequest);
            var response = await _http.PostAsync($"{apiUrl}",jsonContent);
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
            Message = "";
            Notspinning = false;
            await _util.RefreshToken();
            var response = await _http.PostAsync($"{apiUrl}",jsonContent);
            Notspinning = true;
            if (response == null) {
                Message = "Response data is null";
            }
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) {
                L responseDatas=JsonSerializer.Deserialize<L>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            } else {
                Message = content ?? "Content is null";
            }
        }
    }
}
