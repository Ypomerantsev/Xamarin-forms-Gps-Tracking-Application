using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Sensors
{
    public class TipZemljevida
    {
        private String value;
        public delegate void ChangedEventHandler(object sender, EventArgs e);
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

        public void spremenivrednost(String value)
        {
            this.value = value;
            OnChanged(EventArgs.Empty);
        }

        public void callEvent()
        {
            OnChanged(EventArgs.Empty);
        }

        public void attach(TipZemljevida evt)
        {
            // Add "ListChanged" to the Changed event on "List".
            this.Changed += new ChangedEventHandler(evtTrigger);
        }

        // This will be called whenever the list changes.
        public void evtTrigger(object sender, EventArgs e)
        {
            if(MapsPage.tekst == null || value == null) //če se slučajno pred Mapspageam to pokliče ko še ni tekst inicializiran, ki se zagotrovo pokliče
            {
                return;
            }
            MapsPage.tekst.Text = value;
        }

        public void Detach()
        {
            // Detach the event and delete the list
            this.Changed -= new ChangedEventHandler(evtTrigger);
        }
    }
}
