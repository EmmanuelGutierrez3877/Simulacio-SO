using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_SO
{
    public class Proceso
    {
        //public String NombreProgramador;
        public String Operacion; //a realizar(+, -, *, /, residuo) con sus respectivos datos(validar operaciones)
        public int TiempoMaximoEstimado; //(validar, debe ser mayor a 0)
        public int NumeroDePrograma;  //(validar que sea Único, ID)
        public double Resultado;

        public int TiempoTranscurrido = 0;
        public int TiempoRestante = 0;
        public bool error = false;

        public int TiempoRestanteBloqueado = 0;
        public bool AuxRes = false;

        public int TiempoLlegada = 0;       //-Hora en la que el proceso entra al sistema.
        public int TiempoFinalización=0;      //-Hora en la que el proceso termino.
        public int TiempoRetorno = 0;           //Tiempo total desde que el proceso llega hasta que termina.
        public int TiempoRespuesta = 0;         //-Tiempo transcurrido desde que llega hasta que es atendido por primera vez.
        public int TiempoEspera = 0;            //Tiempo que el proceso ha estado esperando para usar el procesador.
        public int TiempoServicio = 0;          //Tiempo que el proceso ha estado dentro del procesador. (Si el  proceso termino su ejecución normal es el TME)

        public String estado = "Nuevo";
        public List<Frame> Frames = new List<Frame>();
        public int tamaño;
        public int nf = 0;
        public List<Frame> FramesReal = new List<Frame>();
        public List<Frame> FramesVirtual = new List<Frame>();

        public Proceso(String Operacion, int TiempoMaximoEstimado, int NumeroDePrograma, double resultado, int tamaño)
        {
            //this.NombreProgramador = NombreProgramador;
            this.Operacion = Operacion;
            this.TiempoMaximoEstimado = TiempoMaximoEstimado;
            this.NumeroDePrograma = NumeroDePrograma;
            this.TiempoRestante = TiempoMaximoEstimado;
            this.Resultado = resultado;
            this.tamaño = tamaño;

            int i =tamaño;
            while (i > 0)
            {
                i = i - 5;
                nf += 1;
            }
        }

        public void Respuesta()
        {
            if (AuxRes==false)
            {
                TiempoRespuesta = (int)TiempoEspera;
                AuxRes = true;
            }
            
        }

        public int Finaliza(int tiempoFinal)
        {
            this.TiempoFinalización = TiempoLlegada+TiempoTranscurrido+TiempoEspera;

           return TiempoFinalización;
        }

        public void obtenerTiempos()
        {
            TiempoServicio = TiempoTranscurrido;
            TiempoRetorno = TiempoFinalización - TiempoLlegada ;
        }

        public bool comp1()
        {
            if (TiempoLlegada+TiempoServicio+TiempoEspera == TiempoFinalización)
            {
                return true;
            }
            return false;
        }

        public bool comp2()
        {
            if (TiempoServicio + TiempoEspera == TiempoRetorno)
            {
                return true;
            }
            return false;
        }
    }
}
