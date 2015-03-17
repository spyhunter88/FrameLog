using System;
using System.Collections.Generic;
using FrameLog.Exceptions;

namespace FrameLog.Logging
{
    public class DeferredValueMap
    {
        private Dictionary<string, DeferredValue> map;
        private object container;

        public DeferredValueMap(object container = null)
        {
            this.map = new Dictionary<string, DeferredValue>();
            this.container = container;
        }

        public void Store(string key, Func<object> futureValue)
        {
            map[key] = new DeferredValue(futureValue);
        }
        public IDictionary<string, object> CalculateAndRetrieve()
        {
            var result = new Dictionary<string, object>();
            foreach (var kv in map)
            {
                try
                {
                    result[kv.Key] = kv.Value.CalculateAndRetrieve();
                }
                catch (Exception e)
                {
                    throw new ErrorInDeferredCalculation(container, kv.Key, e);
                }
            }
            return result;
        }
    }
}
