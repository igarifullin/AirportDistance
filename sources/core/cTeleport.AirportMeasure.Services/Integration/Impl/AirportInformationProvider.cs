using System;
using System.Net.Http;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Errors;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using Newtonsoft.Json;

namespace cTeleport.AirportMeasure.Services.Integration.Impl
{
    public class AirportInformationProvider : IAirportInformationProvider
    {
        private readonly HttpClient _httpClient;

        public AirportInformationProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<AirportDto>> GetAirportAsync(string iataCode)
        {
            var uri = new Uri($"airports/{iataCode}", UriKind.Relative);
            var httpResponse = await _httpClient.GetAsync(uri);
            if (!httpResponse.IsSuccessStatusCode)
            {
                return new Error((int) SystemErrorCodes.SystemError,
                    $"Received HttpErrorCode {httpResponse.StatusCode}");
            }

            if (httpResponse.Content == null)
            {
                return new Error((int)SystemErrorCodes.InvalidRequest,
                    "Received empty response");
            }

            var responseBody = await httpResponse.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<AirportDto>(responseBody);

            return model;
        }
    }
}