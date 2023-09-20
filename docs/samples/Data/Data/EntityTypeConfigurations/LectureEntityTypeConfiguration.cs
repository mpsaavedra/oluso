using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oluso.Data.Extensions;

namespace Data.Data.EntityTypeConfigurations;

public class LectureEntityTypeConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        // configure BusinessEntity inherit properties
        builder.ConfigureBusinessEntity<int, string, Lecture>();
        
        // this class properties
        builder.HasOne<Teacher>(x => x.Teacher)
            .WithMany(x => x.Lectures)
            .HasForeignKey(x => x.TeacherId);
        builder.HasMany(x => x.Students)
            .WithOne(x => x.Lecture)
            .HasForeignKey(x => x.LectureId);
    }
}