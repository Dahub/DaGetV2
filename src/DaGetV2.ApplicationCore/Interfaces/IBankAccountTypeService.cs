namespace DaGetV2.ApplicationCore.Interfaces
{
    using System.Collections.Generic;
    using DTO;

    public interface IBankAccountTypeService
    {
        IEnumerable<BankAccountTypeDto> GetAll(IContext context);
    }
}
