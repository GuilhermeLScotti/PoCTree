using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCTree.Services
{
    class QuotationDto : IDto
    {
        public QuotationDto(int addressId, int quotationId)
        {
            AddressId = addressId;
            QuotationId = quotationId;
            DtoType = DtoType.Quotation;
        }

        public int AddressId { get; set; }
        public DtoType DtoType { get; set; }
        public int QuotationId { get; set; }

        public string GetMyId()
        {
            return $"{QuotationId}{DtoType}";
        }

        public string GetMyParentId()
        {
            return $"{AddressId}{DtoType.Folder}{DtoType.Quotation}";
        }

        public override int GetHashCode()
        {
            return GetMyId().GetHashCode();
        }
    }
}
