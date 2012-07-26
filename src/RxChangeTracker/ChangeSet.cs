using System.Collections.Generic;
using System.Dynamic;

namespace RxChangeTracker {
    public class ChangeSet: DynamicObject {
        private readonly Dictionary<string, ChangeSetProperty> _properties = new Dictionary<string, ChangeSetProperty>();

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            var propertyName = binder.Name;
            ChangeSetProperty property;
            if(_properties.TryGetValue(propertyName, out property)) {
                result = property.Value;
                return true;
            }
            property = new ChangeSetProperty();
            this._properties[propertyName] = property;
            result = property;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            var propertyName = binder.Name;
            ChangeSetProperty property;
            if(_properties.TryGetValue(propertyName, out property)) {
                property.Value = value;
            }else {
                property = new ChangeSetProperty { Value = value };
                _properties[propertyName] = property;
            }
            return true;
        }
    }

    public class ChangeSetProperty:DynamicObject {
        private readonly Dictionary<string, ChangeSetProperty> _properties = new Dictionary<string, ChangeSetProperty>();
        internal object Value { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            var propertyName = binder.Name;
            ChangeSetProperty property;
            if (_properties.TryGetValue(propertyName, out property)) {
                result = property.Value;
                return true;
            }
            property = new ChangeSetProperty();
            this._properties[propertyName] = property;
            result = property;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            var propertyName = binder.Name;
            ChangeSetProperty property;
            if (_properties.TryGetValue(propertyName, out property)) {
                property.Value = value;
            } else {
                property = new ChangeSetProperty { Value = value };
                _properties[propertyName] = property;
            }
            return true;
        }
    }
}