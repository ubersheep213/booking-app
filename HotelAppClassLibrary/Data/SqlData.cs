using HotelAppClassLibrary.Databases;
using HotelAppClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppClassLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }
        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sqlStatement = @"WITH AvailableRooms AS
                                    (SELECT * FROM dbo.Rooms r
                                    WHERE r.Id NOT IN (SELECT b.RoomId from dbo.Bookings b
                                    WHERE (b.StartDate >= @StartDate AND b.StartDate <= @EndDate)
                                    OR (b.EndDate > @StartDate AND b.EndDate <= @EndDate)))
                                    SELECT t.* FROM AvailableRooms
                                    INNER JOIN dbo.RoomTypes t ON t.Id = AvailableRooms.RoomTypeId
                                    WHERE AvailableRooms.Id IN (SELECT MIN(AvailableRooms.Id) FROM AvailableRooms GROUP BY AvailableRooms.RoomTypeId);";

            return _db.LoadData<RoomTypeModel, dynamic>(sqlStatement,
                                                        new { StartDate = startDate, EndDate = endDate },
                                                        connectionStringName, false);
        }
        public void BookRoom(DateTime startDate, DateTime endDate, int roomTypeId, string firstName, string lastName)
        {
            _db.SaveData("dbo.spBookings_BookRoom",
                         new { startDate, endDate, roomTypeId, firstName, lastName },
                         connectionStringName,
                         true);
        }
        public BookingModel SearchBooking(string lastName)
        {
            return _db.LoadData<BookingModel, dynamic>("dbo.spBookings_SearchBooking",
                                                       new { lastName, startDate = DateTime.Now.Date },
                                                       connectionStringName,
                                                       true).First();
        }
        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("dbo.spBookings_CheckIn", new { bookingId }, connectionStringName, true);
        }
        public RoomTypeModel GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetById",
                                                        new { id },
                                                        connectionStringName,
                                                        true).First();
        }
    }
}
