using ArabicNumberGrammar.Mvc.Models;

namespace ArabicNumberGrammar.Mvc.Domain
{
    public static class ArabicNumberComposer
    {
        public static (string phrase, string explanation) Compose(
            int number,
            NounGender nounGender,
            GrammaticalCase grammaticalCase,
            string nounSingular,
            string? nounDual,
            string? nounPlural)
        {
            if (number < 1 || number > 99)
                throw new ArgumentOutOfRangeException(nameof(number), "Only 1..99 supported.");

            if (string.IsNullOrWhiteSpace(nounSingular))
                throw new ArgumentException("NounSingular is required.");

            // 1
            if (number == 1)
            {
                var one = nounGender == NounGender.Masculine ? "واحد" : "واحدة";
                return ($"{one} {nounSingular}",
                    "العدد (1) يوافق المعدود تذكيراً وتأنيثاً، والمعدود مفرد.");
            }

            // 2 (نطلب مثنى جاهز لضمان الصحة)
            if (number == 2)
            {
                if (string.IsNullOrWhiteSpace(nounDual))
                    throw new InvalidOperationException("للعدد 2 لازم تزوّد المثنى (NounDual).");

                return ($"{nounDual}",
                    "العدد (2) الأصل أن يُفهم من تثنية المعدود، لذا نعرض المثنى مباشرة لضمان الصحة.");
            }

            // 3-10 (نطلب جمع جاهز لضمان الصحة)
            if (number >= 3 && number <= 10)
            {
                if (string.IsNullOrWhiteSpace(nounPlural))
                    throw new InvalidOperationException("للأعداد 3–10 لازم تزوّد الجمع (NounPlural).");

                string numWord = Unit3to10(number, nounGender);
                return ($"{numWord} {nounPlural}",
                    "الأعداد (3–10): العدد يخالف المعدود تذكيراً وتأنيثاً، والمعدود جمع.");
            }

            // 11-19
            if (number >= 11 && number <= 19)
            {
                string numWord = Number11to19(number, nounGender, grammaticalCase);
                return ($"{numWord} {nounSingular}",
                    "الأعداد (11–19): المعدود تمييز مفرد. (11 و12) صيغ خاصة، و(13–19) الآحاد يخالف والعشرة توافق.");
            }

            // 20-99
            int tens = (number / 10) * 10;
            int ones = number % 10;

            string tensWord = TensWord(tens, grammaticalCase);

            if (ones == 0)
                return ($"{tensWord} {nounSingular}",
                    "العقود (20/30/.../90): المعدود تمييز مفرد، والعقد يتغير آخره حسب الإعراب (ون/ين).");

            string onesWord = OnesInCompounds(ones, nounGender, grammaticalCase);
            return ($"{onesWord} و{tensWord} {nounSingular}",
                "الأعداد (21–99): المعدود تمييز مفرد. الآحاد (1–2) توافق المعدود، و(3–9) تخالفه، والعقود تتغير حسب الإعراب.");
        }

        // 3-10: العدد يخالف المعدود
        private static string Unit3to10(int n, NounGender nounGender)
        {
            bool nounIsMasc = nounGender == NounGender.Masculine;

            return n switch
            {
                3 => nounIsMasc ? "ثلاثة" : "ثلاث",
                4 => nounIsMasc ? "أربعة" : "أربع",
                5 => nounIsMasc ? "خمسة" : "خمس",
                6 => nounIsMasc ? "ستة" : "ست",
                7 => nounIsMasc ? "سبعة" : "سبع",
                8 => nounIsMasc ? "ثمانية" : "ثمان",
                9 => nounIsMasc ? "تسعة" : "تسع",
                10 => nounIsMasc ? "عشرة" : "عشر",
                _ => throw new ArgumentOutOfRangeException(nameof(n))
            };
        }

        // 11-19 (مع تصحيح 12 حسب الإعراب)
        private static string Number11to19(int n, NounGender nounGender, GrammaticalCase grammaticalCase)
        {
            bool nounIsMasc = nounGender == NounGender.Masculine;

            if (n == 11)
                return nounIsMasc ? "أحد عشر" : "إحدى عشرة";

            if (n == 12)
            {
                bool nomin = grammaticalCase == GrammaticalCase.Nominative;

                if (nounIsMasc)
                    return nomin ? "اثنا عشر" : "اثني عشر";      // رفع / نصب-جر

                return nomin ? "اثنتا عشرة" : "اثنتي عشرة";      // رفع / نصب-جر
            }

            // 13-19: الآحاد يخالف، (عشر/عشرة) يوافق
            int ones = n - 10;

            string onesWord = ones switch
            {
                3 => nounIsMasc ? "ثلاثة" : "ثلاث",
                4 => nounIsMasc ? "أربعة" : "أربع",
                5 => nounIsMasc ? "خمسة" : "خمس",
                6 => nounIsMasc ? "ستة" : "ست",
                7 => nounIsMasc ? "سبعة" : "سبع",
                8 => nounIsMasc ? "ثمانية" : "ثمان",
                9 => nounIsMasc ? "تسعة" : "تسع",
                _ => throw new ArgumentOutOfRangeException(nameof(n))
            };

            string tenPart = nounIsMasc ? "عشر" : "عشرة";
            return $"{onesWord} {tenPart}";
        }

        // العقود: تتغير حسب الإعراب (ون/ين)
        private static string TensWord(int tens, GrammaticalCase grammaticalCase)
        {
            bool nomin = grammaticalCase == GrammaticalCase.Nominative;

            return tens switch
            {
                20 => nomin ? "عشرون" : "عشرين",
                30 => nomin ? "ثلاثون" : "ثلاثين",
                40 => nomin ? "أربعون" : "أربعين",
                50 => nomin ? "خمسون" : "خمسين",
                60 => nomin ? "ستون" : "ستين",
                70 => nomin ? "سبعون" : "سبعين",
                80 => nomin ? "ثمانون" : "ثمانين",
                90 => nomin ? "تسعون" : "تسعين",
                _ => throw new ArgumentOutOfRangeException(nameof(tens))
            };
        }

        // 21-99: 1-2 توافق، 3-9 تخالف + (2) تتغير حسب الإعراب
        private static string OnesInCompounds(int ones, NounGender nounGender, GrammaticalCase grammaticalCase)
        {
            bool nounIsMasc = nounGender == NounGender.Masculine;
            bool nomin = grammaticalCase == GrammaticalCase.Nominative;

            return ones switch
            {
                1 => nounIsMasc ? "واحد" : "إحدى",
                2 => nounIsMasc
                        ? (nomin ? "اثنان" : "اثنين")
                        : (nomin ? "اثنتان" : "اثنتين"),
                3 => nounIsMasc ? "ثلاثة" : "ثلاث",
                4 => nounIsMasc ? "أربعة" : "أربع",
                5 => nounIsMasc ? "خمسة" : "خمس",
                6 => nounIsMasc ? "ستة" : "ست",
                7 => nounIsMasc ? "سبعة" : "سبع",
                8 => nounIsMasc ? "ثمانية" : "ثمان",
                9 => nounIsMasc ? "تسعة" : "تسع",
                _ => throw new ArgumentOutOfRangeException(nameof(ones))
            };
        }
    }
}
