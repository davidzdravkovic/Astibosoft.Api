using Astibosoft.Api.Composition;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAstibosoftServices(builder.Configuration);

var app = builder.Build();

await app.UseAstibosoftPipelineAsync();

app.Run();
