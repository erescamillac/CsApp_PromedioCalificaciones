using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsApp_PromedioCalificaciones
{
    //-- INI: class Asignatura
    //@author: Erick Escamilla Charco
    public class Asignatura{
        private string _nombre;
        private float _calif;

        // constructor sin argumentos
        public Asignatura(){
            _nombre = "none";
            _calif = 0.0f;
        }

        public Asignatura(string name) {
            _nombre = name;
        }

        public Asignatura(string name, float grade){
            this.Nombre = name;
            this.Calif = grade;
        }

        public string Nombre{
            get{
                return _nombre;
            }
            set
            {
                _nombre = value;
            }
        }

        public float Calif {
            get {
                return _calif;
            }
            set
            {
                if (value < 0 || value > 10)
                {
                    // Posible implementación de Excepción personalizada (throw ...)
                    Console.WriteLine("Calificación {0} FUERA DE RANGO [0.0 - 10.0]", value);
                    _calif = 0.0f;
                }
                else {
                    _calif = value;
                }
                
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("", 30);
            sb.Append("[");
            sb.Append(_nombre);
            sb.Append(": ");
            sb.Append(_calif);
            sb.Append("]");
            return sb.ToString();
        }

    }
    //++FIN: class Asignatura

    public class Alumno{
        private string _name;
        private List<Asignatura> _asignaturas;
        private float _promedio;

        public Alumno() {
            _name = "";
            _asignaturas = new List<Asignatura>();
            _promedio = -1;
        }

        public Alumno(string name) {
            _name = name;
            _asignaturas = new List<Asignatura>();
            _promedio = -1;
        }

        public string Name {
            get
            {
                return _name;
            }
            set {
                _name = value;
            }
        }

        public string AsignaturasCargadas {
            get {
                return _asignaturas.ToString();
            }
        }

        public void ResetAsignaturas() {
            _asignaturas.Clear();
            _promedio = -1;
        }

        public void AddAsignatura(Asignatura asignatura){
            if (asignatura != null) {
                _asignaturas.Add(asignatura);
                _promedio = -1;
            }
        }

        public float Promedio() {
            float acumulador = 0.0f;
            if (_promedio < 0) {
                //-- SI AÚN NO SE HA CALCULADO EL PROMEDIO
                foreach (Asignatura asignatura in _asignaturas){
                    acumulador += asignatura.Calif;
                }
                _promedio = acumulador / _asignaturas.Count;

            }
            //-- fin promedio < 0
            return _promedio;
        }

        public string FeedbackMessage() {
            StringBuilder sb = new StringBuilder();
            string apertura, cierre;
            this.Promedio();
            if (_promedio < 7)
            {
                apertura = "Lo sentimos mucho";
                cierre = "Calificación Mala";
            }
            else if (_promedio >= 7 && _promedio <= 8.5)
            {
                apertura = "Bien hecho";
                cierre = "Calificación Buena";
            }
            else if (_promedio < 9.6)
            {
                apertura = "Muy bien";
                cierre = "Calificación muy buena";
            }
            else {
                apertura = "Maravilloso, sigue así";
                cierre = "Calificación Excelente";
            }
            sb.Append(apertura).Append(", ").Append(_name).Append(", HAS OBTENIDO UNA ").Append(cierre).Append(".");
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Asignatura ultimaAsignatura = _asignaturas.Last();
            sb.Append("[Alumno: ").Append(_name).Append(", ASIGNATURAS {");
            foreach(Asignatura asignatura in _asignaturas){
                sb.Append(asignatura.Nombre).Append(" : ").Append(asignatura.Calif);
                if ( !ultimaAsignatura.Equals(asignatura) ) {
                    sb.Append(", ");
                }
            }
            sb.Append("}").Append("]");
            return sb.ToString();
        }

    }
    //++FIN: Clase Alumno

    public class InputReader{

        private static string[] asignaturas = new string[] { "Matemáticas", "Español", "Física" };

        public InputReader(){
        }

        public float readGrade(string asignatura){
            float grade = -1.0f;
            string errorMsg = "";
            bool error = false;

            do {
                if (errorMsg.Length > 0) {
                    Console.WriteLine(errorMsg);
                }
                Console.WriteLine("Ingrese una calificación entre 0.0 y 10.0 para la asignatura {0}: ", asignatura);
                grade = float.Parse(Console.ReadLine());
                error = grade < 0.0 || grade > 10.0 ? true : false;
                if (error) {
                    errorMsg = "ERROR: Calificación FUERA DEL RANGO permitido (0.0 a 10.0)";
                }
            } while (error);

            return grade;
        }

        public void readAllGrades(Alumno alumni) {
            Asignatura asignaturaTmp;
            Console.WriteLine("Ingrese las calificaciones del alumno {0}", alumni.Name);
            foreach (string asignatura in asignaturas) {
                asignaturaTmp = new Asignatura(asignatura);
                asignaturaTmp.Calif = readGrade(asignatura);
                alumni.AddAsignatura(asignaturaTmp);
            }
            Console.WriteLine("--CAPTURA DE CALIFICACIONES del alumno {0} COMPLETA--", alumni.Name);
        }

    }
    //++ FIN: InputReader
    public class App {


        public App() {
        }

        public void execute(){
            char continuar = 'n';
            InputReader inputReader = new InputReader();
            Alumno alumno = new Alumno();
            //-- main cycle
            do {
                alumno.ResetAsignaturas();
                Console.WriteLine("######################################################");
                Console.WriteLine("--Programa para calcular el promedio de un alumno--");
                Console.WriteLine("Ingrese el nombre del alumno: ");
                alumno.Name = Console.ReadLine();
                inputReader.readAllGrades(alumno);
                Console.WriteLine("DATOS capturados del Alumno: [{0}]", alumno);
                Console.WriteLine("Promedio del alumno: {0}", alumno.Promedio());
                Console.WriteLine(alumno.FeedbackMessage());
                Console.WriteLine("\t¿Desea calcular el promedio de otro alumno? [y/n]: ");
                continuar = Console.ReadKey().KeyChar;
            } while(Char.ToLower(continuar).Equals('y'));
            //++ main cycle
        }
        //-- fin: execute()
    }
    //@author: Erick Escamilla Charco
    class Program
    {
        
        /*
        Tarea 10. Elaborar un Programa que le solicite al usuario la introducción (captura) 
        de su nombre completo y la calificación de tres materias ficticias (Matemáticas, Español y Física). 
        Después de ello el programa deberá calcular el promedio de la calificación de las tres materias 
        y mostrarlo en pantalla junto con un mensaje de resultado en base a los siguientes criterios:

        - Si el promedio es menor que 7, deberá emitirse un mensaje de "Calificación Mala".
        - Si el promedio esta entre 7 y 8.5, deberá emitirse un mensaje de "Calificación Buena".
        - Si el promedio esta entre 8.6 y 9.5, deberá emitirse un mensaje de "Calificación Muy Buena".
        - Si el promedio esta entre 9.6 y 10, deberá emitirse un mensaje de "Calificación Excelente". 
        */
        static void Main(string[] args)
        {
            //@author: Erick Escamilla Charco
            App application = new App();

            application.execute();
        }
    }
}
