using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhispMe.DTO.DTOs;
using WhispMe.WEB.Models;

namespace WhispMe.WEB.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registerRequest = new RegisterRequestDto
            {
                Email = Input.Email,
                Password = Input.Password,
                FullName = Input.FullName
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7001/Users/register", registerRequest);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid registration attempt.");
                return Page();
            }
        }
    }
}
