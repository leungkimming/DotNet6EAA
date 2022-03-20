using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using Common.Shared;

namespace API
{
    public class CustomUserException
    {
        public CustomUserException(ILogger<CustomUserException> logger)
        {
            CustomUserExceptionHandler.Init(logger);
        }
    }
}
