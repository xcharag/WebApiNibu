# Global Error Handling — Frontend Guide

## Overview

A global exception-handling middleware has been added to the API. All unhandled exceptions (FK violations, unique constraints, null violations, etc.) now return **structured JSON responses** instead of raw stack traces.

This applies to **every endpoint** — no controller-specific changes needed.

---

## Error Response Format

Every error response follows this consistent shape:

```typescript
interface ErrorResponse {
  statusCode: number;
  errors: string[];
}
```

### Example: Deleting a record that is referenced by other records

```http
DELETE /api/PhaseType/1
```

**Before (❌ raw exception):**
```
Npgsql.PostgresException (0x80004005): 23503: update or delete on table "PhaseTypes"
violates foreign key constraint "FK_Participations_PhaseTypes_PhaseTypeId" ...
[500 lines of stack trace]
```

**After (✅ clean JSON):**
```json
{
  "statusCode": 409,
  "errors": [
    "Cannot complete this operation on 'PhaseTypes' because it is referenced by other records. Remove or reassign the related records first."
  ]
}
```

---

## HTTP Status Codes

| Status Code | When | Example |
|---|---|---|
| `400` | Missing required field, validation error | `"A required field is missing: 'Name'."` |
| `400` | Text too long for column | `"One or more text fields exceed the maximum allowed length."` |
| `400` | Check constraint violated | `"A validation constraint was violated: 'CK_...'."` |
| `409` | Foreign key violation (delete/update blocked) | `"Cannot complete this operation on 'Matches' because it has a dependency on 'Participations'..."` |
| `409` | Unique constraint violation (duplicate) | `"A duplicate record already exists in '...'."` |
| `500` | Unexpected / unknown error | `"An unexpected error occurred. Please try again later."` |

> **Note:** The existing `Result<T>` pattern (used by service layers) still returns `400` for business-rule validation errors via `BadRequest(result.Errors)`. The middleware only catches **unhandled exceptions** that escape the controllers.

---

## Frontend Error Handling

### Recommended Pattern (Svelte / TypeScript)

```typescript
interface ApiError {
  statusCode: number;
  errors: string[];
}

async function apiRequest<T>(url: string, options?: RequestInit): Promise<T> {
  const res = await fetch(url, options);

  if (!res.ok) {
    const body = await res.json();

    // The API returns errors in two formats:
    // 1. string[] (from Result<T> pattern)  → e.g. ["MatchId (99) not found"]
    // 2. ErrorResponse (from middleware)     → e.g. { statusCode: 409, errors: [...] }
    const errors: string[] = Array.isArray(body)
      ? body
      : body.errors ?? [body.message ?? 'Unknown error'];

    throw { statusCode: res.status, errors };
  }

  return res.json();
}
```

### Usage with shadcn Toast

```svelte
<script lang="ts">
  import { toast } from '$lib/components/ui/sonner'; // or your toast lib

  async function deleteItem(id: number) {
    try {
      await apiRequest(`/api/PhaseType/${id}`, { method: 'DELETE' });
      toast.success('Deleted successfully');
    } catch (err: any) {
      // Show each error as a toast
      for (const msg of err.errors) {
        toast.error(msg);
      }
    }
  }
</script>
```

### Common Scenarios

**Deleting a PhaseType that has participations:**
```
409 → "Cannot complete this operation on 'PhaseTypes' because it has a dependency on 'Participations'. Remove or reassign the related records first."
```

**Deleting a Tournament that has participations and rosters:**
```
409 → "Cannot complete this operation on 'Tournaments' because it has a dependency on 'Participations'. Remove or reassign the related records first."
```

**Deleting a School that has students assigned:**
```
409 → "Cannot complete this operation on 'SchoolTable' because it has a dependency on 'personTable'. Remove or reassign the related records first."
```

**Creating a duplicate unique record:**
```
409 → "A duplicate record already exists in 'Users' (constraint: 'IX_Users_Name'). Please use a unique value."
```

---

## What This Does NOT Change

- **Business validation errors** (e.g. `"MatchId (99) not found"`) still return `400` with `string[]` — no change
- **Not found errors** still return `404` with `string[]` — no change  
- **Soft deletes** don't trigger FK violations because the record is just set to `Active = false`
- Only **hard deletes** (`?soft=false`) can trigger FK violations

---

## Checklist

- [ ] Update your global error handler / fetch wrapper to parse both `string[]` and `{ statusCode, errors }` formats
- [ ] Show user-friendly toast messages from the `errors` array
- [ ] On `409` (conflict), prompt the user to remove dependent records first
- [ ] On `500`, show a generic "Something went wrong" message

