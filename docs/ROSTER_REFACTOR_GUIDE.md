# 🏆 Copa UPSA — Roster Refactor Guide (Frontend)

> **Fecha:** 7 de Marzo, 2026
> **Backend:** .NET 9 Web API
> **Frontend:** Svelte + shadcn-svelte
> **Base URL:** `http://localhost:5025/api`

---

## 📋 Resumen del cambio

Se reestructuró completamente la relación de los **Roster** (alineaciones por partido).

### Antes (❌ Eliminado)

```
SchoolStudent ──────► Roster ──────► Match
                        │
                        ▼
                     Position
```

Un `Roster` conectaba directamente un `SchoolStudent` con un `Match`. No había forma de saber qué jugadores estaban inscritos en qué torneo.

### Ahora (✅ Nuevo)

```
SchoolStudent ──► TournamentRoster ──► Roster ──► Match
                        │                 │
                        ▼                 ▼
                   Tournament          Position
                        │
                        ▼
                     School
```

Se introduce una nueva entidad intermedia **`TournamentRoster`** que representa la inscripción de un jugador a un torneo específico para un colegio. El `Roster` ahora apunta a `TournamentRoster` en lugar de a `SchoolStudent` directamente.

---

## 🔑 Conceptos clave

| Concepto | Descripción |
|---|---|
| **TournamentRoster** | "Lista de convocados" — Un jugador inscrito en un torneo para un colegio. Se crea **una vez por torneo**. |
| **Roster** | "Alineación por partido" — Un convocado (`TournamentRoster`) asignado a un partido específico con una posición. Se crea **por cada partido**. |

### Flujo de uso esperado

```
1. Admin crea un Torneo (Tournament)
2. Admin crea Participaciones (Participation) → Colegio inscrito en un torneo
3. Admin inscribe jugadores al torneo → POST /api/TournamentRoster  ← NUEVO
4. Admin crea partidos (Match) entre dos participaciones
5. Admin arma la alineación del partido → POST /api/Roster (usando TournamentRosterId)
6. Se registran estadísticas sobre el Roster → StatisticEvent
```

---

## 🆕 Nuevo endpoint: `TournamentRoster`

**Base:** `GET/POST/PUT/DELETE /api/TournamentRoster`

Este endpoint reemplaza la relación directa `SchoolStudent ↔ Roster`. Ahora primero se inscribe al alumno en un torneo, y luego se usa ese registro para armar alineaciones.

### Tipos TypeScript

```typescript
// ── Response ──
interface TournamentRosterReadDto {
  id: number;
  schoolStudentId: number;
  studentName: string;      // "Juan Pérez" (auto-generado por el backend)
  tournamentId: number;
  tournamentName: string;   // "Copa UPSA 2026" (auto-generado)
  schoolId: number;
  schoolName: string;       // "Colegio La Salle" (auto-generado)
}

// ── Crear ──
interface TournamentRosterCreateDto {
  schoolStudentId: number;
  tournamentId: number;
  schoolId: number;
}

// ── Actualizar (parcial) ──
interface TournamentRosterUpdateDto {
  schoolStudentId?: number;
  tournamentId?: number;
  schoolId?: number;
}

// ── Filtros (query params en GET) ──
interface TournamentRosterFilter {
  schoolStudentId?: number;
  tournamentId?: number;
  schoolId?: number;
  active?: boolean;
  name?: string;            // Busca por nombre del estudiante
}
```

### Endpoints

#### Listar convocados de un torneo

```http
GET /api/TournamentRoster?tournamentId=1&schoolId=5&pageNumber=1&pageSize=20
```

**Response 200:**
```json
{
  "items": [
    {
      "id": 1,
      "schoolStudentId": 42,
      "studentName": "Juan Pérez",
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

#### Obtener uno por ID

```http
GET /api/TournamentRoster/1
```

#### Inscribir jugador a un torneo

```http
POST /api/TournamentRoster
Content-Type: application/json

{
  "schoolStudentId": 42,
  "tournamentId": 1,
  "schoolId": 5
}
```

**Response 201:**
```json
{
  "id": 1,
  "schoolStudentId": 42,
  "studentName": "Juan Pérez",
  "tournamentId": 1,
  "tournamentName": "Copa UPSA 2026",
  "schoolId": 5,
  "schoolName": "Colegio La Salle"
}
```

**Response 400 (validación):**
```json
["SchoolStudentId (999) not found", "TournamentId (888) not found"]
```

#### Actualizar

```http
PUT /api/TournamentRoster/1
Content-Type: application/json

{
  "schoolStudentId": 43
}
```

**Response:** `204 No Content`

#### Eliminar (soft delete por defecto)

```http
DELETE /api/TournamentRoster/1
DELETE /api/TournamentRoster/1?soft=false   ← eliminación permanente
```

**Response:** `204 No Content`

---

## 🔄 Endpoint modificado: `Roster`

**Base:** `GET/POST/PUT/DELETE /api/Roster`

### ⚠️ Cambio importante (BREAKING CHANGE)

| Campo anterior | Campo nuevo | Notas |
|---|---|---|
| `schoolStudentId` | `tournamentRosterId` | En **todos** los DTOs (read, create, update, filter) |

### Tipos TypeScript actualizados

```typescript
// ── Response ──
interface RosterReadDto {
  id: number;
  matchId: number;
  tournamentRosterId: number;  // ← ANTES era schoolStudentId
  positionId: number;
  studentName: string;         // Se resuelve via TournamentRoster → SchoolStudent
  matchName: string;           // "Match #1 - 2026-03-15"
  positionName: string;        // "Portero"
}

// ── Crear ──
interface RosterCreateDto {
  matchId: number;
  tournamentRosterId: number;  // ← ANTES era schoolStudentId
  positionId: number;
}

// ── Actualizar (parcial) ──
interface RosterUpdateDto {
  matchId?: number;
  tournamentRosterId?: number; // ← ANTES era schoolStudentId
  positionId?: number;
}

// ── Filtros (query params en GET) ──
interface RosterFilter {
  matchId?: number;
  participationId?: number;
  tournamentRosterId?: number; // ← ANTES era schoolStudentId
  positionId?: number;
  active?: boolean;
  name?: string;               // Busca por nombre del estudiante
}
```

### Ejemplo: Armar alineación de un partido

```http
POST /api/Roster
Content-Type: application/json

{
  "matchId": 10,
  "tournamentRosterId": 1,
  "positionId": 3
}
```

### Ejemplo: Listar alineación de un partido

```http
GET /api/Roster?matchId=10&pageNumber=1&pageSize=30
```

**Response 200:**
```json
{
  "items": [
    {
      "id": 1,
      "matchId": 10,
      "tournamentRosterId": 1,
      "positionId": 3,
      "studentName": "Juan Pérez",
      "matchName": "Match #1 - 2026-03-15",
      "positionName": "Portero"
    }
  ],
  "pageNumber": 1,
  "pageSize": 30,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

## 🏟️ Endpoint `Match` — Recordatorio

El Match ya requiere **dos participaciones** (ParticipationA y ParticipationB).

### Tipos TypeScript

```typescript
interface MatchReadDto {
  id: number;
  location: string;
  scoreA: number;
  scoreB: number;
  detailPointA: number;
  detailPointB: number;
  startDate: string;        // ISO 8601
  endDate: string;
  numberMatch: number;
  participationAId: number;
  participationBId: number;
  matchStatusId: number;
}

interface MatchCreateDto {
  location: string;
  scoreA: number;
  scoreB: number;
  detailPointA: number;
  detailPointB: number;
  startDate: string;
  endDate: string;
  numberMatch: number;
  participationAId: number;  // Colegio local
  participationBId: number;  // Colegio visitante
  matchStatusId: number;
}
```

### Upload masivo desde Excel

```http
POST /api/Match/upload
Content-Type: multipart/form-data

file: <archivo.xlsx>
```

**Columnas esperadas del Excel (en orden):**

| # | Columna | Formato | Ejemplo |
|---|---------|---------|---------|
| 1 | Fecha | `dd-MM-yyyy` | `15-03-2026` |
| 2 | Hora | `HH:mm:ss` | `14:30:00` |
| 3 | TorneoId | entero | `1` |
| 4 | ColegioA | nombre exacto | `Colegio La Salle` |
| 5 | ColegioB | nombre exacto | `Colegio Don Bosco` |
| 6 | NumeroFecha | entero | `1` |

**Response 200:**
```json
{
  "created": [
    {
      "id": 10,
      "location": "",
      "scoreA": 0,
      "scoreB": 0,
      "startDate": "2026-03-15T14:30:00Z",
      "numberMatch": 1,
      "participationAId": 3,
      "participationBId": 7,
      "matchStatusId": 1
    }
  ],
  "errors": [
    "Fila 3: 'Colegio Fantasma' no tiene una participación en el torneo 1."
  ]
}
```

> **Nota para Svelte:** Para subir el archivo usa `FormData`:
> ```typescript
> const formData = new FormData();
> formData.append('file', fileInput.files[0]);
>
> const res = await fetch('/api/Match/upload', {
>   method: 'POST',
>   body: formData
>   // NO pongas Content-Type, el browser lo agrega con el boundary
> });
> ```

---

## 🧩 Guía de migración para el frontend (Svelte)

### 1. Buscar y reemplazar en todo el proyecto

```
schoolStudentId  →  tournamentRosterId
SchoolStudentId  →  TournamentRosterId
```

Archivos probablemente afectados:
- Tipos/interfaces TypeScript de Roster
- Formularios de creación/edición de Roster
- Tablas/listas de Roster
- Llamadas fetch/axios a `/api/Roster`
- Stores o servicios de Roster

### 2. Crear nuevo servicio para TournamentRoster

```typescript
// src/lib/api/tournamentRoster.ts

const BASE = '/api/TournamentRoster';

export async function getTournamentRosters(params: Record<string, string>) {
  const query = new URLSearchParams(params).toString();
  const res = await fetch(`${BASE}?${query}`);
  return res.json();
}

export async function getTournamentRosterById(id: number) {
  const res = await fetch(`${BASE}/${id}`);
  return res.json();
}

export async function createTournamentRoster(dto: TournamentRosterCreateDto) {
  const res = await fetch(BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
  });
  return res.json();
}

export async function updateTournamentRoster(id: number, dto: TournamentRosterUpdateDto) {
  return fetch(`${BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
  });
}

export async function deleteTournamentRoster(id: number, soft = true) {
  return fetch(`${BASE}/${id}?soft=${soft}`, { method: 'DELETE' });
}
```

### 3. Nueva pantalla: "Convocados del Torneo"

Crear una nueva vista/página que permita:

1. **Seleccionar un torneo** (combo/select de Tournament)
2. **Seleccionar un colegio** (combo/select de School, filtrado por participaciones del torneo)
3. **Ver la lista de convocados** (`GET /api/TournamentRoster?tournamentId=X&schoolId=Y`)
4. **Agregar jugadores** → Select de SchoolStudent del colegio → `POST /api/TournamentRoster`
5. **Remover jugadores** → `DELETE /api/TournamentRoster/{id}`

Ejemplo de componente con shadcn:

```svelte
<script lang="ts">
  import { Button } from '$lib/components/ui/button';
  import * as Table from '$lib/components/ui/table';
  import * as Select from '$lib/components/ui/select';

  let selectedTournament: number;
  let selectedSchool: number;
  let rosters: TournamentRosterReadDto[] = [];

  async function loadRosters() {
    const data = await getTournamentRosters({
      tournamentId: String(selectedTournament),
      schoolId: String(selectedSchool),
      pageSize: '100'
    });
    rosters = data.items;
  }
</script>

<div class="space-y-4">
  <!-- Selectores de torneo y colegio -->
  <!-- ... -->

  <Table.Root>
    <Table.Header>
      <Table.Row>
        <Table.Head>Jugador</Table.Head>
        <Table.Head>Colegio</Table.Head>
        <Table.Head>Acciones</Table.Head>
      </Table.Row>
    </Table.Header>
    <Table.Body>
      {#each rosters as roster}
        <Table.Row>
          <Table.Cell>{roster.studentName}</Table.Cell>
          <Table.Cell>{roster.schoolName}</Table.Cell>
          <Table.Cell>
            <Button variant="destructive" size="sm"
              on:click={() => deleteTournamentRoster(roster.id)}>
              Remover
            </Button>
          </Table.Cell>
        </Table.Row>
      {/each}
    </Table.Body>
  </Table.Root>
</div>
```

### 4. Modificar pantalla de "Alineación por partido"

El formulario de Roster ahora debe:

1. Recibir el `matchId` del partido seleccionado
2. Cargar los **convocados del torneo** (no todos los estudiantes) filtrando por el torneo del partido
3. Mostrar un select con los `TournamentRoster` disponibles
4. Enviar `tournamentRosterId` en lugar de `schoolStudentId`

```
Flujo UI:
┌─────────────────────────────────────────────────┐
│  Partido: Colegio A vs Colegio B                │
│  Fecha: 15/03/2026                              │
├─────────────────────────────────────────────────┤
│  Alineación Local (Colegio A):                  │
│  ┌────────────────────┬──────────────┬────────┐ │
│  │ Jugador (select)   │ Posición     │ Acción │ │
│  │ Juan Pérez         │ Portero      │   🗑️   │ │
│  │ María López        │ Delantera    │   🗑️   │ │
│  │ [Agregar jugador]  │ [Posición ▼] │   ➕   │ │
│  └────────────────────┴──────────────┴────────┘ │
│                                                  │
│  Alineación Visitante (Colegio B):              │
│  ┌────────────────────┬──────────────┬────────┐ │
│  │ ...                │ ...          │ ...    │ │
│  └────────────────────┴──────────────┴────────┘ │
└─────────────────────────────────────────────────┘
```

Para cargar los convocados disponibles del equipo local:

```typescript
// Obtener la participación A del match para saber el schoolId y tournamentId
const match = await getMatchById(matchId);
const participationA = await getParticipationById(match.participationAId);

// Cargar convocados de ese colegio en ese torneo
const convocados = await getTournamentRosters({
  tournamentId: String(participationA.tournamentId),
  schoolId: String(participationA.schoolId),
  pageSize: '100'
});
// Usar convocados.items para popular el <Select>
```

---

## 📌 Paginación (todos los endpoints GET de listado)

Todos los endpoints de listado aceptan estos query params:

| Param | Default | Max | Descripción |
|---|---|---|---|
| `pageNumber` | `1` | — | Página actual |
| `pageSize` | `10` | `500` | Items por página |

La respuesta siempre incluye:

```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 47,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## ⚠️ Errores comunes

| Error | Causa | Solución |
|---|---|---|
| `TournamentRosterId (X) not found` | El ID no existe en TournamentRosters | Verificar que el jugador fue inscrito al torneo primero |
| `SchoolStudentId (X) not found` | El alumno no existe | Verificar el ID del alumno |
| `MatchId (X) not found` | El partido no existe | Verificar el ID del partido |
| `PositionId (X) not found` | La posición no existe | Verificar el ID de la posición |

---

## 🗺️ Diagrama de relaciones (referencia rápida)

```
Tournament
  ├── Participation[] ──► SchoolTable
  │     ├── Match[] (como ParticipationA)
  │     └── Match[] (como ParticipationB)
  │
  └── TournamentRoster[] ──► SchoolStudent     ← NUEVO
        │                 └──► SchoolTable
        │
        └── Roster[] ──► Match
                     └──► Position
                     └──► StatisticEvent[]
```

---

## ✅ Checklist de migración frontend

- [ ] Actualizar tipos TypeScript (reemplazar `schoolStudentId` → `tournamentRosterId` en Roster)
- [ ] Crear servicio API para `TournamentRoster`
- [ ] Crear página de "Convocados del Torneo" (CRUD de TournamentRoster)
- [ ] Modificar formulario de "Alineación" para usar `TournamentRoster` como source de jugadores
- [ ] Modificar listado de Roster para mostrar datos via `TournamentRoster`
- [ ] Actualizar filtros de Roster (`schoolStudentId` → `tournamentRosterId`)
- [ ] Verificar que el upload de Excel de Match sigue funcionando (no cambió)
- [ ] Probar flujo completo: Torneo → Participación → Convocados → Partido → Alineación

