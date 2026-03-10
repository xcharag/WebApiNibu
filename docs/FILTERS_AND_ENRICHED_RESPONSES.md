# 🔍 Copa UPSA — Filters & Enriched Responses (Frontend Guide)

> **Date:** 2026-03-10  
> **Changes:** All text filters now use case-insensitive partial matching (`ILIKE '%word%'`). All read DTOs now include related entity names alongside their IDs.

---

## 1. Text Filters — Now Case-Insensitive Partial Match

**Before:** Filters like `?name=Copa` required an **exact substring match** (case-sensitive).  
**Now:** All text filters use **`%word%` pattern matching** (case-insensitive). Typing `copa` will match `"Copa UPSA 2026"`, `"COPA Intercolegial"`, etc.

### Affected Filters by Entity

| Entity | Filter param | Matches against |
|--------|-------------|-----------------|
| **Match** | `location` | `Match.Location` |
| **MatchStatus** | `name` | `MatchStatus.Name` |
| **Participation** | `key` | `Participation.Key` |
| **PhaseType** | `name` | `PhaseType.Name` |
| **Position** | `code` | `Position.Code` |
| **Position** | `name` | `Position.Name` |
| **Sport** | `name` | `Sport.Name` |
| **Statistic** | `name` | `Statistic.Name` |
| **Tournament** | `name` | `Tournament.Name` |
| **TournamentParent** | `name` | `TournamentParent.Name` |
| **TournamentRoster** | `name` | `FirstName`, `MiddleName`, `LastName`, `MaternalName` |
| **Roster** | `name` | via `TournamentRoster` name fields |

### Example

```http
GET /api/Tournament?name=copa&pageNumber=1&pageSize=10
```

Returns all tournaments whose name **contains** "copa" (case-insensitive).

### Svelte Usage

```typescript
// Search as the user types (with debounce)
const params = new URLSearchParams({
  name: searchTerm,    // e.g. "copa" — partial, case-insensitive
  pageNumber: '1',
  pageSize: '10'
});
const res = await fetch(`/api/Tournament?${params}`);
```

---

## 2. Enriched Read DTOs — Related Entity Names

All read responses now include **human-readable names** for related entities alongside their IDs. You no longer need to make extra API calls to resolve names.

### Match — `GET /api/Match`

**New fields:** `participationASchoolName`, `participationBSchoolName`, `matchStatusName`

```typescript
interface MatchReadDto {
  id: number;
  location: string;
  scoreA: number;
  scoreB: number;
  detailPointA: number;
  detailPointB: number;
  startDate: string;         // ISO datetime
  endDate: string;
  numberMatch: number;
  participationAId: number;
  participationASchoolName: string;  // ← NEW: "Colegio La Salle"
  participationBId: number;
  participationBSchoolName: string;  // ← NEW: "Colegio Don Bosco"
  matchStatusId: number;
  matchStatusName: string;           // ← NEW: "Pendiente"
}
```

**Example response:**
```json
{
  "id": 1,
  "location": "Cancha Principal",
  "scoreA": 2,
  "scoreB": 1,
  "detailPointA": 0,
  "detailPointB": 0,
  "startDate": "2026-03-15T14:30:00Z",
  "endDate": "2026-03-15T16:00:00Z",
  "numberMatch": 1,
  "participationAId": 3,
  "participationASchoolName": "Colegio La Salle",
  "participationBId": 7,
  "participationBSchoolName": "Colegio Don Bosco",
  "matchStatusId": 1,
  "matchStatusName": "Pendiente"
}
```

---

### StatisticEvent — `GET /api/StatisticEvent`

**New fields:** `statisticName`, `rosterStudentName`

```typescript
interface StatisticEventReadDto {
  id: number;
  moment: string;            // TimeOnly as "HH:mm:ss"
  statisticId: number;
  statisticName: string;     // ← NEW: "Gol"
  rosterId: number;
  rosterStudentName: string; // ← NEW: "Juan Pérez"
}
```

**Example response:**
```json
{
  "id": 1,
  "moment": "00:35:00",
  "statisticId": 2,
  "statisticName": "Gol",
  "rosterId": 5,
  "rosterStudentName": "Juan Pérez"
}
```

---

### Already Enriched (No Changes Needed)

These entities already returned related names before this update:

| Entity | Fields already present |
|--------|----------------------|
| **Participation** | `phaseTypeName`, `tournamentName`, `schoolName` |
| **Tournament** | `tournamentParentName`, `sportName`, `categoryName` |
| **Statistic** | `statisticTypeName`, `sportName` |
| **Roster** | `studentName`, `matchName`, `positionName` |
| **TournamentRoster** | `fullName`, `tournamentName`, `schoolName` |

---

### Leaf Entities (No FKs to Resolve)

These entities have no foreign keys, so they only return their own fields:

- **MatchStatus** — `id`, `name`, `description`, `color`
- **PhaseType** — `id`, `name`, `description`
- **Position** — `id`, `code`, `name`, `coordX`, `coordY`
- **Sport** — `id`, `name`, `description`, `icon`
- **TournamentParent** — `id`, `name`, `description`, `category`

---

## 3. Migration Checklist for Svelte Frontend

### Filters
1. **No code changes required** for existing filter inputs — they just work better now (partial + case-insensitive).
2. You can **simplify** any workaround you had for case-sensitive matching.
3. Search inputs can now be used as **"type to search"** with debounce since partial matching is supported.

### Display / Tables
1. **Match lists/tables:** Replace any manual participation/status lookups with the new `participationASchoolName`, `participationBSchoolName`, and `matchStatusName` fields.
2. **StatisticEvent lists:** Use `statisticName` and `rosterStudentName` directly instead of making extra API calls.
3. **Remove** any `$effect` or `onMount` calls that fetched related entities just to resolve names for display.

### Example: Match Table (Before vs After)

**Before** (manual lookup):
```svelte
<!-- Had to fetch participations and statuses separately -->
{#each matches as match}
  <td>{getSchoolName(match.participationAId)}</td>
  <td>{getSchoolName(match.participationBId)}</td>
  <td>{getStatusName(match.matchStatusId)}</td>
{/each}
```

**After** (direct from response):
```svelte
{#each matches as match}
  <td>{match.participationASchoolName}</td>
  <td>{match.participationBSchoolName}</td>
  <td>{match.matchStatusName}</td>
{/each}
```

