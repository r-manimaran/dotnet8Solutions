namespace EFCoreRelations.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //Navigation Property
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
