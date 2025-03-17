using AutoMapper;
using RoleBasedJWT.Model.Dto;
using RoleBasedJWT.Model.Entity;
using RoleBasedJWT.Repository;

namespace RoleBasedJWT.Service
{
    public class AccountService
    {
        private readonly AccountRepository accountRepository;
        private readonly IMapper mapper;

        public AccountService(AccountRepository accountRepository, IMapper mapper) { 
           this.accountRepository = accountRepository;
            this.mapper = mapper;
        }
        public  async Task<AccountDto> AddAccount(AccountDto accountDto)
        {
            var account = mapper.Map<Account>(accountDto);
            Account resAccount = await accountRepository.AddAccount(account);
            var resAccountDto = mapper.Map<AccountDto>(resAccount);
            return resAccountDto;
        }
    }
}
