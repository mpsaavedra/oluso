using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class StudentLecture
{
    [Key]
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public virtual Student Student { get; set; }
    
    public int LectureId { get; set; }
    
    public virtual Lecture Lecture { get; set; }
}