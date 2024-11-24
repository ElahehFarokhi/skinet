using Core.Entities;
using System.Threading.Tasks;
namespace Core.Interfaces;
public interface ICouponService
{
    Task<AppCoupon?> GetCouponFromPromoCode(string code);
}
