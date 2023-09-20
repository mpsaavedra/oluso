using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Data.EntityTypeConfigurations;

public class StudentLectureEntityTypeConfiguration : IEntityTypeConfiguration<StudentLecture>
{
    public void Configure(EntityTypeBuilder<StudentLecture> builder)
    {
        builder.HasOne<Student>(x => x.Student)
            .WithMany(x => x.Lectures)
            .HasForeignKey(x => x.StudentId);
        builder.HasOne(x => x.Lecture)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.LectureId);
    }
}