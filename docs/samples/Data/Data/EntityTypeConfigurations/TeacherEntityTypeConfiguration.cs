using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oluso.Data.Extensions;

namespace Data.Data.EntityTypeConfigurations;

public class TeacherEntityTypeConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // configure BusinessEntity properties
        builder.ConfigureBusinessEntity<int, string, Teacher>();
        
        // this class properties
        builder.HasMany<Lecture>(x => x.Lectures)
            .WithOne(x => x.Teacher)
            .HasForeignKey(x => x.TeacherId);
    }
}