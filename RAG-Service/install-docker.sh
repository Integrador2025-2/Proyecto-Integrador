#!/bin/bash

# Script de instalación automática de RAG-Service con Docker
# Este script configura todo lo necesario para ejecutar el servicio

set -e

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}"
echo "=================================================="
echo "   RAG-Service - Instalación Automática"
echo "=================================================="
echo -e "${NC}"
echo ""

# Función para imprimir con color
print_step() {
    echo -e "${YELLOW}▶ $1${NC}"
}

print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

# 1. Verificar Docker
print_step "Paso 1/6: Verificando Docker..."
if command -v docker &> /dev/null && command -v docker-compose &> /dev/null; then
    print_success "Docker instalado"
    docker --version
    docker-compose --version
else
    print_error "Docker no está instalado"
    echo ""
    echo "Por favor, instala Docker Desktop desde:"
    echo "  https://www.docker.com/products/docker-desktop"
    exit 1
fi
echo ""

# 2. Verificar archivo .env
print_step "Paso 2/6: Configurando variables de entorno..."
if [ ! -f .env ]; then
    cp .env.docker .env
    print_success "Archivo .env creado desde .env.docker"
    
    echo ""
    echo -e "${YELLOW}IMPORTANTE: Necesitas configurar tu API Key${NC}"
    echo ""
    echo "1. Obtén tu API Key de Google Gemini (gratis):"
    echo "   https://aistudio.google.com/app/apikey"
    echo ""
    echo "2. Edita el archivo .env y reemplaza:"
    echo "   GEMINI_API_KEY=tu_api_key_de_gemini_aqui"
    echo ""
    read -p "¿Ya tienes tu API Key? (s/n): " has_key
    
    if [[ $has_key =~ ^[Ss]$ ]]; then
        read -p "Ingresa tu GEMINI_API_KEY: " api_key
        if [ ! -z "$api_key" ]; then
            # Usar sed para reemplazar la línea
            if [[ "$OSTYPE" == "darwin"* ]]; then
                # macOS
                sed -i '' "s/GEMINI_API_KEY=.*/GEMINI_API_KEY=$api_key/" .env
            else
                # Linux
                sed -i "s/GEMINI_API_KEY=.*/GEMINI_API_KEY=$api_key/" .env
            fi
            print_success "API Key configurada"
        fi
    else
        print_error "Necesitas configurar la API Key en .env antes de continuar"
        echo "Ejecuta: nano .env"
        echo "Luego ejecuta este script nuevamente"
        exit 1
    fi
else
    print_success "Archivo .env ya existe"
fi
echo ""

# 3. Crear directorios necesarios
print_step "Paso 3/6: Creando directorios..."
mkdir -p chroma_db uploads generated_budgets
print_success "Directorios creados"
echo ""

# 4. Dar permisos a scripts
print_step "Paso 4/6: Configurando permisos..."
chmod +x docker-manage.sh test-docker.sh 2>/dev/null || true
print_success "Permisos configurados"
echo ""

# 5. Construir imágenes Docker
print_step "Paso 5/6: Construyendo imágenes Docker..."
echo "Esto puede tardar varios minutos la primera vez..."
echo ""

if docker-compose build; then
    print_success "Imágenes construidas exitosamente"
else
    print_error "Error al construir imágenes"
    exit 1
fi
echo ""

# 6. Iniciar servicios
print_step "Paso 6/6: Iniciando servicios..."
if docker-compose up -d; then
    print_success "Servicios iniciados"
else
    print_error "Error al iniciar servicios"
    exit 1
fi
echo ""

# Esperar a que el servicio esté listo
print_step "Esperando a que el servicio esté listo..."
sleep 10

# Verificar health check
max_attempts=12
attempt=0
while [ $attempt -lt $max_attempts ]; do
    if curl -s http://localhost:8001/health > /dev/null 2>&1; then
        print_success "Servicio está listo"
        break
    fi
    attempt=$((attempt + 1))
    echo "Intento $attempt/$max_attempts..."
    sleep 5
done

if [ $attempt -eq $max_attempts ]; then
    print_error "El servicio no respondió después de varios intentos"
    echo "Verifica los logs con: docker-compose logs rag-service"
    exit 1
fi
echo ""

# Mostrar estado
echo -e "${GREEN}"
echo "=================================================="
echo "   ¡Instalación Completada!"
echo "=================================================="
echo -e "${NC}"
echo ""
echo "Tu RAG-Service está funcionando correctamente 🎉"
echo ""
echo -e "${BLUE}URLs disponibles:${NC}"
echo "  📖 API Docs:    http://localhost:8001/docs"
echo "  🌐 API:         http://localhost:8001"
echo "  💻 Streamlit:   http://localhost:8501"
echo ""
echo -e "${BLUE}Comandos útiles:${NC}"
echo "  ./docker-manage.sh status   - Ver estado"
echo "  ./docker-manage.sh logs     - Ver logs"
echo "  ./docker-manage.sh stop     - Detener"
echo "  ./docker-manage.sh restart  - Reiniciar"
echo "  ./docker-manage.sh help     - Ver todos los comandos"
echo ""
echo -e "${BLUE}Próximos pasos:${NC}"
echo "  1. Abre http://localhost:8001/docs en tu navegador"
echo "  2. Prueba el endpoint /health"
echo "  3. Lee API_EXAMPLES.md para ejemplos de uso"
echo "  4. Lee DOCKER_README.md para documentación completa"
echo ""
echo -e "${YELLOW}Tip:${NC} Ejecuta './test-docker.sh' para hacer pruebas automáticas"
echo ""

# Ejecutar test automático si el usuario quiere
read -p "¿Quieres ejecutar las pruebas automáticas ahora? (s/n): " run_test
if [[ $run_test =~ ^[Ss]$ ]]; then
    echo ""
    ./test-docker.sh
fi

echo ""
echo -e "${GREEN}¡Disfruta usando RAG-Service! 🚀${NC}"
echo ""
