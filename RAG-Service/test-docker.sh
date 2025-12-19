#!/bin/bash

# Script de prueba rápida para RAG-Service
# Este script verifica que el servicio está funcionando correctamente

# Colores
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

BASE_URL="http://localhost:8001"

echo "=================================================="
echo "   RAG-Service - Pruebas Rápidas"
echo "=================================================="
echo ""

# Test 1: Health Check
echo -e "${YELLOW}Test 1: Health Check${NC}"
response=$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/health")
if [ "$response" = "200" ]; then
    echo -e "${GREEN}✓ Servicio respondiendo correctamente${NC}"
    curl -s "$BASE_URL/health" | python -m json.tool
else
    echo -e "${RED}✗ Servicio no responde (HTTP $response)${NC}"
    exit 1
fi
echo ""

# Test 2: API Docs disponible
echo -e "${YELLOW}Test 2: Documentación de la API${NC}"
response=$(curl -s -o /dev/null -w "%{http_code}" "$BASE_URL/docs")
if [ "$response" = "200" ]; then
    echo -e "${GREEN}✓ Documentación disponible en $BASE_URL/docs${NC}"
else
    echo -e "${RED}✗ Documentación no disponible${NC}"
fi
echo ""

# Test 3: Verificar que LLM está configurado
echo -e "${YELLOW}Test 3: Configuración de LLM${NC}"
if docker exec rag-service env | grep -q "GEMINI_API_KEY"; then
    echo -e "${GREEN}✓ GEMINI_API_KEY configurada${NC}"
else
    echo -e "${RED}✗ GEMINI_API_KEY no configurada${NC}"
fi
echo ""

# Test 4: Verificar volúmenes
echo -e "${YELLOW}Test 4: Volúmenes de datos${NC}"
if [ -d "./chroma_db" ]; then
    echo -e "${GREEN}✓ Directorio chroma_db existe${NC}"
else
    echo -e "${RED}✗ Directorio chroma_db no existe${NC}"
fi

if [ -d "./uploads" ]; then
    echo -e "${GREEN}✓ Directorio uploads existe${NC}"
else
    echo -e "${RED}✗ Directorio uploads no existe${NC}"
fi

if [ -d "./generated_budgets" ]; then
    echo -e "${GREEN}✓ Directorio generated_budgets existe${NC}"
else
    echo -e "${RED}✗ Directorio generated_budgets no existe${NC}"
fi
echo ""

# Test 5: Logs recientes
echo -e "${YELLOW}Test 5: Últimas 5 líneas de logs${NC}"
docker-compose logs --tail=5 rag-service
echo ""

echo "=================================================="
echo -e "${GREEN}Pruebas completadas${NC}"
echo "=================================================="
echo ""
echo "URLs útiles:"
echo "  - API: $BASE_URL"
echo "  - Swagger Docs: $BASE_URL/docs"
echo "  - ReDoc: $BASE_URL/redoc"
echo ""
