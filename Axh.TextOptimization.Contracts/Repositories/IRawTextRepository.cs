namespace Axh.TextOptimization.Contracts.Repositories
{
    using System.Collections.Generic;

    public interface IRawTextRepository
    {
        IList<RawText> GetAllRawText();
    }
}