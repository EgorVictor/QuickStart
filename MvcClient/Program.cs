using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
// ȷ���Ƿ�ʹ����վ��������ͼ�ı�־��Ĭ��ֵ��
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Add services to the container.
builder.Services.AddControllersWithViews();
//�������֤������ӵ� DI
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5000";

        //�� UserInfo �˵��ȡ����
        options.Scope.Add("profile");
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;

        options.Scope.Add("api1");
        options.Scope.Add("offline_access");
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //RequireAuthorization �������ö�����Ӧ�ó�����������ʡ�
    endpoints.MapDefaultControllerRoute().RequireAuthorization();
});
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
