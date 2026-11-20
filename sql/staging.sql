CREATE DATABASE AnaliticaOpiniones;
GO
USE AnaliticaOpiniones;
GO
CREATE TABLE dbo.StgOpiniones (
    StagingId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Fuente NVARCHAR(20) NOT NULL,
    IdOrigen NVARCHAR(100) NOT NULL,
    Cliente NVARCHAR(200) NULL,
    Producto NVARCHAR(200) NULL,
    Calificacion INT NULL CHECK (Calificacion BETWEEN 1 AND 5 OR Calificacion IS NULL),
    Comentario NVARCHAR(MAX) NULL,
    FechaExtraccion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE INDEX IX_StgOpiniones_Fuente_IdOrigen
ON dbo.StgOpiniones(Fuente, IdOrigen);
GO
-- Tabla de ejemplo para la fuente relacional:
-- CREATE TABLE dbo.Resenas (Id INT, Cliente NVARCHAR(200), Producto NVARCHAR(200),
-- Calificacion INT, Comentario NVARCHAR(MAX), Fecha DATETIME2);
