using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Data.EntityTypeConfigurations;

public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // configure BusinessEntity inherit properties
        // this class properties
        builder.HasMany(x => x.Lectures)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentId);
    }
}