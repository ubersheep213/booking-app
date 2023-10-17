using HotelAppClassLibrary.Models;

namespace HotelAppClassLibrary.Data
{
    public interface IDatabaseData
    {
        void BookRoom(DateTime startDate, DateTime endDate, int roomTypeId, string firstName, string lastName);
        void CheckInGuest(int bookingId);
        List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
		RoomTypeModel GetRoomTypeById(int id);
		BookingModel SearchBooking(string lastName);
    }
}