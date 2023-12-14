CREATE TABLE [dbo].[Attachments] (
    [AttachmentId] INT            IDENTITY (1, 1) NOT NULL,
    [TypeId]       INT            NOT NULL,
    [Name]         NVARCHAR (60)  NOT NULL,
    [RelativePath] NVARCHAR (150) NOT NULL,
    [MessageId]    INT            NOT NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED ([AttachmentId] ASC, [TypeId] ASC),
    CONSTRAINT [FK_Attachments_AttachmentTypes_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[AttachmentTypes] ([AttachmentTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Attachments_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Attachments_MessageId]
    ON [dbo].[Attachments]([MessageId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Attachments_TypeId]
    ON [dbo].[Attachments]([TypeId] ASC);

