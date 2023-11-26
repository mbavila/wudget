using AutoMapper;
using BaseCodeDotNetCore.Data.Entities;
using BaseCodeDotNetCore.Utils.DtoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCodeDotNetCore.Domain.Transactions.ViewModels
{
    public class TransactionViewModel : IMapFrom<Transaction>, ICustomMap
    {
        public int TransactionId { get; set; }

        public string Details { get; set; }

        public DateTime TransactionDate { get; set; }

        public double Amount { get; set; }

        public int SubCategoryID { get; set; }

        public SubCategory SubCategory { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }



        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<Transaction, TransactionViewModel>()
                .ForMember(evm => evm.TransactionId, opt => opt.MapFrom(e => e.TransactionId))
                .ForMember(evm => evm.Details, opt => opt.MapFrom(e => e.Details))
                .ForMember(evm => evm.SubCategoryID, opt => opt.MapFrom(e => e.SubCategoryID))
                .ForMember(evm => evm.TransactionDate, opt => opt.MapFrom(e => e.TransactionDate))
                .ForMember(evm => evm.ModifiedBy, opt => opt.MapFrom(e => e.ModifiedBy))
                .ForMember(evm => evm.ModifiedDate, opt => opt.MapFrom(e => e.ModifiedDate))
                .ForMember(evm => evm.CreatedBy, opt => opt.MapFrom(e => e.CreatedBy))
                .ForMember(evm => evm.Amount, opt => opt.MapFrom(e => e.Amount))
                .ForMember(evm => evm.CreatedDate, opt => opt.MapFrom(e => e.CreatedDate));

            configuration?.CreateMap<TransactionViewModel, Transaction>()
                .ForMember(evm => evm.TransactionId, opt => opt.MapFrom(e => e.TransactionId))
                .ForMember(evm => evm.Details, opt => opt.MapFrom(e => e.Details))
                .ForMember(evm => evm.SubCategoryID, opt => opt.MapFrom(e => e.SubCategoryID))
                .ForMember(evm => evm.TransactionDate, opt => opt.MapFrom(e => e.TransactionDate))
                .ForMember(evm => evm.ModifiedBy, opt => opt.MapFrom(e => e.ModifiedBy))
                .ForMember(evm => evm.ModifiedDate, opt => opt.MapFrom(e => e.ModifiedDate))
                .ForMember(evm => evm.CreatedBy, opt => opt.MapFrom(e => e.CreatedBy))
                .ForMember(evm => evm.Amount, opt => opt.MapFrom(e => e.Amount))
                .ForMember(evm => evm.CreatedDate, opt => opt.MapFrom(e => e.CreatedDate));
        }
    }
}
