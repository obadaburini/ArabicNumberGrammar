using Microsoft.AspNetCore.Mvc;
using ArabicNumberGrammar.Mvc.Models;
using ArabicNumberGrammar.Mvc.Services;

namespace ArabicNumberGrammar.Mvc.Controllers
{
    public class NumbersController : Controller
    {
        private readonly ArabicNumberAgreementService _service;

        public NumbersController(ArabicNumberAgreementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // نموذج افتراضي عشان الصفحة ما تفتح null
            return View(new ComposeRequest());
        }

        [HttpPost]
        public IActionResult Index(ComposeRequest model)
        {
            // Validation مخصص حسب الرقم
            if (model.Number == 2 && string.IsNullOrWhiteSpace(model.NounDual))
                ModelState.AddModelError(nameof(model.NounDual),
                    "للعدد 2 لازم تدخل المثنى (مثل: كتابان / سيارتان).");

            if (model.Number >= 3 && model.Number <= 10 && string.IsNullOrWhiteSpace(model.NounPlural))
                ModelState.AddModelError(nameof(model.NounPlural),
                    "للأعداد من 3 إلى 10 لازم تدخل الجمع (مثل: كتب / سيارات).");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                ViewBag.Result = _service.Compose(model);
            }
            catch (Exception ex)
            {
                // إذا صار أي خطأ غير متوقع
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }
    }
}
