# 🏆 TournamentRoster — Excel Bulk Upload (Frontend Guide)

> **Date:** 2026-03-10  
> **New feature:** Mass-create tournament roster entries from an Excel file.

---

## Summary

A new endpoint allows uploading an `.xlsx` file to bulk-create `TournamentRoster` entries. The Excel file contains player names and document numbers along with a tournament ID and a school name. The backend resolves the school by searching for a **participation** in the given tournament whose school name **contains** the value from the Excel (case-insensitive).

---

## Endpoint

```http
POST /api/TournamentRoster/upload
Content-Type: multipart/form-data

file: <archivo.xlsx>
```

---

## Excel Format

The first row is treated as a **header** and is skipped. Data starts from row 2.

| Column | Header              | Type    | Required | Description |
|--------|---------------------|---------|----------|-------------|
| A (1)  | Nombre              | string  | ✅       | First name |
| B (2)  | Segundo Nombre      | string  | ❌       | Middle name (optional) |
| C (3)  | Apellido Paterno    | string  | ✅       | Last name (paternal) |
| D (4)  | Apellido Materno    | string  | ❌       | Maternal last name (optional) |
| E (5)  | NroDocumento        | string  | ❌       | Document number (optional) |
| F (6)  | TorneoId            | integer | ✅       | Tournament ID |
| G (7)  | Colegio             | string  | ✅       | School name (partial match, see below) |

### School Name Resolution

The `Colegio` column does **not** need to be the exact school name. The backend searches for an **active participation** in the specified tournament whose school name **contains** the provided text (case-insensitive).

**Example:** If the school in the database is `"Colegio La Salle - Santa Cruz"` and the Excel cell contains `"La Salle"`, it will match.

⚠️ If no matching participation is found, the row is **skipped** and an error is added to the response.

---

## Response

### Success (`200 OK`)

```json
{
  "created": [
    {
      "id": 10,
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
    },
    {
      "id": 11,
      "firstName": "María",
      "middleName": null,
      "lastName": "García",
      "maternalName": "Flores",
      "documentNumber": null,
      "fullName": "María García Flores",
      "tournamentId": 1,
      "tournamentName": "Copa UPSA 2026",
      "schoolId": 5,
      "schoolName": "Colegio La Salle"
    }
  ],
  "errors": [
    "Fila 4: No se encontró un colegio con participación en el torneo 1 cuyo nombre contenga 'Colegio Fantasma'.",
    "Fila 6: 'Nombre' es obligatorio."
  ]
}
```

> **Note:** The upload is **not transactional** — rows that succeed are created even if other rows fail. The `errors` array tells you exactly which rows had problems.

### Validation errors per row

| Error | Cause |
|-------|-------|
| `'Nombre' es obligatorio` | Column A is empty |
| `'Apellido Paterno' es obligatorio` | Column C is empty |
| `TorneoId 'X' no es un número válido` | Column F is not a valid integer |
| `'Colegio' es obligatorio` | Column G is empty |
| `No se encontró un colegio con participación en el torneo X cuyo nombre contenga 'Y'` | No active participation found for that school name + tournament |

---

## Excel Template Example

| Nombre | Segundo Nombre | Apellido Paterno | Apellido Materno | NroDocumento | TorneoId | Colegio |
|--------|----------------|------------------|------------------|--------------|----------|---------|
| Juan   | Carlos         | Pérez            | López            | 12345678     | 1        | La Salle |
| María  |                | García           | Flores           |              | 1        | La Salle |
| Pedro  | Antonio        | Ramírez          |                  | 87654321     | 1        | Don Bosco |

---

## Svelte / shadcn Implementation

### File Upload Component

```svelte
<script lang="ts">
  import { Button } from '$lib/components/ui/button';
  import { Input } from '$lib/components/ui/input';

  let fileInput: HTMLInputElement;
  let uploading = false;
  let result: { created: any[]; errors: string[] } | null = null;

  async function upload() {
    if (!fileInput?.files?.[0]) return;

    uploading = true;
    result = null;

    const formData = new FormData();
    formData.append('file', fileInput.files[0]);

    try {
      const res = await fetch('/api/TournamentRoster/upload', {
        method: 'POST',
        body: formData
        // ⚠️ Do NOT set Content-Type header — the browser adds it with the correct boundary
      });

      if (res.ok) {
        result = await res.json();
      } else {
        const errors = await res.json();
        result = { created: [], errors: Array.isArray(errors) ? errors : [errors] };
      }
    } catch (err) {
      result = { created: [], errors: ['Error de conexión con el servidor'] };
    } finally {
      uploading = false;
    }
  }
</script>

<div class="space-y-4">
  <Input
    type="file"
    accept=".xlsx,.xls"
    bind:this={fileInput}
    on:change={() => (result = null)}
  />
  <Button on:click={upload} disabled={uploading}>
    {uploading ? 'Subiendo...' : 'Subir Excel'}
  </Button>

  {#if result}
    {#if result.created.length > 0}
      <div class="rounded-md bg-green-50 p-4">
        <p class="font-medium text-green-800">
          ✅ {result.created.length} jugador(es) creado(s) exitosamente.
        </p>
      </div>
    {/if}

    {#if result.errors.length > 0}
      <div class="rounded-md bg-red-50 p-4 space-y-1">
        <p class="font-medium text-red-800">⚠️ Errores:</p>
        {#each result.errors as error}
          <p class="text-sm text-red-700">• {error}</p>
        {/each}
      </div>
    {/if}
  {/if}
</div>
```

### TypeScript Types

```typescript
interface TournamentRosterUploadResultDto {
  created: TournamentRosterReadDto[];
  errors: string[];
}

interface TournamentRosterReadDto {
  id: number;
  firstName: string;
  middleName: string | null;
  lastName: string;
  maternalName: string | null;
  documentNumber: string | null;
  fullName: string;
  tournamentId: number;
  tournamentName: string;
  schoolId: number;
  schoolName: string;
}
```

---

## Important Notes

1. **School matching is partial and case-insensitive.** Writing `"Salle"` will match `"Colegio La Salle"`. Make sure your Excel values are specific enough to avoid matching the wrong school.
2. **The school must have an active participation** in the specified tournament. If no participation exists, the row is rejected.
3. **Empty optional fields** (middle name, maternal name, document number) are stored as `null`.
4. **The upload is non-transactional** — successful rows are persisted even if other rows fail.
5. **The file must be `.xlsx` format** (Excel 2007+).

