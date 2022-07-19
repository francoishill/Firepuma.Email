using System.Reflection;
using Firepuma.Email.Client;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Configuration, builder.Services);

var app = builder.Build();
ConfigureApp(app);
app.Run();

static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Example API",
            Description = "An example web api to demonstrate Email service",
            Contact = new OpenApiContact
            {
                Name = "Firepuma.Email",
                Url = new Uri("https://github.com/francoishill/Firepuma.Email")
            },
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    services.AddEmailServiceClient(configuration.GetSection("EmailService"));
}

static void ConfigureApp(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
}