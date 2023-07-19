using HotchocolateEndpoint.Enums;

namespace HotchocolateEndpoint.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public Guid InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public virtual ICollection<Student> Students { get; set; }
}