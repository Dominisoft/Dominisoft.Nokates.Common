# Dominisoft.Nokates.Common
## SQL Objects
```

CREATE TABLE [dbo].[RequestMetrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestTrackingId] [uniqueidentifier] NULL,
	[RequestSource] [nvarchar](500) NULL,
	[RemoteIp] [nvarchar](128) NULL,
	[RequestType] [nvarchar](10) NOT NULL,
	[ServiceName] [nvarchar](500) NOT NULL,
	[EndpointDesignation] [nvarchar](500) NOT NULL,
	[RequestPath] [nvarchar](2048) NOT NULL,
	[RequestJson] [nvarchar](max) NOT NULL,
	[ResponseJson] [nvarchar](max) NOT NULL,
	[ResponseCode] [int] NOT NULL,
	[RequestStart] [datetime] NOT NULL,
	[ResponseTime] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw7DayMetrics]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vw7DayMetrics]
as
select 
       ServiceName,
	   EndpointDesignation,
	   Count(*) RequestCount,
	   Min(RequestStart) FirstRequest,
	   Max(RequestStart) LastRequest,
	   DATEDIFF(hour,Min(RequestStart),getdate()) [Index],
	   Avg(responseTime) AverageResponseTime,
	   (select count(*) from RequestMetrics sq
	   where sq.ServiceName = MIN(RequestMetrics.ServiceName) and
	   datepart(hour,sq.RequestStart)=datepart(hour,Min(RequestMetrics.RequestStart))and
	   datepart(day,sq.RequestStart)= datepart(day,Min(RequestMetrics.RequestStart)) and
	   ResponseCode>399
	   ) Errors

from RequestMetrics
where RequestStart > Dateadd(day,-7,getdate())
group by Datepart(hour,RequestStart), datepart(day,RequestStart),ServiceName,endpointDesignation


--select * from LogEntrys
GO
/****** Object:  Table [dbo].[LogEntrys]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEntrys](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Source] [nvarchar](250) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepositoryMetrics]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepositoryMetrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestTrackingId] [uniqueidentifier] NULL,
	[ServiceName] [nvarchar](500) NOT NULL,
	[Query] [nvarchar](250) NOT NULL,
	[Request] [nvarchar](max) NOT NULL,
	[Response] [nvarchar](max) NOT NULL,
	[RequestStart] [datetime] NOT NULL,
	[ResponseTime] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](100) NOT NULL,
	[Description] [nchar](100) NOT NULL,
	[AllowedEndpoints] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nchar](50) NOT NULL,
	[LastName] [nchar](50) NOT NULL,
	[Username] [nchar](100) NOT NULL,
	[Email] [nchar](100) NULL,
	[PhoneNumber] [nchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[AdditionalEndpointPermissions] [nvarchar](max) NOT NULL,
	[Roles] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetEndpointMetricsByService]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[spGetEndpointMetricsByService]
@ServiceName nvarchar(500)
as
begin
select
EndpointDesignation [Name],
RequestCount,
FirstRequest,
LastRequest,
[Index],
AverageResponseTime,
Errors

from vw7DayMetrics
where serviceName = @ServiceName


end
GO
/****** Object:  StoredProcedure [dbo].[spGetMetricsByRequest]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spGetMetricsByRequest]
@requestId uniqueIdentifier
as
begin
select * from RequestMetrics
where RequestMetrics.RequestTrackingId = @requestId
order by RequestStart 
end
GO
/****** Object:  StoredProcedure [dbo].[spGetMetricsByService]    Script Date: 10/18/2022 8:17:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[spGetMetricsByService]
@ServiceName nvarchar(500)
as
begin
select
ServiceName [Name],
Sum(RequestCount)RequestCount,
Min(FirstRequest)FirstRequest,
max(LastRequest)LastRequest,
[Index],
avg(AverageResponseTime)AverageResponseTime,
sum(Errors)Errors

from vw7DayMetrics
where serviceName = @ServiceName
Group by ServiceName,[Index]

```
