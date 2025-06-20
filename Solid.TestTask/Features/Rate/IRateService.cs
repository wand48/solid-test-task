using Solid.TestTask.Models;

namespace Solid.TestTask.Features.Rate
{
    public interface IRateService
    {
        public void ImportRate(RateBase model);

        public void ExportCrossRates(DateOnly date);
    }
}
