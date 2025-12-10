#!/usr/bin/env bash
# Script para configurar el entorno de desarrollo

echo "🔧 Configuración del Entorno de Desarrollo"
echo "=========================================="
echo ""

# Verificar si existe .env
if [ ! -f .env ]; then
    echo "⚠️  No se encontró el archivo .env"
    echo "📋 Copiando .env.example a .env..."
    cp .env.example .env
    echo "✅ Archivo .env creado"
    echo ""
    echo "⚡ IMPORTANTE: Edita el archivo .env con tus credenciales reales"
    echo "   - Contraseña de base de datos"
    echo "   - JWT Secret Key (debe tener al menos 32 caracteres)"
    echo "   - Credenciales de email"
    echo ""
else
    echo "✅ Archivo .env encontrado"
fi

# Verificar Docker
echo "🐳 Verificando Docker..."
if ! command -v docker &> /dev/null; then
    echo "❌ Docker no está instalado"
    echo "   Instala Docker Desktop desde https://www.docker.com/products/docker-desktop"
    exit 1
fi
echo "✅ Docker está instalado"

# Levantar contenedores
echo ""
echo "📦 Levantando contenedores Docker..."
docker-compose up -d

# Esperar a que SQL Server esté listo
echo ""
echo "⏳ Esperando a que SQL Server esté listo..."
sleep 10

# Aplicar migraciones
echo ""
echo "🗄️  Aplicando migraciones..."
dotnet ef database update

echo ""
echo "✅ Configuración completada!"
echo ""
echo "🚀 Para ejecutar la aplicación:"
echo "   dotnet run"
echo ""
echo "📚 Para ver la documentación de la API:"
echo "   https://localhost:5001"
