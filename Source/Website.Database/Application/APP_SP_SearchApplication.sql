USE [PortalModule]
GO
/****** Object:  StoredProcedure [dbo].[APP_SP_SearchApplication]    Script Date: 9/30/2017 12:26:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Huynh Huu Loc
-- Description:	Search application information
-- =============================================
ALTER PROCEDURE [dbo].[APP_SP_SearchApplication]
	@InputName01 varchar(50) = NULL,
	@InputName02 varchar(50) = NULL,
	@SelectName01 varchar(50) = NULL,
	@SelectName02 varchar(50) = NULL,
	@SelectName03 varchar(50) = NULL,
	@DateField varchar(50) = NULL,

	@InputValue01 varchar(max) = NULL,
	@InputValue02 varchar(max) = NULL,
	@SelectValue01 varchar(50) = NULL,
	@SelectValue02 varchar(50) = NULL,
	@SelectValue03 varchar(50) = NULL,
	@DateFrom bigint = NULL,
	@DateTo bigint = NULL
AS
BEGIN
	SET NOCOUNT ON;


	-- SET DEFAULT
	set @InputName01 = isNull(@InputName01, '')
	set @InputName02 = isNull(@InputName02, '')
	set @SelectName01 = isNull(@SelectName01, '')
	set @SelectName02 = isNull(@SelectName02, '')
	set @SelectName03 = isNull(@SelectName03, '') 
	set @DateField = isNull(@DateField, '')
	
	set @InputValue01 = isNull(@InputValue01, '')
	set @InputValue02 = isNull(@InputValue02, '')
	set @SelectValue01 = isNull(@SelectValue01, '')
	set @SelectValue02 = isNull(@SelectValue02, '')
	set @SelectValue03 = isNull(@SelectValue03, '') 
	set @DateFrom = isNull(@DateFrom, 0)
	set @DateTo = isNull(@DateTo, 0)


	-- QUERY COLUMN LIST	
	declare @column as varchar(max) =
	stuff
	(
		(
			select
				',' + QuoteName(FieldName) 
            from
				APP_ApplicationField with(nolock)
			where
				TableName in (
					'APP_ApplicationFieldInteger',
					'APP_ApplicationFieldBigInteger',
					'APP_ApplicationFieldString',
					'APP_ApplicationFieldUnicodeString'
				)
			order by FieldName
            for xml path(''), type
        ).value('.', 'varchar(max)') , 1, 1, ''
	)


	-- BUILD SQL	
	declare @sql as nvarchar(max) = '
		select * into #DataTable from
		(
			select
				*
			from
			(
				select
					APP_ApplicationFieldString.ApplicationID as ID,
					APP_ApplicationField.FieldName,
					case
						when APP_ApplicationFieldInteger.FieldName is not null 
							then convert(varchar, APP_ApplicationFieldInteger.FieldValue)
						when APP_ApplicationFieldBigInteger.FieldName is not null 
							then convert(varchar, APP_ApplicationFieldInteger.FieldValue)
						when APP_ApplicationFieldString.FieldName is not null 
							then APP_ApplicationFieldString.FieldValue
						when APP_ApplicationFieldUnicodeString.FieldName is not null 
							then APP_ApplicationFieldUnicodeString.FieldValue
						else ''''
					end as FieldValue
				from
					dbo.APP_ApplicationField with(nolock)
				left join
					dbo.APP_ApplicationFieldInteger with(nolock)
						on  APP_ApplicationFieldInteger.FieldName = APP_ApplicationField.FieldName						
				left join
					dbo.APP_ApplicationFieldBigInteger with(nolock)
						on  APP_ApplicationFieldBigInteger.FieldName = APP_ApplicationField.FieldName						
				left join
					dbo.APP_ApplicationFieldString with(nolock)
						on  APP_ApplicationFieldString.FieldName = APP_ApplicationField.FieldName						
				left join
					dbo.APP_ApplicationFieldUnicodeString with(nolock)
						on  APP_ApplicationFieldUnicodeString.FieldName = APP_ApplicationField.FieldName						
				where
					APP_ApplicationField.TableName in (
						''APP_ApplicationFieldInteger'',
						''APP_ApplicationFieldBigInteger'',
						''APP_ApplicationFieldString'',
						''APP_ApplicationFieldUnicodeString''
					)
			) as Source
			pivot
			(
				max(FieldValue)
				for FieldName in (' + @column + ')
			) as PivotTable
		) as APP_Extension


		select
			APP_Application.*,
			' + @column + '
		from
			dbo.APP_Application with(nolock)		
		inner join
			#DataTable as APP_Extension
				on APP_Extension.ID = APP_Application.ApplicationID
		where
			1 = 1'
		

	-- ADD QUERY CONDITION
	if @InputName01 <> ''
		begin			
			set @sql = @sql + ' and ' + @InputName01 + ' in (' + @InputValue01 + ')'
		end
	if @InputName02 <> ''
		begin
			set @sql = @sql + ' and ' + @InputName02 + ' in (' + @InputValue02 + ')'
		end
	if @DateField <> ''
		begin
			set @sql = @sql + ' and ' + @DateField + ' between ' + @DateFrom + ' and ' + @DateTo
		end
	if @SelectName01 <> ''
		begin
			set @sql = @sql + ' and ' + @SelectName01 + ' = ''' + @SelectValue01 + ''''
		end
	if @SelectName02 <> ''
		begin
			set @sql = @sql + ' and ' + @SelectName02 + ' = ''' + @SelectValue02 + ''''
		end
	if @SelectName03 <> ''
		begin
			set @sql = @sql + ' and ' + @SelectName03 + ' = ''' + @SelectValue03 + ''''
		end


	-- SET ORDER BY
	set @sql += ' order by DateTimeCreate desc'

	-- RETURN RESULT
	execute sp_executesql @sql
END

