using Microsoft.AspNetCore.Authentication;

namespace P6.StoryTest {
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions {
        public string DefaultUserId { get; set; } = null!;
    }
}
