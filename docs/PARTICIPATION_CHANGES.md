# Participation API — Changes (March 7, 2026)

## Summary

The `Participation` entity has been simplified. The `startDate` and `endDate` fields have been **removed** since a participation's timeframe is already defined by the Tournament it belongs to. A bug causing `NullReferenceException` when creating a participation has also been fixed.

---

## Breaking Changes

### Removed fields

| Field | Removed from |
|---|---|
| `startDate` | `ParticipationReadDto`, `ParticipationCreateDto`, `ParticipationUpdateDto` |
| `endDate` | `ParticipationReadDto`, `ParticipationCreateDto`, `ParticipationUpdateDto` |
| `schoolName` | `ParticipationCreateDto` (was unused — the backend resolves it from `schoolId`) |

### Bug fixed

Creating a participation (`POST /api/Participation`) was throwing a `NullReferenceException` because the backend wasn't loading the navigation properties (Tournament, PhaseType, School) before mapping the response. This is now fixed — the response correctly returns all resolved names.

---

## Updated TypeScript Types

### Before (❌ Remove these fields)

```typescript
// OLD — do NOT use
interface ParticipationReadDto {
  id: number;
  key: string;
  startDate: string;     // ← REMOVED
  endDate: string;       // ← REMOVED
  phaseTypeId: number;
  phaseTypeName: string;
  tournamentId: number;
  tournamentName: string;
  schoolId: number;
  schoolName: string;
}

interface ParticipationCreateDto {
  key: string;
  startDate: string;     // ← REMOVED
  endDate: string;       // ← REMOVED
  phaseTypeId: number;
  tournamentId: number;
  schoolId: number;
  schoolName: string;    // ← REMOVED
}

interface ParticipationUpdateDto {
  key?: string;
  startDate?: string;    // ← REMOVED
  endDate?: string;      // ← REMOVED
  phaseTypeId?: number;
  tournamentId?: number;
  schoolId?: number;
}
```

### After (✅ Use these)

```typescript
interface ParticipationReadDto {
  id: number;
  key: string;
  phaseTypeId: number;
  phaseTypeName: string;
  tournamentId: number;
  tournamentName: string;
  schoolId: number;
  schoolName: string;
}

interface ParticipationCreateDto {
  key: string;
  phaseTypeId: number;
  tournamentId: number;
  schoolId: number;
}

interface ParticipationUpdateDto {
  key?: string;
  phaseTypeId?: number;
  tournamentId?: number;
  schoolId?: number;
}
```

### Filter (query params — unchanged)

```typescript
interface ParticipationFilter {
  key?: string;
  phaseTypeId?: number;
  tournamentId?: number;
  schoolId?: number;
  active?: boolean;
}
```

---

## Endpoints Reference

All endpoints remain the same, only the payloads changed.

### List participations

```http
GET /api/Participation?tournamentId=4&schoolId=1&pageNumber=1&pageSize=20
```

**Response 200:**
```json
{
  "items": [
    {
      "id": 1,
      "key": "A",
      "phaseTypeId": 1,
      "phaseTypeName": "Fase de Grupos",
      "tournamentId": 4,
      "tournamentName": "Copa UPSA 2026",
      "schoolId": 1,
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

### Create a participation

```http
POST /api/Participation
Content-Type: application/json

{
  "key": "A",
  "phaseTypeId": 1,
  "tournamentId": 4,
  "schoolId": 1
}
```

**Response 201:**
```json
{
  "id": 1,
  "key": "A",
  "phaseTypeId": 1,
  "phaseTypeName": "Fase de Grupos",
  "tournamentId": 4,
  "tournamentName": "Copa UPSA 2026",
  "schoolId": 1,
  "schoolName": "Colegio La Salle"
}
```

**Response 400 (validation errors):**
```json
["PhaseTypeId (99) not found", "TournamentId (88) not found"]
```

### Get by ID

```http
GET /api/Participation/1
```

### Update (partial)

```http
PUT /api/Participation/1
Content-Type: application/json

{
  "key": "B",
  "phaseTypeId": 2
}
```

**Response:** `204 No Content`

### Delete

```http
DELETE /api/Participation/1              // soft delete (default)
DELETE /api/Participation/1?soft=false   // hard delete
```

**Response:** `204 No Content`

---

## Frontend Migration Checklist

- [ ] Remove `startDate` and `endDate` from the Participation TypeScript interfaces
- [ ] Remove `schoolName` from the create DTO (backend resolves it automatically)
- [ ] Remove any date picker / date input fields from the Participation create/edit forms
- [ ] Remove date columns from the Participation list/table view
- [ ] Update any API service functions that were sending `startDate`/`endDate` in the request body
- [ ] Test creating a participation — it should now return the full response with resolved names
- [ ] If you were displaying `startDate`/`endDate` anywhere, consider showing the Tournament's dates instead

---

## Quick Find & Delete in your Svelte project

Search for these strings and remove related code:

```
startDate    (in Participation context only — Match still has dates)
endDate      (in Participation context only)
schoolName   (in ParticipationCreateDto only — ReadDto still returns it)
```

> **Note:** `Match` still uses `startDate` and `endDate` — only remove them from Participation-related code.

