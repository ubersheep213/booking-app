CREATE PROCEDURE [dbo].[spBookings_SearchBooking]
	@lastName nvarchar(50),
	@startDate date
AS
begin
	set nocount on;

	select b.* from dbo.Bookings b
	where b.StartDate = @startDate
	and b.GuestId = (select g.Id from dbo.Guests g
	where g.LastName = @lastName);
end
