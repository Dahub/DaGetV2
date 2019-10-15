namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class FirstOperationByOperationTypeIdSpecification : BaseSpecification<Operation>
    {
        public FirstOperationByOperationTypeIdSpecification(Guid operationTypeId) : base(o =>
            o.OperationTypeId.Equals(operationTypeId))
        {
            ApplyPaging(0, 1);
        }
    }
}
