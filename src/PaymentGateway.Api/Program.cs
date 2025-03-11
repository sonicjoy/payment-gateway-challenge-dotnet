using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation;

using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Helpers;
using PaymentGateway.Api.Services.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add CORS policy for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Payment Gateway API",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepository>();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>();

builder.Services.AddHttpClient<IAcquiringBankClient, AcquiringBankClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AcquiringBank:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Running in Development mode");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API v1");
        // Make swagger accessible at the root
        c.RoutePrefix = "";
    });
}
else
{
    Console.WriteLine("Not running in Development mode");
}

// Use CORS before other middleware
app.UseCors("ReactAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
