using AutoMapper;
using Common;
using Data;
using GridCommon2;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.ServiceModel;
using System.Text.Json;

namespace Service {
    public class GridCommon2Service : BaseService {
        private readonly IAppSettings _appSettings;
        private readonly CoreServiceClient? _client;

        public GridCommon2Service(
            IUnitOfWork unitOfWork,
            ILogger<GridCommon2Service> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IAppSettings appSettings
        ) : base(unitOfWork, logger, mapper, httpContextAccessor) {
            this._appSettings = appSettings;

            string endpoint = appSettings.GridCommon2.Endpoint;
             BasicHttpBinding binding = new BasicHttpBinding();
                binding.MaxBufferSize = int.MaxValue;
                binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.AllowCookies = true;
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Ntlm;
                _client = new CoreServiceClient(binding, new EndpointAddress(endpoint));
        }

        private async Task<UserProfilePO> GetGridCommon2UserProfile(string loginIdWithDomainPrefix) {
            if (_client == null) {
                var errorResponse = new ErrorPayloadResponse<SystemConfigurationError>();
                errorResponse.Append(new SystemConfigurationError(SystemConfigurationErrorCategories.GridCommon2EndpointConfigError));
                throw new SystemConfigurationException(errorResponse);
            }
            GetUserProfileRequest userProfileRequest = new GetUserProfileRequest();
            userProfileRequest.Code = loginIdWithDomainPrefix;
            ResponsePO userProfileGC2Response;
            try {
                userProfileGC2Response = await _client.RequestAsync(userProfileRequest);
            } catch (Exception exception) {
                var errorResponse = new ErrorPayloadResponse<GetUserProfileError>();
                errorResponse.Append(new GetUserProfileError(new GetUserProfileError(GetUserProfileErrorCategories.GC2GetUserProfileError)));
                exception.Data["ValidationErrorResponsePayload"] = errorResponse.Details;
                throw;
            }

            if (userProfileGC2Response == null) {
                var errorResponse = new ErrorPayloadResponse<GetUserProfileError>();
                errorResponse.Append(new GetUserProfileError(GetUserProfileErrorCategories.GC2ResponseNotFoundError));
                throw new UserProfileInternalServerException(errorResponse);
            }

            GetUserProfileResponse userProfileResponse;
            try {
                userProfileResponse = (GetUserProfileResponse)userProfileGC2Response;
            } catch (Exception exception) {
                var errorResponse = new ErrorPayloadResponse<GetUserProfileError>();
                errorResponse.Append(new GetUserProfileError(new GetUserProfileError(GetUserProfileErrorCategories.GC2UserProfileMappingError)));
                exception.Data["ValidationErrorResponsePayload"] = errorResponse.Details;
                throw;
            }

            if (userProfileResponse.UserProfile == null) {
                var errorResponse = new ErrorPayloadResponse<GetUserProfileError>();
                errorResponse.Append(new GetUserProfileError(GetUserProfileErrorCategories.NotFoundError));
                throw new UserProfileNotFoundInternalServerException(errorResponse);
            }

            return userProfileResponse.UserProfile;
        }

        public async Task<string[]> GetAccessCodes(string loginId) {
            string loginIdWithDomainPrefix = loginId;
            var errorResponse = new ErrorPayloadResponse<GetUserProfileError>();
            errorResponse.Append(new GetUserProfileError(GetUserProfileErrorCategories.ForbiddenError));
            var userProfile = await GetGridCommon2UserProfile(loginIdWithDomainPrefix);

            if (userProfile?.AccessCodes == null) {
                throw new UserProfileInternalServerException(errorResponse);
            }
            return userProfile.AccessCodes.Items.Where(code => code.StartsWith(_appSettings.GridCommon2.Prefix)).ToArray();
        }

        public async Task<UserProfileDto> GetUserProfile(string loginId) {
            string loginIdWithDomainPrefix = loginId;
            var gridCommon2UserProfile = await GetGridCommon2UserProfile(loginIdWithDomainPrefix);
            return _mapper.Map<UserProfileDto>(gridCommon2UserProfile);
        }

    }
}
