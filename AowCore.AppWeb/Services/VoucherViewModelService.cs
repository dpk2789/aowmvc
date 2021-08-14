using AowCore.Application.IRepository;
using AowCore.AppWeb.Helpers;
using AowCore.AppWeb.Interfaces;
using AowCore.AppWeb.ViewModels;
using AowCore.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AowCore.AppWeb.Services
{
    public class VoucherViewModelService : IVoucherViewModelService
    {
        private readonly ILedgerRepository _ledgerRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IJournalEntryRepository _journalEntryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IVoucherSundryItemRepository _voucherSundryItemRepository;
        private readonly IVoucherItemRepository _voucherItemRepository;
        private readonly ICountryRepository _countryRepository;

        public VoucherViewModelService(ILedgerRepository ledgerRepository, IVoucherRepository voucherRepository,
            IJournalEntryRepository journalEntryRepository, IProductRepository productRepository, IVoucherSundryItemRepository voucherSundryItemRepository,
            IVoucherItemRepository voucherItemRepository, ICountryRepository countryRepository)
        {
            _ledgerRepository = ledgerRepository;
            _voucherRepository = voucherRepository;
            _journalEntryRepository = journalEntryRepository;
            _productRepository = productRepository;
            _voucherSundryItemRepository = voucherSundryItemRepository;
            _voucherItemRepository = voucherItemRepository;
            _countryRepository = countryRepository;
        }
        public async Task<IReadOnlyList<LedgerViewModel>> GetAllLedgers(Guid cmpidG)
        {

            var ledgers = await _ledgerRepository.GetLedgers(cmpidG);
            var items = ledgers.Select(ledger =>
            {
                var viewModel = new LedgerViewModel
                {
                    Id = ledger.Id,
                    Name = ledger.Name,
                    CategoryName = ledger.LedgerCategory.Name,
                    RootCategory = ledger.ParentCategoryId != null ? ledger.Parent.Name : ledger.LedgerCategory.Name
                };
                return viewModel;
            }).ToList();

            return items;
        }

        public bool VoucherExists(Guid id)
        {
            return _voucherRepository.VoucherExistsAny(id);
        }

        //private bool JournalEntryExists(Guid id)
        //{
        //    return _context.JournalEntries.Any(e => e.Id == id);
        //}

        public async Task<List<LedgerViewModel>> GetLedgersForVouchers(Guid cmpidG, string term, string HeadName, string crdr)
        {
            Ledger[] accountsMatching;
            var ledgers = await _ledgerRepository.GetLedgersByTerm(cmpidG, term);

            if (HeadName == "Contra")
            {
                if (crdr == "Cr")
                {
                    accountsMatching = String.IsNullOrWhiteSpace(term) ? null : ledgers.Where(ii => ii.LedgerCategory.Name.Contains("Cash")).ToArray();
                }
                else
                {
                    term = "Bank";
                    accountsMatching = String.IsNullOrWhiteSpace(term) ? null : ledgers.Where(ii => ii.LedgerCategory.Name.Contains(term)).ToArray();
                }
            }
            else
            {
                accountsMatching = ledgers.ToArray();
            }

            List<LedgerViewModel> productViewModelList = new List<LedgerViewModel>();

            if (accountsMatching.Count() != 0)
            {
                foreach (var account in accountsMatching.ToList())
                {
                    var productViewModel = new LedgerViewModel
                    {
                        Id = account.Id,
                        Name = account.Name
                    };
                    // productViewModel.RootCategory = account.Parent.Name;
                    productViewModelList.Add(productViewModel);
                }
            }

            return productViewModelList;
        }

        public async Task<VoucherViewModel> GetVoucherForViewModel(Guid id, Guid cmpidG)
        {
            var voucher = await _voucherRepository.GetVoucherById(id);
            var viewModel = new VoucherViewModel();
            var items = new List<JournalEntryViewModel>();
            viewModel.Id = voucher.Id;
            viewModel.Date = voucher.Date;
            viewModel.VoucherNumber = voucher.VoucherNumber;
            viewModel.VoucherName = voucher.VoucherName;
            var ledgers = await GetAllLedgers(cmpidG);
            foreach (var item in voucher.JournalEntries)
            {
                var ledger = ledgers.Where(x => x.Id == item.LedgerId).FirstOrDefault();
                var jViewModel = new JournalEntryViewModel();
                jViewModel.Id = item.Id;
                jViewModel.VoucherName = item.VoucherName;
                jViewModel.VoucherDate = item.Date;
                jViewModel.CrDrType = item.CrDrType;
                jViewModel.LedgerId = ledger.Id;
                jViewModel.AccountName = ledger.Name;
                jViewModel.VoucherInvoice = item.VoucherNumber;
                jViewModel.CreditAmount = item.CreditAmount;
                jViewModel.DebitAmount = item.DebitAmount;
                jViewModel.SrNo = item.SrNo;
                jViewModel.VoucherId = item.VoucherId;
                // jViewModel.RootCategory = ledger.RootCategory;
                items.Add(jViewModel);
            }
            viewModel.JournalEntryViewModel = items;
            return viewModel;
        }

        public async Task<VoucherInvoiceViewModel> GetVoucherInvoiceForViewModel(Guid id, Guid cmpidG)
        {
            var voucher = await _voucherRepository.GetVoucherByIdIncludeItems(id);
            var viewModel = new VoucherInvoiceViewModel();
            var jEntries = new List<JournalEntryViewModel>();
            var items = new List<VoucherItemsViewModel>();
            decimal ItemsTotal = 0;
            decimal SundryitemsTotal = 0;
            viewModel.Id = voucher.Id;
            viewModel.Date = voucher.Date;
            viewModel.VoucherNumber = voucher.VoucherNumber;
            viewModel.VoucherName = voucher.VoucherName;

            var ledgers = await GetAllLedgers(cmpidG);
            foreach (var jentry in voucher.JournalEntries)
            {
                var ledger = ledgers.Where(x => x.Id == jentry.LedgerId).FirstOrDefault();
                if (voucher.VoucherName == "Sale Invoice")
                {
                    if (jentry.CrDrType == "Dr")
                    {
                        viewModel.LedgerName = ledger.Name;
                        viewModel.LedgerId = ledger.Id;
                    }
                }
                else
                {
                    if (jentry.CrDrType == "Cr")
                    {
                        viewModel.LedgerName = ledger.Name;
                        viewModel.LedgerId = ledger.Id;
                    }
                }
                var jViewModel = new JournalEntryViewModel();
                jViewModel.Id = jentry.Id;
                jViewModel.VoucherName = jentry.VoucherName;
                jViewModel.VoucherDate = jentry.Date;
                jViewModel.CrDrType = jentry.CrDrType;
                jViewModel.LedgerId = ledger.Id;
                jViewModel.AccountName = ledger.Name;
                jViewModel.VoucherInvoice = jentry.VoucherNumber;
                jViewModel.CreditAmount = jentry.CreditAmount;
                jViewModel.DebitAmount = jentry.DebitAmount;
                jViewModel.SrNo = jentry.SrNo;
                jViewModel.VoucherId = jentry.VoucherId;
                // jViewModel.RootCategory = ledger.RootCategory;
                jEntries.Add(jViewModel);
            }
            viewModel.JournalEntryViewModel = jEntries;
            var sundryItems = new List<VoucherSundryItemsViewModel>();
            foreach (var item in voucher.VoucherItems)
            {
                var itemViewModel = new VoucherItemsViewModel();
                itemViewModel.Id = item.Id;
                var product = await _productRepository.GetProductById(item.ProductId);
                itemViewModel.Name = product.Name;
                itemViewModel.ProductId = item.ProductId;
                itemViewModel.Description = item.Description;
                itemViewModel.MRPPerUnit = item.MRPPerUnit;
                itemViewModel.Quantity = item.Quantity;
                itemViewModel.ItemAmount = item.ItemAmount;
                itemViewModel.SrNo = item.SrNo;
                ItemsTotal = item.ItemAmount.Value + ItemsTotal;
                items.Add(itemViewModel);
            }
            viewModel.VoucherItemsViewModels = items;
            foreach (var sundryItem in voucher.VoucherSundryItems)
            {
                var sundryItemViewModel = new VoucherSundryItemsViewModel();
                sundryItemViewModel.Id = sundryItem.Id;
                var product = await _productRepository.GetProductById(sundryItem.ProductId);
                sundryItemViewModel.ProductId = sundryItem.Product.Id;
                sundryItemViewModel.LedgerId = product.Ledger.Id;
                sundryItemViewModel.Name = product.Name;
                sundryItemViewModel.Percent = sundryItem.Percent;
                sundryItemViewModel.ItemAmount = sundryItem.ItemAmount;
                sundryItemViewModel.SrNo = sundryItem.SrNo;
                SundryitemsTotal = sundryItem.ItemAmount.Value + SundryitemsTotal;
                sundryItems.Add(sundryItemViewModel);
            }
            viewModel.VoucherSundryItemsViewModels = sundryItems;
            viewModel.ItemsTotal = ItemsTotal;
            viewModel.SundryTotal = SundryitemsTotal;
            viewModel.Total = viewModel.ItemsTotal + viewModel.SundryTotal;

            return viewModel;
        }
        public async Task<PrintViewModel> GetVoucherInvoiceForBillPrint(Guid id, Guid cmpidG)
        {
            var voucher = await _voucherRepository.GetVoucherByIdIncludeItems(id);
            var jEntries = new List<JournalEntryViewModel>();
            var items = new List<VoucherItemsViewModel>();
            var country = await _countryRepository.FindAsync(voucher.FinancialYear.Company.CountryId);
            PrintViewModel viewModel = new PrintViewModel
            {
                Id = voucher.Id,
                Head = voucher.VoucherName,
                Invoice = voucher.VoucherNumber,
                Date = voucher.Date,
                Total = voucher.Total,
                Subtotal = voucher.Total,
                CompanyName = voucher.FinancialYear.Company.CompanyName,
                AddressLine1 = voucher.FinancialYear.Company.AddressLine1,
                AddressLine2 = voucher.FinancialYear.Company.AddressLine2,
                RegTaxNumber = voucher.FinancialYear.Company.TaxNumber,
                Website = voucher.FinancialYear.Company.Email,
                Email = voucher.FinancialYear.Company.Email,
                Mobile = voucher.FinancialYear.Company.Mobile,
                State = voucher.FinancialYear.Company.State,
                City = voucher.FinancialYear.Company.City,
                PinCode = voucher.FinancialYear.Company.PinCode,
            };
            var regionInfo = new RegionInfo(country.Code);
            string currencySymbol = regionInfo.CurrencySymbol;
            viewModel.CurrecySymbol = currencySymbol;
            //AmountPaid = daily.AmountPaid,
            //AmountDue = daily.AmountDue,
            //TermsDays = daily.TermsDays,
            //TermsEndDate = daily.TermsEndDate
            var ledgers = await GetAllLedgers(cmpidG);
            foreach (var supplier in voucher.JournalEntries)
            {
                var ledger = ledgers.Where(x => x.Id == supplier.LedgerId).FirstOrDefault();
                if (supplier.CrDrType == "Cr")
                {
                    viewModel.CustomerName = ledger.Name;
                    viewModel.CustomerMobile = ledger.Mobile;
                    viewModel.CustomerGSTIN = ledger.RegTaxNumber;
                    viewModel.CustomerAddressLine1 = ledger.ShippingAddressLine1;
                    viewModel.CustomerAddressLine2 = ledger.ShippingAddressLine1;
                    viewModel.CustomerLandMark = ledger.LandMark;
                    viewModel.CustomerState = ledger.State;
                    viewModel.CustomerCity = ledger.City;
                    viewModel.Country = ledger.Country;
                    viewModel.CustomerZipCode = ledger.ZipCode;
                }
            }
            decimal? itemsTotal = 0;
            List<VoucherItemsViewModel> dailyItemViewModelList = new List<VoucherItemsViewModel>();
            foreach (var item in voucher.VoucherItems.OrderBy(da => da.SrNo))
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                VoucherItemsViewModel dailyItemViewModel = new VoucherItemsViewModel
                {
                    Id = item.Id,
                    SrNo = item.SrNo,
                    Name = product.Name,
                    Description = product.AutoGenerateName,
                    ItemTaxCode = product.ProductTaxCode,
                    Quantity = item.Quantity,
                    MRPPerUnit = item.MRPPerUnit,
                    ItemAmount = item.ItemAmount,
                    ItemType = product.ItemType.Replace(" ", string.Empty),

                };
                itemsTotal = item.ItemAmount + itemsTotal;
                dailyItemViewModelList.Add(dailyItemViewModel);
            }
            viewModel.ItemsTotal = itemsTotal;
            viewModel.VoucherItemsViewModels = dailyItemViewModelList;

            decimal? sundryTotal = 0;
            List<VoucherSundryItemsViewModel> voucherSundryItemsViewModelList = new List<VoucherSundryItemsViewModel>();
            foreach (var item in voucher.VoucherSundryItems.OrderBy(da => da.SrNo))
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                VoucherSundryItemsViewModel sundryItemsViewModel = new VoucherSundryItemsViewModel
                {
                    Id = item.Id,
                    SrNo = item.SrNo,
                    Name = product.Name,
                    Description = item.Description,
                    Percent = item.Percent,
                    ItemAmount = item.ItemAmount
                };
                sundryTotal = item.ItemAmount + sundryTotal;
                voucherSundryItemsViewModelList.Add(sundryItemsViewModel);
            }
            viewModel.SundryTotal = sundryTotal.Value;
            viewModel.VoucherSundryItemsViewModels = voucherSundryItemsViewModelList;
            return viewModel;
        }

        public async Task<IEnumerable<VoucherViewModel>> JEntryListViewModel(IEnumerable<Voucher> vouchers, Guid cmpidG)
        {
            var voucherList = new List<VoucherViewModel>();
            var ledgers = await _ledgerRepository.GetLedgers(cmpidG);
            foreach (var voucher in vouchers)
            {
                var voucherViewModel = new VoucherViewModel
                {
                    Id = voucher.Id,
                    Date = voucher.Date,
                    VoucherNumber = voucher.VoucherNumber,
                    Total = voucher.Total,
                    VoucherName = voucher.VoucherName
                };
                var items = new List<JournalEntryViewModel>();
                foreach (var jentry in voucher.JournalEntries)
                {
                    var ledger = ledgers.Where(x => x.Id == jentry.LedgerId).FirstOrDefault();
                    var viewModel = new JournalEntryViewModel();
                    viewModel.Id = jentry.Id;
                    viewModel.CrDrType = jentry.CrDrType;
                    viewModel.VoucherDate = jentry.Date;
                    viewModel.AccountName = ledger.Name;
                    viewModel.VoucherInvoice = jentry.VoucherNumber;
                    viewModel.CreditAmount = jentry.CreditAmount;
                    viewModel.DebitAmount = jentry.DebitAmount;
                    viewModel.SrNo = jentry.SrNo;
                    viewModel.VoucherId = jentry.VoucherId;
                    //   viewModel.RootCategory = ledger.Parent.Name;
                    items.Add(viewModel);
                }
                voucherViewModel.JournalEntryViewModel = items;
                voucherList.Add(voucherViewModel);
            }
            return voucherList;
        }
        public async Task<IEnumerable<VoucherInvoiceViewModel>> PurchaseListToViewModel(IEnumerable<Voucher> vouchers, Guid cmpidG)
        {
            var voucherList = new List<VoucherInvoiceViewModel>();
            var ledgers = await _ledgerRepository.GetLedgers(cmpidG);
            foreach (var voucher in vouchers)
            {
                var voucherViewModel = new VoucherInvoiceViewModel
                {
                    Id = voucher.Id,
                    Date = voucher.Date,
                    VoucherNumber = voucher.VoucherNumber,
                    Total = voucher.Total,
                };
                var journalEntries = new List<JournalEntryViewModel>();
                foreach (var jentry in voucher.JournalEntries)
                {
                    var ledger = ledgers.Where(x => x.Id == jentry.LedgerId).FirstOrDefault();
                    if (voucher.VoucherName == "Sale Invoice")
                    {
                        if (jentry.CrDrType == "Dr")
                        {
                            voucherViewModel.LedgerName = ledger.Name;
                            voucherViewModel.LedgerId = ledger.Id;
                        }
                    }
                    else
                    {
                        if (jentry.CrDrType == "Cr")
                        {
                            voucherViewModel.LedgerName = ledger.Name;
                            voucherViewModel.LedgerId = ledger.Id;
                        }
                    }

                    var viewModel = new JournalEntryViewModel();
                    viewModel.Id = jentry.Id;
                    viewModel.CrDrType = jentry.CrDrType;
                    viewModel.VoucherDate = jentry.Date;
                    viewModel.AccountName = ledger.Name;
                    viewModel.VoucherInvoice = jentry.VoucherNumber;
                    viewModel.CreditAmount = jentry.CreditAmount;
                    viewModel.DebitAmount = jentry.DebitAmount;
                    viewModel.SrNo = jentry.SrNo;
                    viewModel.VoucherId = jentry.VoucherId;
                    //   viewModel.RootCategory = ledger.Parent.Name;
                    journalEntries.Add(viewModel);
                }
                voucherViewModel.JournalEntryViewModel = journalEntries;
                var items = new List<VoucherItemsViewModel>();
                foreach (var item in voucher.VoucherItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId);
                    var viewModel = new VoucherItemsViewModel();
                    viewModel.Id = item.Id;
                    viewModel.Name = product.Name;
                    viewModel.ItemAmount = item.ItemAmount;
                    viewModel.SrNo = item.SrNo;
                    items.Add(viewModel);
                }
                voucherViewModel.VoucherItemsViewModels = items;
                voucherList.Add(voucherViewModel);
            }
            return voucherList;
        }

        public async Task CreateVourcherAsync(string voucherName, string data, decimal Invoice, DateTime Date, Guid fyrId)
        {
            int SrNo = 1;
            var voucher = new Voucher();
            Guid Id = Guid.NewGuid();
            voucher.Id = Id;
            voucher.Date = Date;
            voucher.VoucherName = voucherName;
            voucher.VoucherNumber = Invoice;
            voucher.FinancialYearId = fyrId;
            var deserialiseList = JsonConvert.DeserializeObject<List<JournalEntryViewModel>>(data);

            foreach (var item in deserialiseList)
            {
                var journalEntry = new JournalEntry
                {
                    Id = Guid.NewGuid(),
                    Date = Date,
                    VoucherName = voucherName,
                    SrNo = SrNo,
                    VoucherNumber = Invoice,
                    //dailyAccounts.Is_Item = false;
                    VoucherId = Id,
                    LedgerId = item.LedgerId
                };
                if (item.CrDrType == "Cr")
                {
                    journalEntry.CrDrType = "Cr";
                    journalEntry.CreditAmount = item.CreditAmount.Value;
                }
                else
                {
                    journalEntry.CrDrType = "Dr";
                    journalEntry.DebitAmount = item.DebitAmount.Value;
                }

                await _journalEntryRepository.AddAsync(journalEntry);
                SrNo++;
            }

            await _voucherRepository.AddAsync(voucher);
            await _voucherRepository.SaveChangesAsyncNew();
        }

        public async Task EditVourcherAsync(string voucherName, string voucherData, decimal Invoice, string data, Guid fyrId)
        {
            int SrNo = 1;
            var voucherViewModel = JsonConvert.DeserializeObject<VoucherViewModel>(voucherData);
            var voucher = new Voucher();
            Guid Id = Guid.NewGuid();
            voucher.Id = Id;
            voucher.Date = voucherViewModel.Date;
            voucher.VoucherName = voucherName;
            voucher.VoucherNumber = Invoice;
            voucher.FinancialYearId = fyrId;
            var deserialiseList = JsonConvert.DeserializeObject<List<JournalEntryViewModel>>(data);

            foreach (var item in deserialiseList)
            {
                var journalEntry = new JournalEntry();
                journalEntry.Id = Guid.NewGuid();
                journalEntry.Date = voucherViewModel.Date;
                journalEntry.VoucherName = voucherName;
                journalEntry.SrNo = SrNo;
                journalEntry.VoucherNumber = Invoice;
                //dailyAccounts.Is_Item = false;
                journalEntry.VoucherId = Id;
                journalEntry.LedgerId = item.LedgerId;
                if (item.CrDrType == "Cr")
                {
                    journalEntry.CrDrType = "Cr";
                    journalEntry.CreditAmount = item.CreditAmount.Value;
                }
                else
                {
                    journalEntry.CrDrType = "Dr";
                    journalEntry.DebitAmount = item.DebitAmount.Value;
                }

                await _journalEntryRepository.AddAsync(journalEntry);
                SrNo++;
            }

            await _voucherRepository.AddAsync(voucher);
            await _voucherRepository.SaveChangesAsyncNew();
        }
        public async Task<JsonResultClientSide> EditVoucherInvoice(string data, string data2, string voucherName, decimal Invoice, DateTime Date, int? termsDays, Guid AccountId, Guid VoucherId,
           decimal? Total, string Note, Voucher voucher, Guid cmpidG, Guid fyrId, string actionName)
        {
            var result = new JsonResultClientSide();
            try
            {
                int srno = 1;
                int srnoItem = 1;
                decimal itemTotal = 0;
                voucher.Id = VoucherId;
                voucher.FinancialYearId = fyrId;
                voucher.Date = Date;
                voucher.VoucherName = voucherName;
                voucher.VoucherNumber = Invoice;
                voucher.Note = Note;
                voucher.Total = Total.Value;
                var updateVoucher = await _voucherRepository.GetVoucherByIdIncludeItems(VoucherId);
                if (updateVoucher != null)
                {
                    voucher.Id = updateVoucher.Id;
                    if (voucher.JournalEntries != null)
                    {
                        var journalEntries = voucher.JournalEntries.ToList();
                        if (journalEntries.Count != 0)
                        {
                            foreach (var account in journalEntries)
                            {
                                _journalEntryRepository.Remove(account);
                            }
                        }
                    }
                    if (voucher.VoucherItems != null)
                    {
                        var voucherItems = voucher.VoucherItems.ToList();
                        if (voucherItems.Count != 0)
                        {
                            foreach (var account in voucherItems)
                            {
                                _voucherItemRepository.Remove(account);
                            }
                        }
                    }
                    if (voucher.VoucherSundryItems != null)
                    {
                        var voucherSundryItems = voucher.VoucherSundryItems.ToList();
                        if (voucherSundryItems.Count != 0)
                        {
                            foreach (var account in voucherSundryItems)
                            {
                                _voucherSundryItemRepository.Remove(account);
                            }
                        }
                    }
                }
                if (voucherName == "Purchase Bill")
                {
                    JournalEntry jEntryCredit = new JournalEntry
                    {
                        Id = Guid.NewGuid(),
                        VoucherId = VoucherId,
                        VoucherName = voucherName,
                        Date = Date,
                        SrNo = srno,
                        LedgerId = AccountId,
                        CrDrType = "Cr",
                        VoucherNumber = Invoice,
                        CreditAmount = Total.Value
                    };
                    await _journalEntryRepository.AddAsync(jEntryCredit);
                    if (data2 != null)
                    {
                        var deserialiseList = JsonConvert.DeserializeObject<List<VoucherSundryItemsViewModel>>(data2);
                        foreach (var sundryItem in deserialiseList)
                        {
                            var product = await _productRepository.GetProductById(sundryItem.ProductId);
                            VoucherSundryItem voucherSundryItem = new VoucherSundryItem();
                            voucherSundryItem.Id = Guid.NewGuid();
                            voucherSundryItem.SrNo = srno;
                            voucherSundryItem.ProductId = sundryItem.ProductId;
                            voucherSundryItem.Percent = sundryItem.Percent;
                            voucherSundryItem.ItemAmount = sundryItem.ItemAmount;
                            voucherSundryItem.VoucherId = VoucherId;
                            await _voucherSundryItemRepository.AddAsync(voucherSundryItem);
                            srno++;
                            JournalEntry jEntryDebitTax = new JournalEntry
                            {
                                Id = Guid.NewGuid(),
                                VoucherId = VoucherId,
                                VoucherName = voucherName,
                                Date = Date,
                                SrNo = srno + 1,
                                LedgerId = product.LedgerId.Value,
                                CrDrType = "Dr",
                                VoucherNumber = Invoice,
                                DebitAmount = sundryItem.ItemAmount
                            };
                            await _journalEntryRepository.AddAsync(jEntryDebitTax);
                        }
                    }

                    if (data != null)
                    {
                        var deserialiseList = JsonConvert.DeserializeObject<List<VoucherItemsViewModel>>(data);
                        foreach (var item in deserialiseList)
                        {
                            VoucherItem dailyItem = new VoucherItem();
                            dailyItem.Id = Guid.NewGuid();
                            dailyItem.SrNo = srnoItem;
                            dailyItem.Description = item.Description;
                            dailyItem.MRPPerUnit = item.MRPPerUnit;
                            dailyItem.Price = item.MRPPerUnit.Value;
                            dailyItem.Quantity = item.Quantity;
                            dailyItem.ProductId = item.ProductId;
                            dailyItem.ItemAmount = item.ItemAmount;
                            itemTotal = itemTotal + item.ItemAmount.Value;
                            dailyItem.VoucherId = VoucherId;
                            await _voucherItemRepository.AddAsync(dailyItem);
                            srnoItem++;
                        }
                        var ledger = await _ledgerRepository.GetLedgerFirstOrDefault(cmpidG, "Purchase Account");
                        JournalEntry jEntryDebit = new JournalEntry
                        {
                            Id = Guid.NewGuid(),
                            VoucherId = VoucherId,
                            VoucherName = voucherName,
                            Date = Date,
                            SrNo = srno + 1,
                            LedgerId = ledger.Id,
                            CrDrType = "Dr",
                            VoucherNumber = Invoice,
                            DebitAmount = itemTotal
                        };
                        await _journalEntryRepository.AddAsync(jEntryDebit);
                    }


                }
                else if (voucherName == "Sale Invoice")
                {
                    JournalEntry jEntryDebit = new JournalEntry
                    {
                        Id = Guid.NewGuid(),
                        VoucherId = VoucherId,
                        VoucherName = voucherName,
                        Date = Date,
                        SrNo = srno,
                        LedgerId = AccountId,
                        CrDrType = "Dr",
                        VoucherNumber = Invoice,
                        DebitAmount = Total.Value
                    };
                    await _journalEntryRepository.AddAsync(jEntryDebit);
                    if (data2 != null)
                    {
                        var deserialiseList = JsonConvert.DeserializeObject<List<VoucherSundryItemsViewModel>>(data2);
                        foreach (var sundryItem in deserialiseList)
                        {
                            var product = await _productRepository.GetProductById(sundryItem.ProductId);
                            VoucherSundryItem voucherSundryItem = new VoucherSundryItem();
                            voucherSundryItem.Id = Guid.NewGuid();
                            voucherSundryItem.SrNo = srno;
                            voucherSundryItem.ProductId = sundryItem.ProductId;
                            voucherSundryItem.Percent = sundryItem.Percent;
                            voucherSundryItem.ItemAmount = sundryItem.ItemAmount;
                            voucherSundryItem.VoucherId = VoucherId;
                            await _voucherSundryItemRepository.AddAsync(voucherSundryItem);
                            srno++;
                            JournalEntry jEntryCreditTax = new JournalEntry
                            {
                                Id = Guid.NewGuid(),
                                VoucherId = VoucherId,
                                VoucherName = voucherName,
                                Date = Date,
                                SrNo = srno + 1,
                                LedgerId = product.LedgerId.Value,
                                CrDrType = "Cr",
                                VoucherNumber = Invoice,
                                CreditAmount = sundryItem.ItemAmount
                            };
                            await _journalEntryRepository.AddAsync(jEntryCreditTax);
                        }
                    }

                    if (data != null)
                    {
                        var ledger = await _ledgerRepository.GetLedgerFirstOrDefault(cmpidG, "Sales Account");
                        var deserialiseList = JsonConvert.DeserializeObject<List<VoucherItemsViewModel>>(data);
                        foreach (var item in deserialiseList)
                        {
                            VoucherItem dailyItem = new VoucherItem();
                            dailyItem.Id = Guid.NewGuid();
                            dailyItem.SrNo = srnoItem;
                            dailyItem.Description = item.Description;
                            dailyItem.MRPPerUnit = item.MRPPerUnit;
                            dailyItem.Price = item.MRPPerUnit.Value;
                            dailyItem.Quantity = item.Quantity;
                            dailyItem.ProductId = item.ProductId;
                            dailyItem.ItemAmount = item.ItemAmount;
                            itemTotal = itemTotal + item.ItemAmount.Value;
                            dailyItem.VoucherId = VoucherId;
                            await _voucherItemRepository.AddAsync(dailyItem);
                            srnoItem++;
                        }
                        JournalEntry jEntryCredit = new JournalEntry
                        {
                            Id = Guid.NewGuid(),
                            VoucherId = VoucherId,
                            VoucherName = voucherName,
                            Date = Date,
                            SrNo = srno + 1,
                            LedgerId = ledger.Id,
                            CrDrType = "Cr",
                            VoucherNumber = Invoice,
                            CreditAmount = itemTotal
                        };
                        await _journalEntryRepository.AddAsync(jEntryCredit);
                    }
                }



                if (actionName == "Create")
                {
                    await _voucherRepository.AddAsync(voucher);
                }
                else
                {
                    _voucherRepository.Update(voucher);
                }

                await _voucherRepository.SaveChangesAsyncNew();
                result.Msg = "SucessFully Added";
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                var msg = new ModelStateException(ex);
                result.Msg = msg.InnerException.Message;
                result.Success = false;
                //ModelState.AddModelError("", msg);                           
            }
            return result;
        }
        public async Task<JsonResultClientSide> DeleteConfirmed(Guid id)
        {
            var result = new JsonResultClientSide();
            var voucher = await _voucherRepository.GetVoucherById(id);
            _voucherRepository.Remove(voucher);
            await _voucherRepository.SaveChangesAsyncNew();
            result.Msg = voucher.VoucherName;
            result.Success = true;
            return result;
        }
    }
}
