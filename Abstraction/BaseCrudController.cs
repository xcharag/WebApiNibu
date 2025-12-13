namespace WebApiNibu.Abstraction;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseCrudController<TEntity> : ControllerBase
{
    protected readonly IBaseCrud<TEntity> Service;

    protected BaseCrudController(IBaseCrud<TEntity> service)
    {
        Service = service;
    }

    // GET: api/{entity}
    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll(CancellationToken ct)
    {
        var items = await Service.GetAllAsync(true, ct);
        return Ok(items);
    }

    // GET: api/{entity}/{id}
    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<TEntity>> GetById(int id, CancellationToken ct)
    {
        var item = await Service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/{entity}
    [HttpPost]
    public virtual async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity, CancellationToken ct)
    {
        var created = await Service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = GetId(created) }, created);
    }

    // PUT: api/{entity}/{id}
    [HttpPut("{id:int}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] TEntity dto, CancellationToken ct)
    {
        var updated = await Service.UpdateAsync(id, e => ApplyDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/{entity}/{id}
    [HttpDelete("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await Service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    // Helpers to allow override in derived controllers for custom id mapping and update logic
    protected virtual int GetId(TEntity entity) => (int)entity?.GetType().GetProperty("Id")?.GetValue(entity)!;
    protected virtual void ApplyDto(TEntity target, TEntity source)
    {
        // Default naive mapping: copy all writable properties except keys and audit fields.
        var props = typeof(TEntity).GetProperties().Where(p => p.CanWrite && p.Name is not ("Id" or "CreatedAt" or "CreatedBy" or "UpdatedAt" or "UpdatedBy" or "Active"));
        foreach (var p in props)
        {
            var value = p.GetValue(source);
            p.SetValue(target, value);
        }
    }
}