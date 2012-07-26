using System;

namespace RxChangeTracker {
    internal class TrackedReference {
        private readonly WeakReference _reference;

        public TrackedReference(object target) {
            if (target == null) throw new ArgumentNullException("target");
            this._reference = new WeakReference(target);
        }

        public bool IsAlive { get { return this._reference.IsAlive; } }
        public object Target { get { return this._reference.Target; } }

        public bool TryGetTarget<TTarget>(out TTarget target) {
            if (this.IsAlive) {
                var obj = this._reference.Target;
                if (obj is TTarget) {
                    target = (TTarget)obj;
                    return true;
                }
            }
            target = default(TTarget);
            return false;
        }
    }
}