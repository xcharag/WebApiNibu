# SchoolStudent — Unified Name Search Filter

## Overview

The `SchoolStudent` listing endpoint now supports a single **`name`** query parameter that searches across all four name fields at once:

- `FirstName`
- `MiddleName`
- `PaternalSurname`
- `MaternalSurname`

This replaces the old separate `firstName` and `paternalSurname` filters.

The search uses PostgreSQL `ILIKE` for **case-insensitive partial matching** — it runs entirely on the database, no client-side evaluation.

> **Bugfix (March 7):** Replaced `string.Contains(value, StringComparison)` with `EF.Functions.ILike` — the previous approach could not be translated to SQL by EF Core on PostgreSQL.

---

## Breaking Change

| Before (❌ removed) | After (✅ use this) |
|---|---|
| `?firstName=Juan` | `?name=Juan` |
| `?paternalSurname=Pérez` | `?name=Pérez` |
| both at once was two params | single `name` param covers everything |

---

## How It Works

The `name` parameter performs a **case-insensitive partial match** across all four name columns. If any of them contains the search term, the student is included in the results.

For example, searching `name=per` would match:
- A student with `FirstName = "Perez"` ❌ (not a first name, but illustrates partial match)
- A student with `PaternalSurname = "Pérez"` ✅
- A student with `MiddleName = "Esperanza"` ✅ (contains "per")

---

## Endpoint

```http
GET /api/SchoolStudent?name=juan&pageNumber=1&pageSize=20
```

Combine with other filters:
```http
GET /api/SchoolStudent?name=juan&idSchool=5&isPlayer=true&pageNumber=1&pageSize=20
```

**Response 200:**
```json
{
  "items": [
    {
      "id": 42,
      "firstName": "Juan",
      "middleName": "Carlos",
      "paternalSurname": "Pérez",
      "maternalSurname": "López",
      "documentNumber": "12345678",
      "birthDate": "2010-05-15T00:00:00Z",
      "phoneNumber": "70012345",
      "email": "juan@example.com",
      "idCountry": 1,
      "idDocumentType": 1,
      "idSchool": 5,
      "schoolGrade": 6,
      "isPlayer": true,
      "hasUpsaParents": false,
      "schoolName": "Colegio La Salle",
      "courseName": ""
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

## TypeScript Types

```typescript
interface SchoolStudentFilter {
  name?: string;            // ← unified search across all 4 name fields
  email?: string;
  idCountry?: number;
  idDocumentType?: number;
  idSchool?: number;
  schoolGrade?: string;
  isPlayer?: boolean;
  active?: boolean;
}

interface SchoolStudentReadDto {
  id: number;
  firstName: string;
  middleName: string | null;
  paternalSurname: string;
  maternalSurname: string;
  documentNumber: string;
  birthDate: string;
  phoneNumber: string;
  email: string;
  idCountry: number;
  idDocumentType: number;
  idSchool: number;
  schoolGrade: number;
  isPlayer: boolean;
  hasUpsaParents: boolean;
  schoolName: string;
  courseName: string;
}
```

---

## Svelte Implementation Example

```svelte
<script lang="ts">
  import { Input } from '$lib/components/ui/input';

  let searchName = '';
  let students: SchoolStudentReadDto[] = [];
  let debounceTimer: ReturnType<typeof setTimeout>;

  function onSearchInput(e: Event) {
    const value = (e.target as HTMLInputElement).value;
    searchName = value;

    // Debounce: wait 300ms after the user stops typing
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => loadStudents(), 300);
  }

  async function loadStudents() {
    const params = new URLSearchParams();
    params.set('pageSize', '20');

    if (searchName.trim()) {
      params.set('name', searchName.trim());
    }

    const res = await fetch(`/api/SchoolStudent?${params}`);
    const data = await res.json();
    students = data.items;
  }
</script>

<Input
  placeholder="Search by name..."
  value={searchName}
  on:input={onSearchInput}
/>

<!-- Results -->
{#each students as s}
  <div class="border rounded p-3 mb-2">
    <p class="font-medium">
      {s.firstName} {s.middleName ?? ''} {s.paternalSurname} {s.maternalSurname}
    </p>
    <p class="text-sm text-muted-foreground">{s.schoolName} · Grade {s.schoolGrade}</p>
  </div>
{/each}
```

---

## All Available Filters (full reference)

| Query Param | Type | Description |
|---|---|---|
| `name` | `string` | Searches across FirstName, MiddleName, PaternalSurname, MaternalSurname |
| `email` | `string` | Partial match on email |
| `idCountry` | `int` | Filter by country ID |
| `idDocumentType` | `int` | Filter by document type ID |
| `idSchool` | `int` | Filter by school ID |
| `schoolGrade` | `string` | Filter by grade |
| `isPlayer` | `bool` | Filter players only (`true`) or non-players (`false`) |
| `active` | `bool` | Filter by active status |
| `pageNumber` | `int` | Page number (default: `1`) |
| `pageSize` | `int` | Items per page (default: `10`, max: `500`) |

---

## Frontend Migration Checklist

- [ ] Replace any `firstName` / `paternalSurname` query params with `name`
- [ ] Update TypeScript filter interface (remove `firstName`, `paternalSurname`; add `name`)
- [ ] Add a single search `<Input>` field with debounce for the name search
- [ ] Test searching by first name, middle name, paternal surname, and maternal surname

