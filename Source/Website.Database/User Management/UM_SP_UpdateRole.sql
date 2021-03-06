USE [PortalModule]
GO
/****** Object:  StoredProcedure [dbo].[UM_SP_UpdateRole]    Script Date: 9/29/2017 3:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--	==========================================================================================
--	Author: Huynh Huu Loc
--	Create Date: 02-03-2017
--	Description: Update User roles information.
--	Parameters:
--		@UserID: User ID of User need to update.
--		@Roles: List new roles of User.
--	Return:
--		>0: Success
--		=0: Failure with Error Message.
--	==========================================================================================
ALTER PROCEDURE [dbo].[UM_SP_UpdateRole]
	@UserID int,
	@PreviousRoles varchar(1000),
	@Roles varchar(1000),
	@Remark nvarchar(500),
	@UserIDModify int,
	@DateTimeModify bigint
AS
BEGIN
	SET NOCOUNT ON;


	-- PROCESS
	begin transaction
	begin try
		-- REMOVE OLD ROLES
		delete from Portal.dbo.UserRoles
		where
			UserID = @UserID


		-- INSERT NEW ROLES
		insert into Portal.dbo.UserRoles
		(
			UserID, RoleID, ExpiryDate, IsTrialUsed, EffectiveDate,
			CreatedByUserID, CreatedOnDate, LastModifiedByUserID, LastModifiedOnDate,
			[Status], IsOwner
		)
		select
			@UserID, Value as RoleID, null, 1, null,
			@UserIDModify, GetDate(), @UserIDModify, GetDate(),
			1, 0
		from
			dbo.SYS_FN_SplitStringToIntegerTable(@Roles, ',')

				
		-- INSERT USER REQUEST
		-- RequestTypeID = 1 : Update Roles
		-- RequestStatus = 1 : Approved
		insert into dbo.UM_UserRequest
		(
			UserID, BranchID, NewBranchID, RequestTypeID, CurrentPermission, RequestPermission, RequestReason, RequestStatus,
			Remark, UserIDCreate, DateTimeCreate, UserIDModify, DateTimeModify, UserIDProcess, DateTimeProcess	
		)
		select
			UserID,
			BranchID,
			0 as NewBranchID,
			1 as RequestTypeID,
			@PreviousRoles as CurrentPermission,
			@Roles as RequestPermission,
			'' as RequestReason,
			1 as RequestStatus,
			@Remark, @UserIDModify, @DateTimeModify, 0, 0, @UserIDModify, @DateTimeModify
		from
			dbo.UM_UserBranch with(nolock)
		where
			UserID = @UserID


		-- INSERT USER LOG
		declare @UserRequestID as int = @@identity
		insert into dbo.UM_UserLog(UserID, UserRequestID, LogAction, LogDetail, Remark, UserIDModify, DateTimeModify)
		select @UserID, @UserRequestID, N'CẬP NHẬT QUYỀN', @Roles, @Remark, @UserIDModify, @DateTimeModify


		-- COMMIT AND RETURN
		commit transaction
		select @UserID
	end try

	begin catch
		rollback transaction
		insert into dbo.SYS_Exception(ErrorCode, ErrorMessage, StackTrace, DateTimeCreate)
		values(Error_Number(), Error_Message(), 'PortalModule.dbo.UM_SP_UpdateRole', @DateTimeModify)
		select -1
	end catch
END

