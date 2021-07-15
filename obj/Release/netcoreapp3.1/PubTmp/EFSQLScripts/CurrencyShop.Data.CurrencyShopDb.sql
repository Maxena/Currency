IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [AdMob] (
        [Id] int NOT NULL IDENTITY,
        [Token] nvarchar(max) NULL,
        CONSTRAINT [PK_AdMob] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Ads] (
        [Id] int NOT NULL IDENTITY,
        [AdsUrl] nvarchar(max) NULL,
        [ImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Ads] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [authenticateChatUsers] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_authenticateChatUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Type] nvarchar(max) NULL,
        [ImgUrl] nvarchar(max) NULL,
        [ImgInternetUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Currency] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [LastPrice] int NOT NULL,
        [LastUpdated] datetime2 NOT NULL,
        [ImgUrl] nvarchar(max) NULL,
        [ImgInternetUrl] nvarchar(max) NULL,
        [Type] int NOT NULL,
        CONSTRAINT [PK_Currency] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [PannelUsers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        CONSTRAINT [PK_PannelUsers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Prices] (
        [Id] int NOT NULL IDENTITY,
        [Price] int NOT NULL,
        [Name] nvarchar(max) NULL,
        [Updated] datetime2 NOT NULL,
        CONSTRAINT [PK_Prices] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [words] (
        [Id] int NOT NULL IDENTITY,
        [Word] nvarchar(max) NULL,
        CONSTRAINT [PK_words] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [chatRooms] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Like] int NOT NULL,
        [Dislike] int NOT NULL,
        [Sequence] int NOT NULL,
        [ReportSequnce] int NOT NULL,
        [Reply] int NOT NULL,
        [Report] bit NOT NULL,
        [AuthenticateChatUserId] int NULL,
        CONSTRAINT [PK_chatRooms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_chatRooms_authenticateChatUsers_AuthenticateChatUserId] FOREIGN KEY ([AuthenticateChatUserId]) REFERENCES [authenticateChatUsers] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Brands] (
        [Id] int NOT NULL IDENTITY,
        [categoryId] int NOT NULL,
        [Name] nvarchar(max) NULL,
        [ImgUrl] nvarchar(max) NULL,
        [ImgInternetUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Brands] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Brands_Categories_categoryId] FOREIGN KEY ([categoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE TABLE [Objects] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [ProduceYear] int NOT NULL,
        [Price] float NOT NULL,
        [DatePosted] datetime2 NOT NULL,
        [CategoryId] int NOT NULL,
        [BrandName] nvarchar(max) NULL,
        CONSTRAINT [PK_Objects] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Objects_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE INDEX [IX_Brands_categoryId] ON [Brands] ([categoryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE INDEX [IX_chatRooms_AuthenticateChatUserId] ON [chatRooms] ([AuthenticateChatUserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    CREATE INDEX [IX_Objects_CategoryId] ON [Objects] ([CategoryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210715101428_initialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210715101428_initialCreate', N'3.1.16');
END;

GO

