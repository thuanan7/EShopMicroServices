﻿using Basket.API.Data;

namespace Basket.API.Basket.StoreBasket
{
    public record struct StoreBasketCommand(ShoppingCart Cart) 
        : ICommand<StoreBasketResult>;
    public record struct StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle
            (StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;

            await repository.StoreBasketAsync(cart, cancellationToken);
            //TODO: update cache

            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}
