using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class IdStore<type> : IEnumerable<type> where type : DataStore.Member
    {
        public DataStore DataStore { get; private set; }

        private HashSet<int> memberIds = new HashSet<int>();

        public IdStore(DataStore dataStore)
        {
            this.DataStore = dataStore;
        }
        public void Add(int id)
        {
            this.memberIds.Add(id);
        }
        public void Remove(int id)
        {
            this.memberIds.Remove(id);
        }
        public type Get(int id)
        {
            return this.DataStore.Get<type>(id);
        }

        public IEnumerator<type> GetEnumerator()
        {
            foreach(var id in this.memberIds)
            {
                yield return this.DataStore.Get<type>(id);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var id in this.memberIds)
            {
                yield return this.DataStore.Get<type>(id);
            }
        }
    }
}

