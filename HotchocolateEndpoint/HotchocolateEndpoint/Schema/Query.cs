using Bogus;
using HotchocolateEndpoint.Models;

namespace HotchocolateEndpoint.Queries;

public class Query
{
    [GraphQLDeprecated("This query is deprecated.")]
    public string Instructions => "Test test";

    public IEnumerable<CourseType> GetCourses()
    {
        var studentFaker = new Faker<StudentType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.GPA, f => f.Random.Double(1, 4));

        var instructionFaker = new Faker<InstructorType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Salary, f => f.Random.Double(0, 50000));

        var faker = new Faker<CourseType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Name.JobTitle())
            .RuleFor(x => x.Subject, f => f.PickRandom<Subject>())
            .RuleFor(x => x.Instructor, f => instructionFaker.Generate())
            .RuleFor(x => x.Students, f => studentFaker.Generate(3));
            
        var data = faker.Generate(5);
        
        return data;
    }
}