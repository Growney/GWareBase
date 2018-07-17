CREATE TABLE [dbo].[Tenant]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(100) NOT NULL CONSTRAINT [UQ_Tenant_Name] UNIQUE,
	[CreatedDate] DATETIME NOT NULL CONSTRAINT [DF_Tenant_CreatedDate] DEFAULT (GETUTCDATE()), 
    [ControllerCreationString] VARCHAR(MAX) NOT NULL, 
    [DisplayName] VARCHAR(100), 
    [ImageSource] VARCHAR(MAX) NULL
)
