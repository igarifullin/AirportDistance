using System.Threading.Tasks;

namespace cTeleport.AirportMeasure.Core.Results
{
    public class ValidationResult : Result<bool?>
    {
        public static implicit operator Task<ValidationResult>(ValidationResult r) => Task.FromResult(r);
    }
}