using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsWebAPI.Api;
using NewsWebAPI.Data;
using NewsWebAPI.Repositorys;
using NewsWebAPI.Repositorys.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    x.UseSqlServer(connectionString);
});

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

//Đăng ký repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IContentRepository, ContentRepository>();

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corspolicy");



app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    // Xác thực người dùng
    var authenticationResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

    if (!authenticationResult.Succeeded)
    {
        // Không có hoặc token không hợp lệ
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new MyResponse<string>(false, "Unauthorized. Token is missing or invalid.", "");

        // Chuyển đổi object response thành dạng JSON và gửi về client
        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);

        return;
    }

    // Token hợp lệ, tiếp tục xử lý middleware tiếp theo
    await next();
});


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseStaticFiles();

app.Run();
