#!/bin/bash

# Script para generar un token de prueba para la API
# Este script crea un usuario de prueba y obtiene un token JWT

API_URL="http://localhost:5043/api"

echo "=========================================="
echo "  Generador de Token de Prueba"
echo "=========================================="
echo ""

# Colores para mejor visualización
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# 1. Intentar registrar un usuario de prueba
echo -e "${YELLOW}1. Registrando usuario de prueba...${NC}"
REGISTER_RESPONSE=$(curl -s -X POST "${API_URL}/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Usuario Test",
    "email": "test@example.com",
    "password": "Test123!",
    "confirmPassword": "Test123!",
    "telefono": "1234567890"
  }')

echo "$REGISTER_RESPONSE" | grep -q '"token"'
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Usuario registrado exitosamente${NC}"
    TOKEN=$(echo "$REGISTER_RESPONSE" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
    echo ""
    echo -e "${GREEN}=========================================="
    echo "  TOKEN JWT GENERADO"
    echo "==========================================${NC}"
    echo ""
    echo "$TOKEN"
    echo ""
    echo -e "${YELLOW}Copia este token y úsalo en tus peticiones con el header:${NC}"
    echo -e "Authorization: Bearer ${TOKEN}"
    echo ""
    echo -e "${YELLOW}Ejemplo con curl:${NC}"
    echo "curl -H \"Authorization: Bearer ${TOKEN}\" ${API_URL}/talentohumano"
    echo ""
    exit 0
fi

# 2. Si el registro falla, intentar login con 2FA
echo -e "${YELLOW}Usuario ya existe. Iniciando login con 2FA...${NC}"
INIT_RESPONSE=$(curl -s -X POST "${API_URL}/auth/login/init" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }')

echo "$INIT_RESPONSE" | grep -q '"tempToken"'
if [ $? -ne 0 ]; then
    echo -e "${RED}✗ Error en login. Verifica las credenciales${NC}"
    echo "$INIT_RESPONSE"
    exit 1
fi

TEMP_TOKEN=$(echo "$INIT_RESPONSE" | grep -o '"tempToken":"[^"]*' | cut -d'"' -f4)
echo -e "${GREEN}✓ Código 2FA enviado${NC}"
echo ""
echo -e "${YELLOW}Ingresa el código de 6 dígitos recibido en el email:${NC}"
read -r CODE

# 3. Verificar el código 2FA
echo -e "${YELLOW}3. Verificando código 2FA...${NC}"
VERIFY_RESPONSE=$(curl -s -X POST "${API_URL}/auth/2fa/verify" \
  -H "Content-Type: application/json" \
  -d "{
    \"tempToken\": \"${TEMP_TOKEN}\",
    \"code\": \"${CODE}\"
  }")

echo "$VERIFY_RESPONSE" | grep -q '"token"'
if [ $? -ne 0 ]; then
    echo -e "${RED}✗ Código 2FA inválido${NC}"
    echo "$VERIFY_RESPONSE"
    exit 1
fi

TOKEN=$(echo "$VERIFY_RESPONSE" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
echo ""
echo -e "${GREEN}=========================================="
echo "  TOKEN JWT GENERADO"
echo "==========================================${NC}"
echo ""
echo "$TOKEN"
echo ""
echo -e "${YELLOW}Copia este token y úsalo en tus peticiones con el header:${NC}"
echo -e "Authorization: Bearer ${TOKEN}"
echo ""
echo -e "${YELLOW}Ejemplo con curl:${NC}"
echo "curl -H \"Authorization: Bearer ${TOKEN}\" ${API_URL}/talentohumano"
echo ""
