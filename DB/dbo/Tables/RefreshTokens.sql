CREATE TABLE [dbo].[RefreshTokens] (
    [Token]       NCHAR (64)    NOT NULL,
    [ExpiresAt]   DATETIME2 (7) NOT NULL,
    [UserID]      INT           NOT NULL,
    [AlreadyUsed] BIT           NOT NULL,
    [CreatedAt]   DATETIME2 (7) NULL,
    [UpdatedAt]   DATETIME2 (7) NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED ([Token] ASC),
    CONSTRAINT [FK_RefreshTokens_Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserID]
    ON [dbo].[RefreshTokens]([UserID] ASC);

