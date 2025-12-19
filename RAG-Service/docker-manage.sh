#!/bin/bash

# Script de gestión de Docker para RAG-Service
# Uso: ./docker-manage.sh [comando]

set -e

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Funciones de ayuda
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ $1${NC}"
}

# Verificar que Docker está instalado
check_docker() {
    if ! command -v docker &> /dev/null; then
        print_error "Docker no está instalado. Por favor, instala Docker primero."
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null; then
        print_error "Docker Compose no está instalado. Por favor, instala Docker Compose primero."
        exit 1
    fi
    
    print_success "Docker y Docker Compose están instalados"
}

# Verificar archivo .env
check_env() {
    if [ ! -f .env ]; then
        print_info "Archivo .env no encontrado. Creando desde .env.docker..."
        cp .env.docker .env
        print_error "Por favor, edita el archivo .env con tus API keys antes de continuar."
        exit 1
    fi
    print_success "Archivo .env encontrado"
}

# Construir imágenes
build() {
    print_info "Construyendo imágenes Docker..."
    docker-compose build --no-cache
    print_success "Imágenes construidas exitosamente"
}

# Iniciar servicios
start() {
    print_info "Iniciando servicios..."
    docker-compose up -d
    print_success "Servicios iniciados"
    
    print_info "Esperando a que los servicios estén listos..."
    sleep 5
    
    docker-compose ps
    
    print_success "Servicios disponibles en:"
    echo "  - API: http://localhost:8001"
    echo "  - Docs: http://localhost:8001/docs"
    echo "  - Streamlit: http://localhost:8501"
}

# Detener servicios
stop() {
    print_info "Deteniendo servicios..."
    docker-compose stop
    print_success "Servicios detenidos"
}

# Reiniciar servicios
restart() {
    print_info "Reiniciando servicios..."
    docker-compose restart
    print_success "Servicios reiniciados"
}

# Ver logs
logs() {
    if [ -z "$2" ]; then
        docker-compose logs -f
    else
        docker-compose logs -f "$2"
    fi
}

# Ver estado
status() {
    print_info "Estado de los servicios:"
    docker-compose ps
    
    echo ""
    print_info "Health check del servicio RAG:"
    curl -s http://localhost:8001/health | python -m json.tool || print_error "Servicio no disponible"
}

# Limpiar todo
clean() {
    print_info "¿Estás seguro de que quieres eliminar todos los contenedores y volúmenes? (y/n)"
    read -r response
    if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
        print_info "Limpiando contenedores y volúmenes..."
        docker-compose down -v
        print_success "Limpieza completada"
    else
        print_info "Operación cancelada"
    fi
}

# Entrar al contenedor
shell() {
    SERVICE=${2:-rag-service}
    print_info "Accediendo al shell del contenedor $SERVICE..."
    docker exec -it $SERVICE bash
}

# Ver uso de recursos
stats() {
    docker stats
}

# Backup de datos
backup() {
    BACKUP_DIR="./backups/$(date +%Y%m%d_%H%M%S)"
    print_info "Creando backup en $BACKUP_DIR..."
    mkdir -p "$BACKUP_DIR"
    
    cp -r chroma_db "$BACKUP_DIR/"
    cp -r uploads "$BACKUP_DIR/"
    cp -r generated_budgets "$BACKUP_DIR/"
    
    print_success "Backup completado en $BACKUP_DIR"
}

# Restaurar desde backup
restore() {
    if [ -z "$2" ]; then
        print_error "Uso: $0 restore <directorio_backup>"
        exit 1
    fi
    
    BACKUP_DIR=$2
    if [ ! -d "$BACKUP_DIR" ]; then
        print_error "Directorio de backup no encontrado: $BACKUP_DIR"
        exit 1
    fi
    
    print_info "Restaurando desde $BACKUP_DIR..."
    stop
    
    rm -rf chroma_db uploads generated_budgets
    cp -r "$BACKUP_DIR/chroma_db" .
    cp -r "$BACKUP_DIR/uploads" .
    cp -r "$BACKUP_DIR/generated_budgets" .
    
    start
    print_success "Restauración completada"
}

# Actualizar
update() {
    print_info "Actualizando servicio..."
    stop
    build
    start
    print_success "Actualización completada"
}

# Menú de ayuda
help() {
    echo "RAG-Service Docker Management Script"
    echo ""
    echo "Uso: $0 [comando]"
    echo ""
    echo "Comandos disponibles:"
    echo "  check       - Verificar requisitos de Docker"
    echo "  build       - Construir imágenes Docker"
    echo "  start       - Iniciar servicios"
    echo "  stop        - Detener servicios"
    echo "  restart     - Reiniciar servicios"
    echo "  logs [svc]  - Ver logs (opcionalmente de un servicio específico)"
    echo "  status      - Ver estado de los servicios"
    echo "  clean       - Eliminar contenedores y volúmenes"
    echo "  shell [svc] - Acceder al shell del contenedor"
    echo "  stats       - Ver uso de recursos"
    echo "  backup      - Hacer backup de los datos"
    echo "  restore     - Restaurar desde backup"
    echo "  update      - Actualizar servicio (stop, build, start)"
    echo "  help        - Mostrar esta ayuda"
    echo ""
    echo "Ejemplos:"
    echo "  $0 start                # Iniciar todos los servicios"
    echo "  $0 logs rag-service     # Ver logs del servicio RAG"
    echo "  $0 shell rag-service    # Acceder al shell del contenedor"
    echo "  $0 restore ./backups/20231218_120000  # Restaurar backup"
}

# Comando principal
COMMAND=${1:-help}

case $COMMAND in
    check)
        check_docker
        check_env
        ;;
    build)
        check_docker
        check_env
        build
        ;;
    start)
        check_docker
        check_env
        start
        ;;
    stop)
        stop
        ;;
    restart)
        restart
        ;;
    logs)
        logs "$@"
        ;;
    status)
        status
        ;;
    clean)
        clean
        ;;
    shell)
        shell "$@"
        ;;
    stats)
        stats
        ;;
    backup)
        backup
        ;;
    restore)
        restore "$@"
        ;;
    update)
        update
        ;;
    help)
        help
        ;;
    *)
        print_error "Comando desconocido: $COMMAND"
        echo ""
        help
        exit 1
        ;;
esac
