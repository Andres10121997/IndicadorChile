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
#### Comandos de ciclo de vida.

1. Inicia todos los servicios definidos en el archivo `YAML` en segundo plano (modo `detached`):
```yaml
docker compose up -d
```

2. Reconstruye las imágenes obligatoriamente y luego levanta los contenedores en segundo plano:
```yaml
docker compose up --build -d
```

3. Detiene y elimina los contenedores, las redes y (por defecto) los volúmenes asociados:
```yaml
docker compose down
```

4. Detiene los contenedores en ejecución sin eliminarlos del sistema:
```yaml
docker compose stop
```

5. Vuelve a arrancar los contenedores que se encontraban detenidos:
```yaml
docker compose start
```

6. Reinicia todos los servicios de la aplicación:
```yaml
docker compose restart
```

#### Monitoreo y diagnóstico
1. Muestra el estado actual y los puertos de los contenedores gestionados por `Compose`.
```yaml
docker compose ps
```

2. Muestra en tiempo real los registros (`logs`) de todos los servicios.
```yaml
docker compose logs -f
```

3. Muestra los `logs` en vivo únicamente del servicio especificado.
```yaml
docker compose logs -f <nombre_servicio>
```

4. Muestra los procesos que se están ejecutando dentro de cada contenedor.
```yaml
docker compose top
```

## Licencia
El proyecto Indicadores Chile es un software de código abierto con licencia `GNU General Public License v2.0`.
