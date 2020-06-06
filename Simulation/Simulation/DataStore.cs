using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public sealed class DataStore
    {
        public abstract class Member
        {
            public int Id { get; private set; }
            public DataStore DataStore { get; private set; }

            public Member(DataStore dataStore)
            {
                this.DataStore = dataStore;
                this.Id = this.DataStore.Add(this);
            }
        }

        private Dictionary<int, Member> members = new Dictionary<int, Member>();
        private int nextId;
        private Queue<int> disposedIds = new Queue<int>();

        public int Add(Member memeber)
        {
            var id = this.nextId;
            if (this.disposedIds.Count > 0) id = this.disposedIds.Dequeue();
            else this.nextId += 1;
            this.members.Add(id, memeber);
            return id;
        }
        public void Remove(Member member)
        {
            this.Remove(member.Id);
        }
        public void Remove(int id)
        {
            this.disposedIds.Enqueue(id);
            this.members.Remove(id);
        }
        public Member Get(int id)
        {
            if (this.members.ContainsKey(id)) return this.members[id];
            return null;
        }
        public type Get<type>(int id) where type : Member
        {
            if (this.members.ContainsKey(id)) return (type)this.members[id];
            return null;
        }
        public IEnumerable<type> All<type>() where type : Member
        {
            foreach (var member in this.members.Values)
            {
                yield return (type)member;
            }
        }
        public int Count { get { return this.members.Count; } }
    }
}
