var builder = WebApplication.CreateBuilder(args);

// ����������� ��������
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// ��������� ��������� ��������� ��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Tenders}/{action=Index}/{id?}");
});

app.Run();
