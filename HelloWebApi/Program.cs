var builder = WebApplication.CreateBuilder(args);

//app.MapGet("/", () => "Hello World!");
//Serivce Container 
//Uygulama endpointlerin contoller ile yönlendirilmesi saðlanýyor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//uygulamanýn swagger servisini ekliyoruz.
builder.Services.AddSwaggerGen();

var app = builder.Build();
//uygulamanýn swagger kullanmasýný aktifleþtiriyoruz.
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
