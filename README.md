# WPF_SP — NeptunoDB (ADO .NET SEMANA04)

## Requisitos
- Visual Studio 2022 con carga de trabajo ".NET desktop development"
- SQL Server con la base NeptunoDB ya creada (scripts ejecutados) y sus stored procedures.

## Configuración
1. Abre `WPF_SP.csproj` en Visual Studio (o `dotnet restore` desde consola).
2. Ajusta la cadena de conexión en `App.config` según tu instancia de SQL Server:
   ```xml
   <add name="NeptunoDB" connectionString="Server=TU_SERVIDOR;Database=NeptunoDB;Trusted_Connection=True;TrustServerCertificate=True;" />
   ```
3. Ejecuta (F5). Verás un menú superior con Productos, Categorías, Proveedores y Pedidos.

## Estructura
- `Models/` — Entidades (Producto, Categoria, Proveedor, Pedido, DetallePedidoReporte) con INotifyPropertyChanged.
- `Data/` — Repositorios ADO.NET que invocan los stored procedures (uno por entidad, más búsqueda de proveedores y reporte de pedidos).
- `ViewModels/` — MVVM: ViewModelBase, RelayCommand, y un ViewModel por módulo con comandos Cargar/Nuevo/Guardar/Eliminar.
- `Views/` — UserControls con DataGrid + formulario para cada módulo. ProveedorView incluye filtros de búsqueda; PedidoView incluye una pestaña de reporte por rango de fechas.
- `Converters/` — BoolToEstadoConverter (ejemplo, disponible para usar en las grillas).
- `Themes/Colors.xaml` — Paleta y estilos base (BotonMenu, BotonAccion, estilo de DataGrid).

## Pendiente / a tu criterio
- Reemplazar los TextBox de ProveedorID/CategoriaID/ClienteID/EmpleadoID por ComboBox cargados desde sus respectivos repositorios (por ahora se ingresan como ID numérico para simplificar).
- Ajustar la paleta de Themes/Colors.xaml a tu gusto.
- Agregar validaciones adicionales según lo que pida la rúbrica.
