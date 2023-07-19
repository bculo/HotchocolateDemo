using HotchocolateEndpoint.Schema;
using HotchocolateEndpoint.Schema.Mutations;
using HotchocolateEndpoint.Schema.Queries;
using HotchocolateEndpoint.Schema.Subscriptions;
using HotchocolateEndpoint.Services;
using HotchocolateEndpoint.Services.Courses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();

var app = builder.Build();

var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();
app.MapGraphQL();

app.Run();