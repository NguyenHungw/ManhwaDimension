var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ===== Swagger config =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ==========================

var app = builder.Build();

// cau hinh firebase
//FirebaseApp.Create(new AppOptions
//{
//    Credential = GoogleCredential.FromFile(
//        builder.Configuration["Firebase:CredentialPath"]
//    )
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // ===== Enable Swagger =====
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // vào /swagger
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
