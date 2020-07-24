using System;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICacheableQuery<TResult> : IQuery<TResult>
    {
        /// <summary>
        /// Cache document lifetime
        /// </summary>
        TimeSpan LifeTime { get; }
        
        /// <summary>
        /// Cache document key
        /// </summary>
        string Key { get; }
    }
}