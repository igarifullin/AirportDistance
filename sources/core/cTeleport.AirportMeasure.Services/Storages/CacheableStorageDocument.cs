using System;

namespace cTeleport.AirportMeasure.Services.Storages
{
    public class CacheableStorageDocument
    {
        public DateTime ExpireDate { get; set; }

        public object Value { get; set; }
    }
}