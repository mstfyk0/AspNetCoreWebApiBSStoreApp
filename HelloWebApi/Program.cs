var builder = WebApplication.CreateBuilder(args);

//app.MapGet("/", () => "Hello World!");
//Serivce Container 
//Uygulama endpointlerin contoller ile y�nlendirilmesi sa�lan�yor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//uygulaman�n swagger servisini ekliyoruz.
builder.Services.AddSwaggerGen();

var app = builder.Build();
//uygulaman�n swagger kullanmas�n� aktifle�tiriyoruz.
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
