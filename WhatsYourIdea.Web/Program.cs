using System.Reflection;
using WhatsYourIdea.Application.Extension;
using WhatsYourIdea.Applications.Hasher;
using WhatsYourIdea.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var enviroment = builder.Environment;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddInfrastructureAuth(configuration);
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddApplication(configuration);
services.AddApplicationAuth(configuration);
services.AddApplicationAutoMapper(configuration);
services.AddHasher(configuration);

services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
services.AddRouting(config => config.LowercaseUrls = true);
services.AddMvc(config => config.EnableEndpointRouting = false);

var app = builder.Build();

if(enviroment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.ApplyMigrations();
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