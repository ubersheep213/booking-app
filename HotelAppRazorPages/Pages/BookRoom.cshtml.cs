using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.Models;
using HotelAppRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelAppRazorPages.Pages
{
    public class BookRoomModel : PageModel
    {
		private readonly IDatabaseData _db;

		[BindProperty(SupportsGet = true)]
        public DateSearchModel DateSearch { get; set; }
        [BindProperty, Required]
        public string? FirstName { get; set; }
        [BindProperty, Required]
        public string? LastName { get; set; }
        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }
        public bool IsBooked { get; set; }
        public RoomTypeModel RoomType { get; set; }
        public BookRoomModel(IDatabaseData db)
        {
			_db = db;
		}
        public void OnGet()
        {
            if (RoomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(RoomTypeId);
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.BookRoom(DateSearch.StartDate, DateSearch.EndDate, RoomTypeId, FirstName, LastName);
                return RedirectToPage("Index", new { IsBooked = true });                
            }
            else
            {
                return RedirectToPage(new { RoomTypeId, DateSearch.StartDate, DateSearch.EndDate });
            }
        }
    }
}
