using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Simulacion_SO
{
    public partial class FormTablaProcesos : Form
    {
        List<Proceso> Nuevo = new List<Proceso>();
        List<Proceso> Terminados = new List<Proceso>();
        List<Proceso> Listo = new List<Proceso>();
        List<Proceso> Bloqueados = new List<Proceso>();
        Proceso ejecucion=null;
        int Tiempo;
        String arcSus = Application.StartupPath + "\\Suspendidos.txt";
        List<Proceso> Suspendidos = new List<Proceso>();
        

        public FormTablaProcesos(int Tiempo,Proceso Ejecucion, Cola Nuevos, Cola Listos, List<Proceso> Bloqueados,List<Proceso> Terminados)
        {
            InitializeComponent();
            this.Tiempo = Tiempo;

            while (Nuevos.Count>0)
            {
                Nuevo.Add(Nuevos.Dequeue());
            }
            foreach (Proceso p in Nuevo)
            {
                Nuevos.Enqueue(p);
            }

            while (Listos.Count > 0)
            {
                Listo.Add(Listos.Dequeue());
            }
            foreach (Proceso p in Listo)
            {
                Listos.Enqueue(p);
            }
            RecuperarSuspendidos();

            this.Bloqueados = Bloqueados;
            this.Terminados = Terminados;
            this.ejecucion = Ejecucion;

            cargarListas();


        }

        private void cargarListas()
        {
            //ejecucion
            if (ejecucion != null)
            {
                listViewEjecucion.Items.Add(ejecucion.NumeroDePrograma.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.Operacion);
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoMaximoEstimado.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoTranscurrido.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoRestante.ToString());
                ejecucion.obtenerTiempos();
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoLlegada.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoRespuesta.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoServicio.ToString());
                listViewEjecucion.Items[listViewEjecucion.Items.Count - 1].SubItems.Add(ejecucion.TiempoEspera.ToString());
            }
            //nuevos
            foreach (Proceso p in Nuevo)
            {
                listViewNuevos.Items.Add(p.NumeroDePrograma.ToString());
                listViewNuevos.Items[listViewNuevos.Items.Count - 1].SubItems.Add(p.Operacion);
                listViewNuevos.Items[listViewNuevos.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
            }

            //terminados
            foreach (Proceso p in Terminados)
            {
                listViewResultados.Items.Add(p.NumeroDePrograma.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.Operacion);
                if (p.error)
                {
                    listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add("Error");
                }
                else
                {
                    listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.Resultado.ToString());
                }
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoTranscurrido.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoRestante.ToString());
                //p.obtenerTiempos(Tiempo);
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoLlegada.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoRespuesta.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoFinalización.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoServicio.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoEspera.ToString());
                listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoRetorno.ToString());
            }

            //listos
            foreach (Proceso p in Listo)
            {
                listViewListos.Items.Add(p.NumeroDePrograma.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.Operacion);
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoTranscurrido.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoRestante.ToString());

                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoLlegada.ToString());
                if (p.AuxRes==false)
                {
                    listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add("N/A");
                }
                else
                {
                    listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoRespuesta.ToString());
                }
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoEspera.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoRestanteBloqueado.ToString());

            }
            //bloqueados
            foreach (Proceso p in Bloqueados)
            {
                listViewBloqueados.Items.Add(p.NumeroDePrograma.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.Operacion);
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoTranscurrido.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoRestante.ToString());
                
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoLlegada.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoRespuesta.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoEspera.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoRestanteBloqueado.ToString());

            }

            //Suspendidos
            foreach (Proceso p in Suspendidos)
            {
                listViewSuspendidos.Items.Add(p.NumeroDePrograma.ToString());
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.Operacion);
                if (p.error)
                {
                    listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add("Error");
                }
                else
                {
                    listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.Resultado.ToString());
                }
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoTranscurrido.ToString());
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoRestante.ToString());
                //p.obtenerTiempos(Tiempo);
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoLlegada.ToString());
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoRespuesta.ToString());
                //listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoFinalización.ToString());
                //listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoServicio.ToString());
                listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoEspera.ToString());
                //listViewSuspendidos.Items[listViewSuspendidos.Items.Count - 1].SubItems.Add(p.TiempoRetorno.ToString());
            }

        }

        private void listViewEjecucion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void RecuperarSuspendidos()
        {
            StreamReader reader = new StreamReader(arcSus);
            
            while (true) {
                String res = reader.ReadLine();
                Proceso p = null;

                if (res != null)
                {
                    int np = (int)AuxRecupera(res, 0);
                    String ope = AuxRecuperaS(res, 1);
                    double rs = AuxRecupera(res, 2);
                    int TME = (int)AuxRecupera(res, 3);
                    int TT = (int)AuxRecupera(res, 4);
                    int TR = (int)AuxRecupera(res, 5);
                    int TLL = (int)AuxRecupera(res, 6);
                    int TE = (int)AuxRecupera(res, 7);
                    int TRS = (int)AuxRecupera(res, 8);
                    int Tam = (int)AuxRecupera(res, 9);
                    int nf = (int)AuxRecupera(res, 10);
                    int Tiemp = (int)AuxRecupera(res, 11);
                    int blo = (int)AuxRecupera(res, 12);

                    p = new Proceso(ope, TME, np, rs, Tam);
                    p.TiempoTranscurrido = TT;
                    p.TiempoRestante = TR;
                    p.TiempoLlegada = TLL;
                    p.AuxRes = true;
                    p.TiempoRespuesta = TRS;
                    p.tamaño = Tam;
                    p.nf = nf;
                    p.TiempoEspera = TE + (Tiempo - Tiemp);
                    p.TiempoRestanteBloqueado = blo;

                    Suspendidos.Add(p);
                }
                else break;
            }
            reader.Close();            
        }

        private String AuxRecuperaS(String cad, int salto)
        {
            int i = 0;
            while (salto > 0)
            {
                if (cad[i] == '|')
                {
                    salto--;
                }
                i++;
            }

            String aux = "";
            while (cad[i] != '|')
            {
                aux += cad[i];
                i++;
            }
            i++;

            return aux;
        }

        private double AuxRecupera(String cad, int salto)
        {
            int i = 0;
            while (salto > 0)
            {
                if (cad[i] == '|')
                {
                    salto--;
                }
                i++;
            }

            String aux = "";
            while (cad[i] != '|')
            {
                aux += cad[i];
                i++;
            }
            i++;
            double val;
            double.TryParse(aux, out val);
            return val;
        }

        private void FormTablaProcesos_KeyPress(object sender, KeyPressEventArgs e)
        {
            char tecla = e.KeyChar;
            if (tecla == 'c'||tecla=='C')
            {
                this.Close();
            }
        }
    }
}
