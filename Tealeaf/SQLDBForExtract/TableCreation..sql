-- <Migration ID="ea3d6c8f-fb52-4fcb-9947-2fe78c0fc9e4" />



Print 'Create Table [dbo].[TLWEB_APPDATA_8X]'
GO
CREATE TABLE [dbo].[TLWEB_APPDATA_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_AP__0C1B4A4DC447FA3C] to [dbo].[TLWEB_APPDATA_8X]'
GO
ALTER TABLE [dbo].[TLWEB_APPDATA_8X]
	ADD
	CONSTRAINT [PK__TLWEB_AP__0C1B4A4DC447FA3C]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_APP] on [dbo].[TLWEB_APPDATA_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_APP]
	ON [dbo].[TLWEB_APPDATA_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_APPDATA_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_ATTRIBUTE_8X]'
GO
CREATE TABLE [dbo].[TLWEB_ATTRIBUTE_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[ID]                    [int] NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_AT__0C1B4A4DFAEF25A3] to [dbo].[TLWEB_ATTRIBUTE_8X]'
GO
ALTER TABLE [dbo].[TLWEB_ATTRIBUTE_8X]
	ADD
	CONSTRAINT [PK__TLWEB_AT__0C1B4A4DFAEF25A3]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_ATT] on [dbo].[TLWEB_ATTRIBUTE_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_ATT]
	ON [dbo].[TLWEB_ATTRIBUTE_8X] ([SESSION_KEY])
GO


ALTER TABLE [dbo].[TLWEB_ATTRIBUTE_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_COOKIE_8X]'
GO
CREATE TABLE [dbo].[TLWEB_COOKIE_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](128) NULL,
		[VALUE]                 [nvarchar](256) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_CO__0C1B4A4D904E06F4] to [dbo].[TLWEB_COOKIE_8X]'
GO
ALTER TABLE [dbo].[TLWEB_COOKIE_8X]
	ADD
	CONSTRAINT [PK__TLWEB_CO__0C1B4A4D904E06F4]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_COK] on [dbo].[TLWEB_COOKIE_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_COK]
	ON [dbo].[TLWEB_COOKIE_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_COOKIE_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_DIMENSIONS_8X]'
GO
CREATE TABLE [dbo].[TLWEB_DIMENSIONS_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[EVENT_ID]              [bigint] NOT NULL,
		[DIM_GRP_ID]            [bigint] NOT NULL,
		[FACT_ID]               [bigint] NOT NULL,
		[FACT_VALUE]            [nvarchar](256) NULL,
		[DIMENSION_1]           [nvarchar](256) NULL,
		[DIMENSION_2]           [nvarchar](256) NULL,
		[DIMENSION_3]           [nvarchar](256) NULL,
		[DIMENSION_4]           [nvarchar](256) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_DI__0C1B4A4D439B2032] to [dbo].[TLWEB_DIMENSIONS_8X]'
GO
ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [PK__TLWEB_DI__0C1B4A4D439B2032]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Add Default Constraint [DF__TLWEB_DIM__DIM_G__1DE57479] to [dbo].[TLWEB_DIMENSIONS_8X]'
GO
ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__DIM_G__1DE57479]
	DEFAULT ((0)) FOR [DIM_GRP_ID]
GO


Print 'Add Default Constraint [DF__TLWEB_DIM__EVENT__1CF15040] to [dbo].[TLWEB_DIMENSIONS_8X]'
GO
ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__EVENT__1CF15040]
	DEFAULT ((0)) FOR [EVENT_ID]
GO


Print 'Add Default Constraint [DF__TLWEB_DIM__FACT___1ED998B2] to [dbo].[TLWEB_DIMENSIONS_8X]'
GO
ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X]
	ADD
	CONSTRAINT [DF__TLWEB_DIM__FACT___1ED998B2]
	DEFAULT ((0)) FOR [FACT_ID]
GO


Print 'Create Index [IDX_EFC] on [dbo].[TLWEB_DIMENSIONS_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_EFC]
	ON [dbo].[TLWEB_DIMENSIONS_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_DIMENSIONS_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_EVENT_8X]'
GO
CREATE TABLE [dbo].[TLWEB_EVENT_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[EVENT_ID]              [bigint] NOT NULL,
		[TEXT_FOUND]            [nvarchar](512) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_EV__0C1B4A4D96984469] to [dbo].[TLWEB_EVENT_8X]'
GO
ALTER TABLE [dbo].[TLWEB_EVENT_8X]
	ADD
	CONSTRAINT [PK__TLWEB_EV__0C1B4A4D96984469]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Add Default Constraint [DF__TLWEB_EVE__EVENT__1A14E395] to [dbo].[TLWEB_EVENT_8X]'
GO
ALTER TABLE [dbo].[TLWEB_EVENT_8X]
	ADD
	CONSTRAINT [DF__TLWEB_EVE__EVENT__1A14E395]
	DEFAULT ((0)) FOR [EVENT_ID]
GO


Print 'Create Index [IDX_EV8] on [dbo].[TLWEB_EVENT_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_EV8]
	ON [dbo].[TLWEB_EVENT_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_EVENT_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_HIT_8X]'
GO
CREATE TABLE [dbo].[TLWEB_HIT_8X] (
		[TAB_KEY]                 [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]             [varchar](32) NULL,
		[HIT_KEY]                 [varchar](32) NULL,
		[TLTHID]                  [varchar](32) NULL,
		[HTTP_SECURE]             [varchar](1) NULL,
		[HIT_DURATION]            [bigint] NULL,
		[HIT_NUMBER]              [bigint] NULL,
		[REFERER]                 [nvarchar](512) NULL,
		[REQ_TIMESTAMP]           [datetime] NULL,
		[REQ_END_TIMESTAMP]       [datetime] NULL,
		[RSP_START_TIMESTAMP]     [datetime] NULL,
		[RSP_ACK_TIMESTAMP]       [datetime] NULL,
		[RSP_TIMESTAMP]           [datetime] NULL,
		[REQ_SIZE]                [bigint] NULL,
		[RSP_SIZE]                [bigint] NULL,
		[REQ_CANCELLED]           [varchar](1) NULL,
		[RSP_TTFB]                [bigint] NULL,
		[RSP_TTLB]                [bigint] NULL,
		[RSP_TTLA]                [bigint] NULL,
		[CON_SPEED]               [bigint] NULL,
		[CON_TYPE]                [nvarchar](20) NULL,
		[QUERY_STRING]            [nvarchar](512) NULL,
		[HTTP_STATUS]             [bigint] NULL,
		[URL]                     [nvarchar](256) NULL,
		[REQ_METHOD]              [char](20) NULL,
		[RSP_TYPE]                [char](64) NULL,
		[HOST_NAME]               [varchar](100) NULL,
		[HOST_IP]                 [varchar](40) NULL,
		[WS_GEN]                  [bigint] NULL,
		[NT_GEN]                  [bigint] NULL,
		[RT_GEN]                  [bigint] NULL,
		[ACCEPT_LANG]             [nvarchar](64) NULL,
		[PAGE_RENDER]             [bigint] NULL,
		[PAGE_DWELL]              [bigint] NULL,
		[SESSION_TIMESTAMP]       [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_HI__0C1B4A4DB4BA2027] to [dbo].[TLWEB_HIT_8X]'
GO
ALTER TABLE [dbo].[TLWEB_HIT_8X]
	ADD
	CONSTRAINT [PK__TLWEB_HI__0C1B4A4DB4BA2027]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_HIT] on [dbo].[TLWEB_HIT_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_HIT]
	ON [dbo].[TLWEB_HIT_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_HIT_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_SESSION_8X]'
GO
CREATE TABLE [dbo].[TLWEB_SESSION_8X] (
		[TAB_KEY]                 [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]             [varchar](32) NULL,
		[REMOTE_ADDRESS]          [varchar](40) NULL,
		[CANISTER_LSSN]           [varchar](256) NULL,
		[CANISTER_SESSION_ID]     [bigint] NULL,
		[TLTSID]                  [varchar](32) NULL,
		[TLTUID]                  [varchar](32) NULL,
		[TLTVID]                  [varchar](32) NULL,
		[CANISTER_SERVER]         [varchar](64) NULL,
		[SESSION_TIMESTAMP]       [datetime] NULL,
		[SESSION_DURATION]        [bigint] NULL,
		[HIT_COUNT]               [bigint] NULL,
		[HTTP_USER_AGENT]         [nvarchar](256) NULL,
		[EXTRACTID]               [bigint] NULL,
		[TEALEAF_REPLAY]          [varchar](128) NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_SE__0C1B4A4D347F0A32] to [dbo].[TLWEB_SESSION_8X]'
GO
ALTER TABLE [dbo].[TLWEB_SESSION_8X]
	ADD
	CONSTRAINT [PK__TLWEB_SE__0C1B4A4D347F0A32]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_SES] on [dbo].[TLWEB_SESSION_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_SES]
	ON [dbo].[TLWEB_SESSION_8X] ([SESSION_KEY])
GO


ALTER TABLE [dbo].[TLWEB_SESSION_8X] SET (LOCK_ESCALATION = TABLE)
GO


Print 'Create Table [dbo].[TLWEB_URLFIELD_8X]'
GO
CREATE TABLE [dbo].[TLWEB_URLFIELD_8X] (
		[TAB_KEY]               [bigint] IDENTITY(1, 1) NOT NULL,
		[SESSION_KEY]           [varchar](32) NULL,
		[HIT_KEY]               [varchar](32) NULL,
		[NAME]                  [nvarchar](256) NULL,
		[VALUE]                 [nvarchar](1024) NULL,
		[SESSION_TIMESTAMP]     [datetime] NULL
)
GO


Print 'Add Primary Key [PK__TLWEB_UR__0C1B4A4D4B4D027D] to [dbo].[TLWEB_URLFIELD_8X]'
GO
ALTER TABLE [dbo].[TLWEB_URLFIELD_8X]
	ADD
	CONSTRAINT [PK__TLWEB_UR__0C1B4A4D4B4D027D]
	PRIMARY KEY
	CLUSTERED
	([TAB_KEY])
GO


Print 'Create Index [IDX_UFD] on [dbo].[TLWEB_URLFIELD_8X]'
GO
CREATE NONCLUSTERED INDEX [IDX_UFD]
	ON [dbo].[TLWEB_URLFIELD_8X] ([SESSION_KEY], [HIT_KEY])
GO


ALTER TABLE [dbo].[TLWEB_URLFIELD_8X] SET (LOCK_ESCALATION = TABLE)
GO


