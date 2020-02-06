using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCTree.Services
{
    public class FolderDto : IDto
    {
        public FolderDto(int addressId, DtoType filesType)
        {
            AddressId = addressId;
            DtoType = DtoType.Folder;
            FilesType = filesType;
        }

        public int AddressId { get; set; }
        public DtoType DtoType { get; set; }
        public DtoType FilesType { get; set; }

        public string GetMyId()
        {
            return $"{AddressId}{DtoType}{FilesType}";
        }

        public string GetMyParentId()
        {
            return $"{AddressId}{DtoType.Address}";
        }

        public override int GetHashCode()
        {
            return GetMyId().GetHashCode();
        }
    }
}
