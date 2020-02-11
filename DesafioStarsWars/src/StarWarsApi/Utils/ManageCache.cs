using Microsoft.Extensions.Caching.Distributed;
using System;

namespace StarWarsApi.Utils
{
    public interface IManageCache
    {
        IDistributedCache Cache { get; }

        /// <summary>
        /// Considera o TimeSpan informando para expiração do cache
        /// </summary>
        /// <param name="timeSpan">TimeSpan para expiração do cache</param>
        /// <returns>DistributedCacheEntryOptions</returns>
        DistributedCacheEntryOptions SetCacheExpiration(TimeSpan timeSpan);

        /// <summary>
        /// Considera o total de minutos restantes até 23:59:59 do dia para expiração do cache
        /// </summary>
        /// <returns>DistributedCacheEntryOptions</returns>
        DistributedCacheEntryOptions SetCacheExpiration();
    }

    public class ManageCache: IManageCache
    {
        public IDistributedCache Cache { get; }

        public ManageCache(IDistributedCache distributedCache)
        {
            this.Cache = distributedCache;
        }

        public DistributedCacheEntryOptions SetCacheExpiration(TimeSpan time)
        {
            var optionsCache = new DistributedCacheEntryOptions();
            optionsCache.SetAbsoluteExpiration(time);
            return optionsCache;
        }

        public DistributedCacheEntryOptions SetCacheExpiration()
        {
            var optionsCache = new DistributedCacheEntryOptions();
            optionsCache.SetAbsoluteExpiration(cacheExpirationTimeInMinutesDefault);
            return optionsCache;
        }

        private TimeSpan cacheExpirationTimeInMinutesDefault
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime lastHourDay = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                double totalMinutesToEndDay = lastHourDay.Subtract(DateTime.Now).TotalMinutes;
                return TimeSpan.FromMinutes(totalMinutesToEndDay);
            }
        }

    }
}
