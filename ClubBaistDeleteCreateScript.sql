/*
Club BAIST Database Delete / Creation Script
Eric Dunn
BAIS 3230
Started: Jan 14, 2017
*/

use master
if exists
(
select [name]
from sysdatabases
where [name] = 'ClubBaist'
)
drop database ClubBaist
GO

create database ClubBaist

GO

use ClubBaist

GO

--************Drop Tables if they exist*************

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'Users'
)
drop table Users

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'Location'
)
drop table Location

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'AltHours'
)
drop table AltHours

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'Bill'
)
drop table Bill

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'Reservation'
)
drop table Reservation

if exists
(
	select [name]
	from ClubBaist.dbo.sysobjects
	where [name] = 'MembershipLevel'
)
drop table MembershipLevel

GO

-- *****************************Create Tables****************************************

create table MembershipLevel
(
membershipID tinyint not null
	constraint PK_membershipID primary key clustered identity(1,1),
	constraint CK_membershipIDGTval check (membershipID > 0),
	constraint CK_membershipIDLTval check (membershipID < 100),
levelTitle nvarchar(50),
[description] nvarchar(300),
restricStart time,  --start time when player can start playing a new game
restricEnd time,    --end time after which a player cannot start a new game
price money
)
GO

create table Location
(
locationID smallint not null
	constraint PK_locationID primary key clustered identity(1,1),
	constraint CK_locationIDGTval check (locationID > 0),
	constraint CK_locationIDLTval check (locationID < 1000),
locationName nvarchar(100),
phone nvarchar(15),
[address] nvarchar(60),
City nvarchar(50),
Prov_State nvarchar(50),
Country nvarchar(50),
defaultOpen time,
defaultClose time
)
GO

create table Users
(
email nvarchar(320) not null
	constraint PK_email primary key clustered,
	constraint CK_emailVal check (email like '[a-z,0-9,_,-]%@[a-z,0-9,_,-]%.[a-z][a-z]%'), -- basic email check going into the DB as extra measure
name nvarchar(100) not null,
[password] nvarchar(40) not null,
membershipID tinyint
	constraint FK_membershipID_Location references MembershipLevel (membershipID),
	constraint CK_membershipIDGTval_location check (membershipID > 0),
	constraint CK_membershipIDLTval_location check (membershipID < 100),
homePhone nvarchar(15), --stored as: 17804567891
cellPhone nvarchar(15),
picture image, -- null if no profile pic uploaded
approvedFlag bit, --Null implies no membership submitted, 0 equals not yet approved, 1 equals approved
employeeFlag bit, --Null or 0 implies not an employee, 1 equals employee true
jobDescription nvarchar(140), -- null if not an employee, same max length as twitter
locationID smallint  -- used for employees working at a certain location
	constraint FK_locationID_Location references Location (locationID),
	constraint CK_locationIDGTval_users check (locationID > 0),
	constraint CK_locationIDLTval_users check (locationID < 1000)
)
GO

create table AltHours
(
[date] date not null,
locationID smallint not null
	constraint FK_locationID_AltHours references Location (locationID),
constraint PK_date_location primary key clustered
	([date],locationID),
openToday bit,
openTime time,
closeTime time
)
GO

create table Bill
(
referenceID bigint not null
	constraint PK_referenceID primary key clustered identity(1,1),
email nvarchar(320) not null
	constraint FK_email_Bill references Users (email),
invoiceDate datetime,
	constraint CK_invoiceDateGT check (invoiceDate > '01-01-1900'),
currentCharges money,
balanceCarried money,
gst decimal(5,2),
total money,
paidAmount money,
paidDateTime datetime
)
GO

create table Reservation
(
reservationNumber bigint not null
	constraint PK_reservationNumber primary key clustered identity(1,1),
email nvarchar(320) not null
	constraint FK_email_Reservation references Users (email),
dateAndTime datetime,
numberPlayers tinyint,
	constraint CK_numberPlayersGTVal check (numberPlayers > 0),
	constraint CK_numberPlayersLTVal check (numberPlayers < 5),
numberCarts tinyint,
	constraint CK_numberCartsGTVal check (numberCarts > 0),
	constraint CK_numberCartsLTVal check (numberCarts < 5),
locationID smallint not null
	constraint FK_locationID_Reservation references Location (locationID)
)
GO

-- ************************************* Create Stored Procs *****************************************

/*
Name: spEmailCheck
Description: Checks the database real quick to see if a particular user email has already been taken
Use Case: Part of New User Application
*/

if exists (select name from sysobjects where name='spEmailCheck')
	drop PROC spEmailCheck
go

create proc spEmailCheck
@email nvarchar(320)
as
	select email 'Email'
	from Users
	where email like @email
if @@error<>0
return 1
else
return @@error

go

/*
Name: spLoginUser
Description: Checks the database real quick to see if a particular user email has already been taken
Use Case: Part of New User Application
*/

if exists (select name from sysobjects where name='spLoginUser')
	drop PROC spLoginUser
go

create proc spLoginUser
@email nvarchar(320),
@password nvarchar(100)
as
BEGIN
SET NOCOUNT ON;
	IF @email IS NULL
	BEGIN
		Select -1
	END
	ELSE
	BEGIN
	select email
	from Users
	where email like @email AND [password] like @password
	END
if @@error<>0
SELECT -1
END
go

/*
Name: spGetMembershipTypes
Description: Checks the database real quick to see if a particular user email has already been taken
Use Case: Part of New User Application
*/

if exists (select name from sysobjects where name='spGetMembershipTypes')
	drop PROC spGetMembershipTypes
go

create proc spGetMembershipTypes
as
	select *
	from MembershipLevel
if @@error<>0
return 1
else
return @@error

go

/*
Name: spAddUser
Description: The stored procedure that adds in new users to the database
*/

if exists (select name from sysobjects where name='spAddUser')
	drop PROC spAddUser
go

create proc spAddUser
@Email nvarchar(320),
@Name nvarchar(100),
@Password nvarchar(40),
@membershipID tinyint,
@HomePhone nvarchar(15),
@CellPhone nvarchar(15),
@picture image
as
	BEGIN
	DECLARE @Status INT = 0
	IF @Email is Null
	BEGIN
		SET @Status = 1
		RAISERROR('spAddUser - Parameter @Email is required.',16,1)
	END
	IF @Name is Null
	BEGIN
		SET @Status = 1
		RAISERROR('spAddUser - Parameter @Name is required.',16,1)
	END
	IF @Password is Null
	BEGIN
		SET @Status = 1
		RAISERROR('spAddUser - Parameter @Password is required.',16,1)
	END

	IF @Status = 0
	BEGIN
		INSERT INTO Users (email,name,[password],membershipID,homePhone,cellPhone,picture)
			VALUES (@Email,@Name,@Password,@membershipID,@HomePhone,@CellPhone,@picture)
	IF @@ERROR <> 0
		BEGIN
			SET @Status = 1
			RAISERROR('spAddUser - Error adding user!',16,1)
		END
	END
	RETURN @Status
END
GO

/*
Name: spGetUserPermissions
Description: Checks the database real quick to see if a particular user email has already been taken
Use Case: Part of New User Application
*/

if exists (select name from sysobjects where name='spGetUserPermissions')
	drop PROC spGetUserPermissions
go

create proc spGetUserPermissions
@email nvarchar(320)
as
IF EXISTS(SELECT email FROM ClubBaist..Users WHERE (email LIKE @email AND approvedFlag = 1)) --email exists and user is approved
BEGIN
	select u.email, u.name, u.membershipID, u.approvedFlag, u.employeeFlag, u.locationID, ml.restricStart, ml.restricEnd
	from ClubBaist..Users u
	INNER JOIN ClubBaist..MembershipLevel ml
	ON u.membershipID = ml.membershipID
	where u.email like @email
END
ELSE
BEGIN
	SELECT email, name, membershipID, approvedFlag, employeeFlag, locationID, null, null
	FROM Users
	WHERE email like @email
END
if @@error<>0
return 1
else
return @@error

go

/*
Name: spGetHours
Description: Checks the database to confirm a given location's hours up to 3 months in advance.
			If we know alternate hours exist then grab them, otherwise we only need the default hours.
Use Case: Part of New Reservation
*/

if exists (select name from sysobjects where name='spGetHours')
	drop PROC spGetHours
go

create proc spGetHours
@locationID smallint,
@date date
as
IF EXISTS(SELECT [date] FROM ClubBaist..AltHours WHERE (locationID = @locationID AND [date] < DATEADD(mm,4,@date) AND [date] >= @date)) --alt hours exist from today until exaclty 4 months from now, may change to 3 but I wanted extra dates in case I want to always go to the end of the month even if it exceeds by a bit
BEGIN -- If alt hours were found then we need to know what dates and which hours those are plus our default hours
	select l.defaultOpen, l.defaultClose, ah.[date], ah.openToday, ah.openTime, ah.closeTime
	from ClubBaist..Location l
	INNER JOIN ClubBaist..AltHours ah
	ON l.locationID = ah.locationID
	where l.locationID = @locationID AND [date] < DATEADD(mm,4,@date) AND [date] >= @date
END
ELSE --If no alternate hour overrides were found then select default hours
BEGIN
	SELECT defaultOpen,defaultClose
	FROM Location
	WHERE locationID = @locationID
END
if @@error<>0
return 1
else
return @@error

go

/*
Name: spGetReservations
Description: Checks the database to confirm a given location's reservations up to 3 months in advance
Use Case: Part of New Reservation
*/

if exists (select name from sysobjects where name='spGetReservations')
	drop PROC spGetReservations
go

create proc spGetReservations
@locationID smallint,
@date date
as
SELECT reservationNumber, email, dateAndTime, numberPlayers, numberCarts
FROM Reservation
WHERE locationID = @locationID AND DATEPART(mm,dateAndTime) >= DATEPART(mm,@date) AND DATEPART(mm,dateAndTime) < DATEPART(mm,(DATEADD(mm,4,@date)))

if @@error<>0
return 1
else
return @@error

go

/*
Name: spGetReservationsByUser
Description: Checks the database to confirm a given location's reservations up to 3 months in advance
Use Case: Part of New Reservation
*/

if exists (select name from sysobjects where name='spGetReservationsByUser')
	drop PROC spGetReservationsByUser
go

create proc spGetReservationsByUser
@email nvarchar(320),
@locationID smallint,
@startDate date,
@endDate date
as
IF @email IS NOT NULL
BEGIN
	IF @startDate IS NOT NULL AND @endDate IS NOT NULL
	BEGIN
		SELECT reservationNumber, email, dateAndTime, numberPlayers, numberCarts
		FROM Reservation
		WHERE locationID = @locationID AND email like @email AND dateAndTime >= @startDate AND dateAndTime <= @endDate
	END
	ELSE
	BEGIN
		SELECT reservationNumber, email, dateAndTime, numberPlayers, numberCarts
		FROM Reservation
		WHERE locationID = @locationID AND email like @email
	END
END
ELSE
BEGIN
	IF @startDate IS NOT NULL AND @endDate IS NOT NULL
	BEGIN
		SELECT reservationNumber, email, dateAndTime, numberPlayers, numberCarts
		FROM Reservation
		WHERE locationID = @locationID AND dateAndTime >= @startDate AND dateAndTime <= @endDate
	END
	ELSE
	BEGIN
		SELECT reservationNumber, email, dateAndTime, numberPlayers, numberCarts
		FROM Reservation
		WHERE locationID = @locationID
	END
END

if @@error<>0
return 1
else
return @@error

go

/*
Name: spDeleteReservationByID
Description: Checks the database to confirm a given location's reservations up to 3 months in advance
Use Case: Part of New Reservation
*/

if exists (select name from sysobjects where name='spDeleteReservationByID')
	drop PROC spDeleteReservationByID
go

create proc spDeleteReservationByID
@reservationNumber bigint
as
BEGIN
	DECLARE @Status INT = 0
	IF @reservationNumber is Null
	BEGIN 
	SET @Status = 1
		RAISERROR('spDeleteReservationByID - Parameter @reservationNumber is required.',16,1)
	END

	IF @Status = 0
	BEGIN
		DELETE FROM Reservation WHERE reservationNumber = @reservationNumber
	END
END
if @@error<>0
return 1
else
return @@error

go

/*
Name: spGetLocations
Description: Gets List of all locations
Use Case: Part of Add Reservation
*/

if exists (select name from sysobjects where name='spGetLocations')
	drop PROC spGetLocations
go

create proc spGetLocations
as
	select *
	from Location
if @@error<>0
return 1
else
return @@error

go

/*
Name: spAddReservation
Description: The stored procedure that adds in a new reservation to the database
*/

if exists (select name from sysobjects where name='spAddReservation')
	drop PROC spAddReservation
go

create proc spAddReservation
@email nvarchar(320),
@dateAndTime datetime,
@players tinyint,
@carts tinyint,
@locationID smallint
as
	BEGIN
	DECLARE @Status INT = 0
	IF @email is Null
	BEGIN
		SET @Status = -1
		RAISERROR('spAddReservation - Parameter @email is required.',16,1)
	END
	IF @locationID is Null
	BEGIN
		SET @Status = -1
		RAISERROR('spAddReservation - Parameter @locationID is required.',16,1)
	END

	IF @Status = 0
	BEGIN
		INSERT INTO Reservation (email,dateAndTime,numberPlayers,numberCarts,locationID)
			VALUES (@email,@dateAndTime,@players,@carts,@locationID)
	IF @@ERROR <> 0
		BEGIN
			SET @Status = -1
			RAISERROR('spAddReservation - Error adding reservation!',16,1)
		END
	END

	RETURN @Status
END
GO

--**********************Test data below*********************

-- exec spLoginUser 'eboy06@hotmail.com','Triforce78'  --Use this to test user logins for the website's login page, uncomment to test

insert into MembershipLevel
	(
		levelTitle,
		[description],
		restricStart,
		restricEnd,
		price
	)
	values
	(
		'Gold',
		'Highest level subscriber with no restrictions.',
		null,
		null,
		20000
	)
GO

insert into MembershipLevel
	(
		levelTitle,
		[description],
		restricStart,
		restricEnd,
		price
	)
	values
	(
		'Silver',
		'Moderate level with some restrictions.',
		'09:59:59',
		'18:59:59',
		15000
	)
GO

insert into MembershipLevel
	(
		levelTitle,
		[description],
		restricStart,
		restricEnd,
		price
	)
	values
	(
		'Bronze',
		'Lower level with more restrictions.',
		'11:59:59',
		'16:59:59',
		10000
	)
GO


exec spAddUser 'eboy06@hotmail.com','Eric Dunn', 'Triforce77', 1, '','',null -- insert user as test example

exec spAddUser 'flamingPanzer@gmail.com','Kris Findling', 'Triforce99', 2, '','',null

exec spAddUser 'joeblow@gmail.com', 'Joe Blow', 'Employee#1', null,'','',null

update Users SET employeeFlag = 1 WHERE email LIKE 'joeblow@gmail.com'; --Make Joe Blow an Employee manually

update Users SET approvedFlag = 1 WHERE email LIKE 'eboy06@hotmail.com' --Manually Approve the first user Eric, could change to any user for demo

update Users SET approvedFlag = 1 WHERE email LIKE 'flamingPanzer@gmail.com'

insert into Location
	(
		locationName,
		phone,
		[address],
		City,
		Prov_State,
		Country,
		defaultOpen,
		defaultClose
	)
	values
	(
		'Green Hills',
		'780-422-2525',
		'8899 - 123 Something Street',
		'Edmonton',
		'Alberta',
		'Canada',
		'06:00:00',
		'23:00:00'
	)
GO

insert into Location
	(
		locationName,
		phone,
		[address],
		City,
		Prov_State,
		Country,
		defaultOpen,
		defaultClose
	)
	values
	(
		'Aquatic Ruins',
		'780-422-5050',
		'7788 - 123 Nothing Way',
		'Calgary',
		'Alberta',
		'Canada',
		'08:00:00',
		'22:00:00'
	)
GO

--Sample alt hours special 2am extended hours extravaganza
insert into AltHours
	(
		[date],
		locationID,
		openToday,
		openTime,
		closeTime
	)
	values
	(
		'2017-03-17',
		1,
		1,
		'02:00:00',
		'23:59:59'
	)
GO

--Sample alt hours closed day, Good Friday
insert into AltHours
	(
		[date],
		locationID,
		openToday,
		openTime,
		closeTime
	)
	values
	(
		'2017-04-14',
		1,
		0,
		null,
		null
	)
GO

insert into Reservation
	(
		email,
		dateAndTime,
		numberPlayers,
		numberCarts,
		locationID
	)
	values
	(
		'eboy06@hotmail.com',
		'2017-03-15 12:00:00',
		2,
		1,
		1
	)
GO

insert into Reservation
	(
		email,
		dateAndTime,
		numberPlayers,
		numberCarts,
		locationID
	)
	values
	(
		'flamingPanzer@gmail.com',
		'2017-03-15 14:00:00',
		2,
		1,
		1
	)
GO

insert into Reservation
	(
		email,
		dateAndTime,
		numberPlayers,
		numberCarts,
		locationID
	)
	values
	(
		'flamingPanzer@gmail.com',
		'2017-03-16 14:00:00',
		2,
		1,
		1
	)
GO

insert into Reservation
	(
		email,
		dateAndTime,
		numberPlayers,
		numberCarts,
		locationID
	)
	values
	(
		'eboy06@hotmail.com',
		'2017-09-15 11:00:00',
		4,
		2,
		1
	)
GO

-- Finally can test functions below here:

--exec spGetHours 1, '2017/03/12'  --Tests spGetHours stored proc, can change depending on needs

--exec spGetReservations 1, '2017/03/12'  --Tests reservations grab to ensure checks are working proper