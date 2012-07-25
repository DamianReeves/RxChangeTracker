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
            
            var changeTracker = new ChangeTracker();
            var tracked = new List<object>();

            changeTracker.Tracked.SubscribeOn(Scheduler.CurrentThread).Subscribe(
                tracked.Add,
                ex => Assert.Fail(ex.Message),
                () => { 
                    tracked.Should().Equal(simple); 
                    Console.WriteLine("Executed");
                }
            );
            changeTracker.Add(simple);            

        }

        [Test]
        public void ChangeTracker_IsChanged_When_Monitored_Object_Property_Is_Changed() {
            var simple = new SimpleObject{Name="Original"};
            var changeTracker = new ChangeTracker { simple };
            simple.Name = "New";
            changeTracker.ShouldHave().Properties(c=>c.IsChanged).EqualTo(new {IsChanged=true});
        }
    }
}
