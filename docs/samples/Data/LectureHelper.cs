using AutoMapper;
using Data.Data;
using Data.Models;
using Data.Models.Dtos;
using Data.Repositories;
using Data.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oluso.Data.Extensions;
using Oluso.Extensions;
using Shouldly;

namespace Data;

public class LectureHelper
{
    public async Task RunTests(IServiceProvider hostProvider)
    {
        try
        {
            using var svrScope = hostProvider.CreateScope();
            var provider = svrScope.ServiceProvider;

            Info("Preparing tp execute tests");
            Info("retrieving services from dependency container", 2);
            var teachersRepo = provider.GetRequiredService<ITeacherRepository>();
            var studentsRepo = provider.GetRequiredService<IStudentRepository>();
            var lecturesRepo = provider.GetRequiredService<ILectureRepository>();

            Info("building specification objects", 2);
            var isMale = new StudentIsMale();
            var isUnder21 = new StudentIsUnder21();

            Info("querying with repo using Linq queries", 2);
            (await teachersRepo.CountAsync()).ShouldBe(2);
            (await lecturesRepo.AnyAsync()).ShouldBeTrue();
            (await studentsRepo.Query.Where(x => x.Age > 21).CountAsync()).ShouldBe(4);
            var lectureTeacher = studentsRepo.GetEntity<Student>()
                .Include(x => x.Lectures)
                .ThenInclude(x => x.Lecture)
                .ThenInclude(x => x.Teacher)
                .First(x => x.Id == 1);
            
            (from teacher in teachersRepo.Query where teacher.Lectures.Any() select teacher).Count().ShouldBe(2);


            Info("querying with repo using specifications", 2);
            studentsRepo.Query.AsEnumerable().Where(isMale).Count().ShouldBe(7);
            studentsRepo.Query.AsEnumerable().Where(isUnder21).Count().ShouldBe(2);
            studentsRepo.Query.AsEnumerable().Where(isMale.And(isUnder21)).Count().ShouldBe(1);
            studentsRepo.Query.AsEnumerable().Where(isMale.And(isUnder21.Not())).Count().ShouldBe(6);
            studentsRepo.Query.AsEnumerable().Where(isMale & !isUnder21).Count().ShouldBe(6);
            
            Ok("Done!!");
        }
        catch (Exception e)
        {
            Error(e.Message);
        }
    }

    public async Task SeedDatabase(IServiceProvider hostProvider)
    {
        using var svrScope = hostProvider.CreateScope();
        var provider = svrScope.ServiceProvider;

        Info("Seeding database using DbContext");
        var context = provider.GetRequiredService<ApplicationDbContext>();
        await using var ctx = provider.GetRequiredService<ApplicationDbContext>();

        Info($"Generating data", 2);
        var teacher1 = new Teacher() { Id = 1, Name = "Thomas" };
        var teacher2 = new Teacher() { Id = 2, Name = "John" };
        var student1 = new Student() { Id = 1, Name = "Michael", Gender = Gender.Male, Age = 21 };
        var student2 = new Student() { Id = 2, Name = "Amalia", Gender = Gender.Female, Age = 17 };
        var student3 = new Student() { Id = 3, Name = "Elisabeth", Gender = Gender.Female, Age = 21 };
        var student4 = new Student() { Id = 4, Name = "Donald", Gender = Gender.Male, Age = 25 };
        var student5 = new Student() { Id = 5, Name = "George", Gender = Gender.Male, Age = 25 };
        var student6 = new Student() { Id = 6, Name = "Luis", Gender = Gender.Male, Age = 28 };
        var student7 = new Student() { Id = 7, Name = "Lemuel", Gender = Gender.Male, Age = 28 };
        var student8 = new Student() { Id = 8, Name = "James", Gender = Gender.Male, Age = 21 };
        var student9 = new Student() { Id = 9, Name = "Kirk", Gender = Gender.Male, Age = 18 };
        var lecture1 = new Lecture() { Id = 1, Subject = "Programming 101", Teacher = teacher1 };
        var lecture2 = new Lecture() { Id = 2, Subject = "Design patterns 101", Teacher = teacher2 };
        var studentLecture1 = new StudentLecture() { Lecture = lecture1, Student = student1 };
        var studentLecture2 = new StudentLecture() { Lecture = lecture1, Student = student2 };
        var studentLecture3 = new StudentLecture() { Lecture = lecture1, Student = student3 };
        var studentLecture4 = new StudentLecture() { Lecture = lecture1, Student = student4 };
        var studentLecture5 = new StudentLecture() { Lecture = lecture1, Student = student5 };
        var studentLecture6 = new StudentLecture() { Lecture = lecture2, Student = student6 };
        var studentLecture7 = new StudentLecture() { Lecture = lecture2, Student = student7 };
        var studentLecture8 = new StudentLecture() { Lecture = lecture2, Student = student8 };
        var studentLecture9 = new StudentLecture() { Lecture = lecture2, Student = student9 };

        Info("seeding Teachers", 2);
        ctx.Teachers.AddRange(new[] { teacher1, teacher2 });
        await ctx.SaveChangesAsync();
        await Wait();

        Info($"seeding Students", 2);
        ctx.Students.AddRange(new[]
        {
            student1, student2, student3, student4, student4, student5, student6, student7, student8, student9,
        });
        await ctx.SaveChangesAsync();
        await Wait();

        Info("seeding Lecture", 2);
        ctx.Lectures.AddRange(new[] { lecture1, lecture2 });
        await ctx.SaveChangesAsync();
        await Wait();

        Info("seeding assistance of Students to Lectures", 2);
        ctx.StudentLectures.AddRange(new[]
        {
            studentLecture1, studentLecture2, studentLecture3, studentLecture4, studentLecture5,
            studentLecture6, studentLecture7, studentLecture8, studentLecture9,
        });
        await ctx.SaveChangesAsync();
        await Wait();

        Ok("Database seeded, successfully");
    }

// ===== Helpers [ no actually needed but i stuck in something else ;D ]
    public void Error(string msg)
    {
        ConsoleColor.Red.Write("[ X ] ");
        Console.WriteLine(msg);
    }

    public void Info(string msg, int level = 1)
    {
        if (level == 1)
            ConsoleColor.DarkYellow.Write("----");
        else
        {
            for (var i = 0; i < level - 1; i++)
            {
                ConsoleColor.DarkYellow.Write(" ");
            }

            ConsoleColor.DarkYellow.Write("+");
        }

        for (var i = 0; i < level; i++)
        {
            ConsoleColor.DarkYellow.Write("-");
        }

        if (level > 1)
            ConsoleColor.DarkYellow.Write(">");
        Console.WriteLine($" {msg}");
    }

    public void Ok(string msg)
    {
        ConsoleColor.Green.Write("[ * ] ");
        Console.WriteLine(msg);
    }

    public void Warning(string msg)
    {
        ConsoleColor.DarkYellow.Write("[ ! ] ");
        Console.WriteLine(msg);
    }

    private async Task Wait() => Thread.Sleep(100);
}