# Creación de imagen de `Docker` con `GitHub Actions`.
Para que `GitHub` cree una imagen de `Docker`, debes configurar una `GitHub Action` que use un `Dockerfile` en tu repositorio. La acción automática incluirá pasos para clonar tu código, iniciar sesión en el registro de contenedores (como `GitHub Packages` o `Docker Hub`), construir la imagen y luego enviarla al registro.

## 1. Prepara tu repositorio
  * **<ins>Crea o asegúrate de tener un `Dockerfile`</ins>:** Este archivo contendrá todas las instrucciones para construir tu imagen.
  * **<ins>Sube el `Dockerfile` y tu código</ins>:** Si aún no lo has hecho, sube el `Dockerfile` y los archivos de tu aplicación a tu repositorio de GitHub.

## 2. Configura las credenciales de seguridad
  * **<ins>Crea un `Personal Access Token` (`PAT`)</ins>:** Ve a la [configuración de desarrollador](https://github.com/settings/apps) de tu cuenta de `GitHub`, crea un nuevo `token` con permisos para `write:packages` y `read:packages`.
  * **<ins>Guarda el token como un secreto</ins>:** En tu repositorio, ve a [`Settings`](../../settings) > `Secrets and variables` > [`Actions`](../../settings/secrets/actions) y crea una nueva variable de repositorio (por ejemplo, `DOCKER_TOKEN`) donde almacenes el valor del token.

## 3. Crea un flujo de `GitHub Actions`
  * **<ins>Crea un archivo `YAML` en tu repositorio</ins>:** Dentro de la carpeta `.github/workflows`, crea un archivo, por ejemplo: `docker-image.yml`.
  * **<ins>Define el evento de activación</ins>:** Configura el flujo para que se ejecute automáticamente cada vez que se empuje (`push`) o se envíe un `pull request` a la rama principal u otra rama, por ejemplo:
     ```YAML
     on:
       push:
         branches: [ main ]
       pull_request:
         branches: [ main ]
     ```
  * **<ins>Añade un trabajo (`job`)</ins>:** Este trabajo contendrá los pasos para construir y enviar la imagen.

## 4. Escribe el código del flujo `YAML`
```YAML
name: Build and Push Docker Image

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-push-image:
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        include:
          - project: API
            dockerfile: ./API/Dockerfile
            image_name: mi-repositorio/api
          - project: Web
            dockerfile: ./Web/Dockerfile
            image_name: mi-repositorio/web
    
    steps:
    - name: Checkout code
      id: checkoutCode
      uses: actions/checkout@v4 # Obtiene el código de tu repositorio

    - name: Log in to GitHub Container Registry ${{ matrix.project }}
      id: logInGitHub
      uses: docker/login-action@v4 # Permite iniciar sesión en el registro de contenedores
      with:
        registry: ghcr.io # Usa el registro de contenedores de GitHub
        username: ${{ github.actor }} # Usa tu nombre de usuario de GitHub
        password: ${{ secrets.DOCKER_TOKEN }} # Usa el token de acceso personal guardado como secreto

    - name: Build and push Docker image (${{ matrix.project }})
      id: buildPushImage
      uses: docker/build-push-action@v7 # Acción para construir y subir imágenes de Docker
      with:
        context: . # Construye desde el directorio raíz del proyecto
        file: ${{ matrix.dockerfile }}
        push: true # Sube la imagen al registro después de construirla
        tags: | # Asigna etiquetas a la imagen
          ghcr.io/${{ matrix.image_name }}:${{ github.sha }}
          ghcr.io/${{ matrix.image_name }}:latest
```
 * Este ejemplo utiliza la acción `docker/login-action` para iniciar sesión y la acción `docker/build-push-action` para construir y publicar.
 * La etiqueta `ghcr.io/${{ github.repository }}` se refieren a la URL del registro de `GitHub`, tu nombre de usuario y el nombre del repositorio.
 * Las etiquetas (`tags`) crean 2 versiones de la imagen:
    - Una con el **ID específico del commit** (`${{ github.sha }}`), útil para trazabilidad.
    - Otra con la etiqueta `latest`, que siempre apunta a la versión más reciente.
 * Este ejemplo está pensado para que el repositorio tenga 2 proyectos (aunque puede tener más proyectos o tener 1).
    - Para crear dos o más imágenes `Docker` a partir de proyectos ubicados en un solo repositorio de `GitHub`, la mejor práctica es utilizar una estrategia de matriz (`matrix strategy`). Esto te permite definir un solo flujo de trabajo (`workflow`) que construirá y subirá tus imágenes en paralelo, manteniendo el código limpio y evitando la repetición. Parafraceando del sitio web [Stack Overflow](https://stackoverflow.com/questions/69619444/github-actions-building-a-multi-platform-image-using-multiple-dockerfiles).
    - **<ins>Matriz (`matrix`)</ins>:** La sección `include` define los detalles únicos de cada proyecto. Puedes agregar más bloques `- project: app3...` siguiendo la misma estructura.

## 5. Confirma y ejecuta el flujo
 * Guarda el archivo `YAML` en `.github/workflows/` y confirma los cambios en tu repositorio.
 * `GitHub Actions` ejecutará automáticamente este flujo en la próxima inserción en la rama `main`, construyendo y publicando tu imagen de `Docker` en `GitHub Packages`.

# Más información
1. [Cree y envíe imágenes de Docker](https://github.com/marketplace/actions/build-and-push-docker-images).
2. [Sintaxis básica de redacción y formato (archivo md)](https://docs.github.com/es/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax).
