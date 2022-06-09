using GlobalExceptionHandler;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using WhatsYourIdea.Application.Extension;
using WhatsYourIdea.Applications.Hasher;
using WhatsYourIdea.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var enviroment = builder.Environment;
var configuration = builder.Configuration;

var isSeed = configuration.GetValue<bool>("seed");

services.AddInfrastructure(configuration);
services.AddInfrastructureAuth(configuration);
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddApplication(configuration, enviroment);
services.AddApplicationAuth(configuration);
services.AddApplicationAutoMapper(configuration);
services.AddHasher(configuration);

services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
services.AddRouting(config =>
{
    config.LowercaseUrls = false;
    config.LowercaseQueryStrings = false;
});
services.Configure<JsonSerializerOptions>(x =>
{
    x.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
    x.WriteIndented = true;
});
services.AddMvc(config => config.EnableEndpointRouting = false);

var app = builder.Build();

if(enviroment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.ApplyMigrations(isSeed);
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseGlobalException(app.Services.GetRequiredService<IOptions<JsonSerializerOptions>>().Value);

app.UseAuthentication();
app.UseAuthorization();

app.UseMvcWithDefaultRoute();

app.Run();

public partial class Program { }