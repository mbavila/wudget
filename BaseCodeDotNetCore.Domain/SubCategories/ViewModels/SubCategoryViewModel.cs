using AutoMapper;
using BaseCodeDotNetCore.Data.Entities;
using BaseCodeDotNetCore.Utils.DtoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.SubCategories.ViewModels
{
    public class SubCategoryViewModel : IMapFrom<SubCategory>, ICustomMap
    {
        public int SubCategoryId { get; set; }

        public string Name { get; set; }

        public bool? Active { get; set; }

        public int CategoryID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Category Category { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(evm => evm.SubCategoryId, opt => opt.MapFrom(e => e.SubCategoryId))
                .ForMember(evm => evm.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(evm => evm.CategoryID, opt => opt.MapFrom(e => e.CategoryID))
                .ForMember(evm => evm.Category, opt => opt.MapFrom(e => e.Category))
                .ForMember(evm => evm.ModifiedBy, opt => opt.MapFrom(e => e.ModifiedBy))
                .ForMember(evm => evm.ModifiedDate, opt => opt.MapFrom(e => e.ModifiedDate))
                .ForMember(evm => evm.CreatedBy, opt => opt.MapFrom(e => e.CreatedBy))
                .ForMember(evm => evm.CreatedDate, opt => opt.MapFrom(e => e.CreatedDate));

            configuration?.CreateMap<SubCategoryViewModel, SubCategory>()
                .ForMember(evm => evm.SubCategoryId, opt => opt.MapFrom(e => e.SubCategoryId))
                .ForMember(evm => evm.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(evm => evm.CategoryID, opt => opt.MapFrom(e => e.CategoryID))
                .ForMember(evm => evm.Category, opt => opt.MapFrom(e => e.Category))
                .ForMember(evm => evm.ModifiedBy, opt => opt.MapFrom(e => e.ModifiedBy))
                .ForMember(evm => evm.ModifiedDate, opt => opt.MapFrom(e => e.ModifiedDate))
                .ForMember(evm => evm.CreatedBy, opt => opt.MapFrom(e => e.CreatedBy))
                .ForMember(evm => evm.CreatedDate, opt => opt.MapFrom(e => e.CreatedDate));
        }
    }
}
