using GraphQLPizzaOrder.Core.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace GraphQLPizzaOrder.Core.Services
{
    public interface IEventService
    {
        IObservable<EventDataModel> OnCreateObservable();

        void CreateOrderEvent(EventDataModel orderEvent);

        IObservable<EventDataModel> OnStatusUpdateObservable();

        void StatusUpdateEvent(EventDataModel orderEvent);

    }

    public class EventService : IEventService
    {
        #region Create Event

        private readonly ISubject<EventDataModel> onCreateSubject = new ReplaySubject<EventDataModel>(1);

        // On next publishes the message/data to all subsribers
        public void CreateOrderEvent(EventDataModel orderEvent) => onCreateSubject.OnNext(orderEvent);

        public IObservable<EventDataModel> OnCreateObservable() => onCreateSubject.AsObservable();

        #endregion

        #region StatusUpdate Event

        private readonly ISubject<EventDataModel> onStatusUpdateSubject = new ReplaySubject<EventDataModel>(1);

        public void StatusUpdateEvent(EventDataModel orderEvent) => onStatusUpdateSubject.OnNext(orderEvent);

        public IObservable<EventDataModel> OnStatusUpdateObservable() => onStatusUpdateSubject.AsObservable();

        #endregion
    }
}
