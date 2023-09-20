using Oluso.Data;

namespace Data.Models;

// always inherit from the BusinessEntity
// this class already has the id and audit data
// requirements implemented on it
public partial class Teacher : BusinessEntity<int, string>
{
    public Teacher()
    {
        Lectures = new HashSet<Lecture>();
    }
    public string Name { get; set; }
    
    public virtual ICollection<Lecture> Lectures { get; set; }
}