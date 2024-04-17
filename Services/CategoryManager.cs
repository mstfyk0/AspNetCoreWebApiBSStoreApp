using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public CategoryManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateOneCategory(CategoryDtoForInsertion category)
        {
            var entity =   _mapper.Map<Category>(category);
            _repositoryManager.CategoryRepository.CreateOneCategory(entity);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task DeleteOneCategory(int id ,bool trackChanges)
        {
            //var entity = await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            var entity = await GetOneCategoryByIdAndCheckExists(id, trackChanges);

            //GetOneBookByIdAndCheckExists
            //if (entity is null)
            //    throw new BookNotFoundException(id);

            _repositoryManager.CategoryRepository.DeleteOneCategory(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await _repositoryManager.CategoryRepository.GetAllCategoriesAsync(trackChanges);
        }

        public async Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges)
        {
            return await _repositoryManager.CategoryRepository.GetOneCategoryByIdAsync(id,trackChanges);

        }

        public async Task UpdateOneCategory(int id,CategoryDtoForUpdate category , bool trackChanges)
        {
            var entity = await GetOneCategoryByIdAndCheckExists(id, false);

            entity = _mapper.Map<Category>(category);

            _repositoryManager.CategoryRepository.UpdateOneCategory(entity);
            await _repositoryManager.SaveAsync();
        }

        private async Task<Category> GetOneCategoryByIdAndCheckExists(int id, bool trachChanges)
        {
            var entity = await _repositoryManager.CategoryRepository.GetOneCategoryByIdAsync(id, trachChanges);

            if (entity is null)
            {
                throw new CategoryNotFoundException(id);
            }
            return entity;

        }
    }
}
