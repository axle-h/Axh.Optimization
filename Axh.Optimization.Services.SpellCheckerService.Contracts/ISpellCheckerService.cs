namespace Axh.Optimization.Services.SpellCheckerService.Contracts
{
    using System;

    public interface ISpellCheckerService : IDisposable
    {
        SpellCheckResponse Spell(string word);
    }
}