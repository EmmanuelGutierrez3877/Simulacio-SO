using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulacion_SO
{
    public partial class FormInicio : Form
    {
        //List<Lote> ListaLotes = new List<Lote>();
        Cola Nuevos = new Cola();

        public FormInicio()
        {
            InitializeComponent();
        }

        private void buttonContinuar_Click(object sender, EventArgs e)
        {
            if (numeric1.Value > 0)
            {
                if (numericQuantum.Value > 0)
                {
                    Random rnd = new Random();
                    for (int i=1;i<=numeric1.Value;i++)
                    {

                        Nuevos.Enqueue(NuevoProceso(rnd,i));
                   
                    }
                    FormProcesamientoDeLotes FPL = new FormProcesamientoDeLotes(this, Nuevos, (int)numeric1.Value, rnd, (int)numericQuantum.Value);
                    FPL.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Quantum minimo de 1");
                }
            }
            else
            {
                MessageBox.Show("debes crear minimo un proceso");
            }
        }

        public Proceso NuevoProceso(Random rnd, int i)
        {
            int tme = rnd.Next(7, 18);

            int n1 = rnd.Next(1, 100);
            int op = rnd.Next(1, 6);
            int n2 = rnd.Next(1, 100);
            double res = 0;

            String operacion = n1.ToString();
            if (op == 1)
            {
                operacion += " + " + n2;
                res = n1 + n2;
            }
            else if (op == 2)
            {
                operacion += " - " + n2;
                res = n1 - n2;
            }
            else if (op == 3)
            {
                operacion += " * " + n2;
                res = n1 * n2;
            }
            else if (op == 4)
            {
                operacion += " / " + n2;
                res = (double)n1 / n2;
            }
            else if (op == 5)
            {
                operacion += " % " + n2;
                res = (double)n1 % n2;
            }

            Proceso p = new Proceso(operacion, tme, i, res, rnd.Next(6,30));
            /*
            if (ListaLotes[ListaLotes.Count-1].Count < 3)
            {
                ListaLotes[ListaLotes.Count - 1].Add(p);
            }
            else
            {
                Lote l = new Lote();
                l.Add(p);
                ListaLotes.Add(l);
            }
            */
            return p;
        }

        private void FormInicio_Load(object sender, EventArgs e)
        {

        }

        
    }
}
