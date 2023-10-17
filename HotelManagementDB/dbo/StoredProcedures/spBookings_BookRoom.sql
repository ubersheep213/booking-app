CREATE PROCEDURE [dbo].[spBookings_BookRoom]
	@startDate date,
	@endDate date,
	@roomTypeId int,
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
begin
	set nocount on;

	declare @roomId int;

	set @roomId = 
	(select top 1 r.Id from dbo.Rooms r
	where 
	(@roomTypeId = r.RoomTypeId) and 
	r.Id not in
	(select b.RoomId from dbo.Bookings b
	where (b.StartDate >= @StartDate and b.StartDate <= @endDate)
	or (b.EndDate > @StartDate and b.EndDate <= @endDate)))

	if not exists (select 1 from dbo.Guests where FirstName = @firstName and LastName = @lastName)
	begin
		insert into dbo.Guests (FirstName, LastName)
		values (@firstName, @lastName)
	end

	declare @guestId int;

	set @guestId = (select g.Id from dbo.Guests g
	where g.FirstName = @firstName and g.LastName = @lastName)

	declare @numberOfDays int;
	declare @totalCost money;
	set @numberOfDays = datediff (day, @startDate, @endDate);
	set @totalCost = (select @numberOfDays*t.Price as totalCost from dbo.RoomTypes t where t.Id = @roomTypeId)

	insert into dbo.Bookings (RoomId, GuestId, StartDate, EndDate, CheckedIn, TotalCost)
	values (@roomId, @guestId, @startDate, @endDate, 0, @totalCost);
end
