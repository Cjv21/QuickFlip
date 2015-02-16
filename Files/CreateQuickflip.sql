-- =============================================
-- Create date: 02/12/2015
-- Description:	Creates the QuickFlip database
-- =============================================

USE [aspnet-QuickFlip-20150210135155]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET NOCOUNT ON;

CREATE TABLE [Community]
(
	CommunityId INT NOT NULL IDENTITY,
	CommunityName VARCHAR(100) NOT NULL,
	City VARCHAR(50) NOT NULL,
	State VARCHAR(50) NOT NULL,
	DefaultMeetingLocation VARCHAR(150) NOT NULL,
	PRIMARY KEY (CommunityId)
)


INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Indiana University', 'Bloomington', 'Indiana', 'Indiana University Memorial Union (900 East 7th St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Michigan State University', 'East Lansing', 'Michigan', 'MSU Union (45 Abbot Rd.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Northwestern University', 'Evanston', 'Illinois', 'Norris University Center (1999 Campus Dr.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Ohio State University', 'Columbus', 'Ohio', 'The Ohio Union (1739 North High St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Penn State University', 'State College', 'Pennsylvania', 'Penn State Bookstore (1 East Pollock Rd.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('Purdue University', 'West Lafayette', 'Indiana', 'Purdue Memorial Union (101 North Grant St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Iowa', 'Iowa City', 'Iowa', 'Iowa Memorial Union (125 North Madison St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Illinois - Urbana Champaign', 'Champaign', 'Illinois', 'Illini Union (1401 West Green St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Michigan', 'Ann Arbor', 'Michigan', 'Michigan Union (530 South State St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Minnesota', 'Minneapolis', 'Minnesota', 'Coffman Memorial Union (300 Washington Ave.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Nebraska - Lincoln', 'Lincoln', 'Nebraska', 'Nebraska Union (1400 R St.)')

INSERT INTO [Community] (CommunityName, City, State, DefaultMeetingLocation)
VALUES ('University of Wisconsin - Madison', 'Madison', 'Wisconsin', 'Memorial Union (800 Langdon St.)')


GO


