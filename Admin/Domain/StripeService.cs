using plannerBackEnd.Admin.Domain.DomainObjects;
using plannerBackEnd.Common.Constants;
using plannerBackEnd.Users.Domain.DomainObjects;
using Stripe;
using System;
using System.Collections.Generic;

namespace plannerBackEnd.Admin.Domain
{
    public class StripeService : IStripeService
    {
        // -----------------------------------------------------------------------------

        public StripeService() { }

        // -----------------------------------------------------------------------------
        public Customer CreateCustomer(string email)
        {
            var customerCreate = new CustomerCreateOptions
            {
                Email = email,
            };

            var requestOptions = new RequestOptions
            {
                ApiKey = ApplicationConstants.StripeApiKey
            };
            var service = new CustomerService();
            var customer = service.Create(customerCreate, requestOptions);
            return customer;
        }

        // -----------------------------------------------------------------------------
        public Subscription CreateSubscription(UserBillingSubscriptionRequest subscriptionRequest)
        {
            StripeConfiguration.ApiKey = ApplicationConstants.StripeApiKey;
            // Attach payment method
            var paymentOptions = new PaymentMethodAttachOptions
            {
                Customer = subscriptionRequest.Customer,
            };
            var service = new PaymentMethodService();
            var paymentMethod = service.Attach(subscriptionRequest.PaymentMethod, paymentOptions);

            // Update customer's default invoice payment method
            var customerOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            var customerService = new CustomerService();
            customerService.Update(subscriptionRequest.Customer, customerOptions);

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = subscriptionRequest.Customer,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = subscriptionRequest.Price,
                    },
                },
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");
            var subscriptionService = new SubscriptionService();
            try
            {
                Subscription subscription = subscriptionService.Create(subscriptionOptions);
                return subscription;
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Failed to create subscription.{e}");
                return null;
            }
        }

        // -----------------------------------------------------------------------------
        public Subscription CancelSubscription(UserBillingCancelSubscriptionRequest req)
        {
            StripeConfiguration.ApiKey = ApplicationConstants.StripeApiKey;
            var service = new SubscriptionService();
            var subscription = service.Cancel(req.SubscriptionId, null);
            return subscription;

        }
        // -----------------------------------------------------------------------------
        public Product GetProduct(string productKey)
        {
            StripeConfiguration.ApiKey = ApplicationConstants.StripeApiKey;
            var service = new ProductService();
            return service.Get(productKey);
        }

        // -----------------------------------------------------------------------------
        public List<LimitedPrice> GetListPrices()
        {
            List < LimitedPrice > pricesToReturn = new List<LimitedPrice>();
            StripeConfiguration.ApiKey = ApplicationConstants.StripeApiKey;
            var options = new PriceListOptions { Limit = 2, Active = true};
            var service = new PriceService();
            StripeList<Price> prices = service.List(options);
            foreach (Price price in prices)
            {
                pricesToReturn.Add(new LimitedPrice()
                    {Amount = Convert.ToDouble(price.UnitAmount),
                        Id = price.Id, 
                        Interval = price.Recurring.Interval});
            }
            return pricesToReturn;
        }
    }
}
