using System;
using System.Diagnostics.Contracts;

using RxChangeTracker.Core;

namespace RxChangeTracker {
    internal class ChangeTrackingInfo<TTarget>: IChangeTrackingInfo<TTarget> where TTarget:class {
        //private readonly WeakReference<TTarget> _reference;
        private readonly WeakReference _reference;
        public ChangeTrackingInfo(TTarget target) {
            if (target == null) throw new ArgumentNullException("target");
            Contract.EndContractBlock();

            this._reference = new WeakReference(target);
        }

        public bool IsAlive { get { return this._reference.IsAlive; } }

        public TTarget GetTarget() {
            TTarget target;
            if(_reference.IsAlive) {
                return (TTarget)_reference.Target;
            }
            return null;
        }

        object IChangeTrackingInfo.GetTarget() {
            return this.GetTarget();
        }
    }

    internal static class ChangeTrackingInfo {
        public static IChangeTrackingInfo Create<TTarget>(TTarget target) where  TTarget: class {
            if (target == null) throw new ArgumentNullException("target");
            Contract.EndContractBlock();

            return new ChangeTrackingInfo<TTarget>(target);
        }
    }


    public interface IChangeTrackingInfo {
        bool IsAlive { get; }
        object GetTarget();
    }

    public interface IChangeTrackingInfo<out TTarget> : IChangeTrackingInfo {
        new TTarget GetTarget();
    }
}