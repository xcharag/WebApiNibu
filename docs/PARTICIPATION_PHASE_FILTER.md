# Participation — Phase Type Filter Guide

## Overview

The Participation listing endpoint already supports filtering by **Phase Type**. This guide explains how to implement a phase type combo box filter in the frontend using shadcn-svelte.

---

## How It Works

1. Load all phase types from `GET /api/PhaseType` to populate a `<Select>` combo box
2. When the user picks a phase type, pass `phaseTypeId` as a query param to `GET /api/Participation`
3. The backend filters participations that belong to that phase

---

## Endpoints Involved

### 1. Get all Phase Types (for the combo box)

```http
GET /api/PhaseType?pageSize=100
```

**Response 200:**
```json
{
  "items": [
    { "id": 1, "name": "Fase de Grupos", "description": "Group stage" },
    { "id": 2, "name": "Cuartos de Final", "description": null },
    { "id": 3, "name": "Semifinal", "description": null },
    { "id": 4, "name": "Final", "description": null }
  ],
  "pageNumber": 1,
  "pageSize": 100,
  "totalCount": 4,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### 2. Get Participations filtered by Phase Type

```http
GET /api/Participation?phaseTypeId=1&pageNumber=1&pageSize=20
```

You can combine filters:
```http
GET /api/Participation?phaseTypeId=1&tournamentId=4&schoolId=1&pageNumber=1&pageSize=20
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

---

## TypeScript Types

```typescript
interface PhaseTypeReadDto {
  id: number;
  name: string;
  description: string | null;
}

interface ParticipationFilter {
  key?: string;
  phaseTypeId?: number;    // ← use this to filter by phase
  tournamentId?: number;
  schoolId?: number;
  active?: boolean;
}

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
```

---

## Svelte Implementation Example

```svelte
<script lang="ts">
  import * as Select from '$lib/components/ui/select';
  import { onMount } from 'svelte';

  interface PhaseType {
    id: number;
    name: string;
    description: string | null;
  }

  let phaseTypes: PhaseType[] = [];
  let selectedPhaseTypeId: number | undefined = undefined;
  let participations: ParticipationReadDto[] = [];

  // 1. Load phase types for the combo box
  onMount(async () => {
    const res = await fetch('/api/PhaseType?pageSize=100');
    const data = await res.json();
    phaseTypes = data.items;
  });

  // 2. Reload participations when filter changes
  async function loadParticipations() {
    const params = new URLSearchParams();
    params.set('pageSize', '50');

    if (selectedPhaseTypeId) {
      params.set('phaseTypeId', String(selectedPhaseTypeId));
    }

    const res = await fetch(`/api/Participation?${params}`);
    const data = await res.json();
    participations = data.items;
  }

  // React to selection changes
  $: if (selectedPhaseTypeId !== undefined) {
    loadParticipations();
  }
</script>

<!-- Phase Type filter combo box -->
<Select.Root
  onSelectedChange={(v) => {
    selectedPhaseTypeId = v?.value;
  }}
>
  <Select.Trigger class="w-[240px]">
    <Select.Value placeholder="Filter by phase..." />
  </Select.Trigger>
  <Select.Content>
    <Select.Item value={undefined}>All Phases</Select.Item>
    {#each phaseTypes as phase}
      <Select.Item value={phase.id}>{phase.name}</Select.Item>
    {/each}
  </Select.Content>
</Select.Root>

<!-- Participation list -->
<div class="mt-4">
  {#each participations as p}
    <div class="border rounded p-3 mb-2">
      <p class="font-medium">{p.schoolName} — Group {p.key}</p>
      <p class="text-sm text-muted-foreground">
        {p.tournamentName} · {p.phaseTypeName}
      </p>
    </div>
  {/each}
</div>
```

---

## Available Participation Filters (full reference)

| Query Param | Type | Description |
|---|---|---|
| `key` | `string` | Filter by group key (e.g. `"A"`, `"B"`) |
| `phaseTypeId` | `int` | Filter by phase type ID |
| `tournamentId` | `int` | Filter by tournament ID |
| `schoolId` | `int` | Filter by school ID |
| `active` | `bool` | Filter by active status (`true`/`false`) |
| `pageNumber` | `int` | Page number (default: `1`) |
| `pageSize` | `int` | Items per page (default: `10`, max: `500`) |

All filters are optional and can be combined.

---

## Checklist

- [ ] Create an API service function to fetch phase types (`GET /api/PhaseType`)
- [ ] Add a shadcn `<Select>` combo box above the participation table/list
- [ ] Pass `phaseTypeId` as a query param when fetching participations
- [ ] Add an "All Phases" option to clear the filter
- [ ] Combine with existing filters (tournament, school) if applicable

