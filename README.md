# WPF_SP — Sistema de Mantenimiento NeptunoDB (con Eliminación Lógica)

Proyecto académico de la Semana 05 del curso **Desarrollo de Aplicaciones Empresariales Avanzado**, sobre la base del proyecto de la Semana 04. Se implementó eliminación lógica (campo `Activo`) en lugar de eliminación física, usando `ExecuteNonQuery` para todas las operaciones de escritura.

## Datos del proyecto

- **Curso:** Desarrollo de Aplicaciones Empresariales Avanzado
- **Docente:** Edwin William Arévalo Sermeño
- **Integrante:** Medina Mallqui, Ailyn

## Descripción

Aplicación de escritorio en WPF + ADO.NET para el mantenimiento de Productos, Categorías, Proveedores y Pedidos de NeptunoDB. A partir de esta versión, ninguna operación de "Eliminar" borra un registro físicamente: todas actualizan el campo `Activo` a `0`, y todos los listados/búsquedas/reportes filtran por `Activo = 1`.

## Tecnologías usadas

| Tecnología | Uso |
|---|---|
| **.NET 10** | Framework de la aplicación de escritorio |
| **WPF (XAML)** | Interfaz gráfica |
| **ADO.NET** (`Microsoft.Data.SqlClient`) | Acceso a datos mediante `ExecuteNonQuery` / `ExecuteReader` |
| **SQL Server 2025** | Motor de base de datos (NeptunoDB) |
| **Patrón MVVM** | Arquitectura de la capa de presentación |

## Cambios de la Semana 05 respecto a la Semana 04

1. **Base de datos:** se agregó la columna `Activo BIT DEFAULT 1` a `Productos`, `Categorias`, `Proveedores` y `Pedidos` mediante `ALTER TABLE` (script `alter_activo.sql`).
2. **Stored Procedures:** los procedimientos `*_Eliminar` ahora ejecutan `UPDATE Activo = 0` en vez de `DELETE`. Los procedimientos de listado, búsqueda y reporte ahora filtran por `Activo = 1`.
3. **Capa `Data/`:** los 4 repositorios (`ProductoRepository`, `CategoriaRepository`, `ProveedorRepository`, `PedidoRepository`) se actualizaron para apuntar a los nuevos nombres de SP y parámetros. La forma de invocar `ExecuteNonQuery` no cambió — solo el comportamiento interno del SP.
4. **ViewModels y Views:** no se modificaron (excepto la columna `Producto` del reporte de Pedidos, que ahora muestra el nombre en vez del ID), ya que estaban desacoplados del detalle de acceso a datos.

## Estructura del proyecto

```bash
WPF_SP/
├── App.xaml / App.xaml.cs
├── App.config
├── MainWindow.xaml / .cs
├── Models/
│   ├── ModelBase.cs
│   ├── Producto.cs
│   ├── Categoria.cs
│   ├── Proveedor.cs
│   ├── Pedido.cs
│   └── DetallePedidoReporte.cs
├── Data/
│   ├── ConexionBD.cs
│   ├── ProductoRepository.cs
│   ├── CategoriaRepository.cs
│   ├── ProveedorRepository.cs
│   └── PedidoRepository.cs
├── ViewModels/
│   ├── ViewModelBase.cs
│   ├── RelayCommand.cs
│   ├── ProductoViewModel.cs
│   ├── CategoriaViewModel.cs
│   ├── ProveedorViewModel.cs
│   └── PedidoViewModel.cs
├── Views/
│   ├── ProductoView.xaml
│   ├── CategoriaView.xaml
│   ├── ProveedorView.xaml
│   └── PedidoView.xaml
├── Converters/
│   └── BoolToEstadoConverter.cs
└── Themes/
    └── Colors.xaml
```

## ¿Por qué eliminación lógica?

Borrar físicamente un registro relacionado con otras tablas (por ejemplo, un producto que ya tiene pedidos asociados) puede romper la integridad referencial o perder historial. Con la baja lógica, el registro sigue existiendo en la base de datos pero se "oculta" de la aplicación mediante el filtro `WHERE Activo = 1` en cada consulta, permitiendo conservar el historial completo.

## Funcionalidades implementadas

- [x] CRUD de Productos con baja lógica
- [x] CRUD de Categorías con baja lógica
- [x] CRUD de Proveedores con baja lógica
- [x] Búsqueda de Proveedores por contacto y ciudad (solo activos)
- [x] CRUD de Pedidos con baja lógica
- [x] Reporte de Detalle de Pedidos por rango de fechas (excluye pedidos inactivos)

## Observaciones y conclusiones

1. La baja lógica evita perder historial y relaciones con otras tablas al "eliminar" un registro.
2. `ExecuteNonQuery` se usa para operaciones que no devuelven filas (Insertar/Actualizar/Eliminar); `ExecuteReader` para las que sí (Listar/Reporte).
3. Gracias a la arquitectura en capas de la Semana 04, este cambio solo requirió tocar la capa `Data/`, sin modificar ViewModels ni Views.
4. Es importante verificar los alias de columnas (`AS IdProducto`, etc.) al leer con `SqlDataReader`, ya que un desajuste falla en tiempo de ejecución, no en compilación.
5. Este ejercicio mostró la diferencia entre "borrar" y "desactivar" como buena práctica profesional para trazabilidad y auditoría.
