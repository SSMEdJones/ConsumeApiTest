using AutoMapper;
using ConsumeApiTest.DataAccess.AutoMapper.MappingProfile;
using ConsumeApiTest.DataAccess.ConfiguratonSettings;
using ConsumeApiTest.DataAccess.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
//builder.Services.Configure<AppSettings>(configuration);
builder.Services.Configure<SSMWorkFlowSettings>(configuration.GetSection("ssmWorkFlowSettings"));

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new SSMWorkflowProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<ISSMWorkFlow, SSMWorkFlow>();
builder.Services.AddScoped<ISSMWorkFlowStakeholder, SSMWorkFlowStakeholder>();
builder.Services.AddScoped<ISSMWorkFlowStep, SSMWorkFlowStep>();
builder.Services.AddScoped<ISSMWorkFlowInstance, SSMWorkFlowInstance>();
builder.Services.AddScoped<ISSMWorkFlowStepOption, SSMWorkFlowStepOption>();
builder.Services.AddScoped<ISSMWorkFlowStepResponder, SSMWorkFlowStepResponder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
