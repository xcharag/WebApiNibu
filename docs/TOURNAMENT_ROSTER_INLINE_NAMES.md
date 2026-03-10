# 🏆 TournamentRoster — Inline Names Change (Frontend Guide)

> **Date:** 2026-03-10  
> **Breaking change:** `TournamentRoster` no longer references `SchoolStudent`. Player info is now stored directly on the roster record.

---

## Summary

Previously, a `TournamentRoster` linked to a `SchoolStudent` via `schoolStudentId`. This has been **removed**. Instead, the player's name and document number are stored **directly** on the `TournamentRoster` record.

This means:
- You **no longer need** to create/find a `SchoolStudent` before adding someone to a tournament roster.
- The frontend sends the player's name and document number directly when creating a roster entry.
- The `schoolStudentId` field is **gone** from all DTOs and filters.

---

## TypeScript Types (Updated)

```typescript
// ── Response ──
interface TournamentRosterReadDto {
  id: number;
  firstName: string;
  middleName: string | null;
  lastName: string;
  maternalName: string | null;
  documentNumber: string | null;
  fullName: string;           // Auto-generated: "Juan Carlos Pérez López"
  tournamentId: number;
  tournamentName: string;     // "Copa UPSA 2026" (auto-generated)
  schoolId: number;
  schoolName: string;         // "Colegio La Salle" (auto-generated)
}

// ── Create ──
interface TournamentRosterCreateDto {
  firstName: string;          // Required
  middleName?: string | null; // Optional
  lastName: string;           // Required
  maternalName?: string | null; // Optional
  documentNumber?: string | null; // Optional
  tournamentId: number;
  schoolId: number;
}

// ── Update (partial) ──
interface TournamentRosterUpdateDto {
  firstName?: string | null;
  middleName?: string | null;
  lastName?: string | null;
  maternalName?: string | null;
  documentNumber?: string | null;
  tournamentId?: number | null;
  schoolId?: number | null;
}

// ── Filters (query params on GET) ──
interface TournamentRosterFilter {
  tournamentId?: number;
  schoolId?: number;
  active?: boolean;
  name?: string;    // Searches across firstName, middleName, lastName, maternalName (case-insensitive)
}
```

---

## Endpoints

### List tournament roster entries

```http
GET /api/TournamentRoster?tournamentId=1&schoolId=5&pageNumber=1&pageSize=20
```

**Response 200:**
```json
{
  "items": [
    {
      "id": 1,
      "firstName": "Juan",
      "middleName": "Carlos",
      "lastName": "Pérez",
      "maternalName": "López",
      "documentNumber": "12345678",
      "fullName": "Juan Carlos Pérez López",
      "tournamentId": 1,
      "tournamentName": "Copa UPSA 2026",
      "schoolId": 5,
      "schoolName": "Colegio La Salle"
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

### Search by name

```http
GET /api/TournamentRoster?tournamentId=1&name=juan&pageNumber=1&pageSize=20
```

This searches case-insensitively across `firstName`, `middleName`, `lastName`, and `maternalName`.

### Get by ID

```http
GET /api/TournamentRoster/1
```

### Create a roster entry

```http
POST /api/TournamentRoster
Content-Type: application/json

{
  "firstName": "Juan",
  "middleName": "Carlos",
  "lastName": "Pérez",
  "maternalName": "López",
  "documentNumber": "12345678",
  "tournamentId": 1,
  "schoolId": 5
}
```

**Response 201:**
```json
{
  "id": 1,
  "firstName": "Juan",
  "middleName": "Carlos",
  "lastName": "Pérez",
  "maternalName": "López",
  "documentNumber": "12345678",
  "fullName": "Juan Carlos Pérez López",
  "tournamentId": 1,
  "tournamentName": "Copa UPSA 2026",
  "schoolId": 5,
  "schoolName": "Colegio La Salle"
}
```

**Response 400 (validation):**
```json
["TournamentId (999) not found", "SchoolId (888) not found"]
```

### Update (partial)

```http
PUT /api/TournamentRoster/1
Content-Type: application/json

{
  "firstName": "Pedro",
  "documentNumber": "87654321"
}
```

**Response:** `204 No Content`

### Delete (soft by default)

```http
DELETE /api/TournamentRoster/1
DELETE /api/TournamentRoster/1?soft=false
```

**Response:** `204 No Content`

---

## What Changed for `Roster` (Match Lineups)

The `Roster` endpoint (`/api/Roster`) still uses `tournamentRosterId` to reference a player. The `studentName` field in `RosterReadDto` is now built from the `TournamentRoster.FirstName + TournamentRoster.LastName` instead of from `SchoolStudent`.

```typescript
interface RosterReadDto {
  id: number;
  matchId: number;
  tournamentRosterId: number;
  positionId: number;
  studentName: string;    // Now from TournamentRoster's inline names
  matchName: string;
  positionName: string;
}
```

The `Roster` filter name search also searches across `TournamentRoster`'s inline name fields.

---

## Migration Checklist for Svelte Frontend

1. **Remove** any `schoolStudentId` references when creating/updating `TournamentRoster`.
2. **Update** your create/edit forms to have fields for:
   - `firstName` (required)
   - `middleName` (optional)
   - `lastName` (required)
   - `maternalName` (optional)
   - `documentNumber` (optional)
   - `tournamentId` (required)
   - `schoolId` (required)
3. **Update** your list/table columns — replace `studentName` / `schoolStudentId` with `fullName` (or individual name fields).
4. **Remove** the `schoolStudentId` filter from any filter components.
5. **Update** Roster display — `studentName` still works but now comes from TournamentRoster's inline names.

### Svelte/shadcn Form Example

```svelte
<script lang="ts">
  let form = {
    firstName: '',
    middleName: '',
    lastName: '',
    maternalName: '',
    documentNumber: '',
    tournamentId: 0,
    schoolId: 0
  };

  async function create() {
    const res = await fetch('/api/TournamentRoster', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form)
    });
    if (!res.ok) {
      const errors = await res.json();
      console.error(errors);
      return;
    }
    const created = await res.json();
    console.log('Created:', created);
  }
</script>

<form on:submit|preventDefault={create}>
  <Input bind:value={form.firstName} placeholder="First name" required />
  <Input bind:value={form.middleName} placeholder="Middle name" />
  <Input bind:value={form.lastName} placeholder="Last name" required />
  <Input bind:value={form.maternalName} placeholder="Maternal name" />
  <Input bind:value={form.documentNumber} placeholder="Document number" />
  <!-- tournamentId and schoolId from selects/combo boxes -->
  <Button type="submit">Add to Roster</Button>
</form>
```

