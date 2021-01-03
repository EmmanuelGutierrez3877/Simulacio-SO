using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_SO
{
    public class Cola : List<Proceso>
    {
        public void Enqueue(Proceso p)
        {
            this.Add(p);
        }

        public Proceso Dequeue()
        {
            if (Count > 0)
            {
                Proceso p = this[0];
                this.Remove(p);
                return p;
            }
            return null;
        }

        public Proceso Siguiente()
        {
            if (Count>0)
            {
                return this[0];
            }
            return null;
        }


    }

    
}
