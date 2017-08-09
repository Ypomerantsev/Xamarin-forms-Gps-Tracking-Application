using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace App1.Sensors
{

    public class ArrayWithListener<T> : List<T>
    {
        private String value;
        OstaliSenzorji referenca;
        int limit;
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ChangedEventHandler Changed;

        public ArrayWithListener(OstaliSenzorji os,int limit)
        {
            referenca = os;
            this.limit = limit;
        }

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        // Override some of the methods that can change the list;
        // invoke event after each

        public void AddWithTrigger(T value)
        {
            base.Add(value);

            if(this.Count > limit)
            {
                RemoveAt(limit);
            }

            OnChanged(EventArgs.Empty);
        }


        public void attach(ArrayWithListener<T> evt)
        {
            // Add "ListChanged" to the Changed event on "List".
            this.Changed += new ChangedEventHandler(evtTrigger);
        }

        // This will be called whenever the list changes.
        public void evtTrigger(object sender, EventArgs e)
        {
            referenca.OnChange();
        }

        public void Detach()
        {
            // Detach the event and delete the list
            this.Changed -= new ChangedEventHandler(evtTrigger);
        }
    }



}*/ //DEPRECATED
