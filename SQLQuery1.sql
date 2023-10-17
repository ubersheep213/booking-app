--DECLARE @StartDate datetime2 = '2023-09-09';
--DECLARE @EndDate datetime2 = '2023-09-15';
--SELECT * FROM dbo.Rooms r
--WHERE r.Id NOT IN (SELECT b.RoomId from dbo.Bookings b
--WHERE (b.StartDate >= @StartDate AND b.StartDate <= @EndDate)
--OR (b.EndDate >= @EndDate AND b.EndDate <= @EndDate))

--DECLARE @StartDate datetime2 = '2023-09-09';
--DECLARE @EndDate datetime2 = '2023-09-15';
--SELECT * FROM dbo.Rooms r
--WHERE r.Id IN (SELECT MIN(Id) FROM dbo.Rooms GROUP BY RoomTypeId)
--AND r.Id NOT IN (SELECT b.RoomId from dbo.Bookings b
--WHERE (b.StartDate >= @StartDate AND b.StartDate <= @EndDate)
--OR (b.EndDate >= @EndDate AND b.EndDate <= @EndDate))

DECLARE @StartDate datetime2 = '2023-09-09';
DECLARE @EndDate datetime2 = '2023-09-15';
WITH AvailableRooms AS
(SELECT * FROM dbo.Rooms r
WHERE r.Id NOT IN (SELECT b.RoomId from dbo.Bookings b
WHERE (b.StartDate >= @StartDate AND b.StartDate <= @EndDate)
OR (b.EndDate >= @EndDate AND b.EndDate <= @EndDate)))
SELECT t.* FROM AvailableRooms
INNER JOIN dbo.RoomTypes t ON t.Id = AvailableRooms.RoomTypeId
WHERE AvailableRooms.Id IN (SELECT MIN(AvailableRooms.Id) FROM AvailableRooms GROUP BY AvailableRooms.RoomTypeId)