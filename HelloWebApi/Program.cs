var builder = WebApplication.CreateBuilder(args);

//app.MapGet("/", () => "Hello World!");
//Serivce Container 
//Uygulama endpointlerin contoller ile y�nlendirilmesi sa�lan�yor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//uygulaman�n swagger servisini ekliyoruz.
builder.Services.AddSwaggerGen();

var app = builder.Build();

//E�er ortam de�i�keni (environment variable) development ise Swagger ile bizi kar��las�n diye configure yap�yoruz.
if (app.Environment.IsDevelopment())
{
    //uygulaman�n swagger kullanmas�n� aktifle�tiriyoruz.
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Uygulaman�n controller taraf�ndan y�nlendirilmesi maplenmesi sa�lan�yor.
app.MapControllers();

app.Run();
