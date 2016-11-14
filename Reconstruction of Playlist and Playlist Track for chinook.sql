USE [Chinook]
GO

/****** Object:  Table [dbo].[Playlists]    Script Date: 11/10/2016 3:45:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

drop table PlaylistTracks
drop table Playlists
go

CREATE TABLE Playlists
(
	PlaylistId int IDENTITY(1,1) NOT NULL
	CONSTRAINT PK_Playlists_PlaylistId PRIMARY KEY CLUSTERED,
	Name nvarchar(120) not NULL,
	CustomerId int null
) 

GO


CREATE TABLE PlaylistTracks
(
	PlaylistId int NOT NULL
	Constraint FK_PlaylistTracksPlaylists_PlaylistId Foreign Key references Playlists(PlaylistId),
	TrackId int NOT NULL
	Constraint FK_PlaylistTracksTracks_TrackId Foreign Key references Tracks(TrackId),
	TrackNumber int not null,
	CONSTRAINT PK_PlaylistTracks_PlaylistIdTrackId PRIMARY KEY NONCLUSTERED (PlaylistId, TrackId)
)

GO

