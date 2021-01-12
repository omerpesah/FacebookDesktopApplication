using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace FacebookAppForm
{
    class FacadeEventForm
    {
        FacebookUserUtiles m_FacebookUserUtiles = FacebookUserUtiles.GetInstance;
        private IEnumerable<Event> m_Events;

        public event Action EventsHandler;


        public FacadeEventForm()
        {

        }

        internal void FetchAllEvents()
        {
            try
            {
                m_Events = m_FacebookUserUtiles.FetchAllEvents();
                OnEventsReady();
            }
            catch (Exception ex)
            {
                string str = ex.StackTrace;
            }
        }

        public IEnumerable<Event> Events
        {
            get
            {
                return m_Events;
            }
        }

        protected virtual void OnEventsReady()
        {
            if (EventsHandler != null)
            {
                EventsHandler.Invoke();
            }
        }

        internal IEnumerable<Event> FetchNotYetRepliedEvents()
        {
            return m_FacebookUserUtiles.FetchNotYetRepliedEvents();
        }

        internal IEnumerable<Event> FetchDeclinedEvents()
        {
            return m_FacebookUserUtiles.FetchDeclinedEvents();
        }

        internal IEnumerable<Event> FetchMaybeEvents()
        {
            return m_FacebookUserUtiles.FetchMaybeEvents();
        }
    }
}
