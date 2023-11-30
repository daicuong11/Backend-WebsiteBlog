using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsWebAPI.Api;
using NewsWebAPI.Data;
using NewsWebAPI.Enums;
using NewsWebAPI.Helpers;
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN", policy => policy.RequireRole(Role.ADMIN.ToString()));
    options.AddPolicy("EDITOR", policy => policy.RequireRole(Role.EDITOR.ToString()));
    options.AddPolicy("AUTHOR", policy => policy.RequireRole(Role.AUTHOR.ToString()));
    options.AddPolicy("GUEST", policy => policy.RequireRole(Role.GUEST.ToString()));
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
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ILoveRepository, LoveRepository>();
builder.Services.AddScoped<ISaveArticleRepository, SaveArticleRepository>();

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

//app.Use(async (context, next) =>
//{
//    await next();

//    if (context.Response.StatusCode == 401)
//    {
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsync(JsonSerializer.Serialize(new MyResponse<string>(false, "Xác thực không thành công", "")));
//    }
//});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseStaticFiles();

app.Run();
