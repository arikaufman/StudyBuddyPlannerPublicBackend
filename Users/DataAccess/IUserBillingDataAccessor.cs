using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public interface IUserBillingDataAccessor
    {
        UserBilling Get(int requestedId);
        UserBilling GetByUserId(int requestedId);
        UserBilling Create(UserBilling userBilling);
        UserBilling Update(UserBilling userBilling);
    }
}