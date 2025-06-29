# 🚀 Godot Vertical Shooter Demo (Refactorizado)

Este proyecto es un juego de disparos vertical (vertical shooter) creado con **Godot 4** y **C#**, inspirado en clásicos arcade. Esta versión ha sido completamente **refactorizada** con principios de arquitectura limpia, patrones de diseño y modularidad para facilitar su mantenimiento, escalabilidad y pruebas.

---

## 🎮 Características del juego

- Control de jugador con patrón **Command** (movimiento y disparo desacoplado del input).
- Sistema de enemigos modular con **estrategias de movimiento** (lineal, sinusoidal, serpenteante).
- Sistema de **niveles y dificultad dinámica** basada en puntuación.
- HUD con nivel, puntuación, progreso al siguiente nivel y récord histórico.
- Gestión de audio con música de fondo y efectos (láser, explosiones).
- Persistencia de récords mediante sistema de archivos.
- Separación estricta por responsabilidades: UI, lógica de entidades, servicios, comandos y estrategias.

---

## 🗂️ Estructura del Proyecto

```plaintext
scripts/
├── components/           # Composición de entidades (vida, movimiento)
│   └── interfaces/       # Interfaces para daño y movimiento
├── core/                 # Entidades base y lógica compartida
├── entities/             # Jugador, enemigos, láser
├── factories/            # Factories para instanciar enemigos
├── managers/             # GameManager, ScoreManager, SpawnManager, LevelManager
├── services/             # Servicios como audio y guardado
├── strategies/           # Estrategias de movimiento (Strategy Pattern)
├── ui/                   # HUD y Game Over Screen
├── commands/             # Sistema Command para input y acciones
│   └── interfaces/       # Interfaz ICommand
└── utils/                # Constantes y helpers de archivo
