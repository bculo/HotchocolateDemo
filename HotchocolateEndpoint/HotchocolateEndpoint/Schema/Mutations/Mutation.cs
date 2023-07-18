using System.Net;
using HotChocolate.Subscriptions;
using HotchocolateEndpoint.Schema.Queries;
using HotchocolateEndpoint.Schema.Subscriptions;

namespace HotchocolateEndpoint.Schema.Mutations;

public class Mutation
{
    private readonly List<CourseResult> _courses = new();

    public async Task<CourseResult> CreateCourse(CourseInputType request, [Service] ITopicEventSender topicEventSender)
    {
        CourseResult course = new()
        {
            Name = request.Name,
            Subject = request.Subject,
            Id = Guid.NewGuid(),
            InstructorId = request.InstructionId
        };
        
        _courses.Add(course);
        
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

        return course;
    }
    
    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType request, [Service] ITopicEventSender topicEventSender)
    {
        var course = _courses.FirstOrDefault(x => x.Id == id);

        if (course is null)
        {
            throw new GraphQLException(new Error($"Course with ID {id} not found", "COURSE_NOT_FOUND"));
        }

        course.Subject = request.Subject;
        course.Name = request.Name;
        course.InstructorId = request.InstructionId;

        string updateCourseTopic = $"{course.Id}_CourseUpdated";
        await topicEventSender.SendAsync(updateCourseTopic, course);
        
        return new CourseResult()
        {
            Name = course.Name,
            Subject = course.Subject,
            Id = course.Id
        };
    }

    public bool DeleteCourse(Guid id)
    {
        return _courses.RemoveAll(x => x.Id == id) > 0;
    }
}