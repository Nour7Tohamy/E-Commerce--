namespace E_Commerce.Service.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get Methods

        // ✅ Get all categories
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        // ✅ Get category by id
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        // ✅ Get categories with their products
        public async Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();
            return _mapper.Map<IEnumerable<CategoryWithProductsDto>>(categories);
        }

        // ✅ Get categories with product count
        public async Task<IEnumerable<CategoryWithCountDto>> GetCategoriesWithProductCountAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();

            // error 
            var result = categories.Select(c => new CategoryWithCountDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ProductCount = c.Products?.Count ?? 0
            });

            return result;
        }

        // ✅ Get popular categories (by sales for example)
        public async Task<IEnumerable<CategorySalesDto>> GetPopularCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoriesWithProductsAsync();

            var result = categories
                // error 
                .Select(c => new CategorySalesDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    TotalSales = c.Products?.Sum(p => p.Price) ?? 0
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(5);

            return result;
        }

        // ✅ Search category by name
        public async Task<IEnumerable<CategoryDto>> SearchCategoryAsync(string name)
        {
            var categories = await _unitOfWork.Categories.FindAsync(c => c.Name.Contains(name));
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        #endregion

        #region Create Methods

        // ✅ Add single category
        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        // ✅ Add category + its products
        public async Task<CategoryWithProductsDto> AddCategoryWithProductsAsync(CreateCategoryWithProductsDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryWithProductsDto>(category);
        }

        #endregion

        #region Update Methods

        // ✅ Update category and its products
        public async Task<CategoryWithProductsDto> UpdateCategoryWithProductsAsync(int id, UpdateCategoryWithProductsDto dto)
        {
            var category = await _unitOfWork.Categories.GetCategoryWithProductsByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            // Update category properties
            _mapper.Map(dto, category);

            // Update or replace products inside it (simplified example)
            if (dto.Products != null)
            {
                category.Products.Clear();
                foreach (var productDto in dto.Products)
                {
                    var product = _mapper.Map<Product>(productDto);
                    category.Products.Add(product);
                }
            }

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryWithProductsDto>(category);
        }

        #endregion

        #region Delete Methods

        // ✅ Delete category
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}
