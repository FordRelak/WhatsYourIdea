using System.Reflection;
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

app.UseAuthentication();
app.UseAuthorization();

app.UseMvcWithDefaultRoute();

app.Run();