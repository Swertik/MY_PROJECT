using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.Repositories;

[ApiController]
[Route("api/[controller]")] // Сюда прилетают запросы из браузера
public class StudentsController : ControllerBase
{
    // 1. Объявляем, что нам нужен Репозиторий
    private readonly IStudentRepository _repository;

    // 2. В конструкторе получаем готовый Репозиторий (ASP.NET сам его сюда подставит)
    public StudentsController(IStudentRepository repository)
    {
        if (repository == null)
        {
            throw new ArgumentNullException(nameof(repository));
        }
        _repository = repository;
    }

    // 3. Метод, который слушает GET запросы
    [HttpGet]
    public IActionResult GetAllStudents()
    {
        // Контроллер САМ не ищет данные. Он говорит Репозиторию: "Дай данные".
        var students = _repository.GetAll(); 
        
        // И просто возвращает их с кодом 200 OK
        return Ok(students);
    }

    [HttpGet("{id}")]
    public IActionResult GetStudent(int id)
    {
        // Опять же, делегируем работу Репозиторию
        var student = _repository.GetById(id);

        if (student == null)
            return NotFound(); // 404, если не нашел

        return Ok(student);
    }
}