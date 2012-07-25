using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

namespace RxChangeTracker {
    public class ChangeTracker: IChangeTracking, IEnumerable<object> {
        readonly ISubject<IChangeTrackingInfo> _tracked = new ReplaySubject<IChangeTrackingInfo>();
        private readonly ConditionalWeakTable<object,IChangeTrackingInfo> _targetMap = new ConditionalWeakTable<object, IChangeTrackingInfo>();
        public ChangeTracker() {
            this.Initialize();
        }        
        public bool IsChanged { get; set; }

        public IObservable<object> Tracked { get; private set; }

        public void Add(object target) {
            //dynamic dynamicTarget = target;
            //this.Track(dynamicTarget);
            this.Track(target);
        }

        public void AcceptChanges() {
            throw new NotImplementedException();
        }

        public void Track(object target) {
            var ctTarget = this.CreateChangeTrackingTarget(target);
            _tracked.OnNext(ctTarget);
        }

        protected virtual IChangeTrackingInfo CreateChangeTrackingTarget(object target) {
            return ChangeTrackingInfo.Create(target);
        }

        private void Initialize() {
            //_targetMap.
            IObservable<IChangeTrackingInfo> trackedTargets = _tracked;
            IObservable<object> tracked = 
                from trackingTarget in trackedTargets
                where trackingTarget.IsAlive
                select trackingTarget.GetTarget();
            tracked = tracked.Where(x => x != null).Distinct();
            Tracked = tracked;
        }

        public IEnumerator<object> GetEnumerator() {
            return _tracked.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
