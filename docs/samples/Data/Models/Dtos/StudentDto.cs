using Oluso.Data.AutoMapping;

namespace Data.Models.Dtos;

public class StudentDto : IMapFrom<Student>
{
    public string Name { get; set; }
    
    public int Age { get; set; }
    
    public Gender Gender { get; set; }
}