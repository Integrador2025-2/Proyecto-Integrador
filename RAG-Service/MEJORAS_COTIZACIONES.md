# Resumen de Mejoras: Sistema de Cotizaciones Colombiano

## ✅ Cambios Implementados

### 1. Nuevo Servicio de Cotizaciones (`services/cotizacion_service.py`)

**Características:**
- ✅ Terminología colombiana completa (Cotización, Ítem, Valor unitario, Valor total)
- ✅ Validación estricta: CANTIDAD > 0, VALOR UNITARIO > 0
- ✅ Filtrado automático de filas con TOTAL, SUBTOTAL, NOTA, etc.
- ✅ Agrupación por actividad
- ✅ Generación con Gemini en formato tabla markdown
- ✅ Soporte para IVA (19%) opcional
- ✅ Formato de moneda colombiana ($1.234.567 COP)

**Manejo de Errores:**
- "No se encontraron ítems válidos para cotizar en el archivo."
- "El archivo no contiene la columna requerida: ACTIVIDAD o CANTIDAD o VALOR UNITARIO."

### 2. Mejoras en BudgetExtractor (`services/budget_extractor.py`)

**Mejoras:**
- ✅ Validación estricta de CANTIDAD y VALOR UNITARIO (deben ser > 0)
- ✅ Filtrado mejorado: ignora filas con palabras clave (TOTAL, SUBTOTAL, NOTA, etc.)
- ✅ Terminología colombiana: reconoce variantes de columnas en español
- ✅ Mejor detección de columnas con acentos y variantes

### 3. Nuevo Endpoint API (`main.py`)

**POST `/cotizacion/generar`**
- Recibe archivo Excel
- Parámetros: `incluir_iva` (bool), `tasa_iva` (float)
- Retorna cotización en markdown y datos estructurados
- Manejo de errores en español

### 4. Interfaz Streamlit (`streamlit_app.py`)

**Nueva Pestaña "Cotizaciones":**
- ✅ Carga de archivo Excel
- ✅ Checkbox "Incluir IVA (19%)"
- ✅ Visualización de cotización en tabla markdown
- ✅ Descarga en formato .md (Markdown)
- ✅ Descarga en formato .html (HTML con estilos)
- ✅ Instrucciones y formato esperado del Excel

## 📋 Formato del Excel Requerido

### Columnas Obligatorias:
1. **ACTIVIDAD** (o variantes: Descripción, Concepto, Ítem, Producto, Servicio)
2. **CANTIDAD** (o variantes: Cant, Unidades, Número, Qty)
3. **VALOR UNITARIO** (o variantes: V. Unitario, Costo Unitario, Precio Unitario)

### Columnas Opcionales:
4. **VALOR TOTAL** (se calcula si no está presente)
5. **JUSTIFICACIÓN** (opcional)

### Ejemplo:
| ACTIVIDAD | CANTIDAD | VALOR UNITARIO | VALOR TOTAL | JUSTIFICACIÓN |
|-----------|----------|----------------|-------------|---------------|
| Servicio de consultoría | 40 | 150000 | 6000000 | Horas especializadas |
| Licencias software | 10 | 500000 | 5000000 | Licencias anuales |

## 🚀 Cómo Usar

### Opción 1: API REST

```bash
curl -X POST "http://localhost:8001/cotizacion/generar?incluir_iva=true" \
  -F "file=@cotizacion.xlsx"
```

### Opción 2: Streamlit

```bash
cd RAG-Service
streamlit run streamlit_app.py
```

Luego ir a la pestaña "Cotizaciones"

## 📝 Formato de Salida

La cotización se genera en formato tabla markdown:

```markdown
| Ítem | Descripción | Cantidad | Valor unitario | Valor total |
|------|-------------|----------|----------------|-------------|
| **ACTIVIDAD: [Nombre]** | | | | |
| 1 | Descripción ítem 1 | 10 | $150.000 COP | $1.500.000 COP |
| | **Subtotal por actividad** | | | **$1.500.000 COP** |
| | **TOTAL GENERAL** | | | **$1.500.000 COP** |
| | IVA (19%) | | | **$285.000 COP** |
| | **TOTAL CON IVA** | | | **$1.785.000 COP** |
```

## 🔧 Archivos Modificados/Creados

1. ✅ **NUEVO**: `services/cotizacion_service.py` - Servicio principal de cotizaciones
2. ✅ **MEJORADO**: `services/budget_extractor.py` - Validaciones mejoradas
3. ✅ **MEJORADO**: `main.py` - Nuevo endpoint `/cotizacion/generar`
4. ✅ **MEJORADO**: `streamlit_app.py` - Nueva pestaña "Cotizaciones"
5. ✅ **NUEVO**: `COTIZACIONES_GUIDE.md` - Documentación completa
6. ✅ **MEJORADO**: `requirements.txt` - Agregado `markdown` para exportación HTML

## ✨ Características Destacadas

- **100% Terminología Colombiana**: Todo en español colombiano formal
- **Validación Robusta**: Solo acepta ítems con valores válidos
- **Formato Profesional**: Tabla markdown lista para usar
- **IVA Opcional**: Configurable por actividad/proyecto
- **Interfaz Amigable**: Streamlit para usuarios no técnicos
- **Manejo de Errores Claro**: Mensajes en español comprensibles

## 🎯 Compatibilidad

- ✅ Mantiene compatibilidad con código existente
- ✅ No rompe funcionalidades actuales
- ✅ Extiende el sistema sin modificar lógica core
- ✅ Usa la misma infraestructura LLM (Gemini)

## 📚 Documentación

Ver `COTIZACIONES_GUIDE.md` para documentación completa y ejemplos.

