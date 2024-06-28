using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService
        (DiscountContext _dbContext, ILogger<DiscountService> _logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _dbContext
                .Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }

            _logger.LogInformation("Get Discount for ProductName : {productName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Request"));
            }
            await _dbContext.Coupons.AddAsync(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is [Created] successfully. ProductName : {productName}", coupon.ProductName);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Request"));
            }
            _dbContext.Coupons.Update(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is [Updated] successfully. ProductName : {productName}", coupon.ProductName);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Discount Request"));
            }
            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Discount is [Deleted] successfully. ProductName : {productName}", coupon.ProductName);

            return new DeleteDiscountResponse() { IsSuccess = true};
        }
    }
}
