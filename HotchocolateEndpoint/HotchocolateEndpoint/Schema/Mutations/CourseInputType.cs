using HotchocolateEndpoint.Schema.Queries;

namespace HotchocolateEndpoint.Schema.Mutations;

public class CourseInputType
{
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public Guid InstructionId { get; set; }
}