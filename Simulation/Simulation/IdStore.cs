using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public  class IdStore
    {
        private Dictionary<int, IdMemeber> members = new Dictionary<int, IdMemeber>();
        private int nextId;
        private Queue<int> disposedIds = new Queue<int>();

        public int Add(IdMemeber memeber)
        {
            var id = this.nextId;
            if (this.disposedIds.Count > 0) id = this.disposedIds.Dequeue();
            else this.nextId += 1;
            this.members.Add(id, memeber);
            return id;
        }
        public void Remove(IdMemeber member)
        {
            this.Remove(member.Id);
        }
        public void Remove (int id)
        {
            this.disposedIds.Enqueue(id);
            this.members.Remove(id);
        }
        public IdMemeber Get(int id)
        {
            if (this.members.ContainsKey(id)) return this.members[id];
            return null;
        }
        public type Get<type>(int id) where type : IdMemeber 
        {
            if (this.members.ContainsKey(id)) return (type)this.members[id];
            return null;
        }
        public IEnumerable<type> All<type>() where type : IdMemeber
        {
            foreach(var member in this.members.Values)
            {
                yield return (type)member;
            }
        }
        public int Count { get { return this.members.Count; } }
    }
}
