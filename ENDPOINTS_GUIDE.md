# WebApiNibu – How to add a new endpoint (CQRS-style)

This project follows a **CQRS-ish structure**:

- **Controllers** are thin HTTP adapters.
- **Queries** (filter objects) define *read requirements*.
- **Commands** define *write requests*.
- **Services** (implementations) contain the actual endpoint logic (EF queries, FK validation, transactions, mapping).

> Minimal “contract”
>
>- **Read side**: `QueryAsync(SomeQuery query)` / `GetByIdAsync(id)` returns `*ReadDto`
>- **Write side**: `CreateAsync(Create*Command)` / `UpdateAsync(id, Update*Command)` / `DeleteAsync(id, soft)`
>- **Validation failures**: throw `DomainValidationException` → controller returns HTTP 400 with `{ message, errors[] }`

---

## 0) Quick directory map

- **Entities**: `Data/Entity/.../*.cs`
- **DTOs (read models)**: `Data/Dto/*.cs`
- **Queries (filters)**: `Services/Interface/Queries/*.cs`
- **Commands (create/update)**: `Services/Interface/Commands/*.cs`
- **CQRS service interfaces**: `Services/Interface/*.cs` (e.g. `ISchoolStudent`)
- **CQRS implementations**: `Services/Implementation/*.cs` (e.g. `SchoolStudentImplementation`)
- **Controllers**: `Controllers/*.cs`
- **DI wiring**: `Program.cs`

---

## 1) Decide what you are adding

When you say “new endpoint”, you typically mean one of these:

1. **New CRUD endpoint for a new table/entity** (common)
2. **New query endpoint** (e.g. *Get students by school with extra filters*)
3. **New command endpoint** (e.g. *Assign contact to a school*, *toggle active*, *bulk create*)

This guide covers **(1) adding a new CRUD-style controller + CQRS service**. The same pattern applies to (2) and (3).

---

## 2) Add/verify the Entity and DbSet

### 2.1 Create the entity (EF model)

Create the entity under `Data/Entity/<Domain>/` and ensure:

- It has an `Id` (int) primary key.
- It inherits `BaseEntity` if you want soft delete (`Active`) + audit fields.
- Add navigation properties + `[ForeignKey]` attributes as needed.

### 2.2 Register the DbSet

Open `Data/Context/Oracle/OracleDbContext.cs` and add:

```csharp
public DbSet<MyEntity> MyEntities { get; set; }
```

> Naming matters: controllers/services will use these DbSet property names.

---

## 3) Add the Read DTOs (what the API returns)

In this project the API generally returns DTOs rather than raw EF entities.

Create a `*ReadDto` in `Data/Dto/`.

Example (simplified):

```csharp
public class MyEntityReadDto
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
}
```

If you already have a DTO file for that domain, add it there.

---

## 4) Add the Query object (filters for GET-all)

Create a query class under `Services/Interface/Queries/`.

Example:

```csharp
namespace WebApiNibu.Services.Interface.Queries;

public sealed class MyEntityQuery
{
    public string? Name { get; init; }
    public bool? Active { get; init; }
}
```

**Rules of thumb**

- Make all filter fields optional (`?`).
- Prefer exact match for codes/ids (`==`), partial match for text (`Contains`).
- Add include toggles if the entity has large navigation properties.

---

## 5) Add the Commands (create/update payloads)

Create command records under `Services/Interface/Commands/`.

Example:

```csharp
namespace WebApiNibu.Services.Interface.Commands;

public sealed record CreateMyEntityCommand(string Name);
public sealed record UpdateMyEntityCommand(string Name);
```

### 5.1 (Optional) Add mapping helpers

If you still use existing DTOs internally, you can add mapping extensions in the same file.

---

## 6) Extend the CQRS Interface

Create a service interface under `Services/Interface/`.

Example `IMyEntity.cs`:

```csharp
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Interface;

public interface IMyEntity
{
    Task<IReadOnlyList<MyEntityReadDto>> QueryAsync(MyEntityQuery query, CancellationToken ct = default);
    Task<MyEntityReadDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<MyEntityReadDto> CreateAsync(CreateMyEntityCommand command, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateMyEntityCommand command, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
}
```

**Why this is “CQRS cleaner”**

- Reads and writes are explicit.
- Controllers don’t need low-level EF or FK validation logic.
- Commands/queries are reusable from other endpoints.

---

## 7) Implement the CQRS service (the real logic)

Create an implementation under `Services/Implementation/`.

Typical dependencies:

- `OracleDbContext` for composing queries/Includes and FK checks.
- `IBaseCrud<TEntity>` for create/update/delete consistency.

Example skeleton:

```csharp
public class MyEntityImplementation(IBaseCrud<MyEntity> crud, OracleDbContext db) : IMyEntity
{
    public async Task<IReadOnlyList<MyEntityReadDto>> QueryAsync(MyEntityQuery query, CancellationToken ct = default)
    {
        var q = db.MyEntities.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            q = q.Where(x => x.Name.Contains(query.Name));

        if (query.Active.HasValue)
            q = q.Where(x => x.Active == query.Active.Value);

        var items = await q.ToListAsync(ct);
        return items.Select(MapToReadDto).ToList();
    }

    public async Task<MyEntityReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await crud.GetByIdAsync(id, true, ct);
        return entity is null ? null : MapToReadDto(entity);
    }

    public async Task<MyEntityReadDto> CreateAsync(CreateMyEntityCommand command, CancellationToken ct = default)
    {
        // FK validation example
        // if (!await db.OtherTables.AnyAsync(o => o.Id == command.OtherId, ct))
        //     throw new DomainValidationException("Invalid foreign keys", new[] { $"OtherId ({command.OtherId}) not found" });

        var entity = new MyEntity { Name = command.Name, Active = true };
        var created = await crud.CreateAsync(entity, ct);
        return MapToReadDto(created);
    }

    public Task<bool> UpdateAsync(int id, UpdateMyEntityCommand command, CancellationToken ct = default)
        => crud.UpdateAsync(id, e => e.Name = command.Name, ct);

    public Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
        => crud.DeleteAsync(id, softDelete, ct);

    private static MyEntityReadDto MapToReadDto(MyEntity e) => new() { Id = e.Id, Name = e.Name };
}
```

### 7.1 FK existence checks (recommended)

For create/update when you accept FK ids:

```csharp
if (!await db.Schools.AnyAsync(s => s.Id == command.SchoolId, ct))
    throw new DomainValidationException("Invalid foreign keys", new [] { $"SchoolId ({command.SchoolId}) not found" });
```

This creates a consistent API response in the controller.

---

## 8) Create the Controller (HTTP layer)

Create `Controllers/MyEntityController.cs`.

**Keep it thin**:

- Accept query object on GET-all: `[FromQuery] MyEntityQuery query`
- Accept command objects on POST/PUT: `[FromBody] CreateMyEntityCommand` / `UpdateMyEntityCommand`
- Translate validation exceptions to HTTP 400

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
public class MyEntityController(IMyEntity service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MyEntityReadDto>>> GetAll([FromQuery] MyEntityQuery query, CancellationToken ct)
        => Ok(await service.QueryAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MyEntityReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<MyEntityReadDto>> Create([FromBody] CreateMyEntityCommand command, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMyEntityCommand command, CancellationToken ct)
    {
        try
        {
            var updated = await service.UpdateAsync(id, command, ct);
            return updated ? NoContent() : NotFound();
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }
}
```

---

## 9) Register the service in DI (`Program.cs`)

In `Program.cs`, register the new service:

```csharp
builder.Services.AddScoped<IMyEntity, MyEntityImplementation>();
```

If your implementation also needs `IBaseCrud<TEntity>`, it is already registered generically:

```csharp
builder.Services.AddScoped(typeof(IBaseCrud<>), typeof(BaseCrudImplementation<>));
```

---

## 10) Verify Swagger shows the endpoint

Swagger is already wired in `Program.cs`:

- JSON: `/swagger/v1/swagger.json`
- UI: `/swagger`

After you add the controller, it should appear automatically.

If Swagger UI doesn’t load, check whether the app is failing on startup due to:

- DB connection string
- auto-migrations failing (`ApplyMigrationsAsync`)

---

## 11) Smoke test checklist

### Build

```powershell
dotnet build .\WebApiNibu.sln --no-restore
```

### Run

```powershell
dotnet run --project .\WebApiNibu.csproj
```

Open:

- `https://localhost:<port>/swagger`

---

## 12) When to use the generic BaseCrudController

You already have a `BaseCrudController<TEntity>` which can be used for **simple tables** where you:

- don’t need DTO mapping
- don’t need FK validation
- don’t need filtering

But for most real endpoints in this project, prefer the CQRS approach above so:

- filtering stays organized
- commands include validation
- responses are stable

---

## Example: where to look in this repo

- A **CQRS service with FK checks**: `Services/Implementation/SchoolStudentImplementation.cs`
- A **controller using query+commands**: `Controllers/SchoolStudentController.cs`
- A **service with includes + relationships**: `SchoolTableImplementation.cs`
- The **validation exception**: `Services/Interface/Common/DomainValidationException.cs`

---

## Next step (optional): avoid exceptions entirely (Result<T>)

This repo now supports an even cleaner CQRS style where **commands return a `Result`** instead of throwing exceptions.

### What you get

- No `try/catch` in controllers for validation.
- A consistent response shape for failures:

```json
{ "message": "Invalid foreign keys", "errors": ["..."] }
```

### Result types

Defined in:

- `Services/Interface/Common/Result.cs`

```csharp
public record Result { bool IsSuccess; string? Message; IReadOnlyList<string> Errors; }
public record Result<T> : Result { T? Value; }
```

### Interface pattern

Write-side methods return `Result`/`Result<T>`:

```csharp
Task<Result<MyReadDto>> CreateAsync(CreateMyCommand cmd, CancellationToken ct = default);
Task<Result> UpdateAsync(int id, UpdateMyCommand cmd, CancellationToken ct = default);
Task<Result> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default);
```

### Service implementation pattern

Instead of throwing, return failures:

```csharp
if (!await db.Schools.AnyAsync(s => s.Id == cmd.SchoolId, ct))
    return Result<MyReadDto>.Fail("Invalid foreign keys", new[] { $"SchoolId ({cmd.SchoolId}) not found" });
```

### Controller pattern (no exceptions)

```csharp
var result = await service.CreateAsync(command, ct);
if (!result.IsSuccess)
    return BadRequest(new { message = result.Message, errors = result.Errors });

return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
```

For update/delete, the repo uses the convention:

- `Message == "Not found"` → return `404`
- anything else → return `400`

If you want a more explicit approach, we can add an enum (e.g. `ResultErrorType.NotFound/Validation/Conflict`).
