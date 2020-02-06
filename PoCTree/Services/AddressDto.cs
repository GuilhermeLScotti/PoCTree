using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCTree.Services
{
    class AddressDto : IDto
    {
        public AddressDto(int addressId)
        {
            AddressId = addressId;
            DtoType = DtoType.Address;
        }

        public int AddressId { get; set; }
        public DtoType DtoType { get; set; }

        public virtual string GetMyId()
        {
            return $"{AddressId}{DtoType}";
        }

        public virtual string GetMyParentId()
        {
            return "";
        }

        public override int GetHashCode()
        {
            return GetMyId().GetHashCode();
        }
    }
}
