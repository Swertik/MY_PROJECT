using Teacher_Dashboard_Backend.Models;

namespace Teacher_Dashboard_Backend.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Убеждаемся, что БД существует
        context.Database.EnsureCreated();

        // ==========================================
        // 1. ГРУППЫ (GROUPS)
        // ==========================================
        if (!context.Groups.Any())
        {
            var groups = new Group[]
            {
                new Group { Name = "ПИ-21" }, // Программная Инженерия
                new Group { Name = "ИС-22" }, // Информационные Системы
                new Group { Name = "ДИ-23" }  // Дизайн Интерфейсов
            };
            context.Groups.AddRange(groups);
            context.SaveChanges();
            Console.WriteLine("--> Группы созданы.");
        }

        // ==========================================
        // 2. ЗАДАНИЯ (ASSIGNMENTS)
        // ==========================================
        if (!context.Assignments.Any())
        {
            // Получаем реальные ID групп из базы
            var groupPi = context.Groups.First(g => g.Name == "ПИ-21");
            var groupIs = context.Groups.First(g => g.Name == "ИС-22");
            var groupDi = context.Groups.First(g => g.Name == "ДИ-23");

            var assignments = new Assignment[]
            {
                // --- ПИ-21 (Программирование) ---
                new Assignment { Title = "Lab 1: Git Basics", Description = "Commit, Push, Pull, Merge conflicts.", GroupId = groupPi.Id },
                new Assignment { Title = "Lab 2: C# LINQ", Description = "Select, Where, OrderBy, GroupBy.", GroupId = groupPi.Id },
                new Assignment { Title = "Lab 3: Entity Framework", Description = "DbSet, Migrations, Relations.", GroupId = groupPi.Id },
                new Assignment { Title = "Lab 4: Angular Components", Description = "Inputs, Outputs, Lifecycle hooks.", GroupId = groupPi.Id },
                
                // --- ИС-22 (Базы данных) ---
                new Assignment { Title = "DB 1: ER Diagrams", Description = "Chen notation, Crow's foot notation.", GroupId = groupIs.Id },
                new Assignment { Title = "DB 2: SQL Basics", Description = "SELECT * FROM Students.", GroupId = groupIs.Id },
                new Assignment { Title = "DB 3: Normalization", Description = "1NF, 2NF, 3NF forms.", GroupId = groupIs.Id },

                // --- ДИ-23 (Дизайн) ---
                new Assignment { Title = "UX 1: Figma Intro", Description = "Frames, Auto-layout, Components.", GroupId = groupDi.Id },
                new Assignment { Title = "UX 2: Color Theory", Description = "Contrast, Accessibility, Palettes.", GroupId = groupDi.Id }
            };

            context.Assignments.AddRange(assignments);
            context.SaveChanges();
            Console.WriteLine("--> Задания созданы.");
        }

        // ==========================================
        // 3. СТУДЕНТЫ (STUDENTS)
        // ==========================================
        if (!context.Students.Any())
        {
            var groupPi = context.Groups.First(g => g.Name == "ПИ-21");
            var groupIs = context.Groups.First(g => g.Name == "ИС-22");
            var groupDi = context.Groups.First(g => g.Name == "ДИ-23");

            var students = new Student[]
            {
                // --- Студенты ПИ-21 ---
                new Student { FirstName = "Алексей", LastName = "Смирнов", Email = "smirnov@edu.ru", GroupId = groupPi.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 5, 10), DateTimeKind.Utc) },
                new Student { FirstName = "Мария", LastName = "Кузнецова", Email = "kuznetsova@edu.ru", GroupId = groupPi.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 3, 15), DateTimeKind.Utc) },
                new Student { FirstName = "Дмитрий", LastName = "Попов", Email = "popov@edu.ru", GroupId = groupPi.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 8, 20), DateTimeKind.Utc) },
                new Student { FirstName = "Екатерина", LastName = "Морозова", Email = "morozova@edu.ru", GroupId = groupPi.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2006, 1, 12), DateTimeKind.Utc) },

                // --- Студенты ИС-22 ---
                new Student { FirstName = "Иван", LastName = "Соколов", Email = "sokolov@edu.ru", GroupId = groupIs.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 2, 28), DateTimeKind.Utc) },
                new Student { FirstName = "Андрей", LastName = "Михайлов", Email = "mikhailov@edu.ru", GroupId = groupIs.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 11, 5), DateTimeKind.Utc) },

                // --- Студенты ДИ-23 ---
                new Student { FirstName = "Ольга", LastName = "Васильева", Email = "vasilyeva@edu.ru", GroupId = groupDi.Id, 
                              DateOfBirth = DateTime.SpecifyKind(new DateTime(2005, 6, 14), DateTimeKind.Utc) }
            };

            context.Students.AddRange(students);
            context.SaveChanges();
            Console.WriteLine("--> Студенты созданы.");
        }

        // ==========================================
        // 4. ИСТОРИЯ (COMPLETED ASSIGNMENTS)
        // ==========================================
        if (!context.CompletedAssignments.Any())
        {
            // Нам нужно найти конкретных студентов и задачи
            var smirnov = context.Students.First(s => s.LastName == "Смирнов");
            var kuznetsova = context.Students.First(s => s.LastName == "Кузнецова");
            var sokolov = context.Students.First(s => s.LastName == "Соколов");

            var lab1Git = context.Assignments.First(a => a.Title.Contains("Git"));
            var lab2Linq = context.Assignments.First(a => a.Title.Contains("LINQ"));
            var db1Er = context.Assignments.First(a => a.Title.Contains("ER Diagrams"));

            var history = new List<CompletedAssignment>
            {
                // Смирнов - Отличник (Сдал Git и Linq)
                new CompletedAssignment { 
                    StudentId = smirnov.Id, AssignmentId = lab1Git.Id, IsCompleted = true, 
                    CompletedAt = DateTime.SpecifyKind(DateTime.Now.AddDays(-5), DateTimeKind.Utc) 
                },
                new CompletedAssignment { 
                    StudentId = smirnov.Id, AssignmentId = lab2Linq.Id, IsCompleted = true, 
                    CompletedAt = DateTime.SpecifyKind(DateTime.Now.AddDays(-2), DateTimeKind.Utc) 
                },

                // Кузнецова - Сдала только Git
                new CompletedAssignment { 
                    StudentId = kuznetsova.Id, AssignmentId = lab1Git.Id, IsCompleted = true, 
                    CompletedAt = DateTime.SpecifyKind(DateTime.Now.AddDays(-4), DateTimeKind.Utc) 
                },
                // LINQ она не сдала (записи либо нет, либо IsCompleted = false, сделаем false для примера)
                new CompletedAssignment { 
                    StudentId = kuznetsova.Id, AssignmentId = lab2Linq.Id, IsCompleted = false, 
                    CompletedAt = DateTime.MinValue.ToUniversalTime() // Или просто MinValue, но лучше в UTC
                },

                // Соколов (другая группа) - Сдал ER Diagram
                new CompletedAssignment { 
                    StudentId = sokolov.Id, AssignmentId = db1Er.Id, IsCompleted = true, 
                    CompletedAt = DateTime.SpecifyKind(DateTime.Now.AddDays(-1), DateTimeKind.Utc) 
                }
            };

            context.CompletedAssignments.AddRange(history);
            context.SaveChanges();
            Console.WriteLine("--> История сдач создана.");
        }
    }
}