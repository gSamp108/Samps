using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public abstract class IdMemeber
    {
        public int Id { get; private set; }
        public IdStore IdStore { get; private set; }

        public IdMemeber(IdStore idStore)
        {
            this.IdStore = idStore;
            this.Id = this.IdStore.Add(this);
        }
    }
}
