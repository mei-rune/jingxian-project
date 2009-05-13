

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;

//namespace jingxian.core.runtime
//{
//    public class EventSupplier<TEventArgs> where TEventArgs: EventArgs
//    {
//        private Dictionary<string, List<EventHandler<TEventArgs>>> _Listeners;

//        private Dictionary<string, List<EventHandler<TEventArgs>>> Listeners
//        {
//            get
//            {
//                if (_Listeners == null)
//                {
//                    _Listeners = new Dictionary<string, List<EventHandler<TEventArgs>>>();
//                    _Listeners.Add(string.Empty, new List<EventHandler<TEventArgs>>());
//                }
//                return _Listeners;
//            }
//        }

//        public void Add(EventHandler<TEventArgs> listener)
//        {
//            Add(string.Empty, listener);
//        }

//        public void Add(string context, EventHandler<TEventArgs> listener)
//        {

//            if (context == null)
//                throw new ArgumentNullException("context");
//            if (listener == null)
//                throw new ArgumentNullException("listener");

//            List<EventHandler<TEventArgs>> innerListenerList;

//            if (Listeners.TryGetValue(context, out innerListenerList))
//            {
//                if (innerListenerList.Contains(listener))
//                {
//                    string msg = string.Format(CultureInfo.InvariantCulture, 
//                        Resources.ExceptionMessages.CannotAddListenerForContext, context);

//                    throw new InvalidOperationException(msg);
//                }
//                else
//                {
//                    innerListenerList.Add(listener);
//                }
//            }
//            else
//            {
//                innerListenerList = new List<EventHandler<TEventArgs>>();
//                Listeners.Add(context, innerListenerList);
//                innerListenerList.Add(listener);
//            }
//        }

//        public bool Contains(string context)
//        {
//            return Listeners.ContainsKey(context);
//        }

//        public bool Contains(EventHandler<TEventArgs> listener)
//        {
//            return Contains(string.Empty, listener);
//        }

//        public bool Contains(string context, EventHandler<TEventArgs> listener)
//        {
//            List<EventHandler<TEventArgs>> innerListenerList;
//            if (Listeners.TryGetValue(context, out innerListenerList))
//            {
//                return innerListenerList.Contains(listener);
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public void Remove(EventHandler<TEventArgs> listener)
//        {
//            Remove(string.Empty, listener);
//        }

//        public void Remove(string context, EventHandler<TEventArgs> listener)
//        {
//            if (listener == null)
//                throw new ArgumentNullException("listener");
//            if (context == null)
//                throw new ArgumentNullException("context");

//            List<EventHandler<TEventArgs>> listenerList;

//            if (!Listeners.TryGetValue(context, out listenerList))
//            {
//                string msg = string.Format(CultureInfo.InvariantCulture,
//                    Resources.ExceptionMessages.CannotRemoveListenerForNonExistentContext, context);

//                throw new InvalidOperationException(msg);

//            }

//            if (listenerList.Contains(listener))
//            {
//                listenerList.Remove(listener);
//            }
//            else
//            {
//                string contextString  = (context.Length == 0) ? "global context" : 
//                    string.Format(CultureInfo.InvariantCulture, "context '{0}'", context);
//                string msg = string.Format(CultureInfo.InvariantCulture,
//                    Resources.ExceptionMessages.CannotRemoveListenerForNonExistentContext, contextString);

//                throw new InvalidOperationException(msg);
//            }
//        }


//        public void Notify(TEventArgs e)
//        {
//            Notify(string.Empty, e);
//        }


//        public void Notify(string context, TEventArgs e)
//        {

//            if (!string.IsNullOrEmpty(context))
//            {
//                Notify(string.Empty, e);
//                return;
//            }

//            if (Listeners.ContainsKey(context))
//            {
//                List<EventHandler<TEventArgs>> listenerList;
//                if (Listeners.TryGetValue(context, out listenerList))
//                {
//                    Log.DebugFormat("-> {0} ------=> 通知 {1} 个监听者.", context, listenerList.Count);



//                    foreach (EventHandler<TEventArgs> listener in listenerList)
//                    {
//                            listener(this, e);
	
//                    }
//                }
//            }
//        }

//        #region _logger
//        private logging.ILog _Log;

//        protected logging.ILog Log
//        {
//            get
//            {
//                if (_Log == null)
//                {
//                    string loggerName = string.Format(CultureInfo.InvariantCulture, "jingxian.core.runtime.EventSupplier.{0}", typeof(TEventArgs).Name);
//                    _Log = logging.LogUtils.GetLogger(loggerName);
//                }
//                return _Log;
//            }
//        }
//        #endregion
//    }
//}