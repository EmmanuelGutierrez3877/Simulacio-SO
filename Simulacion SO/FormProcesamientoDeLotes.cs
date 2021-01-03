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
    public partial class FormProcesamientoDeLotes : Form
    {
        Form Anterior;
        int ProcesosPendientes = 0;
        int Tiempo = 0;
        bool pausa = false;
        //int memoria = 3;
        Random rnd;
        FormInicio FI = new FormInicio();
        int ultimo;
        Queue<char> teclas = new Queue<char>();
        int quantum;
        int AuxQ;

        Cola Nuevos;
        Cola Listos = new Cola();
        Proceso Ejecucion = null;
        List<Proceso> Bloqueados = new List<Proceso>();
        List<Proceso> Terminados = new List<Proceso>();
        

        int memoriaF = 36;
        List<Frame> MemoriaReal = new List<Frame>();
        List<Button> MR = new List<Button>();

        String arcSus = Application.StartupPath + "\\Suspendidos.txt";
        int NumSuspendidos = 0;

        List<Frame> MemoriaVirtual = new List<Frame>();
        List<Button> MV = new List<Button>();
        Queue<Frame> cambiados = new Queue<Frame>();

        public FormProcesamientoDeLotes(Form Anterior, Cola Nuevos,int ultimo, Random rnd, int quantum)
        {
            InitializeComponent();
            this.Nuevos = Nuevos;
            this.Anterior = Anterior;
            timer1.Stop();
            this.ultimo = ultimo;
            this.rnd = rnd;
            this.quantum = quantum;
            AuxQ = quantum;

            Proceso so1 = new Proceso("1+1", 0, 0, 0, 0);
            so1.estado = "Sistema Operativo";
            Frame f1 = new Frame();
            Frame f2 = new Frame();
            f1.proceso = so1;
            f2.proceso = so1;
            MemoriaReal.Add(f1);
            MemoriaReal.Add(f2);

            var file = new FileStream(arcSus, FileMode.Create);
            file.Close();

            for (int i = 1;i<=memoriaF;i++)
            {
                foreach (Control c in tableLayoutPanel5.Controls)
                {
                    if (c.Name == "buttonR"+i)
                    {
                        MR.Add(c as Button);
                    }
                    if (c.Name == "buttonV" + i)
                    {
                        MV.Add(c as Button);
                    }
                }
            }

            for (int i = 1; i<=memoriaF-2; i++)
            {
                Frame f = new Frame();
                MemoriaReal.Add(f);
                //f.nf = i;
            }
            for (int i = 1; i <= memoriaF; i++)
            {
                Frame f = new Frame();
                MemoriaVirtual.Add(f);
                //f.nf = i;
            }

            foreach (Proceso p in Nuevos)
            {
                ProcesosPendientes++;
            }
            
            timer1.Interval = 500;
            timer3.Interval = 250;
            LlenarFrames();

            cargarListas();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            //Ejecucion = Listos.Dequeue();
            //Ejecucion.Respuesta();
            Tiempo = 0;
            cargarListas();
            timer1.Start();
        }

        private void cargarListas()
        {
            labelTiempo.Text = "Tiempo Transcurrido: " + Tiempo;
            labelProcesosPendientes.Text = "Nuevos: " + Nuevos.Count;
            labelQuantum.Text = "Quantum: " + quantum;
            listViewListos.Items.Clear();
            listViewBloqueados.Items.Clear();
            listBoxProcesoActual.Items.Clear();
            labelFS.Text = "TF siguiente: ";
            if (Nuevos.Siguiente()!=null)
            {
                labelFS.Text = "TF siguiente: " + Nuevos.Siguiente().nf;
            }

            labelNSus.Text = "Numero de suspendidos: " + NumSuspendidos;
            Proceso px = RecuperarSuspendido();
            if (px!=null)
            {
                labelTamUltSus.Text = "Tamaño frames ultimo: "+px.nf;
            }
            else
            {
                labelTamUltSus.Text = "Tamaño frames ultimo: N/A";
            }

            for (int i=0;i<36;i++)
            {
                MR[i].BackColor = Color.White;
                MR[i].ForeColor = Color.Black;
                MR[i].Text = "null";
                if (MemoriaReal[i].proceso!=null)
                {
                    MR[i].Text = "P:"+MemoriaReal[i].proceso.NumeroDePrograma+"\n"+MemoriaReal[i].memoria+"/5";
                    
                    if (MemoriaReal[i].proceso.estado == "Sistema Operativo")
                    {
                        MR[i].ForeColor = Color.White;
                        MR[i].BackColor = Color.Black;
                        MR[i].Text = "S.O.";
                    }
                    else if (MemoriaReal[i].proceso.estado == "Listo")
                    {
                        MR[i].BackColor = Color.Aquamarine;
                    }
                    else if (MemoriaReal[i].proceso.estado == "Bloqueado")
                    {
                        MR[i].BackColor = Color.MediumPurple;
                    }
                    else if (MemoriaReal[i].proceso.estado == "Ejecucion")
                    {
                        MR[i].BackColor = Color.OrangeRed;
                    }
                    else if (MemoriaReal[i].proceso.estado == "Apartado")
                    {
                        MR[i].BackColor = Color.DarkGreen;
                        MR[i].Text = "Apartado";
                    }
                    else
                    {

                    }
                }
            }

            for (int i = 0; i < 36; i++)
            {
                MV[i].BackColor = Color.White;
                MV[i].ForeColor = Color.Black;
                MV[i].Text = "null";
                if (MemoriaVirtual[i].proceso != null)
                {
                    MV[i].Text = "P:" + MemoriaVirtual[i].proceso.NumeroDePrograma + "\n" + MemoriaVirtual[i].memoria + "/5";

                    if (MemoriaVirtual[i].proceso.estado == "Sistema Operativo")
                    {
                        MV[i].ForeColor = Color.White;
                        MV[i].BackColor = Color.Black;
                        MV[i].Text = "S.O.";
                    }
                    else if (MemoriaVirtual[i].proceso.estado == "Listo")
                    {
                        MV[i].BackColor = Color.Aquamarine;
                    }
                    else if (MemoriaVirtual[i].proceso.estado == "Bloqueado")
                    {
                        MV[i].BackColor = Color.MediumPurple;
                    }
                    else if (MemoriaVirtual[i].proceso.estado == "Ejecucion")
                    {
                        MV[i].BackColor = Color.OrangeRed;
                    }
                    else if (MemoriaVirtual[i].proceso.estado == "Apartado")
                    {
                        MV[i].BackColor = Color.DarkGreen;
                        MV[i].Text = "Apartado";
                    }
                }
            }

            
            
            if (Ejecucion!=null)
            {
                listBoxProcesoActual.Items.Clear();
                listBoxProcesoActual.Items.Add("N. programa:" + Ejecucion.NumeroDePrograma.ToString());
                listBoxProcesoActual.Items.Add("Operacion:" + Ejecucion.Operacion.ToString());
                listBoxProcesoActual.Items.Add("TME:" + Ejecucion.TiempoMaximoEstimado.ToString());
                listBoxProcesoActual.Items.Add("TT:" + Ejecucion.TiempoTranscurrido.ToString());
                listBoxProcesoActual.Items.Add("TR:" + Ejecucion.TiempoRestante.ToString());
                listBoxProcesoActual.Items.Add("Tam:" + Ejecucion.tamaño.ToString());
                listBoxProcesoActual.Items.Add("FRs:" + Ejecucion.nf.ToString());
            }
            foreach (Proceso p in Bloqueados)
            {
                listViewBloqueados.Items.Add(p.NumeroDePrograma.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.Operacion);
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.TiempoRestanteBloqueado.ToString());
                listViewBloqueados.Items[listViewBloqueados.Items.Count - 1].SubItems.Add(p.nf.ToString());
            }
            foreach (Proceso p in Listos)
            {
                listViewListos.Items.Add(p.NumeroDePrograma.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.Operacion);
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoMaximoEstimado.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoTranscurrido.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.TiempoRestante.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.tamaño.ToString());
                listViewListos.Items[listViewListos.Items.Count - 1].SubItems.Add(p.nf.ToString());

            }

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 500;
            Expulsar();
            
            while (teclas.Count > 0)
            {
                Teclazos(teclas.Dequeue());
            }
            LlenarFrames();

            
            cargarListas();

            
            if (ProcesosEnMemoria()!=0 || Nuevos.Count > 0 ||NumSuspendidos>0)
            {
                if (Ejecucion == null)
                {
                    if (Listos.Count >= 1)
                    {
                        Ejecucion = Listos.Dequeue();
                        Ejecucion.Respuesta();
                        Ejecucion.estado = "Ejecucion";
                        TomarApartados();
                        ReiniciaQuantum();
                    }
                    
                }
                
                if (Ejecucion != null)
                {
                    Ejecucion.TiempoRestante--;
                    Ejecucion.TiempoTranscurrido++;
                    if (Ejecucion.TiempoRestante == 0 || Ejecucion.error)
                    {
                        Terminado(Ejecucion);
                        LimpiarFrames(Ejecucion);
                        Ejecucion = null; 
                    }
                }


                foreach (Proceso p in Bloqueados)
                {
                    if (p.TiempoRestanteBloqueado > 0)
                    {
                        p.TiempoRestanteBloqueado--;
                        p.TiempoEspera++;
                        //p.TiempoEspera++;
                    }
                    else
                    {
                        p.estado = "Listo";
                        Listos.Enqueue(p);
                    }
                }
                foreach (Proceso p in Listos)
                {
                    if (Bloqueados.Contains(p))
                    {
                        Bloqueados.Remove(p);
                    }
                    else
                    {
                        p.TiempoEspera++;
                    }

                }
                //
                Tiempo++;
                //Expulsar();

               
                cargarListas();
                timer1.Start();
            }
        }

        private void FormProcesamientoDeLotes_Load(object sender, EventArgs e)
        {
            if (Anterior != null)
            {
                Anterior.Visible = false;
            }
        }

        private void Teclazos(char tecla)
        {

            if (tecla == 'i' || tecla == 'I')
            {
                if (!pausa)
                {
                    buttonInt.BackColor = Color.Green;
                    timer3.Start();

                    Interrupcion();
                }
            }
            else if (tecla == 'e' || tecla == 'E')
            {
                if (!pausa)
                {
                    buttonError.BackColor = Color.Red;
                    timer3.Start();
                    Error();
                }
            }
            else if (tecla == 'n' || tecla == 'N')
            {
                if (!pausa)
                {
                    buttonNuevo.BackColor = Color.Green;
                    timer3.Start();
                    Nuevos.Enqueue(FI.NuevoProceso(rnd, ultimo + 1));
                    ultimo++;
                }
            }
            else if (tecla == 't' || tecla == 'T')
            {
                timer1.Stop();
                buttonTabla.BackColor = Color.Green;
                pausa = true;

                FormTablaProcesos FT = new FormTablaProcesos(Tiempo, Ejecucion, Nuevos, Listos, Bloqueados, Terminados);
                FT.ShowDialog();
                buttonTabla.BackColor = Color.White;
                timer1.Start();
                pausa = false;
            }
            else if (tecla == 's' || tecla == 'S')
            {
                if (!pausa)
                {
                    buttonSus.BackColor = Color.Red;
                    timer3.Start();
                    Suspender();
                }
            }
            else if (tecla == 'r' || tecla == 'R')
            {
                if (!pausa)
                {
                    buttonReg.BackColor = Color.Green;
                    timer3.Start();
                    Recuperar();
                }
            }
            else if (tecla == 'u' || tecla == 'U')
            {
                if (!pausa)
                {
                    buttonVir.BackColor = Color.Green;
                    timer3.Start();
                    EnviarAVirtual();
                }
            }
        }

        private void FormProcesamientoDeLotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            char tecla = e.KeyChar;
            teclas.Enqueue(tecla);

            if (tecla == 'p' || tecla == 'P')
            {
                timer1.Stop();
                buttonPausa.BackColor = Color.Green;
                pausa = true;
            }
            else if (button1.Enabled==false && ProcesosEnMemoria()>0)
            {
                
                if (tecla == 'm' || tecla == 'M')
                {
                    timer1.Stop();
                    buttonTF.BackColor = Color.Green;
                    pausa = true;
                }
                else if (tecla == 'c' || tecla == 'C')
                {
                    if (pausa)
                    {
                        buttonContinuar.BackColor = Color.Aquamarine;
                        buttonPausa.BackColor = Color.White;
                        timer3.Start();
                        timer1.Start();
                        pausa = false;

                    }
                }
                
            }                       
        }

        private void FormProcesamientoDeLotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            Anterior.Close();
        }

        private void Terminado(Proceso p)
        {
            /////////////////////////////////////////////////////////////////////////////////////////
            RegresarApartados();

            ReiniciaQuantum();
            Terminados.Add(p);
            Tiempo=p.Finaliza(Tiempo);
            p.obtenerTiempos();
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
            
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoLlegada.ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoRespuesta.ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoFinalización.ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoServicio.ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoEspera.ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.TiempoRetorno.ToString());

            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.comp1().ToString());
            listViewResultados.Items[listViewResultados.Items.Count - 1].SubItems.Add(p.comp2().ToString());

        }

        private void Interrupcion()
        {
            Proceso p;
            if (Ejecucion!=null)
            {
                if (Ejecucion.TiempoRestante > 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////
                    RegresarApartados();

                    p = Ejecucion;
                    p.TiempoRestanteBloqueado = 10;
                    p.estado = "Bloqueado";
                    Bloqueados.Add(p);
                    Ejecucion = null;
   
                }
            }
        }

        private void Error()
        {
            if (Ejecucion!=null)
            {
                Ejecucion.error = true;
            }  
        }

        private int ProcesosEnMemoria()
        {
            int procesos = Bloqueados.Count + Listos.Count;
            if (Ejecucion != null)
            {
                procesos++;
            }

            return procesos;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            buttonInt.BackColor = Color.White;
            buttonError.BackColor = Color.White;
            buttonContinuar.BackColor = Color.White;
            buttonNuevo.BackColor = Color.White;
            buttonTF.BackColor = Color.White;
            buttonSus.BackColor = Color.White;
            buttonReg.BackColor = Color.White;
            buttonVir.BackColor = Color.White;
            timer3.Stop();
            this.KeyPreview = true;
        }

        private void Expulsar()
        {
            if (AuxQ>1)
            {
                AuxQ--;
            }
            else
            {
                if (Listos.Count > 0 && Ejecucion!=null)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////
                    RegresarApartados();

                    Listos.Enqueue(Ejecucion);
                    Ejecucion.estado = "Listo";
                    Ejecucion = null;
                    AuxQ = quantum;
                }  
            }
        }

        private void ReiniciaQuantum()
        {
            AuxQ = quantum;
        }

        private void listViewBloqueados_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private int FramesDisponibles(List<Frame> M)
        {
            int disp = 0;
            foreach (Frame f in M)
            {
                if (f.proceso == null)
                {
                    disp++;
                }
            }
            return disp;
        }

        private bool AsignarFrames(Proceso p)
        {
            int aux = p.tamaño;

            int fs = p.nf;
            if (FramesDisponibles(MemoriaReal)>=fs)
            {
                foreach(Frame f in MemoriaReal)
                {
                    if (f.proceso==null)
                    {
                        if(aux >= 5)
                        {
                            p.Frames.Add(f);
                            p.FramesReal.Add(f);
                            f.proceso = p;
                            f.memoria = 5;
                            
                            aux = aux - 5;
                        }
                        else if(aux>0)
                        {
                            p.Frames.Add(f);
                            p.FramesReal.Add(f);
                            f.proceso = p;
                            f.memoria = aux;
                            aux = 0;
                            //break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private void LlenarFrames()
        {
            while (true)
            {
                if (Nuevos.Siguiente() != null)
                {
                    if (Nuevos.Siguiente().nf <= FramesDisponibles(MemoriaReal))
                    {
                        Proceso p = Nuevos.Dequeue();
                        AsignarFrames(p);
                        Listos.Enqueue(p);
                        p.estado = "Listo";
                        p.TiempoLlegada = Tiempo;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void LimpiarFrames(Proceso p)
        {
            for(int i=p.Frames.Count; i > 0; i--)
            {
                p.Frames[i-1].proceso = null;
            }
        }

        private void ArchivoSuspendidos(Proceso p)
        {
            StreamWriter writer = new StreamWriter(arcSus, append: true);
            
            writer.WriteLine(p.NumeroDePrograma + "|" + p.Operacion + "|" + p.Resultado + "|" +
                p.TiempoMaximoEstimado + "|" + p.TiempoTranscurrido + "|" + p.TiempoRestante + "|" +
                p.TiempoLlegada + "|" + p.TiempoEspera + "|" + p.TiempoRespuesta + "|" + p.tamaño + "|" +
                p.nf+"|"+Tiempo+"|"+p.TiempoRestanteBloqueado+"|");

            writer.Close();
        }

        public Proceso RecuperarSuspendido()
        {
            StreamReader reader = new StreamReader(arcSus);
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

            }
            reader.Close();
            return p;
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
            int i=0;
            while (salto>0)
            {
                if (cad[i]=='|')
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

        private void Suspender()
        {
            if (Bloqueados.Count>0)
            {
                Proceso p = Bloqueados[0];
                LimpiarFrames(p);
                Bloqueados.Remove(Bloqueados[0]);
                ArchivoSuspendidos(p);
                NumSuspendidos++;
            }
        }

        private void Recuperar()
        {
            if (NumSuspendidos > 0)
            {
                Proceso p = RecuperarSuspendido();
                if (FramesDisponibles(MemoriaReal)>=p.nf)
                {
                    AsignarFrames(p);
                    Bloqueados.Add(p);
                    p.estado = "Bloqueado";
                    NumSuspendidos--;

                    StreamReader reader = new StreamReader(arcSus);
                    String res = reader.ReadLine();
                    res = reader.ReadToEnd();
                    reader.Close();
                    var file = new FileStream(arcSus, FileMode.Create);
                    file.Close();
                    StreamWriter writer = new StreamWriter(arcSus);
                    writer.Write(res);
                    writer.Close();
                }
            }
        }
       
        private void EnviarAVirtual()
        {
            foreach(Proceso p in Listos)
            {
                if (p.FramesReal.Count>1)
                {
                    if (FramesDisponibles(MemoriaVirtual)>0)
                    {
                        Frame f = new Frame(); 
                        f = p.FramesReal[p.FramesReal.Count - 1];
                        

                        for (int i = 0; MemoriaVirtual.Count > i; i++)
                        {
                            if (MemoriaVirtual[i].proceso==null)
                            {
                                MemoriaVirtual[i]= f;
                                break;
                            }
                        }
                        for (int i = 0; MemoriaReal.Count > i; i++)
                        {
                            if (MemoriaReal[i] == f)
                            {
                                MemoriaReal[i]=new Frame();
                                break;
                            }
                        }

                        p.FramesVirtual.Add(f);
                        p.FramesReal.Remove(f);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (Proceso p in Bloqueados)
            {
                if (p.FramesReal.Count > 1)
                {
                    if (FramesDisponibles(MemoriaVirtual) > 0)
                    {
                        Frame f = new Frame();
                        f = p.FramesReal[p.FramesReal.Count - 1];



                        for (int i = 0; MemoriaVirtual.Count > i; i++)
                        {
                            if (MemoriaVirtual[i].proceso == null)
                            {
                                MemoriaVirtual[i] = f;
                                break;
                            }
                        }
                        for (int i = 0; MemoriaReal.Count > i; i++)
                        {
                            if (MemoriaReal[i] == f)
                            {
                                MemoriaReal[i] = new Frame();
                                break;
                            }
                        }

                        p.FramesVirtual.Add(f);
                        p.FramesReal.Remove(f);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (ApartadosNecesariosReal() > ApartadosExistentesReal())
            {
                foreach (Frame f in MemoriaReal)
                {
                    if (f.proceso == null)
                    {
                        f.proceso = new Proceso("", 0, 0, 0, 0);
                        f.proceso.estado = "Apartado";
                        break;
                    }
                }
            }
            
        }

        private int ApartadosNecesariosReal()
        {
            int apartados = 0;
            foreach (Proceso p in Listos)
            {
                if (p.FramesVirtual.Count > apartados)
                {
                    apartados = p.FramesVirtual.Count;
                }
            }
            foreach (Proceso p in Bloqueados)
            {
                if (p.FramesVirtual.Count > apartados)
                {
                    apartados = p.FramesVirtual.Count;
                }
            }
            if (Ejecucion != null)
            {
                if (Ejecucion.FramesVirtual.Count > apartados && Ejecucion.error == false && Ejecucion.TiempoRestante>0)
                {
                    apartados = Ejecucion.FramesVirtual.Count;
                }
            }
            return apartados;
        }

        private int ApartadosExistentesReal()
        {
            int apartados = 0;
            foreach (Frame f in MemoriaReal)
            {
                if(f.proceso!=null)
                if (f.proceso.estado=="Apartado")
                {
                    apartados++;
                }
            }
            foreach (Frame f in MemoriaVirtual)
            {
                if (f.proceso != null)
                    if (f.proceso.estado == "Apartado")
                    {
                        apartados++;
                    }
            }
            return apartados;
        }

        private void intercambio(Frame f1, Frame f2)
        {
            Proceso p = f1.proceso;
            int t = f1.memoria;

            f1.proceso= f2.proceso;
            f1.memoria = f2.memoria;

            f2.proceso = p;
            f2.memoria = t;
        }

        private void TomarApartados()
        {
            if (Ejecucion != null)
            {
                if (Ejecucion.FramesVirtual.Count > 0)
                {
                    foreach (Frame f in Ejecucion.FramesVirtual)
                    {

                        foreach (Frame f2 in MemoriaReal)
                        {
                            if (f2.proceso != null)
                            {
                                if (f2.proceso.estado == "Apartado")
                                {
                                    cambiados.Enqueue(f);
                                    cambiados.Enqueue(f2);
                                    intercambio(f, f2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RegresarApartados()
        {
            while (cambiados.Count>0)
            {
                Frame f1 = cambiados.Dequeue();
                Frame f2 = cambiados.Dequeue();
                intercambio(f1,f2);

            }

            if (ApartadosNecesariosReal() < ApartadosExistentesReal())
            {
                int quitar = ApartadosExistentesReal() - ApartadosNecesariosReal();
                for (int i = MemoriaReal.Count - 1; i > 0; i--)
                {
                    if (quitar > 0)
                    {
                        if(MemoriaReal[i].proceso!=null)
                        if (MemoriaReal[i].proceso.estado == "Apartado")
                        {
                            MemoriaReal[i].proceso = null;
                                quitar--;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
