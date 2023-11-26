namespace BaseCodeDotNetCore.Domain.SubCategories
{
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Paging;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.SubCategories.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<SubCategory> subCategoryRepository;
        private readonly IMapper mapper;

        public SubCategoryService(IUnitOfWork unit, IMapper mapper)
        {
            uow = unit;
            subCategoryRepository = uow == null ? null : uow.GetRepository<SubCategory>();

            this.mapper = mapper;
        }

        public IPaginate<SubCategoryViewModel> GetSubCategoryPaginated(SubCategorySearchViewModel search)
        {
            IPaginate<SubCategoryViewModel> subCategories = null;

            if (search != null)
            {
                subCategories = subCategoryRepository.GetList(
                selector: source => mapper.Map<SubCategoryViewModel>(source),
                predicate: source => (search.CategoryID == 0 || source.CategoryID == search.CategoryID),
                include: source => source.Include(s => s.Category),
                orderBy: source => source.OrderByDescending(x => x.SubCategoryId),
                index: search.Pagination == null ? 0 : search.Pagination.Page,
                size: search.Pagination == null ? 10 : search.Pagination.PageSize);
            }

            return subCategories;
        }

        public int AddNewSubCategory(SubCategoryViewModel subCategory)
        {
            int returnValue = 0;

            if (subCategoryRepository.SingleEntity(e => e.Name.Equals(subCategory.Name)) == null)
            {
                SubCategory entity = mapper.Map<SubCategory>(subCategory);
                entity.CreatedDate = DateTime.Now;
                entity.ModifiedDate = null;

                subCategoryRepository.Add(entity);
                uow.SaveChanges();

                returnValue = entity.SubCategoryId;
            }
            else
            {
                returnValue = -1;
            }

            return returnValue;
        }

        public int UpdateSubCategory(SubCategoryViewModel subCategory)
        {
            int returnValue = 0;

            SubCategory dbSubCategory = subCategoryRepository.SingleEntity(e => e.SubCategoryId == subCategory.SubCategoryId);

            if (dbSubCategory != null)
            {
                SubCategory dbExisting = subCategoryRepository.SingleEntity(
                    e => e.SubCategoryId != subCategory.SubCategoryId &&
                    e.Name.Equals(subCategory.Name));

                if (dbExisting == null)
                {
                    dbSubCategory = mapper.Map<SubCategory>(subCategory);
                    dbSubCategory.ModifiedDate = DateTime.Now;

                    subCategoryRepository.Update(dbSubCategory);
                    uow.SaveChanges();

                    returnValue = dbSubCategory.SubCategoryId;
                }
                else
                {
                    returnValue = -1;
                }
            }

            return returnValue;
        }
    }
}
