using System.Text.RegularExpressions;

namespace cTeleport.AirportMeasure.Data
{
    public static class RegexConstants
    {
        public static Regex Iata = new Regex("[A-Za-z]{3}", RegexOptions.Compiled);
    }
}