USE [master]
GO

CREATE DATABASE [SM_BD]
GO

USE [SM_BD]
GO

CREATE TABLE [dbo].[Error](
	[IdError] [bigint] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Error] [nvarchar](max) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Origen] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Error] PRIMARY KEY CLUSTERED 
(
	[IdError] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Rol](
	[IdRol] [smallint] IDENTITY(1,1) NOT NULL,
	[NombreRol] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [nvarchar](20) NOT NULL,
	[NombreCompleto] [nvarchar](250) NOT NULL,
	[CorreoElectronico] [nvarchar](100) NOT NULL,
	[Contrasenna] [nvarchar](100) NOT NULL,
	[Estado] [bit] NOT NULL,
	[IdRol] [smallint] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Rol] ON 
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol]) VALUES (1, N'Usuario')
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol]) VALUES (2, N'Administrador')
GO
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO

SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
INSERT [dbo].[Usuario] ([IdUsuario], [Identificacion], [NombreCompleto], [CorreoElectronico], [Contrasenna], [Estado], [IdRol]) VALUES (1, N'304590415', N'EDUARDO JOSE CALVO CASTILLO', N'ecalvo90415@ufide.ac.cr', N'$2a$11$UzSyimvpPrlqsvTipYE9Re8xxeA5wUOambfhE86E.z/wAR.v3i.DG', 1, 1)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [Identificacion], [NombreCompleto], [CorreoElectronico], [Contrasenna], [Estado], [IdRol]) VALUES (2, N'603700873', N'FLOR DE LIZ MARTINEZ GARCIA', N'flordelismg07@gmail.com', N'$2a$11$YDQKwTpbQMvWF5RR5OJXT.3FdYRTk5aHXDlKetL7DmNKMcVPUlwHi', 1, 2)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [Identificacion], [NombreCompleto], [CorreoElectronico], [Contrasenna], [Estado], [IdRol]) VALUES (3, N'118110028', N'HERMES JOSUE QUESADA MONTERO', N'hquesada10028@ufide.ac.cr', N'$2a$11$RZri2u8f9zWoppBTFfLcsu869JzJEZHk6W2BQN9hrOP1Ycninx91.', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO

ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [UK_Usuario_CorreoElectronico] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [UK_Usuario_Identificacion] UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarContrasenna] 
	@UsuarioId		int,
	@Contrasenna	nvarchar(100)
AS
BEGIN
	
	UPDATE Usuario
	SET	Contrasenna = @Contrasenna
	WHERE IdUsuario = @UsuarioId

END
GO

CREATE PROCEDURE [dbo].[sp_ConsultarIdentificacion]
	@Identificacion	nvarchar(20)
AS
BEGIN
	
	DECLARE @EstadoActivo BIT = 1

	SELECT	IdUsuario,Identificacion,NombreCompleto,CorreoElectronico,Estado
	FROM	dbo.Usuario
	WHERE	Identificacion = @Identificacion
		AND Estado = 1

END
GO

CREATE PROCEDURE [dbo].[sp_IniciarSesionUsuario]
	@CorreoElectronico	nvarchar(100)
AS
BEGIN
	
	DECLARE @EstadoActivo BIT = 1

	SELECT	IdUsuario,Identificacion,NombreCompleto,CorreoElectronico,
			Estado,Contrasenna, R.IdRol, NombreRol
	FROM	dbo.Usuario U
	INNER JOIN dbo.Rol R ON U.IdRol = R.IdRol
	WHERE	CorreoElectronico = @CorreoElectronico
		AND Estado = 1

END
GO

CREATE PROCEDURE [dbo].[sp_RegistroErrores] 
	@IdUsuario int, 
	@Error     nvarchar(max), 
	@Origen	   nvarchar(100)
AS
BEGIN
	
    INSERT INTO dbo.Error (IdUsuario,Error,FechaHora,Origen)
    VALUES (@IdUsuario, @Error, GETDATE(), @Origen)
	
END
GO

CREATE PROCEDURE [dbo].[sp_RegistroUsuarios] 
	@Identificacion		nvarchar(20),
	@NombreCompleto		nvarchar(250),
	@CorreoElectronico	nvarchar(100),
	@Contrasenna		nvarchar(100)
AS
BEGIN
	
	DECLARE @EstadoActivo BIT = 1
	DECLARE @RolUsuario INT = 1

	IF NOT EXISTS(
		SELECT	1 FROM dbo.Usuario
		WHERE	Identificacion = @Identificacion
			OR	CorreoElectronico = @CorreoElectronico)
	BEGIN

		INSERT INTO dbo.Usuario(Identificacion,NombreCompleto,CorreoElectronico,Contrasenna,Estado,IdRol)
		VALUES(@Identificacion,@NombreCompleto,@CorreoElectronico,@Contrasenna,@EstadoActivo,@RolUsuario)

	END
END
GO