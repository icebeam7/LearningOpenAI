using Helpers;
using Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Services
{
	public class OpenAIService
	{
		HttpClient httpClient;

		public OpenAIService()
		{
			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(Constants.OpenAIBaseUrl);
			httpClient.DefaultRequestHeaders.Authorization = 
				new AuthenticationHeaderValue("Bearer", Constants.OpenAIKey);
		}

		public async Task<FileUploadResponse> UploadFile(string filename, string purpose)
		{
			var endpoint = "/v1/files";

			var content = new MultipartFormDataContent();

			using var file = File.OpenRead(filename);
			using var ms = new MemoryStream();
			file.CopyTo(ms);
			ms.Position = 0;

			var fileContent = new ByteArrayContent(ms.ToArray());
			fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			content.Add(fileContent, "file", filename);

			var stringContent = new StringContent(purpose);
			content.Add(stringContent, "purpose");

			var request = await httpClient.PostAsync(endpoint, content);
			
			if (request.IsSuccessStatusCode)
			{
				var response = await request.Content.ReadFromJsonAsync<FileUploadResponse>();
				return response;
			}

			return default(FileUploadResponse);
		}

		public async Task<FineTuneCreateResponse> CreateFineTune(FineTuneCreateRequest fineTuneCreateRequest)
		{
			var endpoint = "/v1/fine-tunes";

			var json = JsonSerializer.Serialize(fineTuneCreateRequest);
			var content = new StringContent(json, 
				Encoding.UTF8, 
				new MediaTypeHeaderValue("application/json") );

			var request = await httpClient.PostAsync(endpoint, content);

			if (request.IsSuccessStatusCode) 
			{
				var response = await request.Content.ReadFromJsonAsync<FineTuneCreateResponse>();
				return response;
			}

			return default(FineTuneCreateResponse);
		}

		public async Task<FineTuneCreateResponse> RetrieveFineTune(string fineTuneId)
		{
			var endpoint = $"/v1/fine-tunes/{fineTuneId}";

			var request = await httpClient.GetAsync(endpoint);

			if (request.IsSuccessStatusCode)
			{
				var response = await request.Content.ReadFromJsonAsync<FineTuneCreateResponse>();
				return response;
			}

			return default(FineTuneCreateResponse);
		}

		public async Task<CompletionResponse> AskQuestion(CompletionRequest completionRequest)
		{
			var endpoint = "/v1/completions";

			var json = JsonSerializer.Serialize(completionRequest);
			var content = new StringContent(json,
				Encoding.UTF8,
				new MediaTypeHeaderValue("application/json"));

			var request = await httpClient.PostAsync(endpoint, content);

			if (request.IsSuccessStatusCode)
			{
				var response = await request.Content.ReadFromJsonAsync<CompletionResponse>();
				return response;
			}

			return default(CompletionResponse);
		}

	}
}