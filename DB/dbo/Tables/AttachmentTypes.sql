CREATE TABLE [dbo].[AttachmentTypes] (
    [AttachmentTypeId] INT            NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Desc]             NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_AttachmentTypes] PRIMARY KEY CLUSTERED ([AttachmentTypeId] ASC)
);

