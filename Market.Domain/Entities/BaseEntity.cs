using Market.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public abstract class BaseEntity : ICreatedDate, IModifiedDate
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
