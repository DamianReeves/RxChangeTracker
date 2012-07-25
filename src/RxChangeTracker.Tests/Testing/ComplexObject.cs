namespace RxChangeTracker.Testing {
    public class ComplexObject : NotifyingObjectBase {
        public string Name { get; set; }
        public ComplexObject ComplexChild { get; set; }
    }
}