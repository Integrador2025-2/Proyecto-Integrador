# Configuración de Email SMTP (Gmail) para 2FA

Este proyecto usa SMTP con Gmail para enviar códigos de verificación 2FA a cualquier email sin restricciones.

## Configuración Rápida

### 1. Crear una Contraseña de Aplicación en Gmail

1. Ve a tu cuenta de Google: https://myaccount.google.com/
2. Activa la **Verificación en 2 pasos** si no la tienes activada
3. Ve a **Seguridad** → **Contraseñas de aplicaciones**
4. Selecciona "Correo" y "Otro (nombre personalizado)"
5. Escribe un nombre como "Proyecto Integrador 2FA"
6. Google generará una contraseña de 16 caracteres (ej: `abcd efgh ijkl mnop`)
7. **Copia esta contraseña** (sin espacios)

### 2. Configurar en appsettings.Development.json

Actualiza la sección `Email:Smtp` con tus credenciales:

```json
{
  "Email": {
    "From": "tu-email@gmail.com",
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": "587",
      "Username": "tu-email@gmail.com",
      "Password": "abcd efgh ijkl mnop"
    }
  }
}
```

**Importante:** 
- Usa la contraseña de aplicación de 16 caracteres, NO tu contraseña normal de Gmail
- Puedes dejar los espacios en la contraseña, el sistema los eliminará automáticamente
- Para emails institucionales (@ucaldas.edu.co u otros), asegúrate de que tu organización permita acceso SMTP

### Para Emails Institucionales (Google Workspace)

Si usas un email institucional como `@ucaldas.edu.co`:

1. **Verifica que tu organización permita acceso SMTP**
   - Algunas organizaciones deshabilitan el acceso SMTP por seguridad
   - Contacta a tu administrador IT si no puedes crear contraseñas de aplicación

2. **Si tu organización bloquea contraseñas de aplicación**
   - Puede que necesites usar OAuth2 en lugar de contraseña de aplicación
   - O usar un email personal para pruebas

3. **Verifica la configuración SMTP**
   - Para Google Workspace, el host sigue siendo `smtp.gmail.com`
   - El puerto es `587` con TLS

### 3. Verificar Funcionamiento

1. Reinicia el backend
2. Prueba el login con cualquier email
3. El código 2FA se enviará al email del usuario

## Alternativas SMTP

Si no quieres usar Gmail, puedes usar otros proveedores:

### Outlook/Hotmail
```json
{
  "Email": {
    "Smtp": {
      "Host": "smtp-mail.outlook.com",
      "Port": "587",
      "Username": "tu-email@outlook.com",
      "Password": "tu-contraseña"
    }
  }
}
```

### Yahoo
```json
{
  "Email": {
    "Smtp": {
      "Host": "smtp.mail.yahoo.com",
      "Port": "587",
      "Username": "tu-email@yahoo.com",
      "Password": "tu-contraseña-de-aplicacion"
    }
  }
}
```

## Notas de Seguridad

- ⚠️ **NUNCA** commitees las credenciales reales al repositorio
- Usa variables de entorno o secretos en producción
- La contraseña de aplicación es diferente a tu contraseña de Gmail
- Si cambias tu contraseña de Gmail, necesitarás generar una nueva contraseña de aplicación

## Troubleshooting

**Error: "Invalid credentials"**
- Verifica que uses la contraseña de aplicación, no la contraseña normal
- Asegúrate de que la verificación en 2 pasos esté activada

**Error: "Connection timeout"**
- Verifica que el puerto 587 no esté bloqueado por firewall
- Algunos proveedores requieren permitir "aplicaciones menos seguras" (obsoleto, mejor usar contraseña de aplicación)

**Error: "Authentication failed"**
- Regenera la contraseña de aplicación
- Verifica que el username sea el email completo

