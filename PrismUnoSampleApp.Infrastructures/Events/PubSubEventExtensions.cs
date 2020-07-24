using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.Infrastructures.Events
{
    public static class PubSubEventExtensions
    {
        public static IObservable<TPayload> ToObservable<TPayload>(this PubSubEvent<TPayload> self) =>
            Observable.Create<TPayload>(ox =>
            {
                var token = self.Subscribe(x => ox.OnNext(x));
                return () => token.Dispose();
            });
        public static IObservable<Unit> ToObservable(this PubSubEvent self) =>
            Observable.Create<Unit>(ox =>
            {
                var token = self.Subscribe(() => ox.OnNext(Unit.Default));
                return () => token.Dispose();
            });
    }
}
