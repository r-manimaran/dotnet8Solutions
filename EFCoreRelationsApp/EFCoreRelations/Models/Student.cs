namespace EFCoreRelations.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }

    //Navigation Property
    public ICollection<StudentCourse> StudentCourses { get; set; }

}
