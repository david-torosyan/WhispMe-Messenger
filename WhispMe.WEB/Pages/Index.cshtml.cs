using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhispMe.DTO.DTOs;

namespace WhispMe.WEB.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public List<RoomDto> Rooms { get; private set; }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7001/api/Rooms");

            if (response.IsSuccessStatusCode)
            {
                Rooms = await response.Content.ReadFromJsonAsync<List<RoomDto>>()
                        ?? [];
            }
        }
    }
}
