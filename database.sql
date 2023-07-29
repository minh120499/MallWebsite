create database MallWebsite;
go

use mallWebsite;
go

create table Categories
(
    Id         int identity
        constraint PK_Categories
            primary key,
    Name       nvarchar(max) not null,
    Image      nvarchar(max),
    Type       nvarchar(max),
    Status     nvarchar(max),
    CreateOn   datetime2,
    ModifiedOn datetime2
)
go

create table Facilities
(
    Id          int identity
        constraint PK_Facilities
            primary key,
    Name        nvarchar(max) not null,
    Description nvarchar(max),
    Status      nvarchar(max),
    CreateOn    datetime2,
    ModifiedOn  datetime2
)
go

create table Feedbacks
(
    Id       int identity
        constraint PK_Feedbacks
            primary key,
    Message  nvarchar(max) not null,
    Email    nvarchar(max) not null,
    Name     nvarchar(max) not null,
    Status   nvarchar(max),
    CreateOn datetime2
)
go

create table Floors
(
    Id          int identity
        constraint PK_Floors
            primary key,
    Name        nvarchar(max) not null,
    Description nvarchar(max),
    Status      nvarchar(max),
    CreateOn    datetime2,
    ModifiedOn  datetime2
)
go

create table Products
(
    Id          int identity
        constraint PK_Products
            primary key,
    Code        nvarchar(max),
    Image       nvarchar(max),
    Name        nvarchar(max),
    Description nvarchar(max),
    Brand       nvarchar(max),
    Status      nvarchar(max),
    CreateOn    datetime2,
    ModifiedOn  datetime2
)
go

create table ProductCategory
(
    ProductId  int not null
        constraint FK_ProductCategory_Products_ProductId
            references Products
            on delete cascade,
    CategoryId int not null
        constraint FK_ProductCategory_Categories_CategoryId
            references Categories
            on delete cascade,
    constraint PK_ProductCategory
        primary key (ProductId, CategoryId)
)
go

create index IX_ProductCategory_CategoryId
    on ProductCategory (CategoryId)
go

create table Roles
(
    Id               nvarchar(450) not null
        constraint PK_Roles
            primary key,
    Name             nvarchar(256),
    NormalizedName   nvarchar(256),
    ConcurrencyStamp nvarchar(max)
)
go

create table RoleClaims
(
    Id         int identity
        constraint PK_RoleClaims
            primary key,
    RoleId     nvarchar(450) not null
        constraint FK_RoleClaims_Roles_RoleId
            references Roles
            on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_RoleClaims_RoleId
    on RoleClaims (RoleId)
go

create unique index RoleNameIndex
    on Roles (NormalizedName)
    where [NormalizedName] IS NOT NULL
go

create table Stores
(
    Id          int identity
        constraint PK_Stores
            primary key,
    Name        nvarchar(max) not null,
    Image       nvarchar(max),
    Phone       nvarchar(max) not null,
    Email       nvarchar(max) not null,
    FloorId     int           not null
        constraint FK_Stores_Floors_FloorId
            references Floors
            on delete cascade,
    CategoryId  int           not null
        constraint FK_Stores_Categories_CategoryId
            references Categories
            on delete cascade,
    Facilities  nvarchar(max),
    Description nvarchar(max),
    Status      nvarchar(max),
    CreateOn    datetime2,
    ModifiedOn  datetime2
)
go

create table Banners
(
    Id         int identity
        constraint PK_Banners
            primary key,
    Name       nvarchar(max) not null,
    Image      nvarchar(max),
    StoreId    int           not null
        constraint FK_Banners_Stores_StoreId
            references Stores
            on delete cascade,
    Expire     int,
    StartOn    datetime2,
    EndOn      datetime2,
    Status     nvarchar(max),
    CreateOn   datetime2,
    ModifiedOn datetime2
)
go

create index IX_Banners_StoreId
    on Banners (StoreId)
go

create table StoreProducts
(
    Id         int identity
        constraint PK_StoreProducts
            primary key,
    StoreId    int not null
        constraint FK_StoreProducts_Stores_StoreId
            references Stores
            on delete cascade,
    ProductId  int
        constraint FK_StoreProducts_Products_ProductId
            references Products,
    Status     nvarchar(max),
    CreateOn   datetime2,
    ModifiedOn datetime2
)
go

create index IX_StoreProducts_ProductId
    on StoreProducts (ProductId)
go

create index IX_StoreProducts_StoreId
    on StoreProducts (StoreId)
go

create index IX_Stores_CategoryId
    on Stores (CategoryId)
go

create index IX_Stores_FloorId
    on Stores (FloorId)
go

create table Users
(
    Id                   nvarchar(450) not null
        constraint PK_Users
            primary key,
    Phone                nvarchar(max),
    FullName             nvarchar(max) not null,
    Address              nvarchar(max),
    StoreId              int           not null
        constraint FK_Users_Stores_StoreId
            references Stores
            on delete cascade,
    Status               nvarchar(max),
    CreateOn             datetime2,
    ModifiedOn           datetime2,
    UserName             nvarchar(256),
    NormalizedUserName   nvarchar(256),
    Email                nvarchar(256),
    NormalizedEmail      nvarchar(256),
    EmailConfirmed       bit           not null,
    PasswordHash         nvarchar(max),
    SecurityStamp        nvarchar(max),
    ConcurrencyStamp     nvarchar(max),
    PhoneNumber          nvarchar(max),
    PhoneNumberConfirmed bit           not null,
    TwoFactorEnabled     bit           not null,
    LockoutEnd           datetimeoffset,
    LockoutEnabled       bit           not null,
    AccessFailedCount    int           not null
)
go

create table Orders
(
    Id         int identity
        constraint PK_Orders
            primary key,
    SourceId   int
        constraint FK_Orders_Stores_SourceId
            references Stores,
    SaleById   nvarchar(450)
        constraint FK_Orders_Users_SaleById
            references Users,
    TotalPrice real,
    Status     nvarchar(max),
    CreateOn   datetime2,
    ModifiedOn datetime2
)
go

create table OrderLineItems
(
    Id          int identity
        constraint PK_OrderLineItems
            primary key,
    OrderId     int  not null
        constraint FK_OrderLineItems_Orders_OrderId
            references Orders
            on delete cascade,
    ProductId   int  not null
        constraint FK_OrderLineItems_Products_ProductId
            references Products
            on delete cascade,
    ProductName nvarchar(max),
    Price       real not null,
    Quantity    int  not null
)
go

create index IX_OrderLineItems_OrderId
    on OrderLineItems (OrderId)
go

create index IX_OrderLineItems_ProductId
    on OrderLineItems (ProductId)
go

create index IX_Orders_SaleById
    on Orders (SaleById)
go

create index IX_Orders_SourceId
    on Orders (SourceId)
go

create table UserClaims
(
    Id         int identity
        constraint PK_UserClaims
            primary key,
    UserId     nvarchar(450) not null
        constraint FK_UserClaims_Users_UserId
            references Users
            on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_UserClaims_UserId
    on UserClaims (UserId)
go

create table UserLogins
(
    LoginProvider       nvarchar(450) not null,
    ProviderKey         nvarchar(450) not null,
    ProviderDisplayName nvarchar(max),
    UserId              nvarchar(450) not null
        constraint FK_UserLogins_Users_UserId
            references Users
            on delete cascade,
    constraint PK_UserLogins
        primary key (LoginProvider, ProviderKey)
)
go

create index IX_UserLogins_UserId
    on UserLogins (UserId)
go

create table UserRoles
(
    UserId nvarchar(450) not null
        constraint FK_UserRoles_Users_UserId
            references Users
            on delete cascade,
    RoleId nvarchar(450) not null
        constraint FK_UserRoles_Roles_RoleId
            references Roles
            on delete cascade,
    constraint PK_UserRoles
        primary key (UserId, RoleId)
)
go

create index IX_UserRoles_RoleId
    on UserRoles (RoleId)
go

create table UserTokens
(
    UserId        nvarchar(450) not null
        constraint FK_UserTokens_Users_UserId
            references Users
            on delete cascade,
    LoginProvider nvarchar(450) not null,
    Name          nvarchar(450) not null,
    Value         nvarchar(max),
    constraint PK_UserTokens
        primary key (UserId, LoginProvider, Name)
)
go

create index EmailIndex
    on Users (NormalizedEmail)
go

create index IX_Users_StoreId
    on Users (StoreId)
go

create unique index UserNameIndex
    on Users (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table Variants
(
    Id              int identity
        constraint PK_Variants
            primary key,
    ProductId       int           not null
        constraint FK_Variants_Products_ProductId
            references Products
            on delete cascade,
    Image           nvarchar(max),
    Code            nvarchar(max),
    Name            nvarchar(max) not null,
    Price           real,
    InStock         int,
    Options         nvarchar(max),
    Description     nvarchar(max),
    CreateOn        datetime2,
    ModifiedOn      datetime2,
    OrderLineItemId int
        constraint FK_Variants_OrderLineItems_OrderLineItemId
            references OrderLineItems,
    StoreProductId  int
        constraint FK_Variants_StoreProducts_StoreProductId
            references StoreProducts
)
go

create index IX_Variants_OrderLineItemId
    on Variants (OrderLineItemId)
go

create index IX_Variants_ProductId
    on Variants (ProductId)
go

create index IX_Variants_StoreProductId
    on Variants (StoreProductId)
go

create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null
        constraint PK___EFMigrationsHistory
            primary key,
    ProductVersion nvarchar(32)  not null
)
go

