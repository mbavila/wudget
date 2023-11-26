using AutoMapper;
using BaseCodeDotNetCore.Data.Entities;
using BaseCodeDotNetCore.Utils.DtoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Budgets.ViewModels
{
    public class BudgetViewModel : IMapFrom<Budget>, ICustomMap
    {
        public int BudgetID { get; set; }

        public int SubCategoryID { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public DateTime BudgetDate { get; set; }

        public double Amount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<Budget, BudgetViewModel>()
                .ForMember(evm => evm.BudgetID, opt => opt.MapFrom(e => e.BudgetID))
                .ForMember(evm => evm.BudgetDate, opt => opt.MapFrom(e => e.BudgetDate))
                .ForMember(evm => evm.SubCategoryID, opt => opt.MapFrom(e => e.SubCategoryID))
                .ForMember(evm => evm.Amount, opt => opt.MapFrom(e => e.Amount));

            configuration?.CreateMap<BudgetViewModel, Budget>()
                .ForMember(evm => evm.BudgetID, opt => opt.MapFrom(e => e.BudgetID))
                .ForMember(evm => evm.BudgetDate, opt => opt.MapFrom(e => e.BudgetDate))
                .ForMember(evm => evm.SubCategoryID, opt => opt.MapFrom(e => e.SubCategoryID))
                .ForMember(evm => evm.CreatedDate, opt => opt.MapFrom(e => e.CreatedDate))
                .ForMember(evm => evm.ModifiedDate, opt => opt.MapFrom(e => e.ModifiedDate))
                .ForMember(evm => evm.Amount, opt => opt.MapFrom(e => e.Amount));
        }
    }
}
