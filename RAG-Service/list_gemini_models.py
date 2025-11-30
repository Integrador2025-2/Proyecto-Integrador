"""
Script para listar los modelos disponibles en tu cuenta de Google Gemini
"""
import os
import sys
from dotenv import load_dotenv
import google.generativeai as genai

# Configurar codificación UTF-8 para Windows
if sys.platform == 'win32':
    sys.stdout.reconfigure(encoding='utf-8')

load_dotenv()

api_key = os.getenv("GEMINI_API_KEY")

if not api_key:
    print("[ERROR] GEMINI_API_KEY no esta configurada en .env")
    exit(1)

print("Configurando Gemini...")
genai.configure(api_key=api_key)

print("\n=== Modelos disponibles en tu cuenta ===\n")
print("-" * 80)

try:
    for model in genai.list_models():
        if 'generateContent' in model.supported_generation_methods:
            print(f"[OK] {model.name}")
            print(f"     Nombre corto: {model.name.replace('models/', '')}")
            print(f"     Metodos: {', '.join(model.supported_generation_methods)}")
            print(f"     Descripcion: {model.display_name}")
            print("-" * 80)
    
    print("\n[INFO] Para usar un modelo, copia el 'Nombre corto' y configuralo en tu .env:")
    print("       GEMINI_MODEL=nombre_del_modelo\n")
    
    print("Recomendaciones:")
    print("   - gemini-1.5-flash-latest  -> Mas rapido (recomendado)")
    print("   - gemini-1.5-pro-latest    -> Mas potente")
    print("   - gemini-pro               -> Estable y compatible\n")
    
except Exception as e:
    print(f"[ERROR] {e}")
    print("\nSugerencias:")
    print("   1. Verifica que tu GEMINI_API_KEY sea correcta")
    print("   2. Verifica tu conexion a internet")
    print("   3. Prueba usando el modelo: gemini-pro")

