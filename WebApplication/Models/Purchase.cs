using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Item // Товар
    {
        public string Code { get; set; } // Код товара
        public string Title { get; set; } // Наименование
    }
    public class ItemCategory // Товар
    {
        public string Category { get; set; } // Категория
    }
    public class Measure // Единица измерения
    {
        public string Title { get; set; } // Наименование 
    }


    public abstract class BasePosition
    {
        public long? ID { get; set; } // ID
        public int? Number { get; set; } // Порядковый номер строки в заявке
        public string Title { get; set; } // Название
    }

    public class SupplyService : BasePosition
    {
        public Guid? tmcRefId { get; set; } // GUID заявки
        public bool? Urgency { get; set; } // Срочность

        public float? CountClaimed { get; set; } // Количество заявленное
        public float? CountApproved { get; set; } // Количество согласованное
    }
    public class SupplyAdditionalService : BasePosition
    {
        public Guid? SupplyItemRefId { get; set; } // GUID заявки
        public string Code { get; set; } // код доп. услуги
    }

    public class SupplyItem : BasePosition // Строка по заявке
    {
        public List<SupplyAdditionalService> SupplyAdditionalServices { get; set; }
        public Guid? SupplyTicketRefId { get; set; } // GUID заявки
        public Guid? SupplyItemRefId { get; set; } // GUID заявки
        public DateTime? Modified { get; set; } // Дата последнего обновления
        public string ItemTitle { get; set; } // Название товара
        public string ItemCode { get; set; } // Код товара
        public string Category { get; set; } // Категория
        public string Measure { get; set; } // Единица измерения
        public bool? Urgency { get; set; } // Срочность
        public float? CountClaimed { get; set; } // Количество заявленное
        public float? CountWarehouseDivision { get; set; } // Количество на складе подразделения
        public int? StatusDelivery { get; set; } // Статус поставки
        public int? StatusPurchase { get; set; } // Статус закупки
        public float? CountWarehouseCentral { get; set; } // Количество на центральном складе
        public float? CountApproved { get; set; } // Количество согласованное
        public float? CountReserved { get; set; } // Количество зарезервированное
        public float? CountCollected { get; set; } // Количество собранное
        public float? CountMoved { get; set; } // Количество перемещенное
        public float? CountToPurchase { get; set; } // Количество к закупке
        public float? CountInPurchaseOrder { get; set; } // Количество в заказах покупки
        public float? CountPurchased { get; set; } // Количество закупленное
    }

    public class SupplyTicket
    {
        public Guid? SupplyTicketRefId { get; set; } // GUID заявки
        public List<SupplyItem> SupplyItems { get; set; }
    }
}