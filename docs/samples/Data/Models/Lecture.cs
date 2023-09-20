using Oluso.Data;

namespace Data.Models;


// always inherit from the BusinessEntity
// this class already has the id and audit data
// requirements implemented on it
public partial class Lecture : BusinessEntity<int, string>
{
    public Lecture()
    {
        Students = new HashSet<StudentLecture>();
    }
    
    public string Subject { get; set; }
    
    public int TeacherId { get; set; }
    
    public virtual Teacher Teacher { get; set; }
    
    public virtual ICollection<StudentLecture> Students { get; set; }
}