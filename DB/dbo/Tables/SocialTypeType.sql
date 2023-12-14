CREATE TABLE [dbo].[SocialTypeType] (
    [SocialTypeId] INT            NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Desc]         NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_SocialTypeType] PRIMARY KEY CLUSTERED ([SocialTypeId] ASC)
);

