using HotchocolateEndpoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotchocolateEndpoint.Services.Courses;

public class InstructorRepository
{
    private readonly IDbContextFactory<SchoolDbContext> _factory;

    public InstructorRepository(IDbContextFactory<SchoolDbContext> factory)
    {
        _factory = factory;
    }

    public async Task Create(Instructor instructor)
    {
        await using var context = await _factory.CreateDbContextAsync();
        context.Instructors.Add(instructor);
        await context.SaveChangesAsync();
    }
}