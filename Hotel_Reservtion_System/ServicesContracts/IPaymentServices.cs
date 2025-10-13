using Stripe.Checkout;

namespace Hotel_Reservtion_System.ServicesContracts
{
    public interface IPaymentServices
    {
        Task<Session> CreateCheckoutSessionAsync(double amount, string userEmail);
        Task<bool> HandleWebHookAsync(HttpRequest request);
    }
}
