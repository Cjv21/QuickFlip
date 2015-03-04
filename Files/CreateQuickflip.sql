-- =============================================
-- Create date: 02/12/2015
-- Description:	Creates the QuickFlip database
-- =============================================

USE [quickflip-db]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET NOCOUNT ON;

-- COMMUNITY
CREATE TABLE [Community]
(
	CommunityId INT NOT NULL IDENTITY,
	Name VARCHAR(100) NOT NULL,
	City VARCHAR(50) NOT NULL,
	State VARCHAR(50) NOT NULL,
	DefaultMeetingLocation VARCHAR(150) NOT NULL,
	PRIMARY KEY (CommunityId)
)

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Indiana University', 'Bloomington', 'Indiana', 'Indiana University Memorial Union (900 East 7th St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Michigan State University', 'East Lansing', 'Michigan', 'MSU Union (45 Abbot Rd.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Northwestern University', 'Evanston', 'Illinois', 'Norris University Center (1999 Campus Dr.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Ohio State University', 'Columbus', 'Ohio', 'The Ohio Union (1739 North High St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Penn State University', 'State College', 'Pennsylvania', 'Penn State Bookstore (1 East Pollock Rd.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('Purdue University', 'West Lafayette', 'Indiana', 'Purdue Memorial Union (101 North Grant St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Iowa', 'Iowa City', 'Iowa', 'Iowa Memorial Union (125 North Madison St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Illinois - Urbana Champaign', 'Champaign', 'Illinois', 'Illini Union (1401 West Green St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Michigan', 'Ann Arbor', 'Michigan', 'Michigan Union (530 South State St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Minnesota', 'Minneapolis', 'Minnesota', 'Coffman Memorial Union (300 Washington Ave.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Nebraska - Lincoln', 'Lincoln', 'Nebraska', 'Nebraska Union (1400 R St.)')

INSERT INTO [Community] (Name, City, State, DefaultMeetingLocation)
VALUES ('University of Wisconsin - Madison', 'Madison', 'Wisconsin', 'Memorial Union (800 Langdon St.)')

-- USER
DROP TABLE [webpages_UsersInRoles]
DROP TABLE [webpages_Roles]
DROP TABLE [webpages_OAuthMembership]
DROP TABLE [UserProfile]

CREATE TABLE [UserProfile]
(
	UserId INT NOT NULL IDENTITY,
	UserName NVARCHAR(MAX),
	CommunityId INT,
	Email VARCHAR(320),
	Nonce VARCHAR(8),
	Verified BIT NOT NULL DEFAULT 0,
	B64EncodedImage NVARCHAR(MAX),
	PRIMARY KEY (UserId),
	FOREIGN KEY (CommunityId) REFERENCES [Community](CommunityId)
)

CREATE TABLE [UserReview]
(
	UserReviewId INT NOT NULL IDENTITY,
	ReviewedUserId INT NOT NULL,
	ReviewerUserId INT NOT NULL,
	Rating INT,
	Description VARCHAR(1000),
	PRIMARY KEY (UserReviewId),
	FOREIGN KEY (ReviewedUserId) REFERENCES [UserProfile](UserId),
	FOREIGN KEY (ReviewerUserId) REFERENCES [UserProfile](UserId)
)

CREATE TABLE [Message]
(
	MessageId INT NOT NULL IDENTITY,
	RecipientUserId INT NOT NULL,
	SenderUserId INT NOT NULL,
	Content VARCHAR(1000),
	Seen BIT DEFAULT 0,
	PRIMARY KEY (MessageId),
	FOREIGN KEY (RecipientUserId) REFERENCES [UserProfile](UserId),
	FOREIGN KEY (SenderUserId) REFERENCES [UserProfile](UserId)
)


-- POST
CREATE TABLE [Post]
(
	PostId INT NOT NULL IDENTITY,
	CommunityId INT NOT NULL,
	UserId INT NOT NULL,
	CreateDate DATETIME NOT NULL,
	ExpirationDate DATETIME,
	Title VARCHAR(100),
	Description VARCHAR(1000),
	RequiredPrice INT,
	PostType INT NOT NULL,
	AuctionType INT NOT NULL,
	TransactionType INT NOT NULL,
	Settled BIT DEFAULT 0 NOT NULL,
	PRIMARY KEY (PostId),
	FOREIGN KEY (CommunityId) REFERENCES Community(CommunityId)
)

CREATE TABLE [PostMedia]
(
	PostMediaId INT NOT NULL IDENTITY,
	Postid INT NOT NULL,
	B64EncodedImage NVARCHAR(MAX),
	PRIMARY KEY (PostMediaId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

-- CATEGORIES
CREATE TABLE [Auto]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Books]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [CameraPhoto]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [CellPhones]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [ClothingShoe]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Computers]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Electronics]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [HealthBeauty]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Home]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Jobs]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Movies]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Music]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [MusicalInstruments]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [Pets]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [RealEstate]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

CREATE TABLE [SportingGoods]
(
	PostId INT NOT NULL,
	Tags VARCHAR(200),
	PRIMARY KEY (PostId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId)
)

-- OFFER
CREATE TABLE [Offer]
(
	OfferId INT NOT NULL IDENTITY,
	PostId INT NOT NULL,
	UserId INT NOT NULL,
	Amount INT,
	Description VARCHAR(300),
	CreateDate DATETIME NOT NULL,
	Accepted BIT NOT NULL,
	PRIMARY KEY (OfferId),
	FOREIGN KEY (PostId) REFERENCES Post(PostId),
	FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
)

CREATE TABLE [OfferMedia]
(
	OfferMediaId INT NOT NULL IDENTITY,
	OfferId INT NOT NULL,
	B64EncodedImage NVARCHAR(MAX),
	PRIMARY KEY (OfferMediaId),
	FOREIGN KEY (OfferId) REFERENCES Offer(OfferId)
)

-- ALERT
CREATE TABLE [Alert]
(
	AlertId INT NOT NULL IDENTITY,
	OfferId INT,
	MessageId INT,
	AlertType INT NOT NULL,
	CreateDate DATETIME NOT NULL,
	PRIMARY KEY (AlertId),
	FOREIGN KEY (OfferId) REFERENCES [Offer](OfferId),
	FOREIGN KEY (MessageId) REFERENCES [Message](MessageId)
)



