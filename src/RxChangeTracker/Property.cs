using System;
using System.Collections.Generic;

namespace RxChangeTracker {
    public class Property<TValue> {
        private readonly IEqualityComparer<TValue> _equalityComparer;
        private TValue _value;

        public event EventHandler ValueChanged;        

        public Property(string name):this(name, EqualityComparer<TValue>.Default) {}

        public Property(string name, IEqualityComparer<TValue> equalityComparer) {
            this.Name = name;
            _equalityComparer = equalityComparer;
        }

        public Property(string name, TValue initialValue, IEqualityComparer<TValue> equalityComparer) {
            this.Name = name;
            _value = initialValue;
            _equalityComparer = equalityComparer;
        }

        public string Name { get; private set; }

        public IEqualityComparer<TValue> EqualityComparer {
            get { return this._equalityComparer; } 
        }

        public TValue Value {
            get { return this._value; } 
            set { 
                if(!EqualityComparer<TValue>.Default.Equals(_value,value)) {
                    this._value = value; 
                }                
            }
        }

        public void OnValueChanged(EventArgs e) {
            EventHandler handler = this.ValueChanged;
            if (handler != null) handler(this, e);
        }
    }
}