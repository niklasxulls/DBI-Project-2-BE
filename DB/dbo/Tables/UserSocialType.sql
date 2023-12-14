CREATE TABLE [dbo].[UserSocialType] (
    [UserId]       INT            NOT NULL,
    [SocialTypeId] INT            NOT NULL,
    [Url]          NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_UserSocialType] PRIMARY KEY CLUSTERED ([UserId] ASC, [SocialTypeId] ASC),
    CONSTRAINT [FK_UserSocialType_SocialTypeType_SocialTypeId] FOREIGN KEY ([SocialTypeId]) REFERENCES [dbo].[SocialTypeType] ([SocialTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSocialType_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserSocialType_SocialTypeId]
    ON [dbo].[UserSocialType]([SocialTypeId] ASC);

