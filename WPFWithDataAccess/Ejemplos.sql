/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Nombre]
      ,[Apellidos]
      ,[Promedio]
  FROM [EscuelaDB].[Escuela].[Alumno]


  --INSERT INTO [Escuela].Alumno (Nombre, Apellidos, Promedio)
  --VALUES ('Angel','Espinoza Velasco', 10)

	--UPDATE [Escuela].Alumno
	--SET Promedio = 8
	--WHERE Id = 5

	--DELETE FROM [Escuela].Alumno WHERE Id = 7


  --EXEC [Escuela].[SaveAlumno] 'Isaac', 'Rodriguez', 9

  EXEC [Escuela].[GetAlumnoList]