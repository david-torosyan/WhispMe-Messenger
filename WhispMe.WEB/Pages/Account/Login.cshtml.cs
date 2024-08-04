using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhispMe.DTO.DTOs;
using WhispMe.WEB.Models;

namespace WhispMe.WEB.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginRequest = new LoginRequestDto
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7001/Users/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                if (Input.RememberMe == true)
                    SaveToken(token);

                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        private void SaveToken(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7) // Adjust the expiration as needed
            };
            Response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
