# 📘 Ejemplos de Uso - RAG Service API

Colección de ejemplos prácticos para usar la API del RAG Service.

## 🔧 Requisitos

- Servicio corriendo en http://localhost:8001
- `curl` instalado (o usa Postman/Thunder Client)

## 📝 Ejemplos

### 1. Health Check

```bash
curl http://localhost:8001/health
```

**Respuesta esperada:**
```json
{
  "status": "healthy",
  "message": "RAG Budget Automation Service is running"
}
```

---

### 2. Subir un documento PDF

```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@./documentos/proyecto_ejemplo.pdf" \
  -F "project_id=1" \
  -F "document_type=project_document"
```

**Respuesta esperada:**
```json
{
  "message": "Documento procesado exitosamente",
  "document_id": "doc_abc123",
  "filename": "proyecto_ejemplo.pdf",
  "project_id": 1
}
```

---

### 3. Hacer una consulta RAG

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "¿Cuáles son los objetivos específicos del proyecto?",
    "project_id": 1,
    "top_k": 5
  }'
```

**Respuesta esperada:**
```json
{
  "answer": "Los objetivos específicos del proyecto son:\n1. Desarrollar...",
  "sources": [
    {
      "content": "...",
      "metadata": {
        "filename": "proyecto_ejemplo.pdf",
        "page": 3
      }
    }
  ],
  "confidence": 0.85
}
```

---

### 4. Generar resumen ejecutivo completo

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "Genera un RESUMEN EJECUTIVO COMPLETO de este proyecto. Incluye justificación, objetivos, alcance territorial, población objetivo, componentes principales, actividades clave, resultados esperados y actores involucrados.",
    "project_id": 1,
    "top_k": 8
  }'
```

---

### 5. Extraer presupuesto desde archivo Excel

```bash
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@./presupuestos/presupuesto_proyecto.xlsx" \
  -F "project_id=1" \
  -F "sheet_name=Presupuesto"
```

**Respuesta esperada:**
```json
{
  "message": "Presupuesto extraído exitosamente",
  "total_items": 15,
  "items": [
    {
      "description": "Ingeniero de Software Senior",
      "quantity": 2,
      "unit_value": 8000000,
      "total_value": 16000000,
      "category": "TalentoHumano"
    },
    ...
  ],
  "total_budget": 45000000
}
```

---

### 6. Generar presupuesto completo con IA

```bash
curl -X POST "http://localhost:8001/budget/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "project_id": 1,
    "project_description": "Desarrollo de plataforma web de telemedicina cardiovascular para zonas rurales de Colombia, incluyendo módulos de consulta virtual, monitoreo remoto de signos vitales, y capacitación de personal médico local.",
    "duration_years": 2,
    "include_indirect_costs": true
  }'
```

**Respuesta esperada:**
```json
{
  "message": "Presupuesto generado exitosamente",
  "budget_id": "budget_xyz789",
  "total_amount": 125000000,
  "categories": {
    "TalentoHumano": 65000000,
    "EquiposSoftware": 30000000,
    "ServiciosTecnicos": 15000000,
    "MaterialesInsumos": 10000000,
    "CostosIndirectos": 5000000
  },
  "items_count": 28,
  "excel_file": "./generated_budgets/budget_xyz789.xlsx"
}
```

---

### 7. Obtener sugerencias de presupuesto

```bash
curl -X GET "http://localhost:8001/projects/1/budget/suggestions"
```

---

### 8. Generar plan de recursos

```bash
curl -X POST "http://localhost:8001/resources/plan" \
  -H "Content-Type: application/json" \
  -d '{
    "project_id": 1,
    "activities": [
      {
        "id": "act_1",
        "name": "Desarrollo del módulo de consulta virtual",
        "description": "Implementación del sistema de videollamadas y chat en tiempo real"
      },
      {
        "id": "act_2",
        "name": "Integración con dispositivos IoT médicos",
        "description": "Conexión con sensores de presión arterial y glucosa"
      }
    ],
    "available_budget": 50000000,
    "duration_months": 12
  }'
```

---

### 9. Listar documentos de un proyecto

```bash
curl -X GET "http://localhost:8001/projects/1/documents"
```

---

### 10. Eliminar un documento

```bash
curl -X DELETE "http://localhost:8001/documents/doc_abc123"
```

---

## 🐍 Ejemplos en Python

### Subir documento

```python
import requests

url = "http://localhost:8001/documents/upload"
files = {
    'file': open('proyecto.pdf', 'rb')
}
data = {
    'project_id': 1,
    'document_type': 'project_document'
}

response = requests.post(url, files=files, data=data)
print(response.json())
```

### Consultar con RAG

```python
import requests

url = "http://localhost:8001/query"
payload = {
    "question": "¿Cuál es el presupuesto total del proyecto?",
    "project_id": 1,
    "top_k": 5
}

response = requests.post(url, json=payload)
print(response.json()['answer'])
```

### Generar presupuesto

```python
import requests

url = "http://localhost:8001/budget/generate"
payload = {
    "project_id": 1,
    "project_description": "Desarrollo de app móvil educativa",
    "duration_years": 1
}

response = requests.post(url, json=payload)
result = response.json()
print(f"Presupuesto total: ${result['total_amount']:,}")
print(f"Archivo generado: {result['excel_file']}")
```

---

## 🌐 Ejemplos con JavaScript/Fetch

### Consultar con RAG

```javascript
const query = async () => {
  const response = await fetch('http://localhost:8001/query', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      question: '¿Cuáles son los objetivos del proyecto?',
      project_id: 1,
      top_k: 5
    })
  });
  
  const data = await response.json();
  console.log(data.answer);
};

query();
```

### Subir documento

```javascript
const uploadDoc = async (file) => {
  const formData = new FormData();
  formData.append('file', file);
  formData.append('project_id', '1');
  formData.append('document_type', 'project_document');
  
  const response = await fetch('http://localhost:8001/documents/upload', {
    method: 'POST',
    body: formData
  });
  
  const data = await response.json();
  console.log('Documento subido:', data);
};
```

---

## 📊 Casos de Uso Completos

### Flujo completo: Proyecto nuevo a presupuesto

```bash
# 1. Subir documento técnico del proyecto
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@documento_tecnico.pdf" \
  -F "project_id=5"

# 2. Subir presupuesto preliminar (si existe)
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -F "file=@presupuesto_preliminar.xlsx" \
  -F "project_id=5"

# 3. Generar resumen ejecutivo
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "Resume este proyecto incluyendo objetivos, metodología y resultados esperados",
    "project_id": 5
  }'

# 4. Generar presupuesto completo con IA
curl -X POST "http://localhost:8001/budget/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "project_id": 5,
    "project_description": "Proyecto de telemedicina...",
    "duration_years": 2
  }'
```

---

## 🧪 Testing con diferentes archivos

### Probar con PDF
```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@./test_files/documento.pdf" \
  -F "project_id=1"
```

### Probar con Word
```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@./test_files/informe.docx" \
  -F "project_id=1"
```

### Probar con Excel
```bash
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -F "file=@./test_files/presupuesto.xlsx" \
  -F "project_id=1"
```

---

## 🔍 Debugging

### Ver logs en tiempo real
```bash
docker-compose logs -f rag-service
```

### Verificar que el LLM responde
```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "Di hola",
    "project_id": 1
  }'
```

---

## 📚 Recursos Adicionales

- **Swagger UI (interactivo)**: http://localhost:8001/docs
- **ReDoc**: http://localhost:8001/redoc
- **Código fuente**: Ver [main.py](main.py)
- **Guía completa**: Ver [DOCKER_README.md](DOCKER_README.md)

---

## 💡 Tips

1. **Usar Postman/Thunder Client**: Importa las colecciones desde `/docs`
2. **Variables de entorno**: Cambia el `BASE_URL` si tu servicio está en otro puerto
3. **Archivos grandes**: Ajusta `MAX_FILE_SIZE_MB` en `.env`
4. **Timeout**: Para documentos grandes, aumenta el timeout del cliente HTTP

---

**¿Necesitas ayuda?** Revisa la documentación completa o abre un issue en el repositorio.
