using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TimeX.Data;
using Microsoft.Extensions.Configuration;
using TimeX.Models;
using TimeX.Security;
using TimeX.Services;
using TimeX.Core.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using TimeX.Converters;

var modelbuilder = WebApplication.CreateBuilder(args);


// Add services to the container.

modelbuilder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
modelbuilder.Services.AddDbContext<TimeXDbContext>(options =>
options.UseSqlServer(modelbuilder.Configuration.GetConnectionString("DefaultConnection")));


modelbuilder.Services.AddEndpointsApiExplorer();
modelbuilder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

modelbuilder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<TimeXDbContext>()
    .AddDefaultTokenProviders();

modelbuilder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(modelbuilder.Configuration.GetSection("Token:Key").Value
            )),
            ValidateIssuer = false,
            ValidateAudience = false,

        };
    });
modelbuilder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


modelbuilder.Services.AddScoped<UserManager<IdentityUser>>();
modelbuilder.Services.AddScoped<RoleManager<IdentityRole>>();

modelbuilder.Services.AddScoped<IPasswordHasher,PasswordHasher>();
modelbuilder.Services.AddScoped<IAdminService,AdminServices>();
modelbuilder.Services.AddScoped<IBusinessService, BusinessServices>();
modelbuilder.Services.AddScoped<ICustomerService, CustomerService>();
modelbuilder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
modelbuilder.Services.AddScoped<IUserService, UserService>();
modelbuilder.Services.AddScoped<IRoleService, RoleServices>();
modelbuilder.Services.AddHttpContextAccessor();

var app = modelbuilder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // create a list of roles to seed
    var roles = new List<IdentityRole>
    {
        new IdentityRole { Name = "Admin" },
        new IdentityRole { Name = "Business" },
        new IdentityRole { Name = "Customer" }
    };

    // seed the roles
    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role.Name);

        if (!roleExists)
        {
            await roleManager.CreateAsync(role);
        }
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(options =>
    {
        options.WithOrigins("https://localhost:7133").AllowAnyHeader().AllowAnyMethod();
    });
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeX API v1");
    }); 
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
