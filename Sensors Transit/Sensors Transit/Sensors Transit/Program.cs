using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SensorsTransit
{
    //Classe que implementa la informació d'un sol sensor
    public class Sensor
    {
        public int IdSensor { get; set; }
        public int NumVehicles { get; set; }

        public Sensor(int Id, int Vehicles)
        {
            IdSensor = Id;
            NumVehicles = Vehicles;
        }

        public Sensor EnviaDadesCentral()
        {
            Thread.Sleep(100);      //simulem que el sensor té un retard en l'enviament de dades. 
            return this;
        }

        public void Connecta()
        {
            Thread.Sleep(200);
        }

        public void Reset()
        {
            NumVehicles = 0;
        }
    }

    //Contindrà les dades de totes els sensors de la ciutat
    class SensorsCiutat
    {
        private static int NumSensors;
        public int MitjanaVehicles { get; set; }
        private int SumaVehicles;
        public int MaxVehicles { get; set; }
        List<Sensor> Sensors = new List<Sensor>();  //LLista dels sensors
        List<Sensor> SensorsLocal = new List<Sensor>();       //Llista on rebem les dades del nombre de vehicles de cada sensor i la utilitzem per als càlculs
        List<Sensor> SensorsLocalOrd = new List<Sensor>();    //Llista auxiliar on ordenem el nombre de vehicles

        //Constructor de la classe SensorsCiutat. No cal fer res
        public SensorsCiutat(int Num)
        {
            NumSensors = Num;
        }

        //Mètode per inicialitzar els sensors i disposar de dades per a treballar. No cal fer res
        public void InicialitzaSensors()
        {
            Random rnd = new Random();

            for (int i = 0; i < NumSensors; i++)
            {
                Sensors.Add(new Sensor(i, rnd.Next(0, 100)));
            }
        }

        //TASCA A FER: Optimitzar aquest mètode si és possible utilitzant mètodes de programació en paral·lel com Parallel.For
        /// <summary>
        /// Aquest mètode rep les dades de cada sensor a través del mètode EnviaDadesCentral (que té un retard) i un cop rebuda la informació s'afegeix a la llista Vehicles
        /// </summary>
        public void RebreDadesSensors()
        {
            for (int i = 0; i < NumSensors; i++)
            {
                SensorsLocal.Add(Sensors[i].EnviaDadesCentral());
            }
        }

        //TASCA A FER: Cal optimitzar aquest mètode, si és possible utilitzant mètodes de programació en paral·lel, concretamen el Parallel.For
        /// <summary>
        /// Aquest mètode mostra les dades de tots els sensors, però no cal que sigui en ordre.
        /// </summary>
        public void MostraDadesSensors()
        {
            string Missatge;

            for (int i = 0; i < NumSensors; i++)
            {
                Missatge = "Sensor nº " + SensorsLocal[i].IdSensor + " --- " + SensorsLocal[i].NumVehicles + " vehicles comptabilitzats.";
                Mostra(Missatge);
            }
        }

        //No cal optimitzar aquest mètode, només és per comprovar que s'han ordenat bé les dades
        public void MostraDadesOrdenades()
        {
            for (int i = 0; i < NumSensors; i++)
            {
                Console.WriteLine("Sensor nº {0}: {1} vehicles", SensorsLocalOrd[i].IdSensor, SensorsLocalOrd[i].NumVehicles);
            }
        }

        //TASCA A FER: Optimitzar aquest mètode si és possible
        /// <summary>
        /// Aquest mètode connecta amb cada sensor (això té un retard) i un cop connectats es fa un reset del sensor.
        /// </summary>
        public void ResetSensors()
        {
            for (int i = 0; i < NumSensors; i++)
            {
                Sensors[i].Connecta();
                Sensors[i].Reset();
            }
        }

        //TASCA A FER: Optimitzar aquest mètode si és possible
        /// <summary>
        /// Aquest mètode realitza la mitajana de vehicles de tots els sensors. Per fer-ho utilitza el mètode suma (que té un retard)
        /// </summary>
        public void CalcularMitjana()
        {
            for (int i = 0; i < NumSensors; i++)
            {
                SumaVehicles = Suma(SumaVehicles, SensorsLocal[i]);
            }
            MitjanaVehicles = SumaVehicles / NumSensors;
        }

        //TASCA A FER: Optimitzar aquest mètode si és possible
        /// <summary>
        /// Aquest mètode calcula el nombre màxim de vehicles que han passat per un carrer, utilitza el mètode MesGran (que té un retard)
        /// </summary>
        public void CalcularMaxVehicles()
        {
            MaxVehicles = 0;

            for (int i = 0; i < NumSensors - 1; i++)
            {
                MaxVehicles = MesGran(MaxVehicles, SensorsLocal[i]);
            }
        }

        //TASCA A FER: Optimitzar aquest mètode si és possible
        /// <summary>
        /// Aquest mètode ordena el nombre de vehicles. S'utilitza el mètode EstanOrdenats (que té un retard)
        /// </summary>
        public void OrdenaVehicles()
        {
            bool canvi = true;
            Sensor aux;

            SensorsLocalOrd = SensorsLocal;
            while (canvi)
            {
                canvi = false;
                for (int i = 0; i < NumSensors - 1; i++)
                {
                    if (EstanOrdenats(SensorsLocalOrd[i], SensorsLocalOrd[i + 1]))
                    {
                        aux = SensorsLocalOrd[i];
                        SensorsLocalOrd[i] = SensorsLocalOrd[i + 1];
                        SensorsLocalOrd[i + 1] = aux;
                        canvi = true;
                    }
                }
            }
        }

        //Aquest mètode no es pot modificar
        private void Mostra(string Missatge)
        {
            for (int j = 0; j < 10; j++)
            {
                Thread.Sleep(2);    //simulem un càlcul lent
            }
            Console.WriteLine("{0}, ", Missatge);
        }

        //Aquest mètode no es pot modificar
        private int Suma(int i, Sensor S)
        {
            for (int k = 0; k < 10; k++)
            {
                Thread.Sleep(2);    //simulem un càlcul lent
            }
            return i + S.NumVehicles;
        }

        //Aquest mètode no es pot modificar
        private int MesGran(int i, Sensor S)
        {
            for (int k = 0; k < 10; k++)
            {
                Thread.Sleep(2);    //simulem un càlcul lent
            }
            if (i > S.NumVehicles)
                return i;
            else
                return S.NumVehicles;
        }

        //Aquest mètode no es pot modificar
        private bool EstanOrdenats(Sensor i, Sensor j)
        {
            for (int k = 0; k < 1000000; k++) { }     //simulem un càlcul lent
            if (i.NumVehicles > j.NumVehicles) return true;
            else return false;
        }
    }

    class Program
    {
        //TASCA A FER: Intenta utilitza els fils per tal de cridar en paral·lel els mètodes que ho permetin. 
        static void Main(string[] args)
        {
            SensorsCiutat SensorsBCN = new SensorsCiutat(100);

            DateTime T1 = DateTime.Now;

            SensorsBCN.InicialitzaSensors();
            Console.WriteLine("Rebent dades dels sensors...");
            SensorsBCN.RebreDadesSensors();
            Console.WriteLine("Dades rebudes");

            SensorsBCN.MostraDadesSensors();


            Console.WriteLine("Calculant mitjana");
            SensorsBCN.CalcularMitjana();
            Console.WriteLine("Mitjana vehicles: {0}", SensorsBCN.MitjanaVehicles);


            Console.WriteLine("Calculant vehicles max");
            SensorsBCN.CalcularMaxVehicles();
            Console.WriteLine("Max vehicles: {0}", SensorsBCN.MaxVehicles);

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
    }
}
