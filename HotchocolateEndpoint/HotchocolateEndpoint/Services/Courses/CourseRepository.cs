using HotchocolateEndpoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotchocolateEndpoint.Services.Courses;

public class CourseRepository
{
    private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

    public CourseRepository(IDbContextFactory<SchoolDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Course> Find(Guid id, CancellationToken token = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(token);
        return context.Courses.FirstOrDefault(x => x.Id == id);
    }
    
    public async Task Create(Course course, CancellationToken token = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(token);
        context.Courses.Add(course);
        await context.SaveChangesAsync(token);
    }
    
    public async Task Update(Course course, CancellationToken token = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(token);
        context.Courses.Update(course);
        await context.SaveChangesAsync(token);
    }
    
    public async Task<bool> Delete(Course course, CancellationToken token = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(token);
        context.Courses.Remove(course);
        return (await context.SaveChangesAsync(token)) > 0;
    }
}