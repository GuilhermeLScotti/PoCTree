using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCTree.Services
{
    public interface IDto
    {
        int AddressId { get; set; }
        DtoType DtoType { get; set; }

        string GetMyId();
        string GetMyParentId();
    }

    public enum DtoType
    {
        Address,
        Project,
        Quotation,
        Folder
    }
}
