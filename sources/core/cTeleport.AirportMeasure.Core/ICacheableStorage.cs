using System;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICacheableStorage
    {
        /// <summary>
        /// Gets object from storage.
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes object from storage with key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Updates or inserts value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Upsert<T>(string key, T value, TimeSpan? expiry = null);
    }
}