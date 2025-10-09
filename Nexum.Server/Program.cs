using Nexum.Server.DAC;
using Nexum.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DAC services
builder.Services.AddScoped<INexumConfigDAC, NexumConfigDAC>();
builder.Services.AddScoped<ICreditWalletDAC, CreditWalletDAC>();
builder.Services.AddScoped<IProductContactDAC, ProductContactDAC>();
builder.Services.AddScoped<IAccumulatedInterestDAC, AccumulatedInterestDAC>();
builder.Services.AddScoped<IStatementInterestDAC, StatementInterestDAC>();

// Register Service services
builder.Services.AddScoped<IInterestService, InterestService>();
builder.Services.AddScoped<IBillingService, BillingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
