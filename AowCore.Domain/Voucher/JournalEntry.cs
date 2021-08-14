
using AowCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeUtility;

namespace AowCore.Domain
{
    public class JournalEntry : AuditableEntity<Guid>, ITreeNode<JournalEntry>
    {
        public string VoucherName { get; set; }
      
        [Required]
        public decimal VoucherNumber { get; set; }
        public DateTime Date { get; set; }
        private Guid? _parentCategoryId;

        [Display(Name = "Parent Category")]
        public Guid? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set
            {
                if (Id == value)
                    throw new InvalidOperationException("A category cannot have itself as its parent.");

                _parentCategoryId = value;
            }
        }

        public virtual JournalEntry Parent { get; set; }
        public IList<JournalEntry> Children { get; set; }

        public int? SrNo { get; set; }
        public bool? OnRecord { get; set; }
        public Guid? RefAccountId { get; set; }
        public string AccountType { get; set; }        
        public string CrDrType { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebitAmount { get; set; }
        public string Note { get; set; }
        public Guid LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }
        public Guid VoucherId { get; set; }
        public virtual Voucher Vouchers { get; set; }
        //public Guid? FinancialYearId { get; set; }
        //public virtual FinancialYear FinancialYear { get; set; }

        //public Guid? ParticularId { get; set; }
        //public virtual Ledger Particular { get; set; }       
        //public Guid? InvoiceId { get; set; }
        //public virtual Invoice Invoice { get; set; }
        //public Guid? StoreProductId { get; set; }
        //public virtual StoreProduct StoreProduct { get; set; }
        //public Guid? ProductId { get; set; }
        //public virtual Product Product { get; set; }
    }
}
