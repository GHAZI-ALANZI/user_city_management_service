IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UserManagementDB')
BEGIN
    CREATE DATABASE UserManagementDB;
END
GO
