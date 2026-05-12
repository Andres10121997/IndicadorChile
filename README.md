# Indicadores Chile

Esta aplicación web API entrega datos sobre cuanto valen cierta(s) divisa(s) o unidad(es) de cuenta en pesos chilenos en días determinados.

## Comenzando

Estas instrucciones te permitirán obtener una copia del proyecto en funcionamiento en tu máquina local para propósitos de desarrollo y pruebas.

### Prerrequisitos

Qué cosas necesitas para instalar el software y cómo instalarlas:

#### Software

| Nombre | Descripción | URL de referencia |
| :--- | :--- | :--- |
| Docker | Ingrese al sitio web de Docker, descargue el instalador del software e instálelo. <br /> En caso de tener algún problema, ver el video | 1. [Sitio web](https://www.docker.com/) <br /> 2. [Video instalación (ejemplo)](https://www.youtube.com/watch?v=cWuirzMOwwg) |
| Visual Studio (IDE) | Ingrese al sitio web de Visual Studio, descargue el instalador del software e instálelo. | 1. [Sitio web](https://visualstudio.microsoft.com/es/) |

# Docker Compose
## `LANG`, `LANGUAGE` y `LC_ALL`

Para configurar `LANG`, `LANGUAGE` y `LC_ALL` en `docker-compose.yml`, debes definirlas como variables de entorno dentro de la sección `environment` de tu servicio. Esto asegura que el idioma y la configuración regional se establezcan correctamente dentro del contenedor de la aplicación, por ejemplo, `environment: - LANG=es_ES.UTF-8 - LANGUAGE=es_ES:es - LC_ALL=es_ES.UTF-8`.

### Cómo hacerlo
1. Abre tu archivo `docker-compose.yml`.
2. Busca la sección `services` y el servicio para el que quieres configurar el idioma.
3. Añade la sección `environment` si aún no existe.
4. Define las variables de entorno dentro de la sección `environment` con los valores en español.

### Ejemplo práctico
```YML
version: '3.8'

services:
  mi-aplicacion:
    image: mi-imagen-de-aplicacion
    environment:
      - LANG=es_ES.UTF-8
      - LANGUAGE=es_ES:es
      - LC_ALL=es_ES.UTF-8
    ports:
      - "8080:80"
```

### Explicación de las variables
* **<ins>`LANG`</ins>:** Establece la configuración regional predeterminada para la mayoría de los programas.
* **<ins>`LANGUAGE`</ins>:** Se utiliza principalmente para la traducción de mensajes de interfaz de usuario y puede tener múltiples valores.
* **<ins>`LC_ALL`</ins>:** Anula todas las otras variables `LC_*` y `LANG`. Al establecerla, se aplica a todas las categorías de configuración regional.

### ¿Por qué utilizarlo?
#### `DateTimeFormat.MonthNames`
La matriz `DateTimeFormat.MonthNames` de C# considera el idioma y la configuración regional del sistema porque el objeto `DateTimeFormatInfo` que la contiene está diseñado para proporcionar información específica de la referencia cultural para el formato de fecha y hora. Esto permite que la misma aplicación muestre nombres de meses en español, inglés, o cualquier otro idioma soportado, basándose en la configuración del sistema donde se ejecuta el código, lo que garantiza que los nombres de los meses y el formato de fecha sean apropiados para el usuario final.
* **<ins>Especificidad cultural</ins>:** `DateTimeFormatInfo` es una clase que contiene propiedades para el formato de fecha y hora, como los nombres de los meses, días de la semana, y formatos de números específicos de cada cultura (por ejemplo, `es-CL` para español de Chile o `en-US` para inglés de Estados Unidos).
* **<ins>Adaptación automática</ins>:** Cuando se utiliza sin especificar una cultura, C# utiliza la cultura actual del sistema operativo como valor predeterminado. Esto significa que `DateTimeFormat.MonthNames` contendrá automáticamente los nombres de los meses correspondientes a esa cultura.
* **<ins>Uso explícito</ins>:** También es posible sobrescribir este comportamiento y especificar manualmente una cultura diferente al formatear una fecha, por ejemplo, utilizando `ToString("MMMM", new CultureInfo("es-CL"))` para forzar el nombre del mes en inglés.

# Patron(es) de Diseño
## Result Pattern
El Patrón Result es un patrón de diseño que se utiliza para representar el resultado de una operación. Hace explícita la intención de gestionar el éxito o el fracaso, reemplazando mecanismos ambiguos de manejo de errores como el lanzamiento de excepciones.
