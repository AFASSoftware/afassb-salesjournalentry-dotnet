using System;
using System.Text.Json.Serialization;

namespace AFASSB.Models
{

    public class Administration
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string InstanceId { get; set; }
    }

    public class LedgerAccount
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string LedgerAccountNumber { get; set; }
        public string TypeId { get; set; }
        public string InstanceId { get; set; }
        public bool? InvestInAsset { get; set; }
        public string AdministrationId { get; set; }
    }

    public class OrganisationRoot
    {
        public string TrackingToken { get; set; }
        public Organisation[] Result { get; set; }
    }

    public class Organisation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object CocNumber { get; set; }
        public string VatNumber { get; set; }
        public string RelationId { get; set; }
        public object ExternalId { get; set; }
        public bool IsArchived { get; set; }
        public string EmailAddress { get; set; }
    }


    public class PersonRoot
    {
        public string TrackingToken { get; set; }
        public Person[] Result { get; set; }
    }

    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Initials { get; set; }
        public string Prefix { get; set; }
        public string Lastname { get; set; }
        public object ExternalId { get; set; }
        public bool IsArchived { get; set; }
    }

    public class SalesJournalEntry
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; }
        public Guid AdministrationId { get; set; }
        public string RelationType { get; set; }
        public Guid RelationId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PaymentMethod { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PaymentConditionId { get; set; }
        public InvoiceLine[] InvoiceLine { get; set; }
    }

    public class InvoiceLine
    {
        public Guid LedgerAccountId { get; set; }
        public string VatType { get; set; }
        public int AmountExcludingVat { get; set; }
    }

    public class PaymentCondition
    {
        public Guid Id { get; set; }
        public string AdministrationId { get; set; }
        public string Description { get; set; }
    }

}