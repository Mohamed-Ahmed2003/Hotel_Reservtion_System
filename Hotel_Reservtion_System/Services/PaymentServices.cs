using Hotel_Reservtion_System.ServicesContracts;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;


namespace Hotel_Reservtion_System.Services
{
    public class PaymentServices : IPaymentServices
    {
        public Task<Session> CreateCheckoutSessionAsync(double amount, string userEmail)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(amount * 100), // Stripe uses cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Hotel Booking"
                            }
                        },
                        Quantity = 1
                    }
                },
                CustomerEmail = userEmail,
                SuccessUrl = "https://yourfrontend.com/payment-success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://yourfrontend.com/payment-failed",
            };
            var service = new SessionService();
            var session = service.CreateAsync(options);

            return session;

        }

        public Task<bool> HandleWebHookAsync(HttpRequest request)
        {
            var json = new StreamReader(request.Body).ReadToEnd();
            try
            {
                var webHookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");
                var stripeEvent = Stripe.EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], webHookSecret);
                if(stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            catch (StripeException e)
            {
                return Task.FromResult(false);
            }

        }
    }
}
