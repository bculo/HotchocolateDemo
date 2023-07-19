
using HotChocolate.Subscriptions;
using HotchocolateEndpoint.Entities;
using HotchocolateEndpoint.Schema.Subscriptions;
using HotchocolateEndpoint.Services.Courses;

namespace HotchocolateEndpoint.Schema.Mutations;

public class Mutation
{
    private readonly CourseRepository _courseRepo;
    private readonly InstructorRepository _instructorRepository;

    public Mutation(CourseRepository courseRepo, InstructorRepository instructorRepository)
    {
        _courseRepo = courseRepo;
        _instructorRepository = instructorRepository;
    }

    public async Task<CourseResult> CreateCourse(CourseInputType request, 
        [Service] ITopicEventSender topicEventSender,
        CancellationToken token = default)
    {
        Course course = new()
        {
            Name = request.Name,
            Subject = request.Subject,
            Id = Guid.NewGuid(),
            InstructorId = request.InstructionId,
        };
        await _courseRepo.Create(course, token);
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course, token); 
        return new CourseResult()
        {
            Name = course.Name,
            Subject = course.Subject,
            Id = course.Id,
            InstructorId = course.InstructorId
        };
    }
    
    public async Task<CourseResult> UpdateCourse(Guid id, 
        CourseInputType request,
        [Service] ITopicEventSender topicEventSender,
        CancellationToken token)
    {
        var course = await _courseRepo.Find(id, token);
        if (course is null)
            throw new GraphQLException(new Error($"Course with ID {id} not found", "COURSE_NOT_FOUND"));

        course.Subject = request.Subject;
        course.Name = request.Name;
        course.InstructorId = request.InstructionId;
        await _courseRepo.Update(course, token);
        
        var updateCourseTopic = $"{course.Id}_CourseUpdated";
        await topicEventSender.SendAsync(updateCourseTopic, course, token);
        return new CourseResult()
        {
            Name = course.Name,
            Subject = course.Subject,
            Id = course.Id,
            InstructorId = course.InstructorId
        };
    }

    public async Task<bool> DeleteCourse(Guid id, CancellationToken token = default)
    {
        var course = await _courseRepo.Find(id, token);
        if (course is null)
            throw new GraphQLException(new Error($"Course with ID {id} not found", "COURSE_NOT_FOUND"));
        
        return await _courseRepo.Delete(course, token);
    }

    public async Task<InstructorResult> AddInstructor(InstructorInputType request, 
        CancellationToken token = default)
    {
        var instructor = new Instructor
        {
            Id = Guid.NewGuid(),
            Salary = 123.41,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        await _instructorRepository.Create(instructor);
        return new InstructorResult()
        {
            Id = instructor.Id
        };
    }
}