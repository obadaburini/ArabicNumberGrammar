using System.ComponentModel.DataAnnotations;

namespace ArabicNumberGrammar.Mvc.Models
{
    public enum NounGender
    {
        Masculine,
        Feminine
    }

    public enum GrammaticalCase
    {
        Nominative,   // رفع
        Accusative,   // نصب
        Genitive      // جر
    }

    public class ComposeRequest
    {
        [Range(1, 99, ErrorMessage = "العدد لازم يكون بين 1 و 99.")]
        public int Number { get; set; }

        public NounGender Gender { get; set; }

        // افتراضي: نصب (الأشيع)
        public GrammaticalCase Case { get; set; } = GrammaticalCase.Accusative;

        [Required(ErrorMessage = "لازم تدخل المفرد (مثل: كتاب / سيارة).")]
        public string NounSingular { get; set; } = "";

        public string? NounDual { get; set; }     // مطلوب فقط إذا Number = 2
        public string? NounPlural { get; set; }   // مطلوب فقط إذا Number بين 3 و10
    }
}
