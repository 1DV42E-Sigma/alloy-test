using Alloy.Business.Tags;
using Alloy.Models.Pages;
using Alloy.Models.ViewModels;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alloy.Helpers.Compare
{
    public static class CategoryHelper
    {
        private static readonly string CATEGORY_ROOT_NAME = "CompareCategories";
        private static readonly string CATEGORY_ROOT_DESCRIPTION = "Jämförelse-kategorier";

        public static IEnumerable<CategoryItemModel> GetCategoryViewModels(OrganisationalUnitPage currentPage)
        {
            List<CategoryItemModel> tags = new List<CategoryItemModel>();

            foreach (var item in currentPage.Category)
            {
                var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();
                Category cat = categoryRepository.Get(item); // Category.Find(item);
                tags.Add(new CategoryItemModel() { Title = cat.Name, Url = TagFactory.Instance.GetTagUrl(currentPage, cat) });
            }

            return tags;
        }

        public static Category SaveCompareCategory(CategoryRepository repository, string name, string description)
        {
            var category = new Category(GetCompareRootCategory(repository), name);
            category.Description = description;
            repository.Save(category);
            return category;
        }
        public static Category FindCompareCategory(CategoryRepository repository, string name)
        {
            return GetCompareRootCategory(repository).FindChild(name);
        }
        private static Category GetCompareRootCategory(CategoryRepository repository)
        {
            var compareCategory = repository.Get(CategoryHelper.CATEGORY_ROOT_NAME); // Returns a read-only instance
            if (compareCategory != null)
            {
                //repository.Delete(compareCategory);
                return compareCategory;
            }

            Category newCategory = new Category(repository.GetRoot(), CATEGORY_ROOT_NAME);
            newCategory.Description = CategoryHelper.CATEGORY_ROOT_DESCRIPTION;
            repository.Save(newCategory);
            return newCategory;
        }

    }
}