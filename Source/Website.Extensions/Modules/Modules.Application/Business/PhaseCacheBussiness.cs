﻿using System;
using Modules.Application.DataTransfer;
using Website.Library.DataTransfer;
using Website.Library.Extension;
using Website.Library.Interface;

namespace Modules.Application.Business
{
    public class PhaseCacheBussiness<T> : ICache where T : PhaseData
    {
        public Type GetCacheType()
        {
            return typeof(T);
        }

        public OrderedConcurrentDictionary<string, CacheData> Load()
        {
            OrderedConcurrentDictionary<string, CacheData> dictionary =
                new OrderedConcurrentDictionary<string, CacheData>();
            foreach (PhaseData item in PhaseBussiness.GetPhase())
            {
                dictionary.TryAdd(item.Name, item);
            }
            return dictionary;
        }

        public CacheData Reload(string code)
        {
            return PhaseBussiness.GetPhaseByCode(code);
        }
    }
}