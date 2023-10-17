using HotelAppClassLibrary.Data;
using HotelAppClassLibrary.Databases;
using HotelAppClassLibrary.Models;
using HotelAppRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelAppRazorPages.Pages
{
    public class RoomSearchModel : PageModel
    {
		private readonly IDatabaseData _db;
        public List<RoomTypeModel> RoomTypes { get; set; } = new List<RoomTypeModel>();
		[BindProperty]
		public int? RoomTypeId { get; set; }
		[BindProperty(SupportsGet = true)]
        public DateSearchModel DateSearch { get; set; }
		[BindProperty(SupportsGet = true)]
		public bool SearchEnabled { get; set; }
		public RoomSearchModel(IDatabaseData db)
        {
			_db = db;
		}
        public void OnGet()
        {
			ViewData["DatesMessage"] = "Enter dates for your booking.";
			if (SearchEnabled)
			{
				RoomTypes = _db.GetAvailableRoomTypes(DateSearch.StartDate, DateSearch.EndDate);
				ViewData["RoomsMessage"] = RoomTypes.Count > 0 ? 
					$"Available room types for dates from {DateSearch.StartDate:d} to {DateSearch.EndDate:d}." : 
					$"No available rooms for dates from {DateSearch.StartDate:d} to {DateSearch.EndDate:d}.";
			}
			else
			{
				SetDefaultDates();
			}
		}
		public IActionResult OnPostSearch()
		{
			try
			{
				if (DateSearch.StartDate > DateSearch.EndDate ||
				DateSearch.StartDate < DateTime.Today ||
				DateSearch.EndDate < DateTime.Today)
				{
					throw new Exception("Invalid dates.");
				}
				return RedirectToPage(new { SearchEnabled = true, DateSearch.StartDate, DateSearch.EndDate });
			}
			catch (Exception ex)
			{
				ViewData["DatesMessage"] = ex.Message;
			}
			return Page();
		}

		public IActionResult OnPostSelect()
		{			
			if (RoomTypeId == null)
			{
				SetDefaultDates();
				return RedirectToPage(new { DateSearch });
			}
			else 
			{ 
				return RedirectToPage("BookRoom", new { RoomTypeId, DateSearch.StartDate, DateSearch.EndDate }); 
			}						
		}
		private void SetDefaultDates()
		{
			DateSearch.StartDate = DateTime.Now;
			DateSearch.EndDate = DateSearch.StartDate.AddDays(1);
		}
	}
}
