#!/bin/bash

# Script simplificado para obtener token de prueba
# Usa el endpoint dev-login que no requiere 2FA

API_URL="http://localhost:5043/api"

echo "=========================================="
echo "  Generador de Token de Prueba"
echo "  (Modo Desarrollo - Sin 2FA)"
echo "=========================================="
echo ""

# Intentar login directo con el endpoint de desarrollo
echo "Obteniendo token para usuario de prueba..."
echo ""

RESPONSE=$(curl -s -X POST "${API_URL}/auth/dev-login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }')

# Verificar si la respuesta contiene un token
echo "$RESPONSE" | grep -q '"token"'
if [ $? -eq 0 ]; then
    TOKEN=$(echo "$RESPONSE" | grep -o '"token":"[^"]*' | cut -d'"' -f4)
    USER=$(echo "$RESPONSE" | grep -o '"email":"[^"]*' | cut -d'"' -f4)
    ROLE=$(echo "$RESPONSE" | grep -o '"roleName":"[^"]*' | cut -d'"' -f4)
    
    echo "✓ Login exitoso"
    echo ""
    echo "=========================================="
    echo "  INFORMACIÓN DEL USUARIO"
    echo "=========================================="
    echo "Email: $USER"
    echo "Rol: $ROLE"
    echo ""
    echo "=========================================="
    echo "  TOKEN JWT GENERADO"
    echo "=========================================="
    echo ""
    echo "$TOKEN"
    echo ""
    echo "=========================================="
    echo "  CÓMO USAR EL TOKEN"
    echo "=========================================="
    echo ""
    echo "1. En curl:"
    echo "   curl -H \"Authorization: Bearer ${TOKEN}\" \\"
    echo "        ${API_URL}/talentohumano"
    echo ""
    echo "2. En Postman/Thunder Client:"
    echo "   - Ve a Headers"
    echo "   - Agrega: Authorization"
    echo "   - Valor: Bearer ${TOKEN}"
    echo ""
    echo "3. Guardar en variable (bash):"
    echo "   export TOKEN=\"${TOKEN}\""
    echo "   curl -H \"Authorization: Bearer \$TOKEN\" ${API_URL}/talentohumano"
    echo ""
else
    echo "✗ Error al obtener token"
    echo ""
    echo "Respuesta del servidor:"
    echo "$RESPONSE"
    echo ""
    echo "Posibles soluciones:"
    echo "1. Verifica que el backend esté corriendo en ${API_URL}"
    echo "2. Si el usuario no existe, créalo con:"
    echo ""
    echo "   curl -X POST ${API_URL}/auth/register \\"
    echo "        -H \"Content-Type: application/json\" \\"
    echo "        -d '{"
    echo "          \"nombre\": \"Usuario Test\","
    echo "          \"email\": \"test@example.com\","
    echo "          \"password\": \"Test123!\","
    echo "          \"confirmPassword\": \"Test123!\","
    echo "          \"telefono\": \"1234567890\""
    echo "        }'"
    echo ""
fi
