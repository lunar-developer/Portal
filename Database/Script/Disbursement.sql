USE [PortalModule]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_UpdateDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_UpdateDisbursement]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_QueryDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_QueryDisbursement]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessManual]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_ProcessManual]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_ProcessDisbursement]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessCancel]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_ProcessCancel]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessAuto]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_ProcessAuto]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_InsertDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_InsertDisbursement]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_GetDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_GetDisbursement]
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_DeleteDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP PROCEDURE [dbo].[DB_SP_DeleteDisbursement]
GO
/****** Object:  Table [dbo].[DB_DisbursementStatus]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP TABLE [dbo].[DB_DisbursementStatus]
GO
/****** Object:  Table [dbo].[DB_DisbursementLog]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP TABLE [dbo].[DB_DisbursementLog]
GO
/****** Object:  Table [dbo].[DB_DisbursementAutoProcess]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP TABLE [dbo].[DB_DisbursementAutoProcess]
GO
/****** Object:  Table [dbo].[DB_Disbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
DROP TABLE [dbo].[DB_Disbursement]
GO
/****** Object:  Table [dbo].[DB_Disbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DB_Disbursement](
	[DisbursementID] [bigint] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NULL,
	[CustomerID] [varchar](20) NULL,
	[CustomerName] [nvarchar](100) NULL,
	[CurrencyCode] [varchar](3) NULL,
	[Amount] [decimal](18, 0) NULL,
	[DisbursementDate] [int] NULL,
	[DisbursementMethod] [varchar](2) NULL,
	[DisbursementPurpose] [nvarchar](500) NULL,
	[LoanMethod] [varchar](20) NULL,
	[CollectAmount] [decimal](18, 0) NULL,
	[ProcessDate] [int] NULL,
	[DisbursementStatus] [int] NULL,
	[CreateUserID] [int] NULL,
	[CreateDateTime] [bigint] NULL,
	[ModifyUserID] [int] NULL,
	[ModifyDateTime] [bigint] NULL,
	[IsPreapprove] [bit] NULL,
 CONSTRAINT [PK_DB_Disbursement] PRIMARY KEY CLUSTERED 
(
	[DisbursementID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DB_DisbursementAutoProcess]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DB_DisbursementAutoProcess](
	[DisbursementStatus] [int] NOT NULL,
	[Amount] [decimal](18, 0) NULL,
	[StartTime] [tinyint] NULL,
	[EndTime] [tinyint] NULL,
	[Remark] [nvarchar](100) NULL,
	[IsDisable] [bit] NULL,
	[ModifyUserID] [int] NULL,
	[ModifyDateTime] [bigint] NULL,
 CONSTRAINT [PK_DB_DisbursementAutoProcess] PRIMARY KEY CLUSTERED 
(
	[DisbursementStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DB_DisbursementLog]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DB_DisbursementLog](
	[DisbursementLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[DisbursementID] [bigint] NULL,
	[PreviousStatus] [int] NULL,
	[DisbursementStatus] [int] NULL,
	[Remark] [nvarchar](500) NULL,
	[IsSensitiveInfo] [bit] NULL,
	[ModifyUserID] [int] NULL,
	[ModifyDateTime] [bigint] NULL,
 CONSTRAINT [PK_DB_DisbursementLog] PRIMARY KEY CLUSTERED 
(
	[DisbursementLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DB_DisbursementStatus]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DB_DisbursementStatus](
	[DisbursementStatusID] [int] IDENTITY(1,1) NOT NULL,
	[DisbursementStatus] [int] NULL,
	[Remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_DB_DisbursementStatus] PRIMARY KEY CLUSTERED 
(
	[DisbursementStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[DB_DisbursementAutoProcess] ([DisbursementStatus], [Amount], [StartTime], [EndTime], [Remark], [IsDisable], [ModifyUserID], [ModifyDateTime]) VALUES (2, CAST(100000000 AS Decimal(18, 0)), 8, 17, N'SME Auto Approve Process', 0, 0, 0)
GO
INSERT [dbo].[DB_DisbursementAutoProcess] ([DisbursementStatus], [Amount], [StartTime], [EndTime], [Remark], [IsDisable], [ModifyUserID], [ModifyDateTime]) VALUES (3, CAST(200000000 AS Decimal(18, 0)), 8, 17, N'AML Auto Approve Process', 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[DB_DisbursementStatus] ON 

GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (1, 0, N'Mới tạo')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (2, 1, N'Chờ phê duyệt')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (3, 2, N'Đã chuyển đến Phòng SME')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (4, 3, N'Phòng SME đã duyệt')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (5, 4, N'Phòng AML đã duyệt')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (6, 5, N'Đã giải ngân')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (7, 11, N'Bị từ chối')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (8, 12, N'Yêu cầu Hủy')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (9, 13, N'Chờ Phòng SME Hủy yêu cầu')
GO
INSERT [dbo].[DB_DisbursementStatus] ([DisbursementStatusID], [DisbursementStatus], [Remark]) VALUES (10, 14, N'Đã Hủy')
GO
SET IDENTITY_INSERT [dbo].[DB_DisbursementStatus] OFF
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_DeleteDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DB_SP_DeleteDisbursement]
	@DisbursementID bigint
AS
BEGIN
	SET NOCOUNT ON;

	-- VALIDATE STATUS
	declare @Status as int = 0
	select
		@Status = DisbursementStatus
	from
		dbo.DB_Disbursement with(nolock)
	where
		DisbursementID = @DisbursementID

	if @Status > 0
	begin
		select 0 --Can not delete request has been processed
	end


	-- DELETE
	delete from dbo.DB_Disbursement where DisbursementID = @DisbursementID
	delete from dbo.DB_DisbursementLog where DisbursementID = @DisbursementID
	select 1 --Successful
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_GetDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DB_SP_GetDisbursement]
	@DisbursementID bigint	
AS
BEGIN
	SET NOCOUNT ON;

	-- Information
	select
		*
	from
		dbo.DB_Disbursement with(nolock)
	where
		DisbursementID = @DisbursementID


	-- Log
	select
		*
	from
		dbo.DB_DisbursementLog with(nolock)
	where
		DisbursementID = @DisbursementID
	order by
		ModifyDateTime desc, DisbursementLogID desc
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_InsertDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DB_SP_InsertDisbursement]
	@BranchID int,
	@CustomerID varchar(20),
	@CustomerName nvarchar(100),
	@CurrencyCode varchar(3),
	@Amount decimal,
	@DisbursementDate int,
	@DisbursementMethod varchar(2),
	@DisbursementPurpose nvarchar(500),
	@LoanMethod varchar(20),
	@CollectAmount decimal,
	@DisbursementStatus int,
	@CreateUserID int,
	@CreateDateTime bigint
AS
BEGIN
	SET NOCOUNT ON;

	begin transaction
	begin try

		-- Insert main table
		insert into dbo.DB_Disbursement (
			BranchID, CustomerID, CustomerName, CurrencyCode, Amount,
			DisbursementDate, DisbursementMethod, DisbursementPurpose,
			LoanMethod, CollectAmount, DisbursementStatus, CreateUserID, CreateDateTime,
			ProcessDate, IsPreapprove, ModifyUserID, ModifyDateTime
		)
		values (
			@BranchID, @CustomerID, @CustomerName, @CurrencyCode, @Amount,
			@DisbursementDate, @DisbursementMethod, @DisbursementPurpose,
			@LoanMethod, @CollectAmount, @DisbursementStatus, @CreateUserID, @CreateDateTime,
			0, 0, 0, 0
		)
			
		-- Insert log
		declare @DisbursementID as bigint = @@identity
		insert into dbo.DB_DisbursementLog
			(DisbursementID, PreviousStatus, DisbursementStatus, Remark, IsSensitiveInfo, ModifyUserID, ModifyDateTime)
		values
			(@DisbursementID, @DisbursementStatus, @DisbursementStatus, '', 0, @CreateUserID, @CreateDateTime)


		commit transaction;
		select @DisbursementID, 'Successful';
	end try
	-----------------------
	begin catch
		rollback transaction
		select -1, error_message()
	end catch
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessAuto]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DB_SP_ProcessAuto]
	@ResponseCode int output,
	@DisbursementID varchar(max) output,
	@CurrentStatus int,
	@DisbursementStatus int,	
	@ModifyUserID int,
	@ModifyDateTime bigint
AS
BEGIN
	SET NOCOUNT ON;

	-- PREPARE DATA
	declare @StartTime as int
	declare @EndTime as int
	declare @Amount as decimal = -1
	select
		@Amount = Amount,
		@StartTime = StartTime,
		@EndTime = EndTime		
	from
		dbo.DB_DisbursementAutoProcess with(nolock)	
	where
		DisbursementStatus = @CurrentStatus	
	

	-- VALIDATE CONDITION
	declare @Hour as int = DatePart(hour, GetDate())
	if @Amount = -1 or @Hour < @StartTime or @Hour > @EndTime
	begin
		set @ResponseCode = 0
		return;
	end


	-- FILTER LIST AUTO PROCESS
	declare @Table as table (		
		DisbursementID bigint		
	)
	insert into @Table (DisbursementID)
	select
		DisbursementID
	from
		dbo.FN_SplitStringToBigIntegerTable(@DisbursementID, ',')
	inner join
		dbo.DB_Disbursement with(nolock)
			on  DisbursementID = Value
			and DisbursementStatus = @CurrentStatus
			and Amount <= @Amount

	
	-- VALIDATE
	declare @count as int = 0
	select
		@count = count(1)
	from
		@Table

	if @count = 0
	begin
		return;
	end


	-- AUTO PROCESS
	set @DisbursementID =
		stuff
		(
			(
				select
					',' + convert(varchar, DisbursementID) 
				from
					@Table			
				for xml path(''), type
			).value('.', 'varchar(max)') , 1, 1, ''
		)
	declare @Remark as nvarchar(500) = N'Hệ thống tự động duyệt số tiền <= ' + dbo.SYS_FN_FormatAmount(@Amount)
	declare @ResponseTable as table
	(
		ResponseCode int,		
		ResponseMessage varchar(1000),
		ErrorCode int
	)
	insert into @ResponseTable(ResponseCode, ResponseMessage, ErrorCode)
	execute dbo.DB_SP_ProcessManual @DisbursementID, @CurrentStatus, @DisbursementStatus,
		@Remark, 1, @ModifyUserID, @ModifyDateTime


	-- VALIDATE AUO PROCESS
	select @ResponseCode = ResponseCode from @ResponseTable
	if @ResponseCode = -1	-- FAILURE WITH ERROR MESSAGE
	begin
		insert into dbo.SYS_Exception (ErrorCode, ErrorMessage, StackTrace, CreateDateTime)
		select ErrorCode, ResponseMessage, 'dbo.DB_SP_ProcessAuto', dbo.SYS_FN_GetCurrentDateTime() from @ResponseTable
	end 
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessCancel]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DB_SP_ProcessCancel]
	@DisbursementID bigint,	
	@DisbursementStatus int,
	@Remark nvarchar(500),
	@ModifyUserID int,
	@ModifyDateTime bigint	
AS
BEGIN
	SET NOCOUNT ON;

	-- VALIDATE	
	declare @IsProcess as int = 0
	select
		@IsProcess = 1
	from
		dbo.DB_Disbursement with(nolock)
	where
		DisbursementStatus = 5 -- Finished
	and DisbursementID = @DisbursementID
	
	if @IsProcess = 1
		begin
			select 0, 'Request has been disbursed, can not cancel'
			return;
		end


	-- REQUEST CANCEL / APPROVE REQUEST
	begin transaction
	begin try
		-- Prepare Data
		declare @CurrentStatus as int = 0
		declare @IsPreapprove as bit = 0
		select
			@CurrentStatus = DisbursementStatus,
			@IsPreapprove = IsPreapprove
		from
			dbo.DB_Disbursement with(nolock)
		where
			DisbursementID = @DisbursementID


		-- If IsPreapprove = 0
		-- Skip step status = 13 (Approve and Wait for Cancel), update to status = 14 (Canceled)
		if @DisbursementStatus = 13 and @IsPreapprove = 0
		begin
			set @DisbursementStatus = 14
		end


		-- Process Cancel
		update
			dbo.DB_Disbursement
		set
			DisbursementStatus = @DisbursementStatus
		where
			DisbursementID = @DisbursementID



		-- Insert log
		insert into dbo.DB_DisbursementLog (
			DisbursementID, PreviousStatus, DisbursementStatus, Remark,
			IsSensitiveInfo, ModifyUserID, ModifyDateTime)
		values (
			@DisbursementID, @CurrentStatus, @DisbursementStatus, @Remark,
			0, @ModifyUserID, @ModifyDateTime)


		-- Commit & Return
		commit transaction
		select 1, 'successful'
	end try

	begin catch
		rollback transaction
		select -1, error_message()
	end catch
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DB_SP_ProcessDisbursement]
	@DisbursementID varchar(max),
	@CurrentStatus int,
	@DisbursementStatus int,
	@Remark nvarchar(500),
	@IsSensitiveInfo bit,
	@ModifyUserID int,
	@ModifyDateTime bigint
AS
BEGIN
	SET NOCOUNT ON;

	-- INFO TABLE
	declare @ResponseTable as table
	(
		ResponseCode int,
		ResponseMessage varchar(1000),
		ErrorCode int
	)

	-- PROCESS MANUAL
	insert into @ResponseTable(ResponseCode, ResponseMessage, ErrorCode)
	execute dbo.DB_SP_ProcessManual @DisbursementID, @CurrentStatus, @DisbursementStatus,
		@Remark, @IsSensitiveInfo, @ModifyUserID, @ModifyDateTime
	select * from @ResponseTable
	if (select count(1) from @ResponseTable where ResponseCode > 0) = 0	-- FAILURE WITH ERROR MESSAGE
	begin	
		return;
	end
	
	
	-- AUTO PROCESS
	declare @ResponseCode as int = 0

	-- PREAPPROVE -- SME AUTO APPROVE
	if @DisbursementStatus = 2
	begin
		set @CurrentStatus = @DisbursementStatus
		set @DisbursementStatus = @DisbursementStatus + 1
		set @ModifyUserID = 1
		set @ModifyDateTime = dbo.SYS_FN_GetCurrentDateTime()

		execute dbo.DB_SP_ProcessAuto @ResponseCode output, @DisbursementID output, @CurrentStatus, @DisbursementStatus,
			@ModifyUserID, @ModifyDateTime

		if @ResponseCode <= 0
		begin
			return;
		end
	end


	-- APPROVE - AML AUTO APPROVE
	if @DisbursementStatus = 3	
	begin
		set @CurrentStatus = @DisbursementStatus
		set @DisbursementStatus = @DisbursementStatus + 1
		set @ModifyUserID = 1
		set @ModifyDateTime = dbo.SYS_FN_GetCurrentDateTime()

		execute dbo.DB_SP_ProcessAuto @ResponseCode output, @DisbursementID output, @CurrentStatus, @DisbursementStatus,
			@ModifyUserID, @ModifyDateTime

		if @ResponseCode <= 0
		begin
			return;
		end
	end
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_ProcessManual]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Huynh Huu Loc
-- Description:	Process disbursement to next status
-- =============================================
CREATE PROCEDURE [dbo].[DB_SP_ProcessManual]
	@DisbursementID varchar(max),
	@CurrentStatus int,
	@DisbursementStatus int,
	@Remark nvarchar(500),
	@IsSensitiveInfo bit,
	@ModifyUserID int,
	@ModifyDateTime bigint
AS
BEGIN
	SET NOCOUNT ON;

	-- PREPARE DATA
	declare @Table as table (		
		DisbursementID bigint		
	)
	insert into @Table (DisbursementID)
	select
		DisbursementID
	from
		dbo.FN_SplitStringToBigIntegerTable(@DisbursementID, ',')
	inner join
		dbo.DB_Disbursement with(nolock)
			on  DisbursementID = [Value]
			and DisbursementStatus = @CurrentStatus


	-- VALIDATE CURRENT STATUS
	declare @Count as int = 0
	select
		@Count = count(1)
	from
		@Table
	
	if @Count = 0
	begin
		-- 'Not found any valid request => stop process'
		select 
			0 as ResponseCode,
			'Status has changed' as ResponseMessage,
			0 as ErrorCode
		return;
	end


	-- PROCESS
	begin transaction
	begin try
		-- Update to next status
		update
			dbo.DB_Disbursement	
		set
			DisbursementStatus = @DisbursementStatus,
			IsPreapprove =	case 
								when @DisbursementStatus = 3 then 1 -- Update flag Preapprove when SME has been approved
								else IsPreapprove -- Keep old value
							end
		where
			DisbursementID in (select DisbursementID from @Table)


		-- Insert log
		insert into dbo.DB_DisbursementLog (
			DisbursementID, PreviousStatus, DisbursementStatus, Remark,
			IsSensitiveInfo, ModifyUserID, ModifyDateTime)
		select
			DisbursementID, @CurrentStatus, @DisbursementStatus, @Remark,
			@IsSensitiveInfo, @ModifyUserID, @ModifyDateTime
		from
			@Table
		

		-- Commit & Return
		commit transaction
		select 
			@Count as ResponseCode,
			'Successful' as ResponseMessage,
			0 as ErrorCode
	end try

	begin catch
		rollback transaction
		select 
			-1 as ResponseCode,
			error_message() as ResponseMessage,
			error_number() as ErrorCode
	end catch
END
GO
/****** Object:  StoredProcedure [dbo].[DB_SP_QueryDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Huynh Huu Loc
-- Description:	Query list disbursement request
-- =============================================
CREATE PROCEDURE [dbo].[DB_SP_QueryDisbursement]
	@CustomerID varchar(20),
	@CustomerName nvarchar(100),
	@BranchID int,
	@DisbursementStatus int,
	@ProcessDate int,
	@CurrencyCode varchar(3)
AS
BEGIN
	SET NOCOUNT ON;

	-- SET DEFAULT PARAMETERS
	set @CustomerID = isNull(@CustomerID, '')
	set @CustomerName = isNull(@CustomerName, '')
	set @BranchID = isNull(@BranchID, -2)
	set @DisbursementStatus = isNull(@DisbursementStatus, -1)
	set @ProcessDate = isNull(@ProcessDate, -1)
	set @CurrencyCode = isNull(@CurrencyCode, '')


	select
		DisbursementID,
		BranchID,
		CustomerID,
		CustomerName,
		CurrencyCode,
		Amount,
		DisbursementMethod,
		DisbursementPurpose,
		DisbursementDate,
		DisbursementStatus
	from
		dbo.DB_Disbursement with(nolock)
	where
		@CustomerID in ('', CustomerID)
	and (
			@CustomerName = '' 
			or 
			CustomerName like '%' + @CustomerName + '%' COLLATE Latin1_General_CI_AI
		)
	and @BranchID in (-2, @BranchID)
	and @DisbursementStatus in (-1, DisbursementStatus)
	and @ProcessDate in (-1, ProcessDate)
	and @CurrencyCode in ('', CurrencyCode)
END

GO
/****** Object:  StoredProcedure [dbo].[DB_SP_UpdateDisbursement]    Script Date: 6/23/2017 5:11:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DB_SP_UpdateDisbursement]
	@DisbursementID bigint,	
	@CustomerID varchar(20),
	@CustomerName nvarchar(100),
	@CurrencyCode varchar(3),
	@Amount decimal,
	@DisbursementDate int,
	@DisbursementMethod varchar(2),
	@DisbursementPurpose nvarchar(500),
	@LoanMethod varchar(20),
	@CollectAmount decimal,	
	@ModifyUserID int,
	@ModifyDateTime bigint
AS
BEGIN
	SET NOCOUNT ON;

	begin try
		-- update main table
		if (exists(select 1 from dbo.DB_Disbursement with(nolock) where DisbursementID = @DisbursementID))
			begin
				begin transaction

				update dbo.DB_Disbursement
				set					
					CustomerID = @CustomerID,
					CustomerName = @CustomerName,
					CurrencyCode = @CurrencyCode,
					Amount = @Amount,
					DisbursementDate = @DisbursementDate,
					DisbursementMethod = @DisbursementMethod,
					DisbursementPurpose = @DisbursementPurpose,
					LoanMethod = @LoanMethod,
					CollectAmount = @CollectAmount,
					ModifyUserID = @ModifyUserID, 
					ModifyDateTime = @ModifyDateTime
				where
					DisbursementID = @DisbursementID; 

				commit transaction;
				select 1, 'Update Successful';
			end
		else
			begin
				select 0, N'Disbursement does not exist, please verify.';
			end		
	end try
	-----------------------
	begin catch
		rollback transaction
		select -1, error_message()
	end catch
END
GO
