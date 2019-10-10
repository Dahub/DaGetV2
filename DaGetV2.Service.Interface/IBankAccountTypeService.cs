namespace DaGetV2.Service.Interface
{
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using DTO;

    public interface IBankAccountTypeService
    {
        IEnumerable<BankAccountTypeDto> GetAll(IContext context);
    }
}
