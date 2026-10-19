using Asibosoft.Api.Composition;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAsibosoftServices(builder.Configuration);

var app = builder.Build();

await app.UseAsibosoftPipelineAsync();

app.Run();
