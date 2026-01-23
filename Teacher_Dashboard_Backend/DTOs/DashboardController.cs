using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.DTOs;
using Teacher_Dashboard_Backend.Repositories;

namespace Teacher_Dashboard_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _repository;

    public DashboardController(IDashboardRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<StudentDashboardItemDto>> GetDashboard()
    {
        var data = _repository.GetFullDashboard();
        return Ok(data);
    }
}