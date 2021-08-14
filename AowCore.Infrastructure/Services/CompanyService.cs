using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AowCore.Application;
using AowCore.Application.IRepository;
using AowCore.Application.Services;
using AowCore.Domain;
using AowCore.Domain.Region;
using AowCore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace AowCore.Infrastructure.Services
{
    public class CompanyService : BaseManager, ICompanyService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILedgerRepository _ledgerRepository;
        private readonly ILedgerCategoryRepository _ledgerCategoryRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IProductCategoryRepository _ProductCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IApplicationDbContext context, IUnitOfWork unitOfWork, ICurrentUserService currentUserService,
            ICompanyRepository companyRepository, ILedgerRepository ledgerRepository, ILedgerCategoryRepository ledgerCategoryRepository,
            IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, ICountryRepository countryRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _ledgerRepository = ledgerRepository;
            _ledgerCategoryRepository = ledgerCategoryRepository;
            _currentUserService = currentUserService;
            _ProductCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
            _countryRepository = countryRepository;
            _context = context;
        }

        public async Task<IReadOnlyList<Company>> GetAll()
        {
            return await _companyRepository.GetAllAsync();
        }

        public async Task<IReadOnlyList<Country>> GetAllCountries()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<Company> GetCompanyById(Guid cmpId)
        {
            return await _companyRepository.GetByIdAsync(cmpId);
        }

        public async Task<IReadOnlyList<AppUserCompany>> GetCompaniesByUser(string userId)
        {
            return await _context.AppUserCompanies.Where(x => x.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<bool> CreateCompany(Company company, string userName, CancellationToken cancellationToken)
        {
            var userid = _currentUserService.UserId;

            Guid id = Guid.NewGuid();
            company.Id = id;
            await _companyRepository.AddAsync(company);

            Guid assetsId = Guid.NewGuid();
            Guid expensesId = Guid.NewGuid();
            Guid equityId = Guid.NewGuid();
            Guid incomeId = Guid.NewGuid();
            Guid liabilitiesId = Guid.NewGuid();
            Guid ownersDrawId = Guid.NewGuid();
            Guid profitLossId = Guid.NewGuid();

            Guid currentAssetId = Guid.NewGuid();
            Guid currentLiabilityId = Guid.NewGuid();
            Guid dutiesNTaxies = Guid.NewGuid();
            Guid directExpense = Guid.NewGuid();
            Guid inDirectExpense = Guid.NewGuid();
            Guid cashInHand = Guid.NewGuid();
            Guid stockInHand = Guid.NewGuid();


            var categories = new List<LedgerCategory>
                {
                     //ledger parent category
                           new LedgerCategory { Id = incomeId, Name = "Income",CompanyId=id,Type="Master"},
                       new LedgerCategory { Id = expensesId, Name = "Expense",CompanyId=id,Type="Master"},
                       new LedgerCategory { Id = equityId, Name = "Equity" ,CompanyId=id,Type="Master"},
                       new LedgerCategory { Id = liabilitiesId, Name = "Liability",CompanyId=id,Type="Master"},
                         new LedgerCategory {Id=assetsId,   Name = "Asset",CompanyId=id,Type="Master"} ,
                          new LedgerCategory {Id=ownersDrawId,   Name = "Drawings",CompanyId=id,Type="Master"} ,
                           new LedgerCategory {Id=profitLossId,   Name = "Profit & Loss",CompanyId=id,Type="Master"} ,


                        new LedgerCategory { Id = Guid.NewGuid(), Name = "Revenue",ParentCategoryId= incomeId,CompanyId=id,Type="Master"},
                        new LedgerCategory {Id=Guid.NewGuid(),   Name = "Sales Account",ParentCategoryId=incomeId,CompanyId=id,Type="Master"} ,
                        new LedgerCategory {Id=Guid.NewGuid(),   Name = "Debitors",ParentCategoryId=incomeId,CompanyId=id,Type="Master"} ,
                        new LedgerCategory { Id = Guid.NewGuid(), Name = "Other Income",ParentCategoryId= incomeId,CompanyId=id,Type="Master"},
                        new LedgerCategory {Id=Guid.NewGuid(),   Name = "Account Recievable",ParentCategoryId=incomeId,CompanyId=id,Type="Master"} ,


                      new LedgerCategory {Id=directExpense,   Name = "Direct Expense",ParentCategoryId=expensesId,CompanyId=id,Type="Master"} ,
                          new LedgerCategory {Id=inDirectExpense,   Name = "InDirect Expense",ParentCategoryId=expensesId,CompanyId=id,Type="Master"} ,
                       new LedgerCategory { Id = Guid.NewGuid(), Name = "Other Expense",ParentCategoryId= expensesId,CompanyId=id,Type="Master"},
                        //new LedgerCategory {Id=Guid.NewGuid(),   Name = "Purchase Account",ParentCategoryId=directExpense,CompanyId=Id,} ,
                        new LedgerCategory {Id=Guid.NewGuid(),   Name = "Creditors",ParentCategoryId=expensesId,CompanyId=id,Type="Master"} ,
                          new LedgerCategory { Id = Guid.NewGuid(), Name = "Cost Of Good" ,ParentCategoryId=directExpense,CompanyId=id,Type="Master"},


                          new LedgerCategory {Id=Guid.NewGuid(),   Name = "Capital Account",ParentCategoryId=equityId,CompanyId=id,Type="Master"} ,
                          new LedgerCategory {Id=Guid.NewGuid(),   Name = "Owners Equity",ParentCategoryId=equityId,CompanyId=id,Type="Master"} ,
                           // new LedgerCategory {Id=Guid.NewGuid(),   Name = "Owners Opening Balance",ParentCategoryId=EquityId,CompanyId=Id},


                       new LedgerCategory {Id=currentAssetId,   Name = "Current Assets",ParentCategoryId=assetsId,CompanyId=id,Type="Master"},
                     new LedgerCategory {Id=Guid.NewGuid(),   Name = "Fixed Assets",ParentCategoryId=assetsId,CompanyId=id,Type="Master"},
                      new LedgerCategory {Id=Guid.NewGuid(),   Name = "Bank Account",ParentCategoryId=currentAssetId,CompanyId=id,Type="Master"} ,
                         new LedgerCategory {Id=cashInHand,   Name = "Cash In Hand"  ,ParentCategoryId=currentAssetId,CompanyId=id,Type="Master"},
                         new LedgerCategory {Id=stockInHand,   Name = "Current Stock",ParentCategoryId=currentAssetId,CompanyId=id,Type="Master"},


                       new LedgerCategory { Id = Guid.NewGuid(), Name = "Other Liability", ParentCategoryId = liabilitiesId ,CompanyId=id,Type="Master"},
                     new LedgerCategory { Id = currentLiabilityId, Name = "Current Liability", ParentCategoryId = liabilitiesId ,CompanyId=id,Type="Master"} ,
                       new LedgerCategory {Id=Guid.NewGuid(),   Name = "Loans",ParentCategoryId=liabilitiesId,CompanyId=id,Type="Master"} ,
                             new LedgerCategory {Id=dutiesNTaxies,   Name = "Duties N Taxes",ParentCategoryId=currentLiabilityId,CompanyId=id,Type="Master"} ,
                             new LedgerCategory {Id=Guid.NewGuid(),   Name = "Account Payable",ParentCategoryId=liabilitiesId,CompanyId=id,Type="Master"} ,

                      new LedgerCategory {Id=Guid.NewGuid(),   Name = "Owners Draw",ParentCategoryId=ownersDrawId,CompanyId=id,Type="Master"} ,
                };
            categories.ForEach(s => _ledgerCategoryRepository.AddAsync(s));

            var ledgers = new List<Ledger>
                {
                          new Ledger {Id=Guid.NewGuid(),   Name = "Purchase Account",LedgerCategoryId=currentAssetId,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                           new Ledger {Id=Guid.NewGuid(),   Name = "Freight & Dilevery",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Ledger {Id=Guid.NewGuid(),   Name = "Job Work",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                        new Ledger {Id=Guid.NewGuid(),   Name = "Interest Paid",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                      new Ledger {Id=Guid.NewGuid(),   Name = "Depreciation Expense",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                        new Ledger {Id=Guid.NewGuid(),   Name = "Mobile & Internet Recharge",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                         new Ledger {Id=Guid.NewGuid(),   Name = "Salary & Wages",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                          new Ledger {Id=Guid.NewGuid(),   Name = "Office Expense",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Ledger {Id=Guid.NewGuid(),   Name = "Repair & Maintainance",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                              new Ledger {Id=Guid.NewGuid(),   Name = "Bank Service Charges",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Ledger {Id=Guid.NewGuid(),   Name = "Advertising & Promotion",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                             new Ledger {Id=Guid.NewGuid(),   Name = "Travel Expense",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                                new Ledger {Id=Guid.NewGuid(),   Name = "Charity & Donations",LedgerCategoryId=inDirectExpense,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                                new Ledger {Id=Guid.NewGuid(),   Name = "Profit & Loss",LedgerCategoryId=profitLossId,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                                   new Ledger {Id=Guid.NewGuid(),   Name = "Cash",LedgerCategoryId=cashInHand,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                                    new Ledger {Id=Guid.NewGuid(),   Name = "Stock In Hand",LedgerCategoryId=stockInHand,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,

                };

            ledgers.ForEach(l => _ledgerRepository.AddAsync(l));

            Guid productTaxCategory = Guid.NewGuid();
            var sundryItemsCategory = new List<ProductCategory>
                {
                          new ProductCategory {Id=Guid.NewGuid(),   Name = "Discount",CompanyId=id,Type="Sundry Item"} ,
                          new ProductCategory {Id=Guid.NewGuid(),   Name = "Round Off",CompanyId=id,Type="Sundry Item"},
                          new ProductCategory {Id=productTaxCategory,   Name = "Taxation",CompanyId=id,Type="Sundry Item"},
                          new ProductCategory {Id=Guid.NewGuid(),   Name = "Finished Goods",CompanyId=id,Type="Voucher Item"},
                           new ProductCategory {Id=Guid.NewGuid(),   Name = "Raw Materials",CompanyId=id,Type="Voucher Item"}
                };
            sundryItemsCategory.ForEach(l => _ProductCategoryRepository.AddAsync(l));

            Guid iGst = Guid.NewGuid();
            Guid sGst = Guid.NewGuid();
            Guid cGst = Guid.NewGuid();
            Guid uGst = Guid.NewGuid();

            if (company.TaxType == "GST")
            {
                var gstledgers = new List<Ledger>
                {
                          new Ledger {Id=iGst,   Name = "IGST",LedgerCategoryId=dutiesNTaxies,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName },
                           new Ledger {Id=sGst,   Name = "SGST",LedgerCategoryId=dutiesNTaxies,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Ledger {Id=cGst,   Name = "CGST",LedgerCategoryId=dutiesNTaxies,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Ledger {Id=uGst,   Name = "UGST",LedgerCategoryId=dutiesNTaxies,CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                };
                gstledgers.ForEach(l => _ledgerRepository.AddAsync(l));
                var sundryItems = new List<Product>
                {
                    new Product {Id=Guid.NewGuid(),   Name = "IGST 5 %",LedgerId=iGst,Percent="5",ProductCategoryId=productTaxCategory,ItemType="Sundry Item", CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName },
                          new Product {Id=Guid.NewGuid(),   Name = "IGST 12 %",Percent="12",LedgerId=iGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName },
                          new Product {Id=Guid.NewGuid(),   Name = "IGST 18 %",Percent="18",LedgerId=iGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName },
                          new Product {Id=Guid.NewGuid(),   Name = "IGST 28 %",Percent="28",LedgerId=iGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName },
                           new Product {Id=Guid.NewGuid(),   Name = "SGST 2.5 %",Percent="2.5",LedgerId=sGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                           new Product {Id=Guid.NewGuid(),   Name = "SGST 6 %",Percent="6",LedgerId=sGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Product {Id=Guid.NewGuid(),   Name = "SGST 9 %",Percent="9",LedgerId=sGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                              new Product {Id=Guid.NewGuid(),   Name = "SGST 14 %",Percent="14",LedgerId=sGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Product {Id=Guid.NewGuid(),   Name = "CGST 2.5 %",Percent="2.5",LedgerId=cGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Product {Id=Guid.NewGuid(),   Name = "CGST 6 %",Percent="6",LedgerId=cGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                            new Product {Id=Guid.NewGuid(),   Name = "CGST 9 %",Percent="9",LedgerId=cGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                             new Product {Id=Guid.NewGuid(),   Name = "CGST 14 %",Percent="14",LedgerId=cGst,ProductCategoryId=productTaxCategory,ItemType="Sundry Item",CreatedDate=DateTime.Now,UpdatedDate=DateTime.Now,CreatedBy=userName} ,
                };
                sundryItems.ForEach(l => _productRepository.AddAsync(l));
            }
            CompanyUserAdd(id, userid);
            var updated = await _unitOfWork.Commit(cancellationToken);
            return updated > 0;
        }

        public void CompanyUserAdd(Guid cmpId, string userId)
        {
            AppUserCompany appUserCompany = new AppUserCompany
            {
                CompanyId = cmpId,
                ApplicationUserId = userId
            };
            _context.AppUserCompanies.Add(appUserCompany);
        }

        public async Task<bool> UpdateCompany(Company updatePost, CancellationToken cancellationToken)
        {
            _companyRepository.Update(updatePost);
            var updated = await _unitOfWork.Commit(cancellationToken);
            return updated > 0;
        }

        public async Task<bool> DeleteCompany(Company cmp, CancellationToken cancellationToken)
        {
            _companyRepository.Remove(cmp);
            var deleted = await _unitOfWork.Commit(cancellationToken);
            return deleted > 0;
        }
    }
}
