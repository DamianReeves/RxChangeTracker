namespace RxChangeTracker.Testing {
    public class RootObject : NotifyingObjectBase {
        public ComplexObject Child { get; set; }
        public SimpleObject SimpleChild { get; set; }

        public static RootObject CreateSample() {
            return new RootObject {
                Child = new ComplexObject { Name = "Child" },
                SimpleChild = new SimpleObject { Name = "Leaf" },
            };
        }
    }
}