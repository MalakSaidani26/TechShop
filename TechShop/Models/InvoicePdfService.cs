using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace TechShop.Models
{
    public interface IInvoicePdfService
    {
        Task<byte[]> GenerateInvoicePdfAsync(Order order);
    }

    public class InvoicePdfService : IInvoicePdfService
    {
        private readonly IOrderRepository _orderRepository;

        public InvoicePdfService(IOrderRepository orderRepository, IWebHostEnvironment webHostEnvironment)
        {
            _orderRepository = orderRepository;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(Order order)
        {
            return await Task.Run(() => GenerateInvoicePdf(order));
        }

        private byte[] GenerateInvoicePdf(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginHorizontal(20);
                    page.MarginVertical(20);

                    page.Content().Column(col =>
                    {
                        // En-tête
                        col.Item().Text("FACTURE").FontSize(24).Bold().AlignCenter();
                        col.Item().PaddingBottom(20);

                        // Informations commande
                        col.Item().Column(subCol =>
                        {
                            subCol.Item().Text($"Numéro : {order.OrderId}").FontSize(11).Bold();
                            subCol.Item().Text($"Date : {order.CreatedAt:dd/MM/yyyy}").FontSize(10);
                            subCol.Item().Text($"Statut : {GetOrderStatusLabel(order.Status)}").FontSize(10);
                        });
                        col.Item().PaddingBottom(20);

                        // Informations client
                        col.Item().Column(subCol =>
                        {
                            subCol.Item().Text("INFORMATIONS CLIENT").FontSize(12).Bold();
                            subCol.Item().Text(order.CustomerName ?? "N/A").FontSize(11);
                            subCol.Item().Text(order.CustomerEmail ?? "N/A").FontSize(11);
                        });
                        col.Item().PaddingBottom(20);

                        // Tableau des articles
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(5);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            // En-têtes
                            table.Header(header =>
                            {
                                header.Cell().Background("#D3D3D3").Text("Article").FontSize(11).Bold();
                                header.Cell().Background("#D3D3D3").Text("Qty").FontSize(11).Bold().AlignCenter();
                                header.Cell().Background("#D3D3D3").Text("Prix").FontSize(11).Bold().AlignRight();
                                header.Cell().Background("#D3D3D3").Text("Total").FontSize(11).Bold().AlignRight();
                            });

                            // Articles
                            if (order.Items != null)
                            {
                                foreach (var item in order.Items)
                                {
                                    table.Cell().Text(item.ProductName ?? "N/A").FontSize(10);
                                    table.Cell().Text(item.Quantity.ToString()).FontSize(10).AlignCenter();
                                    table.Cell().Text($"{item.UnitPrice:0.00} DT").FontSize(10).AlignRight();
                                    table.Cell().Text($"{item.TotalPrice:0.00} DT").FontSize(10).AlignRight().Bold();
                                }
                            }
                        });
                        col.Item().PaddingBottom(20);

                        // Totaux
                        col.Item().AlignRight().Column(subCol =>
                        {
                            subCol.Item().Row(row =>
                            {
                                row.RelativeColumn().Text("Sous-total :").AlignRight();
                                row.RelativeColumn(1).Text($"{order.TotalAmount:0.00} DT").AlignRight();
                            });
                            subCol.Item().Row(row =>
                            {
                                row.RelativeColumn().Text("Frais de port :").AlignRight();
                                row.RelativeColumn(1).Text("0.00 DT").AlignRight();
                            });
                            subCol.Item().Row(row =>
                            {
                                row.RelativeColumn().Text("TOTAL :").AlignRight().Bold().FontSize(12);
                                row.RelativeColumn(1).Text($"{order.TotalAmount:0.00} DT").AlignRight().Bold().FontSize(12);
                            });
                        });
                        col.Item().PaddingTop(40);

                        // Pied de page
                        col.Item().Text("Merci pour votre achat !").AlignCenter().FontSize(10);
                    });
                });
            });

            return document.GeneratePdf();
        }

        private string GetOrderStatusLabel(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "En attente",
                OrderStatus.Validated => "Validée",
                _ => "Inconnue"
            };
        }
    }
}
