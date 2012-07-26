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
        private readonly ChangeTrackerTable _tracked = new ChangeTrackerTable();
        //private readonly ConditionalWeakTable<object,IChangeTrackingInfo> _targetMap = new ConditionalWeakTable<object, IChangeTrackingInfo>();
        public ChangeTracker() {}        

        public bool IsChanged { get; set; }

        public void Add(object target) {
            //dynamic dynamicTarget = target;
            //this.Track(dynamicTarget);
            this.Track(target);
        }

        public void AcceptChanges() {
            throw new NotImplementedException();
        }

        public void Track(object target) {
            _tracked.Add(target);
        }

        public IEnumerable<object> GetTracked() {
            return _tracked;
        }

        public IEnumerator<object> GetEnumerator() {
            return _tracked.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
