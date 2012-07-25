using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace RxChangeTracker.Core {
    public sealed class WeakReference<T> : IDisposable where T : class {
        private volatile IntPtr _handle;
        private readonly GCHandleType _handleType;

        public WeakReference(T target)
            : this(target, false) {
        }

        [SecuritySafeCritical]
        public WeakReference(T target, bool trackResurrection) {
            if (target == null)
                throw new ArgumentNullException("target");
            this._handleType = trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak;
            this.Target = target;
        }

        [SecuritySafeCritical]
        ~WeakReference() {
            this.Dispose();
        }

        public void Dispose() {
            var ptr = this._handle;
            if ((ptr != IntPtr.Zero) &&
                Interlocked.CompareExchange(ref _handle, IntPtr.Zero, ptr) == ptr) {
                try {
                    var handle = GCHandle.FromIntPtr(ptr);
                    if (handle.IsAllocated)
                        handle.Free();
                } catch { }
            }
            GC.SuppressFinalize(this);
        }

        public bool TryGetTarget(out T target) {
            var ptr = this._handle;
            if (ptr != IntPtr.Zero) {
                try {
                    var handle = GCHandle.FromIntPtr(ptr);
                    if (handle.IsAllocated) {
                        target = (T)handle.Target;
                        return !object.ReferenceEquals(target, null);
                    }
                } catch { }
            }
            target = null;
            return false;
        }

        public bool TryGetTarget(out T target, Func<T> recreator) {
            IntPtr ptr = this._handle;
            try {
                var handle = GCHandle.FromIntPtr(ptr);
                if (handle.IsAllocated) {
                    target = (T)handle.Target;
                    if (!object.ReferenceEquals(target, null))
                        return false;
                }
            } catch { }

            T createdValue = null;
            target = null;

            while ((ptr = this._handle) == IntPtr.Zero || object.ReferenceEquals(target, null)) {
                createdValue = createdValue ?? recreator();
                var newPointer = GCHandle.Alloc(createdValue, this._handleType).AddrOfPinnedObject();
                if (Interlocked.CompareExchange(ref this._handle, newPointer, ptr) == ptr) {
                    target = createdValue;
                    return true;
                } else if ((ptr = this._handle) != IntPtr.Zero) {
                    try {
                        var handle = GCHandle.FromIntPtr(ptr);
                        if (handle.IsAllocated) {
                            target = (T)handle.Target;
                            if (!object.ReferenceEquals(target, null))
                                return false;
                        }
                    } catch { }
                }
            }

            return false;
        }

        public bool IsAlive {
            get {
                var ptr = this._handle;
                return ptr != IntPtr.Zero && GCHandle.FromIntPtr(ptr).IsAllocated;
            }
        }

        public T Target {
            get {
                T target;
                this.TryGetTarget(out target);
                return target;
            }
            set {
                this.Dispose();
                this._handle = GCHandle.Alloc(value, this._handleType).AddrOfPinnedObject();
                GC.ReRegisterForFinalize(this);
            }
        }
    }
}