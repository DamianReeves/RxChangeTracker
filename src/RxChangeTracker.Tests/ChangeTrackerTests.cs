using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using FluentAssertions;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using RxChangeTracker.Testing;

namespace RxChangeTracker {

    public class ChangeTrackerTests: ReactiveTest {

        [Test]
        public void Object_Is_Tracked_When_Added_To_ChangeTracker() {
            var simple = new SimpleObject();

            var changeTracker = new ChangeTracker { simple };
            var tracked = changeTracker.GetTracked();
            tracked.Should().Equal(simple); 
        }        

        [Test]
        public void ChangeTracker_IsChanged_When_Monitored_Object_Property_Is_Changed() {
            var simple = new SimpleObject{Name="Original"};
            var changeTracker = new ChangeTracker { simple };
            simple.Name = "New";
            changeTracker.ShouldHave().Properties(c=>c.IsChanged).EqualTo(new {IsChanged=true});
        }
    }

    public class TestSchedulerUsageFixture {
        [Test]
        public void TestScheduler_Usage() {
            var scheduler = new TestScheduler();
            var wasExecuted = false;
            scheduler.Schedule(() => wasExecuted = true);
            Assert.IsFalse(wasExecuted);
            scheduler.AdvanceBy(1); //execute 1 tick of queued actions
            Assert.IsTrue(wasExecuted);
        }

        [Test]
        public void TestScheduler_Usage2() {
            var expectedValues = new long[] { 0, 1, 2, 3, 4 };
            var actualValues = new List<long>();
            var scheduler = new TestScheduler();
            var interval = Observable
            .Interval(TimeSpan.FromSeconds(1), scheduler)
            .Take(5);
            interval.Subscribe(actualValues.Add);
            scheduler.Start();
            CollectionAssert.AreEqual(expectedValues, actualValues);
            //Executes in less than 0.01s "on my machine"
        }
    }
}
