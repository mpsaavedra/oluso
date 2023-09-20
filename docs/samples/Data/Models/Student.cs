using Oluso.Data;

namespace Data.Models;

// always inherit from the BusinessEntity
// this class already has the id and audit data
// requirements implemented on it
public partial class Student : BusinessEntity<int, string>
{
    public Student()
    {
        Lectures = new HashSet<StudentLecture>();
        Gender = Gender.NotSpecified;
    }
    
    public string Name { get; set; }
    
    public int Age { get; set; }
    
    public Gender Gender { get; set; }
    
    public virtual ICollection<StudentLecture> Lectures { get; set; }
}