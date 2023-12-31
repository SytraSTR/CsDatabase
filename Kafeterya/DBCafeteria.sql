/* 'Execute'ye bastığınız zaman "The connection is broken" gibi bir hata verebilir, Lütfen tekrar basın. Eğer düzelmezse "İnstagram = Sytra.cs"den ulaşabilirsiniz. */

CREATE DATABASE [CafeteriaDB];
GO


USE [CafeteriaDB];
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CafeteriaMenu](
	[FoodIncentives] [varchar](15) NULL,
	[FoodName] [varchar](25) NULL
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CafeteriaOrder](
	[RoomNumber] [varchar](5) NULL,
	[Username] [varchar](25) NULL,
	[FoodPiece] [int] NULL,
	[FoodIncentives] [varchar](15) NULL,
	[FoodName] [varchar](25) NULL,
	[OrderTime] [varchar](20) NULL
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CafeteriaToOrder](
	[RoomNumber] [varchar](5) NULL,
	[UserName] [varchar](25) NULL,
	[FoodPiece] [int] NULL,
	[FoodIncentives] [varchar](15) NULL,
	[FoodName] [varchar](25) NULL,
	[OrderTime] [varchar](20) NULL
) ON [PRIMARY]
GO
