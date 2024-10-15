using PedidosAPI.Models;
using PedidosAPI.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace PedidosAPI.Service
{
    public class EmpacotamentoService : IEmpacotamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _empacotamentoApiBaseUrl;

        public EmpacotamentoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _empacotamentoApiBaseUrl = configuration["EmpacotamentoApi:BaseUrl"];
            if (string.IsNullOrEmpty(_empacotamentoApiBaseUrl))
            {
                throw new InvalidOperationException("A URL base da API de empacotamento está vazia ou ausente.");
            }
        }

        public async Task<ResultadoEmpacotamento> EmpacotarPedidos(PedidoRequest pedidoRequest)
        {
            try
            {
                var token = await ObterTokenAsync();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync($"{_empacotamentoApiBaseUrl}/empacotamento/empacotar", pedidoRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Erro ao empacotar pedidos: {response.StatusCode} - {errorMessage}");
                }

                var empacotamentoResultado = await response.Content.ReadFromJsonAsync<ResultadoEmpacotamento>();
                if (empacotamentoResultado == null)
                {
                    throw new InvalidOperationException("O resultado do empacotamento não pôde ser desserializado.");
                }

                return empacotamentoResultado;
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Houve um problema ao conectar com o serviço de empacotamento.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Ocorreu um erro ao processar a resposta do serviço de empacotamento.", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Um erro inesperado ocorreu ao empacotar pedidos.", ex);
            }
        }

        private async Task<string> ObterTokenAsync()
        {
            var loginInfo = new
            {
                Username = "usuario",
                Password = "senha123"
            };

            var response = await _httpClient.PostAsJsonAsync($"{_empacotamentoApiBaseUrl}/auth/login", loginInfo);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao obter o token: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (responseContent == null || string.IsNullOrEmpty(responseContent.Token))
            {
                throw new InvalidOperationException("Token não encontrado na resposta da API.");
            }

            return responseContent.Token;
        }

    }
}
