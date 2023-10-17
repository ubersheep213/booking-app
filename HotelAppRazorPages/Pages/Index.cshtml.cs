using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.Databases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelAppRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [BindProperty(SupportsGet = true)]
        public bool IsBooked { get; set; }
        public string Message { get; set; } = "";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (IsBooked)
            {
                Message = "Your room is booked";
            }
        }
    }
}