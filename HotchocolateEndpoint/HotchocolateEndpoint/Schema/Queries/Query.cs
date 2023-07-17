using Bogus;
using HotchocolateEndpoint.Schema.Queries;

namespace HotchocolateEndpoint.Schema.Queries;

public class Query
{
    private readonly Faker<StudentType> _studentFaker;
    private readonly Faker<InstructorType> _instructionFaker;
    private readonly Faker<CourseType> _courseFaker;
    
    public Query()
    {
        _studentFaker = new Faker<StudentType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.GPA, f => f.Random.Double(1, 4));
        
        _instructionFaker = new Faker<InstructorType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Salary, f => f.Random.Double(0, 50000));
        
        _courseFaker = new Faker<CourseType>()
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Name.JobTitle())
            .RuleFor(x => x.Subject, f => f.PickRandom<Subject>())
            .RuleFor(x => x.Instructor, f => _instructionFaker.Generate())
            .RuleFor(x => x.Students, f => _studentFaker.Generate(3));
    }
    
    [GraphQLDeprecated("This query is deprecated.")]
    public string Instructions => "Test test";

    public IEnumerable<CourseType> GetCourses()
    {
        return _courseFaker.Generate(5);
    }

    public CourseType GetCourseById(Guid id)
    {
        var instance = _courseFaker.Generate();
        instance.Id = id;
        return instance;
    }
}