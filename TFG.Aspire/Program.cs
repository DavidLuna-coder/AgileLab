using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var sql = builder.AddSqlServer("localdb");
var sqldb = sql.AddDatabase("db");

var api = builder.AddProject<Projects.TFG>("tfgapi");

builder.Build().Run();
