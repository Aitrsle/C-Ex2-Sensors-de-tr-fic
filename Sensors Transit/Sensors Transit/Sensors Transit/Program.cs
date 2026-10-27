// Tarea 7: Main optimizado ejecutando tareas independientes en paralelo
static void Main(string[] args)
{
    SensorsCiutat SensorsBCN = new SensorsCiutat(100);

    DateTime T1 = DateTime.Now;

    SensorsBCN.InicialitzaSensors();

    Console.WriteLine("Rebent dades dels sensors...");
    SensorsBCN.RebreDadesSensors();
    Console.WriteLine("Dades rebudes");

    // Mostramos los datos recibidos
    SensorsBCN.MostraDadesSensors();

    // EJECUCIÓN EN PARALELO: CalcularMitjana y CalcularMaxVehicles no dependen entre sí
    Console.WriteLine("Calculant mitjana i max vehicles en paral·lel...");
    Parallel.Invoke(
        () =>
        {
            SensorsBCN.CalcularMitjana();
            Console.WriteLine("Mitjana vehicles: {0}", SensorsBCN.MitjanaVehicles);
        },
        () =>
        {
            SensorsBCN.CalcularMaxVehicles();
            Console.WriteLine("Max vehicles: {0}", SensorsBCN.MaxVehicles);
        }
    );

    Console.WriteLine("Ordenant vehicles");
    SensorsBCN.OrdenaVehicles();
    SensorsBCN.MostraDadesOrdenades();

    Console.WriteLine("Resetejant sensors");
    SensorsBCN.ResetSensors();

    DateTime T2 = DateTime.Now;
    TimeSpan Diff = TimeSpan.FromTicks(T2.Ticks - T1.Ticks);
    Console.WriteLine("Tot el procés ha trigat {0}", Diff.ToString());

    Console.ReadLine();
}