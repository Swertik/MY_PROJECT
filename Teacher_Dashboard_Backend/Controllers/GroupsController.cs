using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.Repositories;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupRepository _repository;

    public GroupsController(IGroupRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAllGroups()
    {
        var groups = _repository.GetAll();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public IActionResult GetGroup(int id)
    {
        var group = _repository.GetById(id);

        if (group == null)
            return NotFound();

        return Ok(group);
    }
}