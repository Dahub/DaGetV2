namespace DaGetV2.ApplicationCore.Specifications
{
    using System;
    using Domain;

    internal class OperationByIdWithOperationTypeSpecification : BaseSpecification<Operation>
    {
        public OperationByIdWithOperationTypeSpecification(Guid operationId)
            : base(o => o.Id.Equals(operationId))
        {
            AddInclude(o => o.OperationType);
        }
    }
}
