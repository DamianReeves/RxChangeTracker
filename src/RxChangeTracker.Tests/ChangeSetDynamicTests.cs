using FluentAssertions;
using NUnit.Framework;

namespace RxChangeTracker {
    public class ChangeSetDynamicTests {
        [Test]
        public void ChangeSet_0th_Level_Properties_Can_Be_Set() {
            dynamic changeSet = new ChangeSet();
            Assert.DoesNotThrow(() => { changeSet.Name = "Damian"; });            
        }

        [Test]
        public void ChangeSet_0th_Level_Properties_Can_Be_Set_And_Read() {
            dynamic changeSet = new ChangeSet();
            changeSet.Name = "Damian";
            string name = changeSet.Name;
            name.Should().Be("Damian");
        }        

        [Test]
        public void ChangeSet_1st_Level_Properties_Can_Be_Set() {
            dynamic changeSet = new ChangeSet();
            Assert.DoesNotThrow(() => { changeSet.Address.State = "New York"; });
        }

        [Test]
        public void ChangeSet_1st_Level_Properties_Can_Be_Read() {
            dynamic changeSet = new ChangeSet();
            changeSet.Address.State = "New York";
            string state = changeSet.Address.State;
            state.Should().Be("New York");
        }
    }
}