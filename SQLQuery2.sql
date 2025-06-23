DROP TABLE IF EXISTS Bookings;
DROP TABLE IF EXISTS Events;
DROP TABLE IF EXISTS Venues;
DROP TABLE IF EXISTS EventTypes;

-- EVENT TYPES
CREATE TABLE EventTypes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- EVENTS
CREATE TABLE Events (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    EventTypeId INT NOT NULL,
    ImageUrl NVARCHAR(MAX),
    FOREIGN KEY (EventTypeId) REFERENCES EventTypes(Id)
);

-- VENUES
CREATE TABLE Venues (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200),
    Description NVARCHAR(MAX),
    Capacity INT,
    ImageUrl NVARCHAR(MAX),
    IsAvailable BIT NOT NULL DEFAULT 1
);

-- BOOKINGS
CREATE TABLE Bookings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    EventId1 NVARCHAR(50) NOT NULL,
    VenueId1 NVARCHAR(50) NOT NULL,
    BookingDate DATETIME NOT NULL,
    TimeSlot NVARCHAR(50) NOT NULL,
    FOREIGN KEY (EventId1) REFERENCES Events(Id),
    FOREIGN KEY (VenueId1) REFERENCES Venues(Id),
    CONSTRAINT UQ_Venue_Date_Slot UNIQUE (VenueId1, BookingDate, TimeSlot)
);

INSERT INTO EventTypes (Name) VALUES
('Conference'),
('Wedding'),
('Birthday'),
('Concert'),
('Workshop');

-- Optional: Sample SELECT to verify tables
SELECT * FROM EventTypes;
SELECT * FROM Venues;
SELECT * FROM Events;
SELECT * FROM Bookings;
