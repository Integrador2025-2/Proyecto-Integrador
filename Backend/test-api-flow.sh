#!/bin/bash

# Script de prueba del flujo completo de la API
# Colores para output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
RED='\033[0;31m'
NC='\033[0m' # No Color

BASE_URL="http://localhost:5043/api"

echo -e "${CYAN}========================================"
echo "PRUEBA DE FLUJO COMPLETO - BACKEND API"
echo -e "========================================${NC}\n"

# 1. REGISTRO
echo -e "${YELLOW}1️⃣  Registrando nuevo usuario...${NC}"
REGISTER_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@minciencias.com","password":"Password123!","firstName":"Juan","lastName":"Pérez"}')

if echo "$REGISTER_RESPONSE" | grep -q "email"; then
  echo -e "${GREEN}   ✅ Usuario registrado exitosamente${NC}"
else
  echo -e "${YELLOW}   ⚠️  Usuario ya existe, continuando...${NC}"
fi

sleep 1

# 2. LOGIN
echo -e "\n${YELLOW}2️⃣  Iniciando sesión...${NC}"
LOGIN_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@minciencias.com","password":"Password123!"}')

TOKEN=$(echo $LOGIN_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)
USER_ID=$(echo $LOGIN_RESPONSE | grep -o '"userId":[0-9]*' | cut -d':' -f2)

if [ -z "$TOKEN" ]; then
  echo -e "${RED}   ❌ Error en login${NC}"
  echo "$LOGIN_RESPONSE"
  exit 1
fi

echo -e "${GREEN}   ✅ Login exitoso${NC}"
echo -e "   🔑 Token: ${TOKEN:0:50}..."
echo -e "   👤 Usuario ID: $USER_ID"

sleep 1

# 3. CREAR PROYECTO
echo -e "\n${YELLOW}3️⃣  Creando proyecto...${NC}"
PROYECTO_RESPONSE=$(curl -s -X POST "$BASE_URL/proyectos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{\"usuarioId\":$USER_ID,\"nombre\":\"Proyecto IA 2025\",\"descripcion\":\"Investigación en IA\",\"estado\":\"En Progreso\",\"fechaInicio\":\"2025-01-01T00:00:00\",\"fechaFin\":\"2025-12-31T23:59:59\"}")

PROYECTO_ID=$(echo $PROYECTO_RESPONSE | grep -o '"proyectoId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Proyecto creado - ID: $PROYECTO_ID${NC}"

sleep 1

# 4. CREAR OBJETIVO
echo -e "\n${YELLOW}4️⃣  Creando objetivo...${NC}"
OBJETIVO_RESPONSE=$(curl -s -X POST "$BASE_URL/objetivos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{\"proyectoId\":$PROYECTO_ID,\"nombre\":\"Objetivo General\",\"descripcion\":\"Desarrollar sistema IA\",\"resultadoEsperado\":\"Sistema con 95% precisión\"}")

OBJETIVO_ID=$(echo $OBJETIVO_RESPONSE | grep -o '"objetivoId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Objetivo creado - ID: $OBJETIVO_ID${NC}"

sleep 1

# 5. CREAR CADENA DE VALOR
echo -e "\n${YELLOW}5️⃣  Creando cadena de valor...${NC}"
CADENA_RESPONSE=$(curl -s -X POST "$BASE_URL/cadenasdevalor" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{\"objetivoId\":$OBJETIVO_ID,\"nombre\":\"Cadena I+D\",\"objetivoEspecifico\":\"Implementar ML con TensorFlow\"}")

CADENA_ID=$(echo $CADENA_RESPONSE | grep -o '"cadenaDeValorId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Cadena creada - ID: $CADENA_ID${NC}"

sleep 1

# 6. CREAR ACTIVIDAD
echo -e "\n${YELLOW}6️⃣  Creando actividad...${NC}"
ACTIVIDAD_RESPONSE=$(curl -s -X POST "$BASE_URL/actividades" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{\"cadenaDeValorId\":$CADENA_ID,\"nombre\":\"Recolección de Datos\",\"descripcion\":\"Recopilar datasets\",\"duracionAnios\":1,\"valorUnitario\":15000000}")

ACTIVIDAD_ID=$(echo $ACTIVIDAD_RESPONSE | grep -o '"actividadId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Actividad creada - ID: $ACTIVIDAD_ID${NC}"

sleep 1

# 7. CREAR TAREA
echo -e "\n${YELLOW}7️⃣  Creando tarea...${NC}"
TAREA_RESPONSE=$(curl -s -X POST "$BASE_URL/tareas" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{\"actividadId\":$ACTIVIDAD_ID,\"nombre\":\"Análisis de Datos\",\"descripcion\":\"Análisis estadístico\",\"periodo\":\"2025-Q1\",\"monto\":5000000}")

TAREA_ID=$(echo $TAREA_RESPONSE | grep -o '"tareaId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Tarea creada - ID: $TAREA_ID${NC}"

sleep 1

# 8. CREAR ENTIDAD
echo -e "\n${YELLOW}8️⃣  Creando entidad...${NC}"
ENTIDAD_RESPONSE=$(curl -s -X POST "$BASE_URL/entidades" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"nombre":"Universidad Nacional"}')

ENTIDAD_ID=$(echo $ENTIDAD_RESPONSE | grep -o '"entidadId":[0-9]*' | cut -d':' -f2)
echo -e "${GREEN}   ✅ Entidad creada - ID: $ENTIDAD_ID${NC}"

sleep 1

# 9. VERIFICAR
echo -e "\n${YELLOW}9️⃣  Verificando datos...${NC}"
curl -s -X GET "$BASE_URL/proyectos/$PROYECTO_ID" -H "Authorization: Bearer $TOKEN" > /dev/null && echo -e "${GREEN}   ✅ Proyecto verificado${NC}"
curl -s -X GET "$BASE_URL/objetivos/proyecto/$PROYECTO_ID" -H "Authorization: Bearer $TOKEN" > /dev/null && echo -e "${GREEN}   ✅ Objetivos verificados${NC}"
curl -s -X GET "$BASE_URL/cadenasdevalor/objetivo/$OBJETIVO_ID" -H "Authorization: Bearer $TOKEN" > /dev/null && echo -e "${GREEN}   ✅ Cadenas verificadas${NC}"
curl -s -X GET "$BASE_URL/actividades/cadena/$CADENA_ID" -H "Authorization: Bearer $TOKEN" > /dev/null && echo -e "${GREEN}   ✅ Actividades verificadas${NC}"
curl -s -X GET "$BASE_URL/tareas/actividad/$ACTIVIDAD_ID" -H "Authorization: Bearer $TOKEN" > /dev/null && echo -e "${GREEN}   ✅ Tareas verificadas${NC}"

# RESUMEN
echo -e "\n${CYAN}========================================"
echo "RESUMEN"
echo "========================================${NC}"
echo -e "👤 Usuario ID: $USER_ID"
echo -e "📁 Proyecto ID: $PROYECTO_ID"
echo -e "🎯 Objetivo ID: $OBJETIVO_ID"
echo -e "🔗 Cadena ID: $CADENA_ID"
echo -e "⚡ Actividad ID: $ACTIVIDAD_ID"
echo -e "✅ Tarea ID: $TAREA_ID"
echo -e "🏢 Entidad ID: $ENTIDAD_ID"
echo -e "\n${GREEN}✅ FLUJO COMPLETADO EXITOSAMENTE! 🎉${NC}"
echo -e "${CYAN}========================================${NC}\n"
