using ArabicNumberGrammar.Mvc.Models;
using ArabicNumberGrammar.Mvc.Domain;

namespace ArabicNumberGrammar.Mvc.Services
{
    public class ArabicNumberAgreementService
    {
        public ComposeResult Compose(ComposeRequest req)
        {
            var (phrase, explanation) = ArabicNumberComposer.Compose(
                req.Number,
                req.Gender,
                req.Case,
                req.NounSingular,
                req.NounDual,
                req.NounPlural);

            return new ComposeResult
            {
                Phrase = phrase,
                Explanation = explanation
            };
        }
    }
}
