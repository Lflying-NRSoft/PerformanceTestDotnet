using Volo.Abp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ReplaceConfiguration(builder.Configuration);

// Add services to the container.
//builder.Services.AddAutofacServiceProviderFactory();
builder.Services.AddApplication<AppModule>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.UseAutofac();

var app = builder.Build();

app.InitializeApplication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseAutofac();

app.UseAuthorization();

//app.UseAuditing();

app.MapControllers();

app.Run();
