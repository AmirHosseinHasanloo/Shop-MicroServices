using System.Net;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        #region Constructor

        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountRepository discountRepository, ILogger<DiscountController> logger)
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        #endregion


        #region Get Discount

        /// <summary>
        /// دریافت اطلاعات کوپن تخفیف بر اساس نام محصول.
        /// </summary>
        /// <param name="productName">نام محصول برای جستجوی کوپن تخفیف (حداکثر ۵۰ کاراکتر).</param>
        /// <returns>اطلاعات کوپن در صورت وجود.</returns>
        [HttpGet("[action]/{productName:length(1,50)}")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)] // موفقیت
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]         // نام محصول نامعتبر
        [ProducesResponseType((int)HttpStatusCode.NotFound)]          // کوپنی با این نام وجود ندارد
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)] // خطای غیرمنتظره
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            // بررسی اولیه نام محصول
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("نام محصول نمی‌تواند خالی باشد.");
            }

            try
            {
                // دریافت کوپن از مخزن داده‌ها
                var discount = await _discountRepository.GetDiscount(productName);

                if (discount == null)
                {
                    return NotFound($"هیچ کوپنی با نام محصول '{productName}' یافت نشد.");
                }

                return Ok(discount);
            }
            catch (Exception ex)
            {
                // لاگ خطا (در صورت داشتن لاگر)
                _logger.LogError(ex, "خطا در GetDiscount");

                return StatusCode(StatusCodes.Status500InternalServerError, $"خطای غیرمنتظره: {ex.Message}");
            }
        }

        #endregion


        #region Create Discount

        /// <summary>
        /// ایجاد یک تخفیف جدید.
        /// </summary>
        /// <param name="coupon">شیء کوپن شامل اطلاعات تخفیف.</param>
        /// <returns>نتیجه‌ی عملیات ایجاد تخفیف.</returns>
        [HttpPost(Name = "CreateDiscount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // موفقیت
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // ورودی نامعتبر
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // خطای سرور
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            // بررسی صحت داده‌های ورودی
            if (coupon == null)
            {
                return BadRequest("اطلاعات کوپن نمی‌تواند خالی باشد.");
            }

            try
            {
                // تلاش برای ایجاد کوپن تخفیف در مخزن
                var isCreated = await _discountRepository.CreateDiscount(coupon);

                if (isCreated)
                {
                    // موفقیت در ایجاد کوپن
                    return Ok("کوپن با موفقیت ایجاد شد.");
                }

                // اگر کوپن ساخته نشد ولی خطایی هم رخ نداد
                return StatusCode(StatusCodes.Status500InternalServerError, "خطا در ایجاد کوپن.");
            }
            catch (Exception ex)
            {
                // ثبت لاگ خطا (در صورت داشتن سرویس لاگینگ)
                _logger.LogError(ex, "خطا در CreateDiscount");

                // بازگشت خطای سرور با پیام مناسب
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطای غیرمنتظره: {ex.Message}");
            }
        }

        #endregion


        #region Update Discount

        /// <summary>
        /// ویرایش تخفیف.
        /// </summary>
        /// <param name="coupon">شیء کوپن شامل اطلاعات تخفیف.</param>
        /// <returns>نتیجه‌ی عملیات ویرایش تخفیف.</returns>
        [HttpPut(Name = "UpdateDiscount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // موفقیت
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // ورودی نامعتبر
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // خطای سرور
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            // بررسی صحت داده‌های ورودی
            if (coupon == null)
            {
                return BadRequest("اطلاعات کوپن نمی‌تواند خالی باشد.");
            }
            try
            {
                // تلاش برای به‌روزرسانی کوپن تخفیف در مخزن
                var isUpdated = await _discountRepository.UpdateDiscount(coupon);
                if (isUpdated)
                {
                    // موفقیت در به‌روزرسانی کوپن
                    return Ok("کوپن با موفقیت به‌روزرسانی شد.");
                }
                // اگر کوپن به‌روزرسانی نشد ولی خطایی هم رخ نداد
                return StatusCode(StatusCodes.Status500InternalServerError, "خطا در به‌روزرسانی کوپن.");
            }
            catch (Exception ex)
            {
                // ثبت لاگ خطا (در صورت داشتن سرویس لاگینگ)
                _logger.LogError(ex, "خطا در UpdateDiscount");
                // بازگشت خطای سرور با پیام مناسب
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطای غیرمنتظره: {ex.Message}");
            }
        }

        #endregion


        #region Delete Discount

        /// <summary>
        /// حذف تخفیف.
        /// </summary>
        /// <param name="productName">نام محصول مورد نظر برای حذف تخفیف</param>
        /// <returns>نتیجه‌ی عملیات حذف تخفیف.</returns>
        [HttpDelete("[action]/{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // موفقیت
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // ورودی نامعتبر
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))] // خطای سرور
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            // بررسی صحت نام محصول ورودی
            if (string.IsNullOrWhiteSpace(productName) || await _discountRepository.GetDiscount(productName) == null)
            {
                return BadRequest("اطلاعات کوپن معتبر نیست!.");
            }
            try
            {
                // تلاش برای به‌روزرسانی کوپن تخفیف در مخزن
                var isDeleted = await _discountRepository.DeleteDiscount(productName);
                if (isDeleted)
                {
                    // موفقیت در حذف کوپن
                    return Ok("کوپن با موفقیت حذف شد.");
                }
                // اگر کوپن حذف نشد ولی خطایی هم رخ نداد
                return StatusCode(StatusCodes.Status500InternalServerError, "خطا در حذف کوپن.");
            }
            catch (Exception ex)
            {
                // ثبت لاگ خطا (در صورت داشتن سرویس لاگینگ)
                _logger.LogError(ex, "خطا در DeleteDiscount");
                // بازگشت خطای سرور با پیام مناسب
                return StatusCode(StatusCodes.Status500InternalServerError, $"خطای غیرمنتظره: {ex.Message}");
            }
        }

        #endregion
    }
}
