Pet проект. 
Реализует гибкую запросную систему по различным полям выборки с сортировкой и различными отношениями между ними.

Для тестов задайте свою строку подключения в файле appsettings.json в секции ConnectionStrings > PhonesDB
Создайте бд командой Update-Database.

Часто используемые команды: 

Добавить миграцию
        Add-migration Initial -Project ExpressionTreeTest.DataAccess.MSSQL

Удалить миграцию		
        Remove-migration -Project ExpressionTreeTest.DataAccess.MSSQL 
		
Обновить БД
			Update-database
 