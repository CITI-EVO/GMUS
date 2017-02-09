using System;

namespace CITI.EVO.Tools.Helpers
{
    public class BulkCopyCallback
    {
        private readonly Predicate<BulkCopyCallback> predicate;

        public BulkCopyCallback(Predicate<BulkCopyCallback> predicate, Object state)
            : this(null, 0, predicate, state)
        {
        }

        public BulkCopyCallback(long count, Predicate<BulkCopyCallback> predicate, Object state)
            : this(null, count, predicate, state)
        {
        }

        public BulkCopyCallback(String name, long count, Predicate<BulkCopyCallback> predicate, Object state)
        {
            Name = name;
            Count = count;
            State = state;

            this.predicate = predicate;
        }

        public String Name { get; set; }
        public long Count { get; set; }

        public Object State { get; set; }

        public bool? DoCallback()
        {
            if (predicate != null)
            {
                return predicate(this);
            }

            return null;
        }
    }
}
