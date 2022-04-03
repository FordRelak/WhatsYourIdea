using System;

namespace Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}