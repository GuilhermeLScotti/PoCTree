using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCTree.Services
{
    class ProjectDto : IDto
    {
        public ProjectDto(int addressId, int projectId, string description) 
        {
            AddressId = addressId;
            ProjectId = projectId;
            Description = description;
            DtoType = DtoType.Project;
        }
        
        public int AddressId { get; set; }
        public DtoType DtoType { get; set; }
        public int ProjectId { get; set; }

        public string Description { get; set; }

        public string GetMyId()
        {
            return $"{ProjectId}{DtoType}";
        }

        public string GetMyParentId()
        {
            return $"{AddressId}{DtoType.Folder}{DtoType}";
        }
        
        public override int GetHashCode()
        {
            return GetMyId().GetHashCode();
        }
    }
}
