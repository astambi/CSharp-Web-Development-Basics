CREATE DATABASE [BlogDb];
GO
USE [BlogDb];
GO
CREATE TABLE [Users] (
  [UserId] int NOT NULL IDENTITY,
  [Name] nvarchar(max) NOT NULL,
  CONSTRAINT [PK_User] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Posts] (
  [PostId] int NOT NULL IDENTITY,
  [UserId] int NOT NULL,
  [Title] nvarchar(max),
  CONSTRAINT [PK_Post] PRIMARY KEY ([PostId]),
  CONSTRAINT [FK_Post_User_UserId] FOREIGN KEY ([UserId])
  REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comments] (
  [CommentId] int NOT NULL IDENTITY,
  [UserId] int NOT NULL,
  [PostId] int NOT NULL,
  [Content] nvarchar(max),
  CONSTRAINT [PK_Comment] PRIMARY KEY ([CommentId]),
  CONSTRAINT [FK_Comment_User_UserId] FOREIGN KEY ([UserId])
  REFERENCES [Users] ([UserId]) ON DELETE CASCADE,
  CONSTRAINT [FK_Comment_Post_PostId] FOREIGN KEY ([PostId])
  REFERENCES [Posts] ([PostId])
);
GO