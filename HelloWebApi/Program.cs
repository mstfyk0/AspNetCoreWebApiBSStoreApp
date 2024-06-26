var builder = WebApplication.CreateBuilder(args);

//app.MapGet("/", () => "Hello World!");
//Serivce Container 
//Uygulama endpointlerin contoller ile yönlendirilmesi sağlanıyor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//uygulamanın swagger servisini ekliyoruz.
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Eğer ortam değişkeni (environment variable) development ise Swagger ile bizi karşılasın diye configure yapıyoruz.
if (app.Environment.IsDevelopment())
{
    //uygulamanın swagger kullanmasını aktifleştiriyoruz.
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Uygulamanın controller tarafından yönlendirilmesi maplenmesi sağlanıyor.
app.MapControllers();

app.Run();
