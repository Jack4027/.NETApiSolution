using OhpenApi.Services;
using OhpenContracts.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics.CodeAnalysis;

//Creating Builder object
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Using scoped allows the application to maintain a state for each individual request
// For each  HTTP request there will be 1 instantiation of the given service
// which will last for the duration of the request, this helps to avoid problems that 
// may occur when multiple requests are using the same service object such as unexpected
// alterations in the data or an unexpected sequence of operations being performed
builder.Services.AddScoped<IJobService, BaseJobService>();

//Adding swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configuring Swagger
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Adding and naming swagger endpoint
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ohpen API v1");
    });

// Mappping Controllers
app.MapControllers();

//redirect http => https
app.UseHttpsRedirection();

// allow app to access the files in wwwroot, not used currently
app.UseStaticFiles();

app.Run();
