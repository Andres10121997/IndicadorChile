# Indicadores Chile

Este proyecto tiene 2 submódulos:
1. **<ins>Aplicación API</ins>:** Cuyo propósito es obtener y enviar distintas divisas de Chile.
2. **<ins>Aplicación Web</ins>:** Cuyo propósito es mostrar las distintas divisas.

## Comenzando

Estas instrucciones te permitirán obtener una copia del proyecto en funcionamiento en tu máquina local para propósitos de desarrollo y pruebas.

### Prerrequisitos

Qué cosas necesitas para instalar el software y cómo instalarlas:

#### Software

| Nombre | Descripción | URL de referencia |
| :--- | :--- | :--- |
| `Docker` | Ingrese al sitio web de `Docker`, descargue el instalador del software e instálelo. <br /> En caso de tener algún problema, ver el video | 1. [Sitio web](https://www.docker.com/) <br /> 2. [Video instalación (ejemplo)](https://www.youtube.com/watch?v=cWuirzMOwwg) |

## Comandos
### `Docker Compose`

Inicia todos los servicios definidos en el archivo `YAML` en segundo plano (modo `detached`):
```docker-compose
docker compose up -d
```

:---

Reconstruye las imágenes obligatoriamente y luego levanta los contenedores en segundo plano:
```
docker compose up --build -d
```

## Licencia
El proyecto Indicadores Chile es un software de código abierto con licencia `GNU General Public License v2.0`.
