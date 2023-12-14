CREATE TABLE [dbo].[Users] (
    [UserId]         INT            IDENTITY (1, 1) NOT NULL,
    [Firstname]      NVARCHAR (50)  NOT NULL,
    [Lastname]       NVARCHAR (50)  NOT NULL,
    [Email]          NVARCHAR (50)  NOT NULL,
    [Password]       NCHAR (64)     NOT NULL,
    [Salt]           NVARCHAR (32)  NOT NULL,
    [ProfilePicture] NVARCHAR (MAX) NULL,
    [StatusId]       INT            NOT NULL,
    [CreatedAt]      DATETIME2 (7)  NULL,
    [UpdatedAt]      DATETIME2 (7)  NULL,
    [StatusText]     NVARCHAR (500) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_Users_UserStatusTypes_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[UserStatusTypes] ([UserStatusId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Email]
    ON [dbo].[Users]([Email] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Users_StatusId]
    ON [dbo].[Users]([StatusId] ASC);

