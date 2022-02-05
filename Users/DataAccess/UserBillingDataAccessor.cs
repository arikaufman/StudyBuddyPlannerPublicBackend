using AutoMapper;
using plannerBackEnd.Users.DataAccess.Dao;
using plannerBackEnd.Users.DataAccess.Entities;
using plannerBackEnd.Users.Domain.DomainObjects;

namespace plannerBackEnd.Users.DataAccess
{
    public class UserBillingDataAccessor : IUserBillingDataAccessor
    {
        private readonly IMapper mapper;
        private readonly UserBillingDao userBillingDao;

        // -----------------------------------------------------------------------------

        public UserBillingDataAccessor(IMapper mapper, UserBillingDao userBillingDao)
        {
            this.mapper = mapper;
            this.userBillingDao = userBillingDao;
        }

        // -----------------------------------------------------------------------------

        public UserBilling Get(int requestedId)
        {
            return mapper.Map<UserBillingEntity, UserBilling>(userBillingDao.Get(requestedId));
        }

        // -----------------------------------------------------------------------------

        public UserBilling GetByUserId(int requestedId)
        {
            return mapper.Map<UserBillingEntity, UserBilling>(userBillingDao.GetByUserId(requestedId));
        }

        // -----------------------------------------------------------------------------

        public UserBilling Create(UserBilling userBilling)
        {
            return mapper.Map<UserBillingEntity, UserBilling>(userBillingDao.Create(mapper.Map<UserBilling, UserBillingEntity>(userBilling)));
        }

        // -----------------------------------------------------------------------------

        public UserBilling Update(UserBilling userBilling)
        {
            return mapper.Map<UserBillingEntity, UserBilling>(userBillingDao.Update(mapper.Map<UserBilling, UserBillingEntity>(userBilling)));
        }


    }
}