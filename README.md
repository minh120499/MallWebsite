# Mục tiêu của project

Tạo một website của trung tâm mua sắm (mall) để quảng cáo công trình và tiện ích của họ. (construction and features).

Thông tin ABCD-Mall:

- Nằm ở ngoại ô thành phố Mumbai và được mở bởi công ty ABCD Developers Pvt Ltd.
- Trung tâm thương mại có diện tích 250.000 sq ft (23.000 m2) với mặt tiền dài 700 ft (210 m).
- Nó được thiết kế bởi Cooper và các đối tác của họ.
- Trung tâm mua sắm có 4 tầng, được điều hòa không khí, bao gồm một diện tích khoảng 80.000 sq ft (7.000 m2) trên mỗi tầng.
- Khu đất bao gồm một cơ sở đỗ xe 7 tầng.

# Yêu cầu thiết kế website

- Trang chủ:

  - Ở giữa trang, nó phải hiển thị hình ảnh của trung tâm mua sắm kèm thông tin liên quan về nó.

  - Các liên kết phải được hiển thị để điều hướng trang web và khám phá các tính năng của trung tâm mua sắm. (nav bar).

- Các tính năng và chức năng:

  - Phải có một mô-đun riêng để sử dụng các tính năng của quản trị viên và khách hàng.

  - Quản trị viên sẽ có khả năng chỉnh sửa dữ liệu của trung tâm mua sắm như chỉnh sửa tên cửa hàng, thêm, xóa và xoá. Khi thêm chi tiết, một số hình ảnh cần được thêm vào trang web.

  - Quản trị viên cũng có khả năng chỉnh sửa thông tin về phim của trung tâm mua sắm.

  - Khách hàng sẽ có khả năng xem các tính năng và chỉnh sửa phần mong muốn của họ.

  - Trung tâm mua sắm có tính năng ưu đãi cho phép khách hàng đặt vé xem phim trực tuyến thông qua trang web.

  - Thu thập phản hồi từ khách hàng.

  - Khu vực bộ sưu tập sẽ được tải lên bởi quản trị viên và bao gồm các hình ảnh về các sản phẩm khác nhau có sẵn.

# Chức năng của website

## Admin:

- Quản trị viên sẽ phải đăng nhập vào trang web để chỉnh sửa các tính năng.

- Sẽ có quyền THÊM-SỬA-XOÁ nội dung như trung tâm mua sắm (kèm danh sách một số mặt hàng nó sở hữu), quầy hàng trong khu ẩm thực (có thể thêm một số mặt hàng).

- Có thể thêm thông tin vào phần bộ sưu tập, nơi trình bày các hình ảnh về các sản phẩm của các thương hiệu hàng đầu hoặc hình ảnh thức ăn.

- Có thể xem phản hồi từ người dùng khách hàng.

- Quản trị viên sẽ có quyền THÊM-SỬA-XOÁ các phim đang chiếu trong rạp đa rạp cũng như số ghế có sẵn phải được thêm vào.

- Có thể xem các vé đã đặt bởi khách hàng.

## Client:

- Khách hàng có thể xem trang web nhưng không có phần đăng nhập.

- Họ có thể xem dữ liệu như danh sách trung tâm **mua sắm**, **khu ẩm thực**, **bộ sưu tập**, **thông tin liên hệ**.

- Họ có thể cung cấp phản hồi.

- Họ có thể đặt vé trực tuyến bằng cách cung cấp thông tin thẻ (lưu ý nếu tất cả các ghế đã được đặt thì không cho phép đặt vé).

- Có thể tìm kiếm tên các cửa hàng thương hiệu có sẵn, cũng như trong thực đơn khu ẩm thực.

- Có thể xem sơ đồ toàn bộ trung tâm mua sắm.

- Có thể xem chi tiết liên hệ.

```sql
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

create table Users
(
    Id                   nvarchar(450) not null
        constraint PK_Users
            primary key,
    Phone                nvarchar(max),
    FullName             nvarchar(max) not null,
    Address              nvarchar(max),
    Status               nvarchar(max),
    CreateOn             datetime2     not null,
    ModifiedOn           datetime2     not null,
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

create unique index UserNameIndex
    on Users (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table __EFMigrationsHistory
(
    MigrationId    nvarchar(150) not null
        constraint PK___EFMigrationsHistory
            primary key,
    ProductVersion nvarchar(32)  not null
)
go

-- Cyclic dependencies found

create table Banners
(
    Id         int identity
        constraint PK_Banners
            primary key,
    Name       nvarchar(max) not null,
    Image      nvarchar(max),
    Status     nvarchar(max),
    CreateOn   datetime2     not null,
    ModifiedOn datetime2     not null
)
go

create table Banners
(
    Id         int identity
        constraint PK_Banners
            primary key,
    Name       nvarchar(max) not null,
    Image      nvarchar(max),
    Status     nvarchar(max),
    CreateOn   datetime2,
    ModifiedOn datetime2
)
go


```
