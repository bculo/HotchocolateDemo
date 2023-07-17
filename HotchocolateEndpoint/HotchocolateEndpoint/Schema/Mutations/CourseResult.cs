using HotchocolateEndpoint.Schema.Queries;

namespace HotchocolateEndpoint.Schema.Mutations;

public class CourseResult
{
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public Guid Id { get; set; }
}