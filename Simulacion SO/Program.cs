using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulacion_SO
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            //List<Lote> ListaLotes = new List<Lote>();
            //Lote L = new Lote();
            //ListaLotes.Add(L);

            //FormInicio FP = new FormInicio();
            //FP.ShowDialog();

            Application.Run(new FormInicio());
        }
    }
}
