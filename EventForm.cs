using FacebookWrapper.ObjectModel;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FacebookAppForm
{
    public partial class EventForm : FacebookGeneralForm
    {

        private FacadeEventForm m_FacadeEventForm = new FacadeEventForm();
        private FacebookObjectCollection<Event> m_EventsList;
        public EventForm()
            : base()
        {
            InitializeComponent();
        }

        public override void InitializeForm()
        {
            m_EventsList = new FacebookObjectCollection<Event>();
            string[] typeEvents = { "Maybe", "NotYetReplied", "Declined" };
            m_EventsComboBox.Items.AddRange(typeEvents);
        }

        protected override void OnShown(EventArgs e)
        {
            m_FacadeEventForm.EventsHandler += dataReady;
            Thread threadFetchData = new Thread(() => m_FacadeEventForm.FetchAllEvents());
            threadFetchData.Start();
        }

        private void dataReady()
        {
            this.Invoke(new Action(addDataBinding));
        }

        private void addDataBinding()
        {
            if (m_FacadeEventForm.Events.Count<Event>() > 0)
            {
                eventBindingSource.DataSource = m_FacadeEventForm.Events;
            }
        }

        private void eventsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedEmployee = (int)m_EventsComboBox.SelectedIndex;
            try
            {
                switch ((eEventsType)selectedEmployee)
                {
                    case eEventsType.Maybe:
                        {
                            eventBindingSource.DataSource = m_FacadeEventForm.FetchMaybeEvents();
                            break;
                        }
                    case eEventsType.NotYetReplied:
                        {
                            eventBindingSource.DataSource = m_FacadeEventForm.FetchNotYetRepliedEvents();
                            break;
                        }
                    case eEventsType.Declined:
                        {
                            eventBindingSource.DataSource = m_FacadeEventForm.FetchDeclinedEvents();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}

