using System;

namespace RxChangeTracker {
    public class SnapshotProperty<TValue>:Property<TValue> {
        public SnapshotProperty(string name): base(name) {}

        public IObservable<TValue> Values { get; private set; }
    }
}