from fastapi import FastAPI, HTTPException, UploadFile, File, Depends
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from typing import List, Optional, Dict, Any
import os
from dotenv import load_dotenv
import uvicorn

from services.document_processor import DocumentProcessor
from services.rag_service import RAGService
from services.budget_automation import BudgetAutomationService
from models.schemas import (
    DocumentUpload, 
    QueryRequest, 
    QueryResponse, 
    BudgetGenerationRequest,
    BudgetGenerationResponse,
    ProjectDocument
)

# Cargar variables de entorno
load_dotenv()

app = FastAPI(
    title="RAG Budget Automation Service",
    description="Servicio RAG para automatización de presupuestos basado en documentos de proyectos",
    version="1.0.0"
)

# Configurar CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000", "http://localhost:5000"],  # Frontend y Backend
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Inicializar servicios
document_processor = DocumentProcessor()
rag_service = RAGService()
budget_automation = BudgetAutomationService()

# Modelos de datos
class HealthResponse(BaseModel):
    status: str
    message: str

@app.get("/health", response_model=HealthResponse)
async def health_check():
    """Endpoint de salud del servicio"""
    return HealthResponse(
        status="healthy",
        message="RAG Budget Automation Service is running"
    )

@app.post("/documents/upload")
async def upload_document(
    file: UploadFile = File(...),
    project_id: int = None,
    document_type: str = "project_document"
):
    """Subir y procesar un documento de proyecto"""
    try:
        # Validar tipo de archivo
        allowed_extensions = ['.pdf', '.docx', '.txt', '.xlsx']
        file_extension = os.path.splitext(file.filename)[1].lower()
        
        if file_extension not in allowed_extensions:
            raise HTTPException(
                status_code=400, 
                detail=f"Tipo de archivo no soportado. Permitidos: {allowed_extensions}"
            )
        
        # Procesar documento
        content = await document_processor.process_document(file)
        
        # Almacenar en base de datos vectorial
        document_id = await rag_service.add_document(
            content=content,
            metadata={
                "filename": file.filename,
                "project_id": project_id,
                "document_type": document_type,
                "file_extension": file_extension
            }
        )
        
        return {
            "message": "Documento procesado exitosamente",
            "document_id": document_id,
            "filename": file.filename,
            "project_id": project_id
        }
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error procesando documento: {str(e)}")

@app.post("/query", response_model=QueryResponse)
async def query_documents(request: QueryRequest):
    """Realizar consulta semántica sobre los documentos"""
    try:
        response = await rag_service.query(
            question=request.question,
            project_id=request.project_id,
            top_k=request.top_k or 5
        )
        
        return QueryResponse(
            answer=response["answer"],
            sources=response["sources"],
            confidence=response["confidence"]
        )
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error en consulta: {str(e)}")

@app.post("/budget/generate", response_model=BudgetGenerationResponse)
async def generate_budget(request: BudgetGenerationRequest):
    """Generar presupuesto automáticamente basado en documentos de proyecto"""
    try:
        budget_data = await budget_automation.generate_budget(
            project_id=request.project_id,
            project_description=request.project_description,
            budget_categories=request.budget_categories,
            duration_years=request.duration_years
        )
        
        return BudgetGenerationResponse(
            project_id=request.project_id,
            budget_data=budget_data,
            generated_at=budget_data.get("generated_at"),
            confidence_score=budget_data.get("confidence_score", 0.0)
        )
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error generando presupuesto: {str(e)}")

@app.get("/projects/{project_id}/documents")
async def get_project_documents(project_id: int):
    """Obtener documentos asociados a un proyecto"""
    try:
        documents = await rag_service.get_project_documents(project_id)
        return {"project_id": project_id, "documents": documents}
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error obteniendo documentos: {str(e)}")

@app.delete("/documents/{document_id}")
async def delete_document(document_id: str):
    """Eliminar un documento"""
    try:
        await rag_service.delete_document(document_id)
        return {"message": "Documento eliminado exitosamente", "document_id": document_id}
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error eliminando documento: {str(e)}")

@app.get("/projects/{project_id}/budget/suggestions")
async def get_budget_suggestions(project_id: int, category: str = None):
    """Obtener sugerencias de presupuesto para un proyecto específico"""
    try:
        suggestions = await budget_automation.get_budget_suggestions(
            project_id=project_id,
            category=category
        )
        return {"project_id": project_id, "suggestions": suggestions}
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error obteniendo sugerencias: {str(e)}")

if __name__ == "__main__":
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=8001,
        reload=True
    )
