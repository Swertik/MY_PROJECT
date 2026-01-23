using Teacher_Dashboard_Backend.Models;

namespace Teacher_Dashboard_Backend.Services;

public class DatabaseService
{

public List<User> Users { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<Student> Students { get; set; } = new();
    public List<Assignment> Assignments { get; set; } = new();
    public List<CompletedAssignment> CompletedAssignments { get; set; } = new();

    public DatabaseService()
    {
        SeedData();
    }

    private void SeedData()
    {
        // 1. Initialize Groups
        var groupPi = new Group { Id = 1, Name = "ПИ-21" };
        var groupIs = new Group { Id = 2, Name = "ИС-22" };
        var groupDi = new Group { Id = 3, Name = "ДИ-23" };

        Groups.AddRange(new[] { groupPi, groupIs, groupDi });

        // 2. Initialize Assignments Helpers
        void AddTasks(Group group, (int id, string name)[] tasks)
        {
            foreach (var t in tasks)
            {
                var assignment = new Assignment
                {
                    Id = t.id,
                    Title = t.name,
                    Description = $"Description for {t.name}",
                    GroupId = group.Id,
                    Group = group
                };
                Assignments.Add(assignment);
                group.Assignments.Add(assignment);
            }
        }

        // PI-21 Tasks
        AddTasks(groupPi, new[] {
            (1, "Lab 1: Git Basics"), (2, "Lab 2: Angular Components"), (3, "Lab 3: Services & DI"),
            (4, "Lab 4: Routing"), (5, "Lab 5: Forms"), (6, "Lab 6: HTTP Client"),
            (7, "Lab 7: RxJS Basics"), (8, "Lab 8: Final Project")
        });

        // IS-22 Tasks
        AddTasks(groupIs, new[] {
            (10, "DB 1: SQL Basics"), (11, "DB 2: Joins & Unions"), (12, "DB 3: Normalization"),
            (13, "DB 4: Transactions"), (14, "DB 5: NoSQL Introduction")
        });

        // DI-23 Tasks
        AddTasks(groupDi, new[] {
            (20, "UX 1: Figma Basics"), (21, "UX 2: Color Theory"),
            (22, "UX 3: Typography"), (23, "UX 4: Prototyping")
        });

        // 3. Raw Data for Students & History
        var studentData = new[]
        {
            // --- PI-21 ---
            new { Id = 101, Fio = "Смирнов Алексей Владимирович", GroupId = 1, History = new[] 
                { (1001, 1, true, "2025-09-01T10:00:00"), (1002, 2, true, "2025-09-05T12:30:00"), (1003, 3, true, "2025-09-10T14:15:00"), (1004, 4, true, "2025-09-15T09:00:00"), (1005, 5, true, "2025-09-20T16:45:00"), (1006, 6, true, "2025-09-25T11:20:00"), (1007, 7, true, "2025-10-01T13:10:00"), (1008, 8, true, "2025-10-10T15:00:00") } 
            },
            new { Id = 102, Fio = "Кузнецова Мария Игоревна", GroupId = 1, History = new[] 
                { (2001, 1, true, "2025-09-02T11:00:00"), (2002, 2, true, "2025-09-07T13:45:00"), (2003, 3, true, "2025-09-12T10:30:00"), (2004, 4, false, ""), (2005, 5, false, "") } 
            },
            new { Id = 103, Fio = "Попов Дмитрий Сергеевич", GroupId = 1, History = new[] 
                { (3001, 1, true, "2025-09-05T09:15:00"), (3002, 2, false, "") } 
            },
            new { Id = 110, Fio = "Морозова Екатерина Ильинична", GroupId = 1, History = new[] 
                { (1101, 1, true, "2025-02-01T10:00:00"), (1102, 2, false, "") } 
            },

            // --- IS-22 ---
            new { Id = 104, Fio = "Соколов Иван Петрович", GroupId = 2, History = new[] 
                { (4001, 10, true, "2025-09-02T10:00:00"), (4002, 11, true, "2025-09-09T11:30:00"), (4003, 12, true, "2025-09-16T14:20:00"), (4004, 13, true, "2025-09-23T16:00:00"), (4005, 14, true, "2025-09-30T09:45:00") } 
            },
            new { Id = 105, Fio = "Михайлов Андрей Дмитриевич", GroupId = 2, History = new[] 
                { (5001, 10, true, "2025-09-03T12:00:00"), (5002, 11, true, "2025-09-11T10:15:00"), (5003, 12, false, "") } 
            },
            new { Id = 106, Fio = "Новикова Елена Сергеевна", GroupId = 2, History = new[] 
                { (6001, 10, false, "") } 
            },
            new { Id = 107, Fio = "Лебедев Максим Викторович", GroupId = 2, History = new (int, int, bool, string)[] { } },

            // --- DI-23 ---
            new { Id = 108, Fio = "Васильева Ольга Андреевна", GroupId = 3, History = new[] 
                { (8001, 20, true, "2025-09-01T15:00:00"), (8002, 21, true, "2025-09-08T16:30:00"), (8003, 22, true, "2025-09-15T14:45:00"), (8004, 23, true, "2025-09-22T11:00:00") } 
            },
            new { Id = 109, Fio = "Федоров Николай Павлович", GroupId = 3, History = new[] 
                { (9001, 20, true, "2025-09-03T13:20:00"), (9002, 21, false, ""), (9003, 22, false, "") } 
            }
        };


        foreach (var data in studentData)
        {
            var parts = data.Fio.Split(' ');
            var lastName = parts.Length > 0 ? parts[0] : "Unknown";
            var firstName = parts.Length > 1 ? parts[1] : "Unknown";

            var student = new Student
            {
                Id = data.Id,
                LastName = lastName,
                FirstName = firstName,
                Email = $"student{data.Id}@school.ru",
                DateOfBirth = new DateTime(2005, 1, 1)
            };

            Students.Add(student);

            var group = Groups.FirstOrDefault(g => g.Id == data.GroupId);
            if (group != null) group.Students.Add(student);

            foreach (var (histId, assignId, isComp, dateStr) in data.History)
            {
                var assignment = Assignments.FirstOrDefault(a => a.Id == assignId);
                DateTime completedAt = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(dateStr)) DateTime.TryParse(dateStr, out completedAt);

                var record = new CompletedAssignment
                {
                    Id = histId,
                    StudentId = student.Id,
                    Student = student,
                    AssignmentId = assignId,
                    Assignment = assignment,
                    IsCompleted = isComp,
                    CompletedAt = completedAt
                };
                CompletedAssignments.Add(record);
            }
        }

        Users.Add(new User 
        { 
            Id = 1, 
            Username = "teacher", 
            PasswordHash = "password",
            Role = "Teacher" 
        });
    }
}