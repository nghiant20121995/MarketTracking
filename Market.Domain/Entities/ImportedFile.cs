using Market.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class ImportedFile : BaseEntity, IIsDeleted
    {
        public string? Name { get; set; }
        public int FileType { get; set; }
        public string? Path { get; set; }
        public string? Url { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsProcessed { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
