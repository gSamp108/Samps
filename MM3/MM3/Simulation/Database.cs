using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class Database
    {
        public class Member
        {
            public int DatabaseId { get; private set; }
            public Database Database { get; private set; }

            public Member(Database database)
            {
                this.Database = database;
                this.DatabaseId = this.Database.RegisterNewMember(this);
            }
        }

        private int nextId = 0;
        private Queue<int> unusedIds = new Queue<int>();
        private Dictionary<int, Member> members = new Dictionary<int, Member>();
        private bool enumeratorLock = false;
        private Queue<Member> enumeratorLockQueue = new Queue<Member>();

        public int RegisterNewMember(Member member)
        {
            var id = this.GetNewMemberId();
            if (!this.enumeratorLock) this.members.Add(id, member);
            else this.enumeratorLockQueue.Enqueue(member);
            return id;
        }

        private int GetNewMemberId()
        {
            if (this.unusedIds.Count > 0) return this.unusedIds.Dequeue();
            else return (this.nextId++);
        }

        public IEnumerable<Member> Members
        {
            get
            {
                this.enumeratorLock = true;
                foreach(var member in this.members.Values)
                {
                    yield return member;
                }
                this.enumeratorLock = false;
                while (this.enumeratorLockQueue.Count>0)
                {
                    var member = this.enumeratorLockQueue.Dequeue();
                    this.members.Add(member.DatabaseId, member);
                }
            }
        }
    }
}
