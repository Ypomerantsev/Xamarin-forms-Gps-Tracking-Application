using System;
namespace App1.Droid
{
    using System.Collections;

    // A delegate type for hooking up change notifications.
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    // A class that works just like ArrayList, but sends event
    // notifications whenever the list changes.
    public class EventTrigger
    {
        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ChangedEventHandler Changed;
        

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            /*if (Changed != null)
                Changed(this, e);*/
            Changed?.Invoke(this, e);
        }

        // Override some of the methods that can change the list;
        // invoke event after each
        public void callEvent()
        {
            OnChanged(EventArgs.Empty);
        }

        internal void evtTrigger()
        {
            throw new NotImplementedException();
        }

        internal void evtTrigger(object v, object sender, EventArgs eventArgs, object e)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class EventListener
    {
        private EventTrigger evt;

        public EventListener(EventTrigger evt)
        {
            this.evt = evt;
            // Add "ListChanged" to the Changed event on "List".
            evt.Changed += new ChangedEventHandler(evtTrigger);
        }

        // This will be called whenever the list changes.
        public abstract void evtTrigger(object sender, EventArgs e);

        public void Detach()
        {
            // Detach the event and delete the list
            evt.Changed -= new ChangedEventHandler(evtTrigger);
            evt = null;
        }
    }
}