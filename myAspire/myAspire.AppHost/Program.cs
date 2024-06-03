using Projects;

var builder = DistributedApplication.CreateBuilder(args);


//var sql = builder.AddSqlServer("sql");
//var sqldb = sql.AddDatabase("sqldb");

var admin = builder
    .AddProject<ESCenter_Administrator>("admin");

var client = builder
    .AddProject<ESCenter_Client>("client");

var api = builder
    .AddProject<ESCenter_Api>("api");

builder.Build().Run();