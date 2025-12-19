# Guía de Generación de Cotizaciones

## Descripción

Sistema mejorado para generar cotizaciones formales en formato colombiano desde archivos Excel, adaptado para el Sistema General de Regalías (SGR) y entidades públicas colombianas.

## Características Principales

### ✅ Terminología Colombiana
- **"Cotización"** en lugar de "quote"
- **"Ítem"** en lugar de "item"
- **"Valor unitario"** y **"Valor total"** en lugar de "unit price" / "total price"
- **"Subtotal por actividad"** y **"Total general"** en pesos colombianos ($ COP)
- Formato de moneda: **$1.234.567 COP** (con puntos como separadores de miles)

### ✅ Validación Mejorada de Ítems
- Valida que **CANTIDAD** sea numérica > 0
- Valida que **VALOR UNITARIO** sea numérico > 0
- Ignora automáticamente filas con texto como "TOTAL", "SUBTOTAL", "NOTA", etc.
- Usa columna **ACTIVIDAD** para agrupar ítems en la cotización

### ✅ Generación con Gemini
- Genera cotización en formato **tabla markdown** profesional
- Agrupa ítems por **actividad**
- Incluye **subtotal por actividad**
- Incluye **total general**
- Opción de incluir **IVA (19%)** y **Total con IVA**

### ✅ Manejo de Errores en Español
- Si no hay ítems válidos: "No se encontraron ítems válidos para cotizar en el archivo."
- Si falta una columna clave: "El archivo no contiene la columna requerida: ACTIVIDAD o CANTIDAD o VALOR UNITARIO."

### ✅ Compatibilidad con Streamlit
- Interfaz web para cargar Excel
- Checkbox: "Incluir IVA (19%)"
- Visualización de cotización en pantalla
- Descarga en formato `.md` (Markdown) o `.html`

## Uso del Endpoint API

### POST `/cotizacion/generar`

**Parámetros:**
- `file` (multipart/form-data): Archivo Excel (.xlsx o .xls)
- `incluir_iva` (query param, opcional): `true` o `false` (por defecto: `false`)
- `tasa_iva` (query param, opcional): Tasa de IVA como decimal (por defecto: `0.19`)

**Ejemplo con cURL:**

```bash
curl -X POST "http://localhost:8001/cotizacion/generar?incluir_iva=true&tasa_iva=0.19" \
  -F "file=@cotizacion.xlsx"
```

**Respuesta:**

```json
{
  "message": "Cotización generada exitosamente",
  "filename": "cotizacion.xlsx",
  "cotizacion_markdown": "| Ítem | Descripción | Cantidad | Valor unitario | Valor total |\n...",
  "items": [
    {
      "actividad": "Servicio de consultoría",
      "cantidad": 40,
      "valor_unitario": 150000,
      "valor_total": 6000000,
      "justificacion": "Horas de consultoría especializada"
    }
  ],
  "totales": {
    "subtotal": 6000000,
    "iva": 1140000,
    "total": 7140000,
    "tasa_iva": 0.19
  },
  "incluye_iva": true,
  "tasa_iva": 0.19,
  "total_items": 1,
  "fecha_generacion": "2024-12-05T10:30:00"
}
```

## Formato del Archivo Excel

### Columnas Requeridas

El sistema detecta automáticamente las columnas, pero deben estar presentes:

1. **ACTIVIDAD** (o variantes: "Descripción", "Concepto", "Ítem", "Producto", "Servicio")
   - Descripción del ítem a cotizar

2. **CANTIDAD** (o variantes: "Cant", "Unidades", "Número", "Qty")
   - Debe ser numérica y mayor a 0

3. **VALOR UNITARIO** (o variantes: "V. Unitario", "Costo Unitario", "Precio Unitario")
   - Debe ser numérico y mayor a 0
   - En pesos colombianos (COP)

### Columnas Opcionales

4. **VALOR TOTAL** (o variantes: "V. Total", "Costo Total", "Total", "Subtotal")
   - Si no está presente, se calcula como: CANTIDAD × VALOR UNITARIO

5. **JUSTIFICACIÓN** (o variantes: "Justificación", "Observaciones", "Notas")
   - Texto descriptivo adicional del ítem

### Ejemplo de Archivo Excel

| ACTIVIDAD | CANTIDAD | VALOR UNITARIO | VALOR TOTAL | JUSTIFICACIÓN |
|-----------|----------|----------------|-------------|---------------|
| Servicio de consultoría técnica | 40 | 150000 | 6000000 | Horas de consultoría especializada |
| Licencias de software | 10 | 500000 | 5000000 | Licencias anuales Microsoft 365 |
| Equipos de cómputo | 5 | 2000000 | 10000000 | Laptops para desarrollo |

### Validaciones Automáticas

- ✅ Se ignoran filas con palabras: TOTAL, SUBTOTAL, NOTA, OBSERVACIÓN, etc.
- ✅ CANTIDAD debe ser numérica y > 0
- ✅ VALOR UNITARIO debe ser numérico y > 0
- ✅ Los ítems se agrupan automáticamente por actividad

## Uso en Streamlit

1. **Abrir la aplicación Streamlit:**
   ```bash
   cd RAG-Service
   streamlit run streamlit_app.py
   ```

2. **Ir a la pestaña "Cotizaciones"**

3. **Cargar archivo Excel:**
   - Hacer clic en "Cargar archivo Excel"
   - Seleccionar archivo .xlsx o .xls

4. **Configurar IVA (opcional):**
   - Marcar checkbox "Incluir IVA (19%)" si aplica
   - Ajustar tasa de IVA si es diferente al 19%

5. **Generar cotización:**
   - Hacer clic en "Generar Cotización"
   - La cotización se mostrará en formato de tabla markdown

6. **Descargar:**
   - Descargar como Markdown (.md)
   - Descargar como HTML (.html)

## Estructura de la Cotización Generada

La cotización se genera en formato de tabla markdown con la siguiente estructura:

```markdown
| Ítem | Descripción | Cantidad | Valor unitario | Valor total |
|------|-------------|----------|----------------|-------------|
| **ACTIVIDAD: [Nombre de la actividad]** | | | | |
| 1 | Descripción del ítem 1 | 10 | $150.000 COP | $1.500.000 COP |
| 2 | Descripción del ítem 2 | 5 | $200.000 COP | $1.000.000 COP |
| | **Subtotal por actividad** | | | **$2.500.000 COP** |
| | | | | |
| **ACTIVIDAD: [Otra actividad]** | | | | |
| 3 | Descripción del ítem 3 | 2 | $500.000 COP | $1.000.000 COP |
| | **Subtotal por actividad** | | | **$1.000.000 COP** |
| | | | | |
| | **TOTAL GENERAL** | | | **$3.500.000 COP** |
| | IVA (19%) | | | **$665.000 COP** |
| | **TOTAL CON IVA** | | | **$4.165.000 COP** |
```

## Mejoras Implementadas en BudgetExtractor

El servicio `BudgetExtractor` también fue mejorado con:

1. **Validación estricta:**
   - CANTIDAD debe ser > 0
   - VALOR UNITARIO debe ser > 0
   - Retorna `None` si los valores no son válidos

2. **Filtrado mejorado:**
   - Ignora filas con palabras clave: TOTAL, SUBTOTAL, NOTA, etc.

3. **Terminología colombiana:**
   - Reconoce variantes de columnas en español colombiano
   - Mejor detección de columnas con acentos y variantes

## Solución de Problemas

### Error: "No se encontraron ítems válidos para cotizar en el archivo"
- Verifica que el archivo tenga al menos una fila con datos válidos
- Asegúrate de que CANTIDAD y VALOR UNITARIO sean numéricos y > 0
- Verifica que no todas las filas contengan palabras clave ignoradas (TOTAL, SUBTOTAL, etc.)

### Error: "El archivo no contiene la columna requerida: ACTIVIDAD o CANTIDAD o VALOR UNITARIO"
- Verifica que el archivo tenga columnas con estos nombres (o variantes reconocidas)
- El sistema reconoce variantes como "Descripción", "Cant", "V. Unitario", etc.
- Revisa que los nombres de columnas estén en la primera fila del Excel

### La cotización no incluye IVA aunque lo marqué
- Verifica que hayas marcado el checkbox "Incluir IVA (19%)"
- Revisa que la tasa de IVA sea correcta (por defecto 19%)

### Los ítems no se agrupan por actividad
- El sistema agrupa automáticamente por la columna ACTIVIDAD
- Si todos los ítems tienen la misma actividad, se agruparán en "General"
- Verifica que la columna ACTIVIDAD tenga valores distintos para diferentes grupos

## Archivos Modificados

1. **`services/cotizacion_service.py`** (NUEVO)
   - Servicio principal para generar cotizaciones
   - Validación de ítems
   - Generación con Gemini
   - Formato colombiano

2. **`services/budget_extractor.py`** (MEJORADO)
   - Validación estricta de CANTIDAD y VALOR UNITARIO
   - Filtrado de filas con palabras clave
   - Terminología colombiana mejorada

3. **`main.py`** (MEJORADO)
   - Nuevo endpoint `/cotizacion/generar`
   - Manejo de errores en español

4. **`streamlit_app.py`** (MEJORADO)
   - Nueva pestaña "Cotizaciones"
   - Interfaz para cargar Excel y generar cotizaciones
   - Descarga en formato .md y .html

## Dependencias

Asegúrate de tener instaladas las dependencias:

```bash
pip install -r requirements.txt
```

Si quieres descargar como HTML, también necesitas:

```bash
pip install markdown
```

## Ejemplo Completo

Ver el archivo de ejemplo en la documentación o crear un Excel con el formato mostrado arriba.

## Notas Técnicas

- El sistema usa Gemini 1.5 Pro para generar cotizaciones formales
- Si Gemini no está disponible, se genera una cotización básica sin LLM
- Los valores se formatean automáticamente con separadores de miles (puntos)
- El formato de moneda es siempre en pesos colombianos (COP)

