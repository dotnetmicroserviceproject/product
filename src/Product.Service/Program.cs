using common.Authentication;
using common.Filters.Swashbuckle;
using common.HealthChecks;
using common.Identity;
using common.MassTransit;
using common.MongoDB;
using common.Swagger;
using MassTransit;
using Product.Service;
using Product.Service.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMongo()
    .AddMongoRepository<Items>("Items")
     .AddJwtBearerAuthentication();
builder.Services.AddMassTransitWithMessageBroker(builder.Configuration, retryConfigurator =>
{
        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
});
   

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Policies.Read, policy =>
        {
            policy.RequireRole("Admin");
            policy.RequireClaim("scope", "product.readaccess", "product.fullaccess");
        });

        options.AddPolicy(Policies.Write, policy =>
        {
            policy.RequireRole("Admin");
            policy.RequireClaim("scope", "product.writeaccess", "product.fullaccess");
        });
    });

//MongoDB Health Check
builder.Services.AddHealthChecks()
                    .AddMongoDb();

builder.Services.AddApplicationConfig(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocs(
    title: "Product Microservice API",
    description: "Handles product management",
    version: "v1"
);
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<SnakeCaseDictionaryFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaFilter<SnakeCaseDictionaryFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(
     options =>
     {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service API v1");
     });

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapPlayEconomyHealthChecks();

app.Run();
