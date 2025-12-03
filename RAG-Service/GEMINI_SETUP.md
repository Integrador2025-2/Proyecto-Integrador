# Configuración de Google Gemini

## ¿Por qué Gemini?

Google Gemini ofrece:
- **Capa gratuita generosa**: 60 requests por minuto (RPM) gratis
- **Sin necesidad de tarjeta de crédito** para comenzar
- **Modelo potente**: Gemini 1.5 Flash es rápido y preciso
- **Soporte JSON nativo**: Ideal para generación estructurada de presupuestos

## Obtener tu API Key de Gemini (GRATIS)

### Paso 1: Acceder a Google AI Studio

1. Ve a: https://aistudio.google.com/apikey
2. Inicia sesión con tu cuenta de Google

### Paso 2: Crear API Key

1. Haz clic en **"Create API Key"** o **"Get API Key"**
2. Selecciona un proyecto de Google Cloud o crea uno nuevo
3. Tu API Key se generará automáticamente
4. **Copia tu API Key** (se ve algo así: `AIzaSy...`)

### Paso 3: Configurar en el proyecto

1. Abre tu archivo `.env` en la carpeta `RAG-Service`
2. Agrega o actualiza estas líneas:

```env
# Usar Gemini como proveedor
LLM_PROVIDER=gemini

# Tu API Key de Gemini
GEMINI_API_KEY=AIzaSy_tu_api_key_aqui

# Modelo a usar (gemini-1.5-flash-latest es el más rápido y gratuito)
GEMINI_MODEL=gemini-1.5-flash-latest
```

### Paso 4: Reiniciar el servicio

```bash
cd RAG-Service
python main.py
```

## Límites de la Capa Gratuita

| Característica | Límite Gratuito |
|----------------|-----------------|
| Requests por minuto (RPM) | 60 |
| Tokens por minuto (TPM) | 1,500,000 |
| Requests por día (RPD) | 1,500 |

**Nota**: Estos límites son más que suficientes para desarrollo y testing.

## Modelos Disponibles

### Gemini 1.5 Flash (Recomendado para este proyecto)
- **Velocidad**: Muy rápida
- **Costo**: Gratuito hasta los límites
- **Uso**: Ideal para generación de presupuestos y respuestas rápidas
- **Nombre**: `gemini-1.5-flash-latest` o `gemini-1.5-flash`

### Gemini 1.5 Pro
- **Velocidad**: Más lenta
- **Capacidad**: Mayor precisión y contexto más largo
- **Costo**: Gratuito hasta los límites (menor RPM)
- **Nombre**: `gemini-1.5-pro-latest` o `gemini-1.5-pro`

### Gemini Pro (Legacy - siempre disponible)
- **Velocidad**: Rápida
- **Capacidad**: Buena para la mayoría de casos
- **Costo**: Gratuito
- **Nombre**: `gemini-pro`

## Verificar que funciona

Después de configurar, deberías ver en los logs:

```
INFO:     Uvicorn running on http://0.0.0.0:8001 (Press CTRL+C to quit)
```

**Sin** mensajes de:
```
Advertencia: LLM no disponible...
```

## Cambiar entre OpenAI y Gemini

En tu archivo `.env`:

```env
# Para usar Gemini (GRATIS)
LLM_PROVIDER=gemini
GEMINI_API_KEY=tu_api_key_de_gemini

# Para usar OpenAI (REQUIERE PAGO)
LLM_PROVIDER=openai
OPENAI_API_KEY=tu_api_key_de_openai
```

## Solución de Problemas

### Error: "GEMINI_API_KEY no está configurada"
- Verifica que hayas copiado correctamente el API key en `.env`
- Asegúrate de que no haya espacios antes o después del valor
- Reinicia el servicio después de modificar `.env`

### Error: "Invalid API key"
- Verifica que el API key sea correcto
- Intenta generar una nueva API key en Google AI Studio

### Error de cuota (429)
- Espera unos minutos, podrías haber excedido el límite de RPM
- Los límites se resetean cada minuto

## Más Información

- Documentación oficial: https://ai.google.dev/docs
- Playground: https://aistudio.google.com/
- Límites y pricing: https://ai.google.dev/pricing

## Ventajas vs OpenAI

| Característica | Gemini (Gratuito) | OpenAI (Pago) |
|----------------|-------------------|---------------|
| Costo inicial | $0 | Requiere crédito |
| RPM gratuito | 60 | ~3 (muy limitado) |
| Setup | Sin tarjeta | Requiere tarjeta |
| Velocidad | Muy rápida | Rápida |
| Calidad | Excelente | Excelente |

## ¿Necesitas más capacidad?

Si necesitas más de 60 requests por minuto:
1. Configura billing en Google Cloud (solo pagas lo que usas)
2. Los precios de Gemini son muy competitivos:
   - Gemini 1.5 Flash: ~$0.075 por 1M tokens
   - Gemini 1.5 Pro: ~$1.25 por 1M tokens

