# TODO

Es necesario conectar los datos de SonarQube, Gitlab y OpenProject
Conexión de Commit con Tarea con análisis de SonarQube.

Sería interesante capturar las actualizaciones de esos eventos. Hay dos opciones.
Polling o trabajar con webhooks.

Datos y enlaces
OpenProject -> MergeRequestId -> Commits -> SonarQube
SonarQube -> Commit -> Merge Request -> OP Task