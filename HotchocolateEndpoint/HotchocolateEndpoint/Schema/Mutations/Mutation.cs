using System.Net;
using HotchocolateEndpoint.Schema.Queries;

namespace HotchocolateEndpoint.Schema.Mutations;

public class Mutation
{
    private readonly List<CourseType> _courses = new();

    public CourseResult CreateCourse(string name, Subject subject, Guid instructorId)
    {
        CourseType newInstructor = new()
        {
            Name = name,
            Subject = subject,
            Id = Guid.NewGuid(),
            Instructor = new InstructorType()
            {
                Id = instructorId
            }
        };
        
        _courses.Add(newInstructor);

        return new CourseResult()
        {
            Name = newInstructor.Name,
            Subject = newInstructor.Subject,
            Id = newInstructor.Id
        };
    }
    
    public CourseResult UpdateCourse(Guid id, string name, Subject subject, Guid instructorId)
    {
        var existingInstance = _courses.FirstOrDefault(x => x.Id == id);

        if (existingInstance is null)
        {
            throw new GraphQLException(new Error($"Course with ID {id} not found", "COURSE_NOT_FOUND"));
        }

        existingInstance.Subject = subject;
        existingInstance.Name = name;
        if (existingInstance.Instructor is not null)
        {
            existingInstance.Instructor.Id = instructorId;
        }
        
        return new CourseResult()
        {
            Name = existingInstance.Name,
            Subject = existingInstance.Subject,
            Id = existingInstance.Id
        };
    }

    public bool DeleteCourse(Guid id)
    {
        return _courses.RemoveAll(x => x.Id == id) > 0;
    }
}