namespace HotchocolateEndpoint.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public double GPA { get; set; }
    public virtual ICollection<Course> Courses { get; set; }
}