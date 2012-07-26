using System.Collections;
using System.Collections.Generic;

namespace RxChangeTracker {
    internal class ChangeTrackerTable : IEnumerable<object> {
        private readonly LinkedList<TrackedReference> _targets = new LinkedList<TrackedReference>();
        private readonly System.Threading.ReaderWriterLockSlim _targetsLock = new System.Threading.ReaderWriterLockSlim();

        public void Add(object target) {            
            var trackedTarget = new TrackedReference(target);
            //TODO: Add logic for ensuring the same item was not added twice
            //Eventually we will need a lookup record as well.
            _targetsLock.EnterWriteLock();
            try {
                this._targets.AddLast(trackedTarget);
            } finally {
              _targetsLock.ExitWriteLock();  
            }            
        }

        public IEnumerable<object> GetActiveTargets() {
            return this.GetActiveTargets<object>();
        }

        public IEnumerable<TTarget> GetActiveTargets<TTarget>() {
            _targetsLock.EnterUpgradeableReadLock();
            try {
                var current = this._targets.First;
                while (current != null) {
                    var trackingTarget = current.Value;
                    TTarget target;
                    if (trackingTarget.TryGetTarget(out target)) {
                        //The target is alive (so return it)
                        yield return target;
                    } else {
                        //The target is not alive so remove it
                        _targetsLock.EnterWriteLock();
                        _targets.Remove(trackingTarget);
                        _targetsLock.ExitWriteLock();
                    }
                    current = current.Next;
                }
            }finally {
                _targetsLock.ExitUpgradeableReadLock();
            }
        }

        public IEnumerator<object> GetEnumerator() {
            return this.GetActiveTargets().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}