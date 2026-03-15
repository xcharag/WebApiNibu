# Match Results Excel Upload

## Overview

A new endpoint has been added to upload match results in bulk via an Excel file. This allows updating the scores (and optional detail points) for existing matches.

## New Endpoint

### `POST /api/Match/upload-results`

**Content-Type:** `multipart/form-data`

Uploads an `.xlsx` file to update scores for existing matches.

---

## Excel File Format

The Excel file must have the following columns **in this exact order**, with headers in the first row:

| Column | Header            | Type    | Required | Description                                          |
|--------|-------------------|---------|----------|------------------------------------------------------|
| A      | PartidoId         | Integer | ✅       | The `Id` of the match to update                      |
| B      | ResultadoEquipoA  | Decimal | ✅       | Score for Team A (`ScoreA`)                          |
| C      | ResultadoEquipoB  | Decimal | ✅       | Score for Team B (`ScoreB`)                          |
| D      | DetallePuntosA    | Decimal | ❌       | Optional detail points for Team A (`DetailPointA`)   |
| E      | DetallePuntosB    | Decimal | ❌       | Optional detail points for Team B (`DetailPointB`)   |

### Example Excel Content

| PartidoId | ResultadoEquipoA | ResultadoEquipoB | DetallePuntosA | DetallePuntosB |
|-----------|------------------|------------------|----------------|----------------|
| 1         | 3                | 1                | 10.5           | 5.0            |
| 2         | 2                | 2                |                |                |
| 5         | 0                | 1                | 3.0            |                |

- Row 1: Match #1 gets ScoreA=3, ScoreB=1, DetailPointA=10.5, DetailPointB=5.0
- Row 2: Match #2 gets ScoreA=2, ScoreB=2 (no detail points — existing values are preserved)
- Row 3: Match #5 gets ScoreA=0, ScoreB=1, DetailPointA=3.0 (DetailPointB left unchanged)

---

## Response Format

### Success Response (`200 OK`)

```json
{
  "updated": [
    {
      "id": 1,
      "location": "Cancha 1",
      "scoreA": 3,
      "scoreB": 1,
      "detailPointA": 10.5,
      "detailPointB": 5.0,
      "startDate": "2026-03-15T10:00:00",
      "endDate": "2026-03-15T12:00:00",
      "numberMatch": 1,
      "participationAId": 10,
      "participationASchoolName": "Colegio San Agustín",
      "participationBId": 11,
      "participationBSchoolName": "Colegio La Salle",
      "matchStatusId": 1,
      "matchStatusName": "Pendiente"
    }
  ],
  "errors": [
    "Fila 4: Partido con Id 999 no fue encontrado."
  ]
}
```

The response always contains:
- **`updated`**: Array of `MatchReadDto` objects for matches that were successfully updated.
- **`errors`**: Array of error messages for rows that failed. Each error specifies the row number and reason.

### Error Scenarios

| Scenario                           | Error Message Example                                           |
|------------------------------------|-----------------------------------------------------------------|
| Invalid PartidoId (not a number)   | `Fila 3: PartidoId 'abc' no es un número válido.`              |
| Invalid ResultadoEquipoA           | `Fila 3: ResultadoEquipoA 'xyz' no es un número válido.`       |
| Invalid ResultadoEquipoB           | `Fila 3: ResultadoEquipoB '' no es un número válido.`          |
| Invalid DetallePuntosA             | `Fila 3: DetallePuntosA 'bad' no es un número válido.`         |
| Invalid DetallePuntosB             | `Fila 3: DetallePuntosB 'bad' no es un número válido.`         |
| Match not found / inactive         | `Fila 3: Partido con Id 999 no fue encontrado.`                |

> **Note:** Rows with errors are skipped — valid rows in the same file are still processed. The response includes both the successfully updated matches and the error list, so you can display partial results.

---

## Svelte (ShadCN) Implementation Guide

### 1. File Input Component

```svelte
<script lang="ts">
  import { Button } from "$lib/components/ui/button";
  import { Input } from "$lib/components/ui/input";
  import { Label } from "$lib/components/ui/label";

  let file: File | null = null;
  let loading = false;
  let result: { updated: any[]; errors: string[] } | null = null;

  async function uploadResults() {
    if (!file) return;
    loading = true;
    result = null;

    const formData = new FormData();
    formData.append("file", file);

    try {
      const res = await fetch("/api/Match/upload-results", {
        method: "POST",
        body: formData,
        credentials: "include", // send accessToken cookie
      });
      result = await res.json();
    } catch (err) {
      console.error(err);
    } finally {
      loading = false;
    }
  }
</script>

<div class="space-y-4">
  <Label for="results-file">Subir resultados de partidos (.xlsx)</Label>
  <Input
    id="results-file"
    type="file"
    accept=".xlsx"
    on:change={(e) => (file = e.target.files?.[0] ?? null)}
  />
  <Button on:click={uploadResults} disabled={!file || loading}>
    {loading ? "Subiendo..." : "Subir resultados"}
  </Button>
</div>

{#if result}
  {#if result.updated.length > 0}
    <div class="mt-4 p-4 bg-green-50 rounded">
      <p class="font-semibold text-green-700">
        {result.updated.length} partido(s) actualizados
      </p>
    </div>
  {/if}

  {#if result.errors.length > 0}
    <div class="mt-4 p-4 bg-red-50 rounded">
      <p class="font-semibold text-red-700">Errores:</p>
      <ul class="list-disc pl-5 text-sm text-red-600">
        {#each result.errors as error}
          <li>{error}</li>
        {/each}
      </ul>
    </div>
  {/if}
{/if}
```

### 2. Key Differences from Match Creation Upload

| Feature         | `/api/Match/upload` (Create)             | `/api/Match/upload-results` (Results)     |
|-----------------|------------------------------------------|-------------------------------------------|
| Purpose         | Create new matches from Excel            | Update scores of existing matches         |
| Response field  | `created` (array of new matches)         | `updated` (array of updated matches)      |
| Columns         | Fecha, Hora, TorneoId, ColegioA, etc.    | PartidoId, ResultadoEquipoA/B, Detalle    |
| Match lookup    | By school name + tournament              | By match ID directly                      |
| Optional cols   | None                                     | DetallePuntosA, DetallePuntosB            |

---

## Existing Endpoints Reference

| Method | Endpoint                    | Description                          |
|--------|-----------------------------|--------------------------------------|
| GET    | `/api/Match`                | List matches (paginated + filtered)  |
| GET    | `/api/Match/{id}`           | Get single match by ID               |
| POST   | `/api/Match`                | Create a single match                |
| POST   | `/api/Match/upload`         | Bulk create matches from Excel       |
| POST   | `/api/Match/upload-results` | **NEW** Bulk update results from Excel |
| PUT    | `/api/Match/{id}`           | Update a single match                |
| DELETE | `/api/Match/{id}?soft=true` | Delete a match (soft/hard)           |

