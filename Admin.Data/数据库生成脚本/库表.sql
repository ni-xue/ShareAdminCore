USE [master]
GO
/****** 库名:  Database [WebDB]    脚本 Date: 2021/6/15 16:27:07 ******/
CREATE DATABASE [WebDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebDB', FILENAME = N'D:\SqlBase\WebDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'WebDB_log', FILENAME = N'D:\SqlBase\WebDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WebDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WebDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WebDB] SET  MULTI_USER 
GO
ALTER DATABASE [WebDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [WebDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebDB', N'ON'
GO
ALTER DATABASE [WebDB] SET QUERY_STORE = OFF
GO
USE [WebDB]
GO
/****** Object:  User [User]    Script Date: 2021/6/15 16:27:08 ******/
CREATE USER [User] FOR LOGIN [User] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [User]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [User]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [User]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [User]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [User]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [User]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [User]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [User]
GO
/****** Object:  Table [dbo].[BaseLog]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaseLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BaseID] [int] NOT NULL,
	[BaseName] [nvarchar](50) NOT NULL,
	[ModuleType] [nvarchar](50) NOT NULL,
	[ModuleText] [nvarchar](max) NOT NULL,
	[BaseIp] [nvarchar](50) NOT NULL,
	[BaseTime] [datetime] NOT NULL,
 CONSTRAINT [PK_BaseLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaseUser]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaseUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BaseName] [nvarchar](50) NOT NULL,
	[BasePwd] [nvarchar](50) NOT NULL,
	[BaseRank] [varchar](50) NOT NULL,
	[BaseRankId] [int] NOT NULL,
	[Nullity] [int] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[IsIdent] [int] NULL,
	[LastLoginIP] [nvarchar](20) NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[FinalLoginIP] [nvarchar](20) NOT NULL,
	[FinalLoginDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BaseUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[V_BaseUser]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_BaseUser]
AS
SELECT   BaseName, BasePwd, BaseRank, BaseRankId, AddTime, Nullity, IsIdent,
                    (SELECT   COUNT(*) AS Expr1
                     FROM      dbo.BaseLog
                     WHERE   (BaseID = dbo.BaseUser.ID) AND (ModuleType = 'Login')) AS Logins, LastLoginIP, LastLoginDate, 
                FinalLoginIP, FinalLoginDate, ID
FROM      dbo.BaseUser
GO
/****** Object:  Table [dbo].[AllocationDirectory]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllocationDirectory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
	[PermissionType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AllocationDirectory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionDirectory]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionDirectory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Controller] [varchar](100) NULL,
	[Action] [varchar](100) NULL,
	[Title] [varchar](50) NOT NULL,
	[ImgStyle] [varchar](200) NULL,
	[Keywords] [varchar](200) NULL,
	[Sort_Id] [int] NOT NULL,
	[HideStatus] [int] NOT NULL,
	[Grades] [int] NOT NULL,
	[State] [int] NOT NULL,
	[Authority] [varchar](500) NOT NULL,
 CONSTRAINT [PK_PermissionDirectory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleDirectory]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleDirectory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[Descriptions] [varchar](50) NULL,
	[HomeState] [bit] NOT NULL,
 CONSTRAINT [PK_RoleDirectory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AllocationDirectory] ADD  CONSTRAINT [DF_AllocationDirectory_PermissionId]  DEFAULT ((0)) FOR [PermissionId]
GO
ALTER TABLE [dbo].[BaseLog] ADD  CONSTRAINT [DF_BaseLog_BaseTime]  DEFAULT (getdate()) FOR [BaseTime]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_Nullity]  DEFAULT ((0)) FOR [Nullity]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_IsIdent]  DEFAULT ((0)) FOR [IsIdent]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_LastLoginIP_1]  DEFAULT (N' ') FOR [LastLoginIP]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_LastLoginDate]  DEFAULT (getdate()) FOR [LastLoginDate]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_FinalLoginIP]  DEFAULT (N' ') FOR [FinalLoginIP]
GO
ALTER TABLE [dbo].[BaseUser] ADD  CONSTRAINT [DF_BaseUser_FinalLoginDate]  DEFAULT (getdate()) FOR [FinalLoginDate]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_ParentID]  DEFAULT ((0)) FOR [ParentID]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_Sort_Id]  DEFAULT ((99)) FOR [Sort_Id]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_HideStatus]  DEFAULT ((0)) FOR [HideStatus]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_Grades]  DEFAULT ((1)) FOR [Grades]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_State]  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[PermissionDirectory] ADD  CONSTRAINT [DF_PermissionDirectory_Action]  DEFAULT ('Show') FOR [Authority]
GO
ALTER TABLE [dbo].[RoleDirectory] ADD  CONSTRAINT [DF_RoleDirectory_HomeState]  DEFAULT ((0)) FOR [HomeState]
GO
/****** Object:  StoredProcedure [dbo].[NET_PM_GetMenuByUserID]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


----------------------------------------------------------------------
-- 菜单加载
CREATE PROC [dbo].[NET_PM_GetMenuByUserID]		
  AS

-- 属性设置
SET NOCOUNT ON

-- 角色标识
DECLARE @RoleID	INT

-- 执行逻辑
BEGIN
			-- 父级版块
			SELECT Id, Title, ImgStyle, Sort_Id, HideStatus, Grades, Authority
			FROM PermissionDirectory WITH(NOLOCK)
			WHERE  ParentID=0
			ORDER BY Sort_Id

			-- 二级版本
			SELECT  *
			FROM PermissionDirectory WITH(NOLOCK)
			WHERE  ParentID<>0 and Grades=2
			ORDER BY ParentID,Sort_Id
	       ---三级菜单
	        SELECT  *
			FROM PermissionDirectory WITH(NOLOCK)
			WHERE  ParentID<>0 and Grades=3
			ORDER BY ParentID,Sort_Id
END
RETURN 0




GO
/****** Object:  StoredProcedure [dbo].[NET_PM_GetPermission]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


----------------------------------------------------------------------------------------------------

-- 根据菜单ID获取菜单信息已经相关权限信息
CREATE PROCEDURE [dbo].[NET_PM_GetPermission]
    @strID INT,										-- 菜单ID
	@strErrorDescribe	NVARCHAR(127) OUTPUT			-- 输出信息
  AS

-- 属性设置
SET NOCOUNT ON

-- 基本信息
DECLARE @Id INT

-- 执行逻辑
BEGIN
	IF @strID = 0
	BEGIN
	SET @strErrorDescribe=N'角色ID不能为空！'
	RETURN 1;
	END

	DECLARE	@RoleID INT

    SELECT @RoleID = ISNULL((SELECT TOP(1)1 FROM  [dbo].[PermissionDirectory] WITH(NOLOCK) WHERE ID = @strID),0)

	IF @strID != -1 AND  @RoleID = 0
	BEGIN
	SET @strErrorDescribe=N'菜单记录不存在！'
	RETURN 2;
	END

	-- 父级版块
	SELECT Id, Title, ImgStyle, Sort_Id, HideStatus, Grades, Authority
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE ParentID=0 AND State = 0
	ORDER BY Sort_Id
	-- 二级版本
	SELECT Id, ParentID, Title, ImgStyle, Sort_Id, HideStatus, Grades, Authority
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE  ParentID<>0 and Grades=2 AND State = 0
	ORDER BY ParentID,Sort_Id

	-- 角色信息
	IF @RoleID > 0
	BEGIN
	SELECT TOP(1)* FROM  [dbo].[PermissionDirectory] WITH(NOLOCK) WHERE ID = @strID
	END
END
SET @strErrorDescribe=N'查询完成！'
RETURN 0




GO
/****** Object:  StoredProcedure [dbo].[NET_PM_GetPermissionByUserID]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


----------------------------------------------------------------------
CREATE PROC [dbo].[NET_PM_GetPermissionByUserID]
	@dwUserID INT							-- 管理员标识	
  AS

-- 属性设置
SET NOCOUNT ON

-- 角色标识
DECLARE @RoleID	INT

-- 执行逻辑
BEGIN
	-- 角色获取
	IF @dwUserID=1 SET @RoleID=1
	ELSE 
		SELECT @RoleID=BaseRankId FROM BaseUser WITH(NOLOCK) WHERE ID=@dwUserID

	-- 超级管理员
	IF @dwUserID=1 OR @RoleID=1
		BEGIN
			SELECT 1 as RoleId,ParentID,id as PermissionId,0 AS PermissionType 
			FROM PermissionDirectory 
			WHERE HideStatus=0
			ORDER BY ParentID,Sort_Id
		END
	ELSE
		BEGIN
			SELECT rip.RoleId as RoleId, m.ParentID, rip.PermissionId as PermissionId,PermissionType
			FROM AllocationDirectory AS rip, PermissionDirectory AS m
			WHERE rip.RoleId=@RoleID AND m.Id=rip.PermissionId AND m.HideStatus=0 
			ORDER BY m.ParentID,m.Sort_Id
		END
END
RETURN 0




GO
/****** Object:  StoredProcedure [dbo].[NET_PM_GetRoleInformation]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


----------------------------------------------------------------------------------------------------

-- 根据角色ID获取角色信息已经相关权限信息
CREATE PROCEDURE [dbo].[NET_PM_GetRoleInformation]
    @strUserID INT,										-- 代理id
	@strErrorDescribe	NVARCHAR(127) OUTPUT			-- 输出信息
  AS

-- 属性设置
SET NOCOUNT ON

-- 基本信息
DECLARE @Id INT

-- 执行逻辑
BEGIN
	IF @strUserID = 0
	BEGIN
	SET @strErrorDescribe=N'角色ID不能为空！'
	RETURN 1;
	END

	DECLARE	@RoleID INT

    SELECT @RoleID = ISNULL((SELECT TOP(1)1 FROM  [dbo].[RoleDirectory] WITH(NOLOCK) WHERE ID = @strUserID),0)

	IF @strUserID != -1 AND  @RoleID = 0
	BEGIN
	SET @strErrorDescribe=N'角色不存在！'
	RETURN 2;
	END

	IF @strUserID = 1
	BEGIN
		-- 父级版块
	SELECT Id, Title, ImgStyle, Sort_Id, HideStatus, Grades, Authority,(a.Authority) AS PermissionType,0  AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE ParentID=0
	ORDER BY Sort_Id
	-- 二级版本
	SELECT  *,(a.Authority) AS PermissionType,0  AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE  ParentID<>0 and Grades=2
	ORDER BY ParentID,Sort_Id
	--三级菜单
	SELECT  *,(a.Authority) AS PermissionType,0  AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE  ParentID<>0 and Grades=3
	ORDER BY ParentID,Sort_Id
	-- 角色信息
	IF @RoleID > 0
	BEGIN
	SELECT TOP(1)* FROM  [dbo].[RoleDirectory] WITH(NOLOCK) WHERE ID = @strUserID
	END
	SET @strErrorDescribe=N'超级管理组拥有所有权限！'
	RETURN 0;
	END

	-- 父级版块
	SELECT Id, Title, ImgStyle, Sort_Id, HideStatus, Grades, Authority,
	(SELECT PermissionType FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id) AS PermissionType,
	 ISNULL((SELECT Id FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id),0) AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE ParentID=0
	ORDER BY Sort_Id
	-- 二级版本
	SELECT  *,
	(SELECT PermissionType FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id) AS PermissionType,
	ISNULL((SELECT Id FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id),0) AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE  ParentID<>0 and Grades=2
	ORDER BY ParentID,Sort_Id
	--三级菜单
	SELECT  *,
	(SELECT PermissionType FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id) AS PermissionType,
	ISNULL((SELECT Id FROM AllocationDirectory WITH(NOLOCK) WHERE RoleId = @strUserID AND PermissionId = a.Id),0) AS ActionId
	FROM PermissionDirectory AS a WITH(NOLOCK)
	WHERE  ParentID<>0 and Grades=3
	ORDER BY ParentID,Sort_Id
	-- 角色信息
	IF @RoleID > 0
	BEGIN
	SELECT TOP(1)* FROM  [dbo].[RoleDirectory] WITH(NOLOCK) WHERE ID = @strUserID
	END
END
SET @strErrorDescribe=N'查询完成！'
RETURN 0




GO
/****** Object:  StoredProcedure [dbo].[WEB_PageView]    Script Date: 2021/6/15 16:27:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


                
                CREATE PROCEDURE [dbo].[WEB_PageView]
                	@IsSql			INT = 0,				-- 使用表名还是使用SQL执行分页（0：表名，1：SQL）
                	@TableName		NVARCHAR(max),			-- 表名
                	@ReturnFields	NVARCHAR(max) = '*',	-- 查询列数
                	@PageSize		INT = 10,				-- 每页数目
                	@PageIndex		INT = 1,				-- 当前页码
                	@Where			NVARCHAR(max) = '',		-- 查询条件
                	@Order			NVARCHAR(max),			-- 排序字段
                	@PageCount		INT OUTPUT,				-- 页码总数
                	@RecordCount	INT OUTPUT	        	-- 记录总数
                  AS
                
                --设置属性
                SET NOCOUNT ON
                
                -- 变量定义
                DECLARE @TotalRecord INT
                DECLARE @TotalPage INT
                DECLARE @CurrentPageSize INT
                DECLARE @TotalRecordForPageIndex INT
                
                BEGIN
                	IF @Where IS NULL SET @Where=N''
                	
                	-- 记录总数
                	DECLARE @countSql NVARCHAR(max)  
                	
					IF @IsSql = 1
					BEGIN
                		SET @countSql='SELECT @TotalRecord=Count(*) FROM ('+@TableName+') AS A '+@Where
                		EXECUTE sp_executesql @countSql,N'@TotalRecord int out',@TotalRecord OUT
                	END
					ELSE
                	IF @RecordCount IS NULL
                	BEGIN
                		SET @countSql='SELECT @TotalRecord=Count(*) FROM '+@TableName+' WITH(NOLOCK) '+@Where
                		EXECUTE sp_executesql @countSql,N'@TotalRecord int out',@TotalRecord OUT
                	END
                	ELSE
                	BEGIN
                		SET @TotalRecord=@RecordCount
                	END		
                	
                	SET @RecordCount=@TotalRecord
                	SET @TotalPage=(@TotalRecord-1)/@PageSize+1	
                	SET @CurrentPageSize=(@PageIndex-1)*@PageSize + 1
                
                	-- 返回总页数和总记录数
                	SET @PageCount=@TotalPage
                	SET @RecordCount=@TotalRecord
                	IF @PageCount IS NULL SET @PageCount = 0
                	IF @RecordCount IS NULL SET @RecordCount = 0
                
                	-- 返回记录
                	SET @TotalRecordForPageIndex=@PageIndex*@PageSize
                	
					IF @IsSql = 1
					BEGIN
						EXEC	('SELECT * FROM (SELECT '+@ReturnFields+', ROW_NUMBER() OVER ('+@Order+') AS PageView_RowNo
                			FROM ('+@TableName+ ') AS A ' + @Where +' ) AS TempPageViewTable
                			WHERE TempPageViewTable.PageView_RowNo BETWEEN '+@CurrentPageSize+' AND '+ @TotalRecordForPageIndex)
                	END
					ELSE
					BEGIN
                		EXEC	('SELECT * FROM (SELECT '+@ReturnFields+', ROW_NUMBER() OVER ('+@Order+') AS PageView_RowNo
                			FROM '+@TableName+ ' WITH(NOLOCK) ' + @Where +' ) AS TempPageViewTable
                			WHERE TempPageViewTable.PageView_RowNo BETWEEN '+@CurrentPageSize+' AND '+ @TotalRecordForPageIndex)
                	END	
                	
					--EXEC	('SELECT TOP (' + @PageSize +') *
     --           			FROM (SELECT TOP '+@TotalRecordForPageIndex+' '+@ReturnFields+', ROW_NUMBER() OVER ('+@Order+') AS PageView_RowNo
     --           			FROM '+@TableName+ ' WITH(NOLOCK) ' + @Where +' ) AS TempPageViewTable
     --           			WHERE TempPageViewTable.PageView_RowNo > 
     --           			'+@CurrentPageSize+' AND TempPageViewTable.PageView_RowNo <= 
     --           			'+ @TotalRecordForPageIndex)

	                --EXEC   ('SELECT  TOP 10 * 
	                --		  FROM (SELECT '+@ReturnFields+', ROW_NUMBER() OVER ('+@OrderBy+') AS PageView_RowNo 
	                --		  FROM   '+@TableName+ ' (NOLOCK) ' + @Where +' ) AS TempPageViewTable 
	                --		  WHERE PageView_RowNo BETWEEN '+@CurrentPageSize+' + 1 AND '+@TotalRecordForPageIndex)

                END
                RETURN 0
                





GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限分配标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllocationDirectory', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色Id关联' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllocationDirectory', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联权限目录Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllocationDirectory', @level2type=N'COLUMN',@level2name=N'PermissionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllocationDirectory', @level2type=N'COLUMN',@level2name=N'PermissionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'BaseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'BaseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块类型（如：update）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'ModuleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块名称（如：修改了什么东西？）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'ModuleText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'BaseIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseLog', @level2type=N'COLUMN',@level2name=N'BaseTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'BaseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'BasePwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理员等级（如：超级管理员）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'BaseRank'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'BaseRankId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0,正常 1 禁用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'Nullity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0未绑定 1 已绑定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'IsIdent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次登陆时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'LastLoginIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次登陆时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'LastLoginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后登陆时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'FinalLoginIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后登陆时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BaseUser', @level2type=N'COLUMN',@level2name=N'FinalLoginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级一级Id,若无父级 则为0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航关键字（关联操作权限）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Controller'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作权限目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Action'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片链接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'ImgStyle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点链接（非空）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Keywords'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Sort_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐藏状态 0显示，1隐藏' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'HideStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Grades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态（是菜单还是页面 0：菜单，1：页面）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作权限目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionDirectory', @level2type=N'COLUMN',@level2name=N'Authority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleDirectory', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleDirectory', @level2type=N'COLUMN',@level2name=N'RoleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleDirectory', @level2type=N'COLUMN',@level2name=N'Descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启首页权限（0为关闭，1为开启）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleDirectory', @level2type=N'COLUMN',@level2name=N'HomeState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "BaseUser"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_BaseUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'V_BaseUser'
GO
USE [master]
GO
ALTER DATABASE [WebDB] SET  READ_WRITE 
GO
