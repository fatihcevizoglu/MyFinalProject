using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal ,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
           var result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName), 
                CheckIfProductCountCategoryCorrect(product.CategoryId),
                CheckCategoryLimitExceded());
            if (result != null )
            {
                return result;
            }
           
                    _productDal.Add(product);

                    return new SuccessResult(Messages.ProductAdded);
            
        }

       
        public IResult Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 23)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);

        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 24)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }

        private IResult CheckIfProductCountCategoryCorrect(int categoryid)
        {

            var cat = _productDal.GetAll(p => p.CategoryId == categoryid).Count;

            if (cat >= 10)
            {
                return new ErrorResult(Messages.ProductCountCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productname)
        {

            var result = _productDal.GetAll(p => p.ProductName == productname).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

    }
}
